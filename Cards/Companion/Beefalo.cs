using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Beefalo : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("beefalo", "Beefalo")
                .SetCardSprites("Beefalo.png", "Wendy_BG.png")
                .WithPools("GeneralUnitPool")
                .SetStats(8, 3, 5)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Knockback", 1), TStack("Pigheaded", 1) };
                })
        );
    }
}
