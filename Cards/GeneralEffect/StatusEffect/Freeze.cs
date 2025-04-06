using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Freeze : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("freeze")
                .WithTitle("Freeze")
                .WithShowName(false)
                .WithDescription(
                    "<keyword=tgestudio.wildfrost.dstmod.froze> when more than or equal to max <keyword=health>, Can be remove by <keyword=tgestudio.wildfrost.dstmod.overheat>"
                )
                .WithTitleColour(new Color(0.60f, 0.81f, 0.98f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("freezeresist")
                .WithTitle("Freeze Resist")
                .WithShowName(false)
                .WithDescription("Immune to <keyword=dstmod.freeze>".Process())
                .WithTitleColour(new Color(0.60f, 0.81f, 0.98f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectFreeze>("Freezing")
                .SubscribeToAfterAllBuildEvent<StatusEffectFreeze>(data =>
                {
                    data.tempTrait = TryGet<StatusEffectData>("Temporary Froze");
                    data.frozeEffect = TryGet<StatusEffectData>("Temporary Froze");
                    data.freezeEffect = TryGet<StatusEffectData>("Freezing");

                    data.heatEffect = TryGet<StatusEffectData>("Overheat");
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                    data.removeOnDiscard = true;
                })
                .Subscribe_WithStatusIcon("freeze icon")
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmune>("Immune To Freeze")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.stackable = false;
                    data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Freezing"), TryGet<StatusEffectData>("Temporary Froze") };
                })
                .Subscribe_WithStatusIcon("freeze resist icon")
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "freeze icon", statusType: "dst.freeze", mod.ImagePath("Icons/Freezing.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithApplyVFX(mod.ImagePath("Animations/Freeze_Apply.gif"))
                .WithApplySFX(mod.ImagePath("Sounds/Freeze_Apply.wav"))
                .WithKeywords(iconKeywordOrNull: "freeze")
        );
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "freeze resist icon", statusType: "dst.freezeresist", mod.ImagePath("Icons/Freeze_Resist.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.counter)
                .WithTextboxSprite()
                .WithKeywords(iconKeywordOrNull: "freezeresist")
        );
    }
}
