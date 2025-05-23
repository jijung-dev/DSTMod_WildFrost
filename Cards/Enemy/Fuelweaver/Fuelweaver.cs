using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Fuelweaver : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("forestFuelweaver", "Forest Fuelweaver")
                .SetBossSprites("ForestFuelweaver.png", "Wendy_BG.png")
                .WithText("<hiddencard=dstmod.fern><hiddencard=dstmod.mysteriousPlant><hiddencard=dstmod.lightFlower>".Process())
                .SetStats(30, null, 4)
                .WithCardType("Boss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("CaveFuelweaver", 1),
                        SStack("Pre Turn Fill Player Board With Floral", 1),
                        SStack("On Turn Apply Sanity To Everything", 4),
                        SStack("Immune To Sanity", 1),
                        SStack("ImmuneToSnow", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("caveFuelweaver", "Cave Fuelweaver")
                .SetBossSprites("CaveFuelweaver.png", "Wendy_BG.png")
                .SetStats(40, 5, 3)
                .WithCardType("Boss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 4) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("AncientFuelweaver", 1),
                        SStack("When Deployed Fill Board With Cave Enemies", 1),
                        SStack("Immune To Sanity", 1),
                        SStack("ImmuneToSnow", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("ancientFuelweaver", "Ancient Fuelweaver")
                .SetBossSprites("AncientFuelweaver.png", "Wendy_BG.png")
                .WithText("<hiddencard=dstmod.skullWovenShadow><hiddencard=dstmod.handWovenShadow><hiddencard=dstmod.unseenHand>".Process())
                .SetStats(50, 6, 5)
                .WithCardType("Boss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 5) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Turn Fill Board With Woven Shadow", 1),
                        SStack("Immune To Sanity", 1),
                        SStack("ImmuneToSnow", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "CaveFuelweaver")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("caveFuelweaver");
                })
        );
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "AncientFuelweaver")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("ancientFuelweaver");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Player Board With Floral")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.withCards = new CardData[]
                    {
                        TryGet<CardData>("fern"),
                        TryGet<CardData>("mysteriousPlant"),
                        TryGet<CardData>("lightFlower"),
                    };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Player;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Enemy Board With Cave Enemies")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[]
                    {
                        TryGet<CardData>("caveSpider"),
                        TryGet<CardData>("batilisk"),
                        TryGet<CardData>("spitter"),
                        TryGet<CardData>("danglingDepthDweller"),
                    };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                })
        );
        assets.Add(
            StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Board With Cave Enemies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Fill Enemy Board With Cave Enemies");
                })
        );
        assets.Add(
            StatusCopy("Pre Trigger Copy Effects Of RandomAlly", "Pre Turn Fill Player Board With Floral")
                .WithText("Before attacking, fill player's board with <Florals>")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXPreTrigger>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Fill Player Board With Floral");
                })
        );
        assets.Add(
            StatusCopy("On Turn Apply Attack To Self", "On Turn Apply Sanity To Everything")
                .WithText("Apply <{a}><keyword=tgestudio.wildfrost.dstmod.sanity> to everything")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.effectToApply = TryGet<StatusEffectData>("Sanity");
                })
        );
        assets.Add(
            StatusCopy("On Turn Apply Attack To Self", "On Turn Fill Board With Woven Shadow")
                .WithText("Fill board with <Woven Shadows>")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.stackable = false;
                    data.effectToApply = TryGet<StatusEffectData>("Fill Board With Woven Shadow");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Board With Woven Shadow")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.withCards = new CardData[]
                    {
                        TryGet<CardData>("skullWovenShadow"),
                        TryGet<CardData>("handWovenShadow"),
                        TryGet<CardData>("unseenHand"),
                    };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Full;
                })
        );
    }
}
