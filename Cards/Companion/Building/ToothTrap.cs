using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class ToothTrap : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("toothTrap", "Tooth Trap")
                .SetSprites("ToothTrap.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .SetStats(null, 1, 0)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.unitPool);
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Require Wood", 1),
                        SStack("Scrap", 2),
                        SStack("MultiHit", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1) };
                })
        );
    }
}
