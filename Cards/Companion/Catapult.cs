using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Catapult : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("catapult", "Catapult")
                .SetCardSprites("Catapult.png", "Wendy_BG.png")
                .SetStats(null, 4, 0)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Tape When Destroyed", 1),
                        SStack("Immune To Summoned", 1),
                        SStack("Scrap", 2),
                    };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade("CrownCursed") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("catapultnorequired", "Catapult")
                .SetCardSprites("Catapult.png", "Wendy_BG.png")
                .SetStats(null, 4, 0)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Immune To Summoned", 1), SStack("Scrap", 3) };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade("CrownCursed") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("tape", "Trusty Tape")
                .SetCardSprites("Tape.png", "Wendy_BG.png")
                .WithCardType("Item")
                .FreeModify(
                    delegate (CardData data)
                    {
                        data.playOnSlot = true;
                        data.canPlayOnEnemy = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Summon Catapult", 1),
                        SStack("Require Rock", 1),
                        SStack("Require Wood", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Catapult")
                .WithText("Summon <card=dstmod.catapult>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.setCardType = TryGet<CardType>("Clunker");
                    data.summonCard = TryGet<CardData>("catapult");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("Gain Tape When Destroyed")
                .WithText("Drop <card=dstmod.tape>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("drop") };
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Gain Tape");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Tape")
                .FreeModify(
                    delegate (StatusEffectData data)
                    {
                        data.stackable = false;
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantGainCard>(
                    delegate (StatusEffectInstantGainCard data)
                    {
                        data.cardGain = TryGet<CardData>("tape");
                    }
                )
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectTriggerWhenCertainAllyAttacks>("Trigger When Winona Attacks")
                .WithText("Trigger when <card=dstmod.winona> attacks".Process())
                .WithCanBeBoosted(false)
                .FreeModify(data =>
                {
                    data.isReaction = true;
                    data.stackable = false;
                })
                .SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenCertainAllyAttacks>(data =>
                {
                    data.allyInRow = false;
                    data.ally = TryGet<CardData>("winona");
                })
        );
    }

    protected override void CreateFinalSwapAsset()
    {
        var scripts = new List<CardScript>
        {
            new Scriptable<CardScriptAddPassiveEffect>(r =>
            {
                r.effect = TryGet<StatusEffectData>("Trigger When Winona Attacks");
                r.countRange = new Vector2Int(1, 1);
            }),
        };
        finalSwapAsset = (TryGet<CardData>("catapultnorequired"), scripts.ToArray());
    }
}
