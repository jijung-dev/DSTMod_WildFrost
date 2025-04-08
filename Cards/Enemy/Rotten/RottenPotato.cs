using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class RottenPotato : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("rottenPotato", "Rotten Giant Potato")
                .SetCardSprites("RottenPotato.png", "Wendy_BG.png")
                .WithText("Destroy Self")
                .SetStats(4, null, 5)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Potato When Destroyed", 1),
                        SStack("Destroy Self After Counter Turn", 1),
                        SStack("When Destroyed Apply Shroom To Random Enemy", 2),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("When Destroyed Apply Spice To Allies", "When Destroyed Apply Shroom To Random Enemy")
                .WithText("When destroyed, apply <{a}><keyword=shroom> to enemies ")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Shroom");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomEnemy;
                })
        );
    }
}
