using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Axe : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("axe", "Axe")
                .SetStats(null, 2, 0)
                .SetSprites("Axe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("AxeType", 1) };
                    }
                )
        );
    }
}
