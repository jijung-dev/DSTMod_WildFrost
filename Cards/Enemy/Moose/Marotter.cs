using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Marotter : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("marotter", "Marotter")
                .SetCardSprites("Marotter.png", "Wendy_BG.png")
                .SetStats(6, 2, 4)
                .WithCardType("Enemy")
                .WithValue(4 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Turn Apply Wetness To Right Most Item In Hand", 1),
                        SStack("Gain Meat When Destroyed", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Apply Wetness To Right Most Item In Hand")
                .WithText("Apply <{a}><keyword=dstmod.wetness> to right most item in hand".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RightCardInHand;
                    data.effectToApply = TryGet<StatusEffectData>("Wetness");
                    data.applyConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsItem>() };
                })
        );
    }
}
