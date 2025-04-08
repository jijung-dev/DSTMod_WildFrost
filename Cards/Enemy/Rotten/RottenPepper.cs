using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class RottenPepper : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("rottenPepper", "Rotten Giant Pepper")
                .SetCardSprites("RottenPepper.png", "Wendy_BG.png")
                .SetStats(2, 0, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Pepper When Destroyed", 1),
                        SStack("On Turn Apply Overheat To RandomEnemy", 1),
                        SStack("When Destroyed Apply Spice To Allies", 2),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("On Turn Apply Demonize To RandomEnemy", "On Turn Apply Overheat To RandomEnemy")
                .WithText("Apply <{a}><keyword=dstmod.overheat> to a random enemy".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Overheat");
                })
        );
    }
}
