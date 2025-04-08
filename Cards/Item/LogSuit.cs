using Deadpan.Enums.Engine.Components.Modding;

public class LogSuit : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("logSuit", "Log Suit")
                .SetCardSprites("LogSuit.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithPools("GeneralItemPool")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Shell", 5) };
                })
        );
    }
}
