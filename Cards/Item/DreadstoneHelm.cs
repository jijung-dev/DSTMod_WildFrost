using Deadpan.Enums.Engine.Components.Modding;

public class DreadstoneHelm : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("dreadstoneHelm", "Dreadstone Helm")
                .SetStats(null, 1, 0)
                .SetSprites("DreadstoneHelmet.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(75)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Block", 1), SStack("Null", 3) };
                    data.traits = new System.Collections.Generic.List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
    }
}
