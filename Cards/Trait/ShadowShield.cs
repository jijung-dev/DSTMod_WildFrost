using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class ShadowShield : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("shadowshield")
                .WithTitle("Shadow Shield")
                .WithShowName(true)
                .WithDescription("Immune to 100% all damage")
                .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Shadow Shield")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("shadowshield");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Shadow Shield") };
                })
        );
    }
}
