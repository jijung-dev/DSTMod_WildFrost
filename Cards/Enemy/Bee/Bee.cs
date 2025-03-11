using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Bee : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("bee", "Bee")
                .SetSprites("Bee.png", "Wendy_BG.png")
                .SetStats(4, 2, 2)
                .WithCardType("Enemy")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Bee", 1) };
                })
        );
    }
}
