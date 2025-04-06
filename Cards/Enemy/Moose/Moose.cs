using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Moose : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("moose", "Moose")
                .SetSprites("Moose.png", "Wendy_BG.png")
                .SetStats(20, 2, 5)
                .WithCardType("Boss")
                .WithValue(18 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Trigger When Mosling Is Hit", 1),
                        SStack("Apply Wetness To All Item In Hand When Mosling Is Hit", 1),
                        SStack("When Destroyed Mosling Gain Barrage", 1),
                        SStack("When Destroyed Mosling Increase Attack", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenCertainAllyIsHit>("Trigger When Mosling Is Hit")
                .WithText("Trigger when <card=dstmod.mosling> is hit".Process())
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.isReaction = true;
                        data.stackable = false;
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCertainAllyIsHit>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Trigger");
                    data.ally = TryGet<CardData>("mosling");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenCertainAllyIsHit>("Apply Wetness To All Item In Hand When Mosling Is Hit")
                .WithText("and apply <{a}><keyword=dstmod.wetness> to items in hand".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCertainAllyIsHit>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.EnemyHand | StatusEffectApplyX.ApplyToFlags.Hand;
                    data.effectToApply = TryGet<StatusEffectData>("Wetness");
                    data.ally = TryGet<CardData>("mosling");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Mosling Gain Barrage")
                .WithText("<keyword=dstmod.enraged2> <card=dstmod.mosling> when destroyed".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.effectToApply = TryGet<StatusEffectData>("Temporary Barrage");
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("moslingOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Mosling Increase Attack")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.effectToApply = TryGet<StatusEffectData>("Increase Attack");
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("moslingOnly") };
                })
        );
    }
}
