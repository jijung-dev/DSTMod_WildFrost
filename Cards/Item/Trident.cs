using Deadpan.Enums.Engine.Components.Modding;

public class Trident : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("trident", "Strident Trident")
                .SetStats(null, 2, 0)
                .SetSprites("Trident.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SetStartWithEffect(SStack("Hit All Enemies", 1))
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.needsTarget = false;
                })
        );
    }
}
