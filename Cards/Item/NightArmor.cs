using Deadpan.Enums.Engine.Components.Modding;

public class NightArmor : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("nightArmor", "Night Armor")
                .SetStats(null, null, 0)
                .SetSprites("NightArmor.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(60)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 4), SStack("Shell", 6) };
                })
        );
    }
}
