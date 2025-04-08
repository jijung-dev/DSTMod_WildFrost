using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;
using static Ext;

public class ResourceRequired : DataBase
{
    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Rabbit")
                .WithText("Require <{a}> <card=dstmod.rabbit>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
                    data.type = "dst.require";
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("require") };
                    data.requireType = NoTargetTypeExt.RequireRabbit;
                    data.requireCard = TryGet<CardData>("rabbit");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Rock")
                .WithText("Require <{a}><keyword=tgestudio.wildfrost.dstmod.rock><hiddencard=dstmod.chest>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
                    data.type = "dst.require";
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("require") };
                    data.requireType = NoTargetTypeExt.RequireRock;
                    data.removeEffect = TryGet<StatusEffectData>("Rock");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Wood")
                .WithText("Require <{a}><keyword=tgestudio.wildfrost.dstmod.wood><hiddencard=dstmod.chest>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
                    data.type = "dst.require";
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("require") };
                    data.requireType = NoTargetTypeExt.RequireWood;
                    data.removeEffect = TryGet<StatusEffectData>("Wood");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Gold")
                .WithText("Require <{a}><keyword=tgestudio.wildfrost.dstmod.gold><hiddencard=dstmod.chest>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
                    data.type = "dst.require";
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("require") };
                    data.requireType = NoTargetTypeExt.RequireGold;
                    data.removeEffect = TryGet<StatusEffectData>("Gold");
                })
        );
        assets.Add(
            StatusCopy("Lose Scrap", "Lose Require Wood")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantLoseX>(data =>
                {
                    data.statusToLose = TryGet<StatusEffectData>("Require Wood");
                })
        );
        assets.Add(
            StatusCopy("Lose Scrap", "Lose Require Rock")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantLoseX>(data =>
                {
                    data.statusToLose = TryGet<StatusEffectData>("Require Rock");
                })
        );
        assets.Add(
            StatusCopy("Lose Scrap", "Lose Require Gold")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantLoseX>(data =>
                {
                    data.statusToLose = TryGet<StatusEffectData>("Require Gold");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenHitUnNullable>("When Hit By Axe Dies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHitUnNullable>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Kill");
                    data.attackerConstraints = new TargetConstraint[] { TryGetConstraint("axeOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenHitUnNullable>("When Hit By Pickaxe Dies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHitUnNullable>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Kill");
                    data.attackerConstraints = new TargetConstraint[] { TryGetConstraint("pickaxeOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Gain Rock To Chest")
                .WithText("Drop <{a}><keyword=dstmod.rock><hiddencard=dstmod.chest>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.doPing = true;
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("drop2") };
                    data.canBeBoosted = false;
                    data.targetMustBeAlive = false;
                    data.effectToApply = TryGet<StatusEffectData>("Rock");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Self | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("chestOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Gain Gold To Chest")
                .WithText("Drop <{a}><keyword=dstmod.gold><hiddencard=dstmod.chest>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.doPing = true;
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("drop2") };
                    data.canBeBoosted = false;
                    data.targetMustBeAlive = false;
                    data.effectToApply = TryGet<StatusEffectData>("Gold");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Self | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("chestOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Gain Wood To Chest")
                .WithText("Drop <{a}><keyword=dstmod.wood><hiddencard=dstmod.chest>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.doPing = true;
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("drop2") };
                    data.canBeBoosted = false;
                    data.targetMustBeAlive = false;
                    data.effectToApply = TryGet<StatusEffectData>("Wood");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Self | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("chestOnly") };
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("require")
                .WithDescription("\"Require\" resources in Chest or card in hand to play, remove the effect/card afterwards")
                .WithTitleColour(new Color(0.2588235f, 0.06666667f, 0.1294118f, 1f))
                .WithBodyColour(new Color(0.4313726f, 0.2f, 0.1921569f, 1f))
                .WithPanelColour(new Color(0.9058824f, 0.8274511f, 0.6784314f, 0.9411765f))
                .WithNoteColour(new Color(0.4313726f, 0.2f, 0.1921569f, 1f))
                .WithIconTint(new Color(1f, 0.792156862745098f, 0.3411764705882353f))
                .WithCanStack(false)
                .WithShowIcon(false)
                .WithShow(false)
                .SubscribeToAfterAllBuildEvent(data => data.panelSprite = TryGet<KeywordData>("Active").panelSprite)
        );
    }
}
