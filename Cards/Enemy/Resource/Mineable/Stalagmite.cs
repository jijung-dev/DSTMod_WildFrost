using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Stalagmite : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("stalagmite", "Stalagmite")
                .SetStats(null, null, 0)
                .SetSprites("Stalagmite.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceMineable", 1),
                        SStack("When Destroyed By Pickaxe Gain Rock", 1),
                        SStack("When Destroyed By Pickaxe Gain Gold", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                })
        );
    }
}
