using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Chopable : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("chopable")
                .WithTitle("Chopable")
                .WithShowName(true)
                .WithDescription("Can only be damaged by <Axes> cards")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Chopable")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("chopable");
                    data.effects = new[] { TryGet<StatusEffectData>("Immune To Everything") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectResource>("ResourceChopable")
                .WithIsStatus(true)
                .SubscribeToAfterAllBuildEvent<StatusEffectResource>(data =>
                {
                    data.allowedCards = new TargetConstraint[] { TryGetConstraint("axeOnly") };
                    data.preventDeath = true;
                    data.type = "dst.resource";
                })
                .Subscribe_WithStatusIcon("resource icon")
        );
    }
}
