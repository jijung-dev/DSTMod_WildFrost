using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Mightiness : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("mightiness")
                .WithTitle("Mightiness")
                .WithShowName(false)
                .WithDescription(
                    "At 7+ <keyword=dstmod.mightiness> became <Mighty>, At 3- <keyword=dstmod.mightiness> became <Wimpy>|Max 10 stacks, countdown each turn".Process()
                )
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("mighty")
                .WithTitle("Mighty")
                .WithDescription("Double <keyword=attack> and <Super Unmovable>|Attack increase based on max attack")
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
                .WithShowIcon(false)
                .WithShow(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("wimpy")
                .WithTitle("Wimpy")
                .WithDescription("Half <keyword=attack>|Attack decrease based on max attack")
                .WithTitleColour(new Color(0.07f, 0.43f, 0.17f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
                .WithShowIcon(false)
                .WithShow(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectMightiness>("Mightiness")
                .SubscribeToAfterAllBuildEvent<StatusEffectMightiness>(data =>
                {
                    data.canBeBoosted = false;
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("mighty"), TryGet<KeywordData>("wimpy") };
                    data.tempTrait = TryGet<StatusEffectData>("Temporary Super Unmovable");
                    data.type = "dst.mightiness";
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintDoesDamage>() };
                })
                .Subscribe_WithStatusIcon("mightiness icon")
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Super Unmovable")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("Super Unmovable");
                })
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "mightiness icon", statusType: "dst.mightiness", mod.ImagePath("Icons/Mightiness_icon.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.damage)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite(mod.ImagePath("Icons/Mightiness.png"))
                .WithApplyVFX(mod.ImagePath("Animations/Mightiness_Apply.gif"))
                .WithApplySFX(mod.ImagePath("Sounds/Mightiness_Apply.wav"))
                // .WithEffectDamageSFX(mod.ImagePath("Heat_Attack.wav"))
                .WithKeywords(iconKeywordOrNull: "mightiness")
        );
    }
}
