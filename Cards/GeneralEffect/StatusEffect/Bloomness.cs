using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Bloomness : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("bloomness")
                .WithTitle("Bloomness")
                .WithShowName(false)
                .WithDescription(
                    "At 5+ <keyword=dstmod.bloomness> became <Bloom I>, At 10+ <keyword=dstmod.bloomness> became <Bloom II>, At 17+ <keyword=dstmod.bloomness> became <Bloom III>|Increase by 1 every turn \nMax 20 stacks".Process()
                )
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithNoteColour(new Color(0.07f, 0.3f, 0.17f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("bloom1")
                .WithTitle("Bloom I")
                .WithShowName(true)
                .WithDescription("Reduce max <keyword=counter> by 1")
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("bloom2")
                .WithTitle("Bloom II")
                .WithShowName(true)
                .WithDescription("Reduce max <keyword=counter> by 2")
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("bloom3")
                .WithTitle("Bloom III")
                .WithShowName(true)
                .WithDescription("Reduce max <keyword=counter> by 3 and allies by 1 and increase regeneration by 1")
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("plant")
                .WithTitle("Plant")
                .WithShowName(true)
                .WithDescription(
                    "No healing can increase <Bloomness>\nWhen <keyword=dstmod.overheat>'d took damage instead\nWhen <keyword=dstmod.froze> increase <keyword=counter> by 4 instead".Process()
                )
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectBloomness>("Bloomness")
                .SubscribeToAfterAllBuildEvent<StatusEffectBloomness>(data =>
                {
                    data.preventDeath = true;
                    data.type = "dst.bloomness";
                    data.eventPriority = -10;
                    data.stage1 = new CardData.StatusEffectStacks[]
                    {
                        SStack("Temporary Bloom I", 1),
                        SStack("While Active Reduce Max Counter To Self", 1),
                    };

                    data.stage2 = new CardData.StatusEffectStacks[]
                    {
                        SStack("Temporary Bloom II", 1),
                        SStack("While Active Reduce Max Counter To Self", 2),
                    };

                    data.stage3 = new CardData.StatusEffectStacks[]
                    {
                        SStack("Temporary Bloom III", 1),
                        SStack("While Active Reduce Max Counter To Self", 3),
                        SStack("While Active Reduce Max Counter To Allies", 1),
                    };
                })
                .Subscribe_WithStatusIcon("bloomness icon")
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectOngoingReduceMaxCounter>("Ongoing Reduce Max Counter")
                .SubscribeToAfterAllBuildEvent<StatusEffectOngoingReduceMaxCounter>(data =>
                {
                    data.canBeBoosted = false;
                    //data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintDoesAttack>() };
                })
        );
        assets.Add(
            StatusCopy("While Active Teeth To Allies", "While Active Reduce Max Counter To Allies")
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.canBeBoosted = false;
                    data.hiddenKeywords = null;
                    data.textKey = null;
                    data.affectsSelf = false;
                    data.effectToApply = TryGet<StatusEffectData>("Ongoing Reduce Max Counter");
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintMaxCounterMoreThan>(counter =>
                        {
                            counter.moreThan = 1;
                        }),
                    };
                })
        );
        assets.Add(
            StatusCopy("While Active Teeth To Allies", "While Active Reduce Max Counter To Self")
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.canBeBoosted = false;
                    data.hiddenKeywords = null;
                    data.textKey = null;
                    data.effectToApply = TryGet<StatusEffectData>("Ongoing Reduce Max Counter");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.affectsSelf = true;
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Bloom I")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.canBeBoosted = false;
                    data.trait = TryGet<TraitData>("Bloom I");
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Bloom II")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.canBeBoosted = false;
                    data.trait = TryGet<TraitData>("Bloom II");
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Bloom III")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.canBeBoosted = false;
                    data.trait = TryGet<TraitData>("Bloom III");
                })
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Bloom I")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("bloom1");
                })
        );
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Bloom II")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("bloom2");
                })
        );
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Bloom III")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("bloom3");
                })
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "bloomness icon", statusType: "dst.bloomness", mod.ImagePath("Icons/Bloomness_icon.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite(mod.ImagePath("Icons/Bloomness.png"))
                // .WithApplySFX(mod.ImagePath("Heat_Attack.wav"))
                // .WithEffectDamageSFX(mod.ImagePath("Heat_Attack.wav"))
                .WithKeywords(iconKeywordOrNull: "bloomness")
        );
    }
}
