using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Rabbit : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("rabbit", "Rabbit")
                .SetSprites("Rabbit.png", "Wendy_BG.png")
                .SetStats(2, null, 0)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Morsel When Destroyed", 1),
                        SStack("Immune To Summoned", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Rabbit In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantGainCard>(data =>
                {
                    data.cardGain = TryGet<CardData>("rabbit");
                })
        );
    }
}
