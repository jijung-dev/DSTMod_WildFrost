using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BlueMushtree : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("blueMushtree", "Blue Mushtree")
                .SetStats(null, null, 0)
                .SetSprites("BlueMushtree.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceChopable", 2),
                        SStack("When Destroyed By Axe Gain Wood", 1),
                        SStack("Gain Blue Cap When Destroyed", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
