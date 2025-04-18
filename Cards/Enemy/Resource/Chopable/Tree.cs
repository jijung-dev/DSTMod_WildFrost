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
                .SetCardSprites("Tree.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Scrap", 3), SStack("When Destroyed Gain Wood To Chest", 2) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
