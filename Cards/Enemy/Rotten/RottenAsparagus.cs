using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class RottenAsparagus : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("rottenAsparagus", "Rotten Giant Asparagus")
                .SetSprites("RottenAsparagus.png", "Wendy_BG.png")
                .SetStats(2, null, 4)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Asparagus When Destroyed", 1),
                        SStack("On Turn Boost Random Ally Effect", 1),
                    };
                })
        );
    }
}
