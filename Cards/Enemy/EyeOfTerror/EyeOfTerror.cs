using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class EyeOfTerror : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("eyeOfTerror", "Eye Of Terror")
                .SetBossSprites("EyeOfTerror.png", "Wendy_BG.png")
                .WithText("Take 1<keyword=health>")
                .SetStats(15, 0, 3)
                .WithCardType("Miniboss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Health Lost Summon Egg Of Terror", 1),
                        SStack("Eye Of Terror Enraged", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("eyeOfTerrorEnraged", "Eye Of Terror Enraged")
                .SetBossSprites("EyeOfTerrorEnraged.png", "Wendy_BG.png")
                .WithText("Take 1<keyword=health>")
                .SetStats(15, 1, 3)
                .WithCardType("Miniboss")
                .WithValue(15 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("MultiHit", 1),
                        SStack("When Health Lost Summon Egg Of Terror", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXEveryYHealthLost>("When Health Lost Summon Egg Of Terror")
                .WithText("When every 5<keyword=health> is lost, summon <card=dstmod.eggOfTerror>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXEveryYHealthLost>(data =>
                {
                    data.healthLost = 5;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Egg Of Terror");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "Eye Of Terror Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("eyeOfTerrorEnraged");
                })
        );
    }
}
