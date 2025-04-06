using Deadpan.Enums.Engine.Components.Modding;

public class Toadstool : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("toadstool", "Toadstool")
                .SetSprites("Toadstool.png", "Wendy_BG.png")
                .SetStats(30, null, 5)
                .WithCardType("Miniboss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Toadstool Enraged", 1),
                        SStack("On Turn Lose Fill Board With Boomshroom", 1),
                        SStack("On Turn Fill Board With Boomshroom", 1),
                        SStack("Immune To Shroom", 1),
                        SStack("ImmuneToSnow", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("toadstoolEnraged", "Toadstool Enraged")
                .SetSprites("ToadstoolEnraged.png", "Wendy_BG.png")
                .SetStats(20, 1, 5)
                .WithCardType("Miniboss")
                .WithValue(28 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Shroom", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Hit All Units", 1),
                        SStack("When Shroom Applied To Self Heal Instead", 4),
                        SStack("When Deployed Fill Enemy Board With Sporecaps", 1),
                        SStack("Immune To Damage From Toadstool", 1),
                        SStack("ImmuneToSnow", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmuneToDamageFromCertainCard>("Immune To Damage From Toadstool")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmuneToDamageFromCertainCard>(data =>
                {
                    data.notAllowedCards = new TargetConstraint[] { TryGetConstraint("noToadstool") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Board With Boomshroom")
                .WithText("Fill board with <card=dstmod.boomshroom>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.eventPriority = -1;
                    data.withCards = new CardData[] { TryGet<CardData>("boomshroom") };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Full;
                })
        );
        assets.Add(
            StatusCopy("Lose Scrap", "Lose Fill Board With Boomshroom")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantLoseX>(data =>
                {
                    data.statusToLose = TryGet<StatusEffectData>("Fill Board With Boomshroom");
                })
        );
        assets.Add(
            StatusCopy("On Turn Apply Attack To Self", "On Turn Lose Fill Board With Boomshroom")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.doPing = false;
                    data.textKey = null;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Lose Fill Board With Boomshroom");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Enemy Board With Sporecaps")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[]
                    {
                        TryGet<CardData>("sporecapAttack"),
                        TryGet<CardData>("sporecapCounter"),
                        TryGet<CardData>("sporecapSnow"),
                        TryGet<CardData>("sporecapIncreaseEffect"),
                    };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                })
        );
        assets.Add(
            StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Enemy Board With Sporecaps")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Fill Enemy Board With Sporecaps");
                })
        );
        assets.Add(
            StatusCopy("When Shell Applied To Self Gain Spice Instead", "When Shroom Applied To Self Heal Instead")
                .WithText("When <keyword=shroom>'d, restore 2x<keyword=health> instead")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(data =>
                {
                    data.whenAppliedTypes = new string[] { "shroom" };
                    data.effectToApply = TryGet<StatusEffectData>("Heal");
                    data.adjustAmount = true;
                    data.multiplyAmount = 2;
                })
        );
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "Toadstool Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("toadstoolEnraged");
                })
        );
        assets.Add(
            StatusCopy("On Turn Apply Attack To Self", "On Turn Fill Board With Boomshroom")
                .WithText("Fill board with <card=dstmod.boomshroom>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.stackable = false;
                    data.effectToApply = TryGet<StatusEffectData>("Fill Board With Boomshroom");
                })
        );
    }
}
