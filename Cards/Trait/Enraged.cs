using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Enraged : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("enraged")
                .WithTitle("Enraged")
                .WithShowName(true)
                .WithDescription("Trigger when <card=tgestudio.wildfrost.dstmod.beeQueenEnraged> attacks")
                .WithTitleColour(new Color(1.00f, 0.4f, 0f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Enraged")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("enraged");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Trigger When Enraged Bee Queen Attacks") };
                })
        );
    }
}
