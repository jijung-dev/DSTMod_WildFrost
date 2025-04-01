using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Dragonfly : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("dragonfly", "Dragonfly")
                .SetSprites("Dragonfly.png", "Wendy_BG.png")
                .SetStats(30, 3, 4)
                .WithCardType("Boss")
                .WithValue(13 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Dragonfly Enraged", 1),
                        SStack("When Health Lost Summon Lavae", 1),
                        SStack("Immune To Overheat", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("dragonflyEnraged", "Dragonfly Enraged")
                .SetSprites("DragonflyEnraged.png", "Wendy_BG.png")
                .SetStats(25, 1, 5)
                .WithCardType("Boss")
                .WithValue(13 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Overheat", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Hit All Units", 1),
                        SStack("When Overheat Applied To Self Gain Spice Instead", 1),
                        SStack("Immune To Damage From DFly", 1),
                        SStack("MultiHit", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXEveryYHealthLost>("When Health Lost Summon Lavae")
                .WithText("When every <5><keyword=health> is lost, summon <card=dstmod.lavae>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXEveryYHealthLost>(data =>
                {
                    data.healthLost = 5;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Lavae");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmuneToDamageFromCertainCard>("Immune To Damage From DFly")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmuneToDamageFromCertainCard>(data =>
                {
                    data.notAllowedCards = new TargetConstraint[] { TryGetConstraint("noDfly") };
                })
        );
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "Dragonfly Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("dragonflyEnraged");
                })
        );
        assets.Add(
            StatusCopy("When Shell Applied To Self Gain Spice Instead", "When Overheat Applied To Self Gain Spice Instead")
                .WithText("When <keyword=dstmod.overheat>'d, gain 2X<keyword=spice> instead".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(data =>
                {
                    data.whenAppliedTypes = new string[] { "dst.overheat" };
                    data.effectToApply = TryGet<StatusEffectData>("Spice");
                    data.adjustAmount = true;
                    data.multiplyAmount = 2;
                })
        );
    }
}
