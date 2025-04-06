using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class GreenMushtree : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("greenMushtree", "Green Mushtree")
                .SetStats(null, null, 0)
                .SetSprites("GreenMushtree.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 3),
                        SStack("When Destroyed Gain Wood To Chest", 1),
                        SStack("Gain Green Cap When Destroyed", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
