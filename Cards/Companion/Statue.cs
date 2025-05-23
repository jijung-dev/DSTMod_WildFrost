using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Statue : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("statue", "Statue")
                .SetCardSprites("Statue.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .SetStats(null, null, 0)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Scrap", 2), SStack("When Destroyed Gain Rock To Chest", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Statue In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantGainCard>(data =>
                {
                    data.cardGain = TryGet<CardData>("statue");
                })
        );
    }
}
