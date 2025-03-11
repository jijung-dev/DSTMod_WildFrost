using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Tree : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("tree", "Tree")
                .SetStats(null, null, 0)
                .SetSprites("Tree.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(4 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceChopable", 3),
                        SStack("When Destroyed By Axe Gain Wood", 2),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
