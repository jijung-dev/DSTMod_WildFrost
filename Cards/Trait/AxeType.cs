using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class AxeType : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("axetype")
                .WithTitle("Axes")
                .WithShowName(true)
                .WithDescription("Can instant kill <keyword=dstmod.chopable>".Process())
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("axetypenorequired")
                .WithTitle("Axes")
                .WithShowName(true)
                .WithDescription("Apply <keyword=dstmod.chopablenorequired> and deal double <keyword=attack> to <keyword=dstmod.chopablenorequired>".Process())
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("AxeType")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("axetype");
                })
        );
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("AxeTypeNoRequired")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("axetypenorequired");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Double Attack When Target Chopable") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectTemporaryIncreaseAttackPreTriggerWhenTargetCertainCard>("Double Attack When Target Chopable")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryIncreaseAttackPreTriggerWhenTargetCertainCard>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Double Attack");
                    data.mustHaveTarget = true;
                    data.oncePerTurn = false;
                    data.constraints = new TargetConstraint[] { TryGetConstraint("chopableOnly") };
                })
        );
    }
}
