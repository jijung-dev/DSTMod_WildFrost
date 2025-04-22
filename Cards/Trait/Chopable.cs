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
                .WithDescription("Get instant kill by <Axes>")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("chopablenorequired")
                .WithTitle("Chopable")
                .WithShowName(true)
                .WithDescription("Take Double damage from <Axes>")
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
                    data.effects = new[]
                    {
                        TryGet<StatusEffectData>("Building Immune To Everything"),
                        TryGet<StatusEffectData>("When Hit By Axe Dies"),
                    };
                })
        );
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("ChopableNoRequired")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("chopablenorequired");
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Chopable")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("ChopableNoRequired");
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintHasStatus>(r =>
                        {
                            r.not = true;
                            r.status = TryGet<StatusEffectData>("Temporary Chopable");
                        }),
                    };
                })
        );
    }
}
