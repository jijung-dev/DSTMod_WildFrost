using Deadpan.Enums.Engine.Components.Modding;

public class MysteriousPlant : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("mysteriousPlant", "Mysterious Plant")
                .SetCardSprites("MysteriousPlant.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .SetStats(1, null, 0)
                .WithValue(1 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.cardType = TryGet<CardType>("Friendly");
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Lesser Glow Berry When Destroyed", 1),
                        SStack("Cannot Recall", 1),
                    };
                })
        );
    }
}
