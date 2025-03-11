using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class CaveSpider : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("caveSpider", "Cave Spider")
                .SetSprites("CaveSpider.png", "Wendy_BG.png")
                .SetStats(8, 1, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Frontline", 1), TStack("Monster", 1) };
                })
        );
    }
}
