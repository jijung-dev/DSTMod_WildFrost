using Deadpan.Enums.Engine.Components.Modding;

public class MysteriousPlant : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("mysteriousPlant", "Mysterious Plant")
                .SetSprites("MysteriousPlant.png", "Wendy_BG.png")
                .WithCardType("Friendly")
                .SetStats(1, null, 0)
                .WithText("When destroyed gain <card=dstmod.lesserGlowBerry>".Process())
                .WithValue(1 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Lesser Glow Berry When Destroyed", 1) };
                })
        );
    }
}
