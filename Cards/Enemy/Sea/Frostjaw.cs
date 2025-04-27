using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Frostjaw : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("frostjaw", "Frostjaw")
                .SetBossSprites("Frostjaw.png", "Wendy_BG.png")
                .WithText("Dive Down <hiddencard=dstmod.frostjawDived>".Process())
                .SetStats(20, 3, 5)
                .WithCardType("Miniboss")
                .WithValue(20 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 2) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Spark", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Hit All Enemies", 1),
                        SStack("On Turn Apply Dive Down Frostjaw To Self", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("frostjawDived", "Frostjaw Dived")
                .SetCardSprites("FrostjawDived.png", "Wendy_BG.png")
                .WithText("Dive Up <hiddencard=dstmod.frostjaw>".Process())
                .SetStats(null, null, 2)
                .WithCardType("Miniboss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Chest Health", 1),
                        SStack("Dive Up Frostjaw", 1),
                        SStack("On Turn Apply Reduce Chest Health To Self", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Dive Down Frostjaw")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.killSelfWhenApplied = true;
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("frostjawDived");
                    data.animation = new Scriptable<CardAnimationPing>();
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Dive Up Frostjaw")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("frostjaw");
                    data.animation = new Scriptable<CardAnimationPing>();
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Turn Apply Dive Down Frostjaw To Self")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("divedown") };
                    data.doPing = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Dive Down Frostjaw");
                })
        );
    }
}
