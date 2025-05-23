using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class BeeTrait : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("bee")
                .WithTitle("Bee")
                .WithDescription("A bee a cute little bee")
                .WithShowName(true)
                .WithTitleColour(new Color(1.00f, 0.6353f, 0f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Bee")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("bee");
                    data.effects = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Immune To Summoned"),
                        TryGet<StatusEffectData>("Gain Honey When Destroyed"),
                    };
                })
        );
    }
}
