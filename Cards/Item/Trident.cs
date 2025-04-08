using Deadpan.Enums.Engine.Components.Modding;

public class Trident : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("trident", "Strident Trident")
                .SetStats(null, 2, 0)
                .SetCardSprites("Trident.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SetStartWithEffect(SStack("Hit All Enemies", 1))
                .WithPools("GeneralItemPool")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.needsTarget = false;
                })
        );
    }
}
