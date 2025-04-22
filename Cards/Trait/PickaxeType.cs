using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class PickaxeType : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("pickaxetype")
                .WithTitle("Pickaxes")
                .WithShowName(true)
                .WithDescription("Can instant kill <keyword=dstmod.mineable>".Process())
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("pickaxetypenorequired")
                .WithTitle("Pickaxes")
                .WithShowName(true)
                .WithDescription("Apply <keyword=dstmod.mineablenorequired> and deal double <keyword=attack> to <keyword=dstmod.mineablenorequired>".Process())
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("PickaxeType")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("pickaxetype");
                })
        );
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("PickaxeTypeNoRequired")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("pickaxetypenorequired");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Double Attack When Target Mineable") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectTemporaryIncreaseAttackPreTriggerWhenTargetCertainCard>("Double Attack When Target Mineable")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryIncreaseAttackPreTriggerWhenTargetCertainCard>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Double Attack");
                    data.mustHaveTarget = true;
                    data.oncePerTurn = false;
                    data.constraints = new TargetConstraint[] { TryGetConstraint("mineableOnly") };
                })
        );
    }
}
