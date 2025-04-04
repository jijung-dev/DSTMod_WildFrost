using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class GildedSeaStack : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("gildedSeaStack", "Gilded Sea Stack")
                .SetStats(null, null, 0)
                .SetSprites("GildedSeaStack.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(3 * 36)
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
