using Deadpan.Enums.Engine.Components.Modding;

public class BerryBush : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("berryBush", "Berry Bush")
                .SetStats(2, null, 0)
                .SetCardSprites("BerryBush.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .WithValue(1 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Berries When Destroyed", 1) };
                })
        );
    }
}
