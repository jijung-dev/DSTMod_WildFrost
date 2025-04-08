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
                .SetCardSprites("Axe.png", "Wendy_BG.png")
                .WithPools("GeneralItemPool")
                .WithValue(30)
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("AxeType", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("goldenAxe", "Golden Axe")
                .SetStats(null, 2, 0)
                .SetCardSprites("GoldenAxe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("AxeType", 1), TStack("Barrage", 1) };
                    }
                )
        );
    }
}
