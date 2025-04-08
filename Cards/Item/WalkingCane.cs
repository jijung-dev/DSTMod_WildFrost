using Deadpan.Enums.Engine.Components.Modding;

public class WalkingCane : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("walkingCane", "Walking Cane")
                .SetDamage(1)
                .SetCardSprites("WalkingCane.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Reduce Counter", 2) };
                })
        );
    }
}
