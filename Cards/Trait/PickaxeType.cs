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
                .WithDescription($"Can damage <keyword=tgestudio.wildfrost.dstmod.mineable>")
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
    }
}
