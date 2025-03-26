using Deadpan.Enums.Engine.Components.Modding;

public class Cactus : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("cactus", "Cactus")
                .SetStats(3, null, 0)
                .SetSprites("Cactus.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .WithValue(1 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Teeth", 1), SStack("Gain Cactus Flesh When Destroyed", 1) };
                })
        );
    }
}
