using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class PossesedVarg : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("possesedVarg", "Possesed Varg")
                .SetBossSprites("PossesedVarg.png", "Wendy_BG.png")
                .WithText("<keyword=dstmod.alphahound><hiddencard=dstmod.hound><hiddencard=dstmod.redHound><hiddencard=dstmod.blueHound>".Process())
                .SetStats(40, 5, 4)
                .WithCardType("BossSmall")
                .WithValue(20 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("MultiHit", 1),
                        SStack("On Counter Turn Summon Hounds", 1),
                        SStack("ImmuneToSnow", 1),
                        SStack("When Deployed Fill Board With Hounds", 1),
                        SStack("When Destroyed Kill All Allies", 1),
                        SStack("While Active When Destroyed Summon Horror Hound To Allies", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Aimless", 1), TStack("Wild", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("horrorHound", "Horror Hound")
                .SetCardSprites("HorrorHound.png", "Wendy_BG.png")
                .SetStats(5, 2, 3)
                .WithCardType("Enemy")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Wild", 1) };
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("possesed")
                .WithTitle("Possesed")
                .WithShowName(true)
                .WithDescription("When destroyed summon <card=dstmod.horrorHound>".Process())
                .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("While Active Snow Immune To Allies", "While Active When Destroyed Summon Horror Hound To Allies")
                .WithText("While active <keyword=dstmod.possesed> all <Hounds><hiddencard=dstmod.horrorHound>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("When Destroyed Summon Horror Hound");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.applyConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintIsSpecificCard>(r =>
                        {
                            r.not = true;
                            r.allowedCards = new CardData[] { TryGet<CardData>("horrorHound") };
                        }),
                    };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Summon Horror Hound")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Horror Hound");
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Horror Hound")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("horrorHound");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Horror Hound")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Horror Hound");
                })
        );
        assets.Add(
            StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Board With Hounds")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Fill Board With Hounds");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Board With Hounds")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("hound"), TryGet<CardData>("blueHound"), TryGet<CardData>("redHound") };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillSlots>("Fill Slot Possesed Varg")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("possesedVarg") };
                    int[] ran = new int[] { 8, 9, 10, 12, 13, 14 };
                    data.slotID = ran[UnityEngine.Random.Range(0, ran.Length)];
                })
        );
    }
}
