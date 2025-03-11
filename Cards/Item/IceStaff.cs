using Deadpan.Enums.Engine.Components.Modding;

public class IceStaff : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("iceStaff", "Ice Staff")
                .SetSprites("IceStaff.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Snow", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Card Played Increase Snow", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Increase Snow")
                .WithText("Increase <keyword=snow> by 1 when played")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Increase Effects");
                })
        );
    }
}
