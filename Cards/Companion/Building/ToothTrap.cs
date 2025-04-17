using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class ToothTrap : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("toothTrap", "Tooth Trap")
                .SetCardSprites("ToothTrap.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithPools("GeneralUnitPool")
                .SetStats(null, 2, 0)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Require Wood", 1),
                        SStack("Scrap", 4),
                        SStack("MultiHit", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1) };
                })
        );
    }
}
