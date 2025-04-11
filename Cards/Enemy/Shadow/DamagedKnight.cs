using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class DamagedKnight : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("damagedKnight", "Damaged Knight")
                .SetCardSprites("DamagedKnight.png", "Wendy_BG.png")
                .SetStats(10, 4, 3)
                .WithCardType("Enemy")
                .WithValue(5 * 36)
        );
    }
}
