using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Pickaxe : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("pickaxe", "Pickaxe")
                .SetStats(null, 1, 0)
                .SetSprites("Pickaxe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("PickaxeType", 1) };
                    }
                )
        );
    }
}
