using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class GoldOre : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("goldOre", "Gold Ore")
                .SetStats(null, null, 0)
                .SetSprites("GoldOre.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(4 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceMineable", 2),
                        SStack("When Destroyed By Pickaxe Gain Gold", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                })
        );
    }
}
