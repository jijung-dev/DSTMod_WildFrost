using Deadpan.Enums.Engine.Components.Modding;

public class FootballHelmet : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("footballHelmet", "Football Helmet")
                .SetStats(null, null, 0)
                .SetCardSprites("FootballHelmet.png", "Wendy_BG.png")
                .WithPools("GeneralItemPool")
                .WithCardType("Item")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Shell", 3) };
                    data.traits = new System.Collections.Generic.List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
    }
}
