using System;
using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class CrystalDeerclops : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("crystalDeerclops", "Crystal Deerclops")
                .SetBossSprites("CrystalDeerclops.png", "Wendy_BG.png")
                .WithText("Apply <keyword=dstmod.froze>".Process())
                .SetStats(40, 5, 8)
                .WithCardType("BossSmall")
                .WithValue(20 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Temporary Froze", 1), SStack("Sanity", 3) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ImmuneToSnow", 1),
                        SStack("Immune To Freeze", 1),
                        SStack("Trigger Against When Freezing Applied", 1),
                        SStack("When Deployed Fill Board With Ice Hounds", 1),
                        SStack("When Destroyed Kill All Allies", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("iceHound", "Ice Hound")
                .SetCardSprites("HoundIce.png", "Wendy_BG.png")
                .SetStats(6, 2, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Monster Meat When Destroyed", 1),
                        SStack("MultiHit", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Wild", 1), TStack("Aimless", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Trigger Against When Weakness Applied", "Trigger Against When Freezing Applied")
                .WithText("Trigger against enemies that is hit with <keyword=dstmod.freeze>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenStatusApplied>(data =>
                {
                    data.friendlyFire = false;
                    data.targetStatus = TryGet<StatusEffectData>("Freezing");
                })
        );
        assets.Add(
            StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Board With Ice Hounds")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Fill Board With Ice Hounds");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Board With Ice Hounds")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("hound"), TryGet<CardData>("iceHound") };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillSlots>("Fill Slot Crystal Deerclops")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("crystalDeerclops") };
                    int[] ran = new int[] { 8, 9, 10, 12, 13, 14 };
                    data.slotID = ran[UnityEngine.Random.Range(0, ran.Length)];
                })
        );
    }
}
