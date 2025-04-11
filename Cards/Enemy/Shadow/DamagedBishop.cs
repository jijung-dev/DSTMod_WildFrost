using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class DamagedBishop : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("damagedBishop", "Damaged Bishop")
                .SetCardSprites("DamagedBishop.png", "Wendy_BG.png")
                .SetStats(6, 2, 2)
                .WithCardType("Enemy")
                .WithValue(5 * 36)
                .SetTraits(TStack("Longshot", 1))
        );
    }
}
