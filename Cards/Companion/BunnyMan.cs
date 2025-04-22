using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class BunnyMan : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("bunnyman", "Bunnyman")
                .SetCardSprites("Bunnyman.png", "Wendy_BG.png")
                .SetStats(8, 4, 3)
                .WithText("When hit escape to hand and becomes <card=dstmod.bunnymanInjured>".Process())
                .WithPools("GeneralUnitPool")
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[2]
                    {
                        SStack("After Hit Escape", 1),
                        SStack("After Hit Summon Bunnyman Injured In Hand", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("bunnymanInjured", "Bunnyman Injured")
                .SetCardSprites("BunnymanInjured.png", "Wendy_BG.png")
                .SetStats(6, 1, 0)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Turn Increase Health And Damage", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Junk", "Summon Bunnyman Injured")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("bunnymanInjured");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Junk In Hand", "Instant Summon Bunnyman Injured In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Bunnyman Injured");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenHit>("After Hit Escape")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Escape");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenHit>("After Hit Summon BunnymanInjured")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Bunnyman Injured In Hand");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectGainCardWhenHit>("After Hit Summon Bunnyman Injured In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectGainCardWhenHit>(data =>
                {
                    data.cardGain = TryGet<CardData>("bunnymanInjured");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Increase Health And Damage")
                .WithText("Increase <keyword=attack> and <keyword=health> by <{a}>")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Increase Attack & Health");
                })
        );
    }

    protected override void CreateFinalSwapAsset()
    {
        var scripts = new List<CardScript>
        {
            new Scriptable<CardScriptRemovePassiveEffect>(r =>
            {
                r.toRemove = new StatusEffectData[] { TryGet<StatusEffectData>("After Hit Summon Bunnyman Injured In Hand") };
            }),
            new Scriptable<CardScriptAddPassiveEffect>(r =>
            {
                r.effect = TryGet<StatusEffectData>("After Hit Summon BunnymanInjured");
                r.countRange = new Vector2Int(1, 1);
            }),
        };
        finalSwapAsset = (TryGet<CardData>("bunnyman"), scripts.ToArray());
    }
}
