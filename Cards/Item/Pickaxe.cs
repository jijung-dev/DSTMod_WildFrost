using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Pickaxe : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("pickaxe", "Pickaxe")
                .SetStats(null, 2, 0)
                .SetSprites("Pickaxe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithPools("GeneralItemPool")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("PickaxeType", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("goldenPickaxe", "Golden Pickaxe")
                .SetStats(null, 2, 0)
                .SetSprites("GoldenPickaxe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("PickaxeType", 1), TStack("Barrage", 1) };
                    }
                )
        );
    }
}
