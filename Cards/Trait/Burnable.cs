using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Burnable : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("burntTree", "Burnt Tree")
                .SetStats(null, null, 0)
                .SetSprites("Stick.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Scrap", 2), SStack("When Destroyed Gain Charcoal", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("charcoal", "Charcoal")
                .SetSprites("Stick.png", "Wendy_BG.png")
                .SetStats(null, null, 0)
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Double Overheat", 1) };
                    }
                )
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("burnable")
                .WithTitle("Burnable")
                .WithShowName(true)
                .WithDescription("When <keyword=dstmod.overheat>'d got burned".Process())
                .WithTitleColour(new Color(0.38f, 0.09f, 0.02f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Become Burnt")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.preventDeath = false;
                    data.nextPhase = TryGet<CardData>("burntTree");
                    data.animation = new Scriptable<CardAnimationShake>();
                    data.byCertainEffect = TryGet<StatusEffectData>("Overheat");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectBypass>("Bypass Overheat")
                .SubscribeToAfterAllBuildEvent<StatusEffectBypass>(data =>
                {
                    data.effect = TryGet<StatusEffectData>("Overheat");
                })
        );
        assets.Add(
            StatusCopy("When Destroyed Add Health To Allies", "When Destroyed Gain Charcoal")
                .WithText("Gain <card=dstmod.charcoal> when destroyed".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.canBeBoosted = false;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Gain Charcoal In Hand");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Charcoal In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantGainCard>(data =>
                {
                    data.cardGain = TryGet<CardData>("charcoal");
                })
        );
        assets.Add(
            StatusCopy("Double Spice", "Double Overheat")
                .WithText("Double the target's <keyword=dstmod.overheat>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantDoubleX>(data =>
                {
                    data.statusToDouble = TryGet<StatusEffectData>("Overheat");
                })
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Burnable")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("burnable");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Bypass Overheat"), TryGet<StatusEffectData>("Become Burnt") };
                })
        );
    }
}
