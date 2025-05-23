using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class RedMushtree : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("redMushtree", "Red Mushtree")
                .SetStats(null, null, 0)
                .SetCardSprites("RedMushtree.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 2),
                        SStack("When Destroyed Gain Wood To Chest", 1),
                        SStack("Gain Red Cap When Destroyed", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
