using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Beefalo : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("beefalo", "Beefalo")
                .SetSprites("Beefalo.png", "Wendy_BG.png")
                .SetTraits(TStack("Knockback", 1), TStack("Pigheaded", 1))
                .SetStats(8, 3, 5)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.unitPool);
                    data.traits = new List<CardData.TraitStacks>() { TStack("Knockback", 1), TStack("Pigheaded", 1) };
                })
        );
    }
}
