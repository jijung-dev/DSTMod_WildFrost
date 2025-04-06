using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Deerclops : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("deerclops", "Deerclops")
                .SetSprites("Deerclops.png", "Wendy_BG.png")
                .SetStats(30, 4, 4)
                .SetTraits(TStack("Knockback", 1))
                .WithCardType("Boss")
                .WithValue(18 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 2), SStack("Sanity", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("ImmuneToSnow", 1) };
                })
        );
    }
}
