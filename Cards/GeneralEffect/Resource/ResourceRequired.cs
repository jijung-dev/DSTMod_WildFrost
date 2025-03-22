using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using DSTMod_WildFrost.PatchingScript;
using UnityEngine;
using WildfrostHopeMod.VFX;

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
                    data.requireType = NoTargetTypeExt.RequireCard;
                    data.requireCard = TryGet<CardData>("rabbit");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Rock")
                .WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.rock>")
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
                    data.requireType = NoTargetTypeExt.RequireRock;
                    data.removeEffect = TryGet<StatusEffectData>("Rock");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectCraft>("Require Wood")
                .WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.wood>")
                .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(data =>
                {
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
}
