using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Consumbale : DataBase
{
    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Consumable")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("consumable");
                    data.effects = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Destroy After Use"),
                        TryGet<StatusEffectData>("Free Action"),
                        TryGet<StatusEffectData>("Kill Self When Redraw Hit"),
                    };
                    data.overrides = new TraitData[] { TryGet<TraitData>("Food") };
                })
        );
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Consumable Slow")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("consumableslow");
                    data.effects = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Destroy After Use"),
                        TryGet<StatusEffectData>("Kill Self When Redraw Hit"),
                    };
                    data.overrides = new TraitData[] { TryGet<TraitData>("Food"), TryGet<TraitData>("Consumable") };
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("consumable")
                .WithTitle("Consumable")
                .WithShowName(true)
                .WithDescription("Consume, Free Action, Destroy self when <Redraw Bell> is hit|Cannot be cooked")
                .WithTitleColour(new Color(1f, 0.57f, 0.21f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("consumableslow")
                .WithTitle("Consumable Slow")
                .WithShowName(true)
                .WithDescription("Consume, Destroy self when <Redraw Bell> is hit|Cannot be cooked")
                .WithTitleColour(new Color(1f, 0.57f, 0.21f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }
}
