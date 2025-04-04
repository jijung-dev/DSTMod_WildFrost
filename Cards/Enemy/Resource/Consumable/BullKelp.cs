using Deadpan.Enums.Engine.Components.Modding;

public class BullKelp : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("bullKelp", "Bull Kelp")
                .SetStats(2, null, 0)
                .SetSprites("BullKelp.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .WithValue(1 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Kelp Fronds When Destroyed", 1) };
                })
        );
    }
}
