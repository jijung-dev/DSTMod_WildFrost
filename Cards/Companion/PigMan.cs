using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class PigMan : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("pigman", "Pig")
                .SetSprites("Pig.png", "Wendy_BG.png")
                .SetStats(6, 4, 0)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.unitPool);
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1), TStack("Pigheaded", 1) };
                })
        );
    }
}
