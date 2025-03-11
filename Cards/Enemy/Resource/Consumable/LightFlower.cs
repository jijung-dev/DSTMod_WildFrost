using Deadpan.Enums.Engine.Components.Modding;

public class LightFlower : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("lightFlower", "Light Flower")
                .SetSprites("LightFlower.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .SetStats(1, null, 0)
                .WithText("When destroyed gain <card=dstmod.lightBulb>".Process())
                .WithValue(1 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Light Bulb When Destroyed", 1) };
                })
        );
    }
}
