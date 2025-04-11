using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Krampus : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("krampus", "Krampus")
                .SetCardSprites("Krampus.png", "Wendy_BG.png")
                .SetStats(2, null, 4)
                .WithCardType("Enemy")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Card Played Destroy Rightmost Card In Hand", 1),
                        SStack("On Turn Escape To Self", 1),
                    };
                })
        );
    }
}
