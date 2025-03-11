using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class HammerType : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("hammertype")
                .WithTitle("Hammers")
                .WithShowName(true)
                .WithDescription("Can use to destroy <keyword=tgestudio.wildfrost.dstmod.building> to gain back resources used")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("HammerType")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("hammertype");
                })
        );
    }
}
