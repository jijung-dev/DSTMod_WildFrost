using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class KittyKit : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("kittykit", "Kittykit")
                .IsPet("", true)
                .SetSprites("Kittykit.png", "Wendy_BG.png")
                .WithText("Gain 3-6<keyword=blings>")
                .SetTraits(TStack("Greed", 1))
                .SetAttackEffect(SStack("Gain Gold Range (3-6)", 1))
                .SetStats(5, 1, 3)
                .WithCardType("Friendly")
        );
    }
}
