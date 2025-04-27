using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class TwinOfTerror : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("retinazor", "Retinazor")
                .SetBossSprites("Retinazor.png", "Wendy_BG.png")
                .WithText("Take 1<keyword=health>")
                .SetStats(18, 0, 6)
                .WithCardType("Miniboss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Health Lost Summon Egg Of Terror 2", 1),
                        SStack("Retinazor Enraged", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("retinazorEnraged", "Retinazor Enraged")
                .SetBossSprites("RetinazorEnraged.png", "Wendy_BG.png")
                .WithText("Take 1<keyword=health>")
                .SetStats(15, 1, 5)
                .WithCardType("Miniboss")
                .WithValue(11 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("MultiHit", 1),
                        SStack("When Health Lost Summon Egg Of Terror 2", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spazmatism", "Spazmatism")
                .SetBossSprites("Spazmatism.png", "Wendy_BG.png")
                .WithText("Take 1<keyword=health>")
                .SetStats(20, 2, 4)
                .WithCardType("Miniboss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 1), SStack("Spazmatism Enraged", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spazmatismEnraged", "Spazmatism Enraged")
                .SetBossSprites("SpazmatismEnraged.png", "Wendy_BG.png")
                .WithText("Take 1<keyword=health>")
                .SetStats(15, 2, 3)
                .WithCardType("Miniboss")
                .WithValue(12 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 2) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXEveryYHealthLost>("When Health Lost Summon Egg Of Terror 2")
                .WithText("When every 3<keyword=health> is lost, summon <card=dstmod.eggOfTerror>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXEveryYHealthLost>(data =>
                {
                    data.healthLost = 3;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Egg Of Terror");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "Spazmatism Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("spazmatismEnraged");
                })
        );
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "Retinazor Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("retinazorEnraged");
                })
        );
    }
}
