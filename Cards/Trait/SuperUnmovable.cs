using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class SuperUnmovable : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("superunmovable")
                .WithTitle("Super Unmovable")
                .WithShowName(true)
                .WithDescription("Cannot move, like... at all")
                .WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Super Unmovable")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("superunmovable");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Unshovable"), TryGet<StatusEffectData>("Unmovable") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUnshovable>("Unshovable"));
    }
}
