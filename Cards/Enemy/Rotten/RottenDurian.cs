using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class RottenDurian : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("rottenDurian", "Rotten Giant Durian")
                .SetSprites("RottenDurian.png", "Wendy_BG.png")
                .SetStats(8, null, 0)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Durian When Destroyed", 1), SStack("Teeth", 2) };
                })
        );
    }
}
