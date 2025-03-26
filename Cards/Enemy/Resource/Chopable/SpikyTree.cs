using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class SpikyTree : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spikyTree", "Spiky Tree")
                .SetStats(null, null, 0)
                .SetSprites("SpikyTree.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceChopable", 2),
                        SStack("When Destroyed By Axe Gain Wood", 1),
                        SStack("Teeth", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }
}
