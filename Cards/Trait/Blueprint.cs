using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Blueprint : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("blueprint")
                .WithTitle("Blueprint")
                .WithShowName(true)
                .WithDescription("When played gain <card=tgestudio.wildfrost.dstmod.hammer>")
                .WithTitleColour(new Color(0.0627f, 0.0941f, 0.4706f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Blueprint")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("blueprint");
                })
        );
    }
}
