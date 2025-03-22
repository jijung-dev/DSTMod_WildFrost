using Deadpan.Enums.Engine.Components.Modding;

public class LogSuit : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("logSuit", "Log Suit")
                .SetSprites("LogSuit.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Shell", 5) };
                })
        );
    }
}
