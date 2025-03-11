using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Monster : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("monster")
                .WithTitle("Monster")
                .WithShowName(true)
                .WithDescription("When destroyed gain <card=tgestudio.wildfrost.dstmod.monsterMeat>")
                .WithTitleColour(new Color(0.57f, 0.40f, 1.00f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Monster")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("monster");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Gain Monster Meat When Destroyed") };
                })
        );
    }
}
