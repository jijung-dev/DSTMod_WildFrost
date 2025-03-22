using Deadpan.Enums.Engine.Components.Modding;

public class Spear : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("spear", "Spear")
                .SetSprites("Spear.png", "Wendy_BG.png")
                .SetStats(null, 3, 0)
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.WithPools(mod.itemPool);
                    }
                )
        );
    }
}
