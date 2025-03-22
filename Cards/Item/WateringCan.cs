using Deadpan.Enums.Engine.Components.Modding;

public class WateringCan : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("wateringCan", "Watering Can")
                .SetSprites("WateringCan.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SetTraits(TStack("Noomlin", 1), TStack("Barrage", 1))
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 1) };
                })
        );
    }
}
