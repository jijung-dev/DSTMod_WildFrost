using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class DepthsWorm : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("depthsWorm", "Depths Worm")
                .SetCardSprites("DepthsWorm.png", "Wendy_BG.png")
                .WithText("Dive down <hiddencard=dstmod.depthsWormDived>".Process())
                .SetStats(10, 4, 3)
                .WithCardType("Enemy")
                .WithValue(6 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Lesser Glow Berry When Destroyed", 1),
                        SStack("On Turn Apply Dive Down Depths Worm To Self", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("depthsWormDived", "Depths Worm Dived")
                .SetCardSprites("DepthsWormDived.png", "Wendy_BG.png")
                .WithText("Dive up <hiddencard=dstmod.depthsWorm>".Process())
                .SetStats(null, null, 3)
                .WithCardType("Enemy")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Chest Health", 1),
                        SStack("Dive Up Depths Worm", 1),
                        SStack("On Turn Apply Reduce Chest Health To Self", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Dive Down Depths Worm")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.killSelfWhenApplied = true;
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("depthsWormDived");
                    data.animation = new Scriptable<CardAnimationPing>();
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Dive Up Depths Worm")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("depthsWorm");
                    data.animation = new Scriptable<CardAnimationPing>();
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Turn Apply Dive Down Depths Worm To Self")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.doPing = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Dive Down Depths Worm");
                })
        );
    }
}
