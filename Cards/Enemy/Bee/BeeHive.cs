using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class BeeHive : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("beehive", "Beehive")
                .SetCardSprites("Beehive.png", "Wendy_BG.png")
                .SetStats(10, null, 0)
                .WithCardType("Enemy")
                .WithValue(4 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Trigger When Ally Bee Is Hit", 1),
                        SStack("Summon Killer Bee PreTrigger", 1),
                        SStack("On Turn Apply Ink To Self", 4),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenUnitIsHit>("Trigger When Ally Bee Is Hit")
                .WithIsReaction(true)
                .WithText("After ally <keyword=dstmod.bee> is hit summon <card=dstmod.killerBee>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenUnitIsHit>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Trigger");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.unitConstraints = new TargetConstraint[] { TryGetConstraint("beeOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXPreTrigger>("Summon Killer Bee PreTrigger")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXPreTrigger>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Killer Bee");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Apply Ink To Self")
                .WithIsReaction(true)
                .WithText("then apply <{a}><keyword=null> to self")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Null");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
    }
}
