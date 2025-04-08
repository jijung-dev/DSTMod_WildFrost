using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Vargling : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("vargling", "Vargling")
                .IsPet("", true)
                .SetCardSprites("Vargling.png", "Wendy_BG.png")
                .SetStartWithEffect(SStack("MultiHit", 1))
                .SetStats(5, 2, 3)
                .WithCardType("Friendly")
        );
    }
}
