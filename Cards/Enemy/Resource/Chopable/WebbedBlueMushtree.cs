using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class WebbedBlueMushtree : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("webbedBlueMushtree", "Webbed Blue Mushtree")
                .SetStats(null, null, 0)
                .SetSprites("WebbedBlueMushtree.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 3),
                        SStack("When Destroyed Gain Wood To Chest", 1),
                        SStack("Gain Blue Cap When Destroyed", 1),
                        SStack("When Hit Summon Dangling Depth Dweller", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
