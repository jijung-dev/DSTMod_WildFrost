using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class DamagedRook : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("damagedRook", "Damaged Rook")
                .SetCardSprites("DamagedRook.png", "Wendy_BG.png")
                .SetStats(10, 3, 4)
                .WithCardType("Enemy")
                .WithValue(5 * 36)
                .SetTraits(TStack("Barrage", 1))
        );
    }
}
