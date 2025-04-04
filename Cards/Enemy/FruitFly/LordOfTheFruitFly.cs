using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class LordOfTheFruitFly : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("lordFruitFly", "Lord of the Fruit Flies")
                .SetSprites("LordOfTheFruitFlies.png", "Wendy_BG.png")
                .SetStats(20, 1, 3)
                .WithCardType("Miniboss")
                .WithValue(15 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Shroom", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Hit All Enemies", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("fruitFly", "Fruit Fly")
                .SetSprites("FruitFly.png", "Wendy_BG.png")
                .WithText("Double the target's <keyword=shroom>")
                .SetStats(6, 1, 3)
                .WithCardType("Enemy")
                .WithValue(4 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Double Shroom", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Aimless", 1) };
                })
        );
    }
}
