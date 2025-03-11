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
                .WithDescription("Can damage <keyword=tgestudio.wildfrost.dstmod.chopable>")
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
    }
}
