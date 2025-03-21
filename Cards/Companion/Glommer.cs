using Deadpan.Enums.Engine.Components.Modding;

public class Glommer : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("glommer", "Glommer")
                .SetSprites("Glommer.png", "Wendy_BG.png")
                .SetStats(3, null, 6)
                .WithCardType("Friendly")
                .SetStartWithEffect(SStack("On Turn Add Attack To Allies", 2))
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.unitPool);
                })
        );
    }
}
