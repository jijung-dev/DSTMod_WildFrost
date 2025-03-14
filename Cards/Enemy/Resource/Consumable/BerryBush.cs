using Deadpan.Enums.Engine.Components.Modding;

public class BerryBush : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("berryBush", "Berry Bush")
                .SetStats(2, null, 0)
                .SetSprites("BerryBush.png", "Wendy_BG.png")
                .WithText("When destroyed gain <card=dstmod.berries>".Process())
                .WithCardType("Friendly")
                .WithValue(1 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Berries When Destroyed", 1) };
                })
        );
    }
}
