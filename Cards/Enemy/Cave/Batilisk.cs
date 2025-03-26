using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Batilisk : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("batilisk", "Batilisk")
                .WithText("Take <2><keyword=health>")
                .SetSprites("Batilisk.png", "Wendy_BG.png")
                .SetStats(6, 2, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] {SStack("Gain Monster Meat When Destroyed", 1)};
                    data.traits = new List<CardData.TraitStacks>() { TStack("Aimless", 1)};
                })
        );
    }
}
