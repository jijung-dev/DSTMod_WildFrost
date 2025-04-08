using Deadpan.Enums.Engine.Components.Modding;

public class Pearl : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("pearl", "Pearl")
                .SetCardSprites("Pearl.png", "Wendy_BG.png")
                .SetTraits(TStack("Fragile", 1))
                .WithPools("GeneralUnitPool")
                .SetStats(1, null, 4)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Turn Reduce Counter To Allies", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("On Turn Add Attack To Allies", "On Turn Reduce Counter To Allies")
                .WithText("Reduce <keyword=counter> by <{a}> to all allies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.canBeBoosted = false;
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
                })
        );
    }
}
