using Deadpan.Enums.Engine.Components.Modding;

public class PanFlute : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("panFlute", "Pan Flute")
                .SetSprites("PanFlute.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Snow All Enemies", 1) };
                    data.needsTarget = false;
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCardPlayed>("Snow All Enemies")
                .WithText("Apply <keyword=snow> by <{a}> to all enemies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Snow");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                })
        );
    }
}
