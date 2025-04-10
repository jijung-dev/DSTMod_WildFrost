using Deadpan.Enums.Engine.Components.Modding;

public class JuicyBerryBush : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("juicyBerryBush", "Juicy Berry Bush")
                .SetStats(5, null, 0)
                .SetCardSprites("JuicyBerryBush.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Juicy Berries When Destroyed", 3) };
                })
        );
    }
}
