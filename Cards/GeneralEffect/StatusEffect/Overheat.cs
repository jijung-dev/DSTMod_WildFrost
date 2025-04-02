using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Overheat : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("overheat")
                .WithTitle("Overheat")
                .WithShowName(false)
                .WithDescription(
                    "Reduce max <keyword=health> by half when more than or equal to max <keyword=health>, Can be remove by <keyword=tgestudio.wildfrost.dstmod.freeze> or <keyword=tgestudio.wildfrost.dstmod.froze>"
                )
                .WithTitleColour(new Color(0.94f, 0.58f, 0.24f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("overheatresist")
                .WithTitle("Overheat Resist")
                .WithShowName(false)
                .WithDescription("Immune to <keyword=dstmod.overheat>".Process())
                .WithTitleColour(new Color(0.94f, 0.58f, 0.24f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectHeat>("Overheat")
                .SubscribeToAfterAllBuildEvent<StatusEffectHeat>(data =>
                {
                    data.type = "dst.overheat";
                    data.overheatingEffect = TryGet<StatusEffectData>("Lose Half Health");
                    data.freezeEffect = TryGet<StatusEffectData>("Freezing");
                    data.frozeEffect = TryGet<StatusEffectData>("Temporary Froze");
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintHasHealth>() };
                })
                .Subscribe_WithStatusIcon("overheat icon")
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmune>("Immune To Overheat")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Overheat") };
                })
                .Subscribe_WithStatusIcon("overheat resist icon")
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "overheat icon", statusType: "dst.overheat", mod.ImagePath("Icons/Heat.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithApplyVFX(mod.ImagePath("Animations/Heat_Apply.gif"))
                .WithApplySFX(mod.ImagePath("Sounds/Heat_Attack.wav"))
                .WithKeywords(iconKeywordOrNull: "overheat")
        );
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "overheat resist icon", statusType: "dst.overheatresist", mod.ImagePath("Icons/Overheat_Resist.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.counter)
                .WithTextboxSprite()
                .WithKeywords(iconKeywordOrNull: "overheatresist")
        );
    }
}
