using Deadpan.Enums.Engine.Components.Modding;
using Steamworks.Ugc;
using UnityEngine;

public class Food : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("food")
                .WithTitle("Food")
                .WithShowName(true)
                .WithDescription("Consume, Free Action, Destroy self when <Redraw Bell> is hit|Can be cooked")
                .WithTitleColour(new Color(1f, 0.57f, 0.21f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Food")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("food");
                    data.effects = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Free Action"),
                        TryGet<StatusEffectData>("Destroy After Use"),
                        TryGet<StatusEffectData>("Kill Self When Redraw Hit"),
                    };
                    data.overrides = new TraitData[] { TryGet<TraitData>("Consumable") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenRedrawHitExt>("Kill Self When Redraw Hit")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenRedrawHitExt>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Kill");
                })
        );
    }
}
