using Deadpan.Enums.Engine.Components.Modding;

public class DreadstoneArmor : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("dreadstoneArmor", "Dreadstone Armor")
                .SetStats(null, 1, 0)
                .SetSprites("DreadstoneArmor.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(60)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Block", 2), SStack("Null", 3) };
                })
        );
    }
}
