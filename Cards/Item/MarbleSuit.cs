using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class MarbleSuit : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("marbleSuit", "Marble Suit")
                .SetStats(null, null, 0)
                .SetSprites("MarbleSuit.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Block", 3) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Require Rock", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                })
        );
    }
}
