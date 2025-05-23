using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class PigMan : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("pigman", "Pig")
                .SetCardSprites("Pig.png", "Wendy_BG.png")
                .WithPools("GeneralUnitPool")
                .SetStats(6, 4, 0)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1), TStack("Pigheaded", 1) };
                })
        );
    }
}
