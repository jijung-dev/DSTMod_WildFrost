using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class CanCook : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("cancook")
                .WithTitle("Can Cook")
                .WithShowName(true)
                .WithDescription("Use <keyword=dstmod.food> on this to gain <keyword=dstmod.cooked> card".Process())
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithNoteColour(new Color(0.88f, 0.33f, 0.96f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Can Cook")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("cancook");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("ByPassHasHealthConstraint") };
                })
        );
    }
}
