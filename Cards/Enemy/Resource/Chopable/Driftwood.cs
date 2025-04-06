using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Driftwood : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("driftwood", "Driftwood")
                .SetStats(null, null, 0)
                .SetSprites("Driftwood.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 2),
                        SStack("When Destroyed Gain Wood To Chest", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
