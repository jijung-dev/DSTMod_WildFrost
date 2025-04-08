using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class TallStalagmite : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("tallStalagmite", "Tall Stalagmite")
                .SetStats(null, null, 0)
                .SetCardSprites("TallStalagmite.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 4),
                        SStack("When Destroyed Gain Rock To Chest", 2),
                        SStack("When Destroyed Gain Gold To Chest", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                })
        );
    }
}
