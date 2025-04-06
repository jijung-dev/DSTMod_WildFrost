using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Boulder : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("boulder", "Boulder")
                .SetStats(null, null, 0)
                .SetSprites("Boulder.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 3),
                        SStack("When Destroyed Gain Rock To Chest", 2),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                })
        );
    }
}
