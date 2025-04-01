using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Boomshroom : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("boomshroom", "Boomshroom")
                .SetSprites("Boomshroom.png", "Wendy_BG.png")
                .SetStats(1, null, 3)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.cardType = TryGet<CardType>("Clunker");
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Destroy Self After Counter Turn", 1),
                        SStack("Cannot Recall", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Kaboom", 1) };
                })
        );
    }
}
