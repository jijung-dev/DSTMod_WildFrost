using Deadpan.Enums.Engine.Components.Modding;

public class FriendlyFly : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("friendlyFly", "Friendly Fly")
                .SetCardSprites("FriendFly.png", "Wendy_BG.png")
                .WithPools("GeneralUnitPool")
                .SetStats(2, null, 6)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Turn Boost Random Ally Effect", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Boost Random Ally Effect")
                .WithText("Boost random ally effect by <{a}>")
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.canBeBoosted = false;
                        data.stackable = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("noBuilding"), TryGetConstraint("noChestHealth") };
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAlly;
                    data.effectToApply = TryGet<StatusEffectData>("Increase Effects");
                })
        );
    }
}
