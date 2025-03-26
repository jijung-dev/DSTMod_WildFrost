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
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("require2") };
                    data.requireType = NoTargetTypeExt.RequireRabbit;
                    data.requireCard = TryGet<CardData>("rabbit");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Rock")
                .WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.rock>")
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("require") };
                    data.requireType = NoTargetTypeExt.RequireRock;
                    data.removeEffect = TryGet<StatusEffectData>("Rock");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Wood")
                //.WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.wood>")
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("require") };
                    data.requireType = NoTargetTypeExt.RequireWood;
                    data.removeEffect = TryGet<StatusEffectData>("Wood");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Gold")
                .WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.gold>")
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
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
                .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Hammer Gain Rock")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                    data.cardConstrains = new TargetConstraint[] { TryGetConstraint("hammerOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Rock");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Hammer Gain Gold")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                    data.effectToApply = TryGet<StatusEffectData>("Gold");
                    data.cardConstrains = new TargetConstraint[] { TryGetConstraint("hammerOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Hammer Gain Wood")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                    data.effectToApply = TryGet<StatusEffectData>("Wood");
                    data.cardConstrains = new TargetConstraint[] { TryGetConstraint("hammerOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Pickaxe Gain Rock")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                    data.effectToApply = TryGet<StatusEffectData>("Rock");
                    data.cardConstrains = new TargetConstraint[] { TryGetConstraint("pickaxeOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Pickaxe Gain Gold")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                    data.effectToApply = TryGet<StatusEffectData>("Gold");
                    data.cardConstrains = new TargetConstraint[] { TryGetConstraint("pickaxeOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Axe Gain Wood")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                    data.effectToApply = TryGet<StatusEffectData>("Wood");
                    data.cardConstrains = new TargetConstraint[] { TryGetConstraint("axeOnly") };
                })
        );
    }
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("require")
                .WithDescription("Requires resources in <card=dstmod.chest> to play, remove the effects afterwards".Process())
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
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("require2")
                .WithDescription("Requires <card=dstmod.rabbit> in hand to play, remove the card afterwards".Process())
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
