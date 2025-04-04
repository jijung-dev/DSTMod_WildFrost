using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Frog : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("frog", "Frog")
                .SetSprites("Frog.png", "Wendy_BG.png")
                .SetStats(4, 1, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] 
                    { 
                        SStack("On Turn Apply Wetness To Random Card In Hand", 1),
                        SStack("Gain Frog Legs When Destroyed", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Apply Wetness To Random Card In Hand")
                .WithText("Apply <{a}><keyword=dstmod.wetness> to random item in hand".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomCardInHand;
                    data.effectToApply = TryGet<StatusEffectData>("Wetness");
                    data.applyConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsItem>() };
                })
        );
    }
}
