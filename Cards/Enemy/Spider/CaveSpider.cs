using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class CaveSpider : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("caveSpider", "Cave Spider")
                .SetCardSprites("CaveSpider.png", "Wendy_BG.png")
                .SetStats(10, 2, 3)
                .WithCardType("Enemy")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("MultiHit", 1),
                        SStack("Gain Monster Meat When Destroyed", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Frontline", 1) };
                })
        );
    }
}
