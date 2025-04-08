using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Gunpowder : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("gunpowder", "Gunpowder")
                .SetSprites("Gunpowder.png", "Wendy_BG.png")
                .WithText("Destroy Self")
                .WithPools("GeneralUnitPool")
                .WithCardType("Clunker")
                .SetStats(null, null, 3)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Explode", 5) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 2),
                        SStack("Destroy Self After Counter Turn", 1),
                        SStack("Temporary Froze", 1),
                        SStack("ByPassHasHealthConstraint", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect() { }
}
