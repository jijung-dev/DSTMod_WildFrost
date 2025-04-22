using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Wortox : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("wortox", "Wortox")
                .WithCardType("Leader")
                .SetLeaderSprites("Wortox.png", "Wendy_BG.png")
                .SetStats(10, 4, 4)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Enemy Is Killed Gain Random Soul", 1) };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade(), LeaderExt.AddRandomHealth(0, 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("souls", "Souls")
                .SetCardSprites("Souls.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
                    data.needsTarget = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Card Played Heal To Allies", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("soul", "Soul")
                .SetCardSprites("Soul.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 3) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenCardDestroyed>("When Enemy Is Killed Gain Random Soul")
                .WithText("When an Enemy is killed, Add <card=dstmod.soul> or <card=dstmod.souls> to hand".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(data =>
                {
                    data.descColorHex = "F99C61";
                    data.canBeAlly = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.constraints = new TargetConstraint[] { TryGetConstraint("noChopable"), TryGetConstraint("noMineable") };
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Random Soul In Hand");
                })
        );
        assets.Add(
            StatusCopy("Summon Junk", "Summon Soul")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("soul");
                })
        );
        assets.Add(
            StatusCopy("Summon Junk", "Summon Souls")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("souls");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonRandom>("Instant Summon Random Soul In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                    data.randomCards = new StatusEffectSummon[]
                    {
                        TryGet<StatusEffectSummon>("Summon Soul"),
                        TryGet<StatusEffectSummon>("Summon Souls"),
                    };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Heal To Allies")
                .WithText("Restore {0} to all allies")
                .WithTextInsert("<{a}><keyword=health>")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.effectToApply = TryGet<StatusEffectData>("Heal");
                })
        );
    }

    protected override void CreateFinalSwapAsset()
    {
        var scripts = new List<CardScript>
        {
            new Scriptable<CardScriptAddPassiveEffect>(r =>
            {
                r.effect = TryGet<StatusEffectData>("On Card Played Heal To Allies");
                r.countRange = new Vector2Int(3, 4);
            }),
            new Scriptable<CardScriptRemovePassiveEffect>(r =>
            {
                r.toRemove = new StatusEffectData[] { TryGet<StatusEffectData>("When Enemy Is Killed Gain Random Soul") };
            }),
        };
        finalSwapAsset = (TryGet<CardData>("wortox"), scripts.ToArray());
    }
}
