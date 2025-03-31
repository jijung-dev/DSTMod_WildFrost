using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class MourningGlory : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("mourningglory")
                .WithTitle("Mourning Glory")
                .WithShowName(true)
                .WithDescription(
                    "When <card=dstmod.abigail> got destroyed, gain <card=dstmod.abigailFlower> and gain 1 <keyword=dstmod.mourningglory>,".Process()
                        + " each stack reduce <card=dstmod.abigail> max <keyword=health>|Max 5 stacks".Process()
                )
                .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(true)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Mourning Glory")
                .SubscribeToAfterAllBuildEvent<TraitData>(
                    delegate(TraitData data)
                    {
                        data.keyword = TryGet<KeywordData>("mourningglory");
                        data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("While Active Reduce Abigail Max Health") };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectWhileActiveX>("While Active Reduce Abigail Max Health")
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.canBeBoosted = false;
                        data.stackable = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Max Health Safe");
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("abigailOnly") };
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Mourning Glory")
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.stackable = true;
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("Mourning Glory");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenCardDestroyedWithLimit>("When Abigail Destroyed Mourning Glory")
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyedWithLimit>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Temporary Mourning Glory");
                    data.constraints = new TargetConstraint[] { TryGetConstraint("abigailOnly") };

                    data.litmitCount = 4;
                    data.traitToLimit = TryGet<TraitData>("Mourning Glory");
                })
        );
    }
}
