using Deadpan.Enums.Engine.Components.Modding;

public class Fern : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("fern", "Fern")
                .SetSprites("Fern.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .SetStats(1, null, 0)
                .WithValue(1 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Foliage When Destroyed", 1) };
                })
        );
    }
}
