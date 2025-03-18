using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class ShadowAlign : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("shadowalign")
                .WithTitle("Shadow Align")
                .WithShowName(true)
                .WithDescription("Immune to <keyword=dstmod.Sanity>".Process())
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
                .Create("Shadow Align")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("shadowalign");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Immune To Sanity") };
                })
        );
    }
}
