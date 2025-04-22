using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Mineable : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("mineable")
                .WithTitle("Mineable")
                .WithShowName(true)
                .WithDescription("Get instant kill by <Pickaxes>")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("mineablenorequired")
                .WithTitle("Mineable")
                .WithShowName(true)
                .WithDescription("Take Double damage from <Pickaxes>")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Mineable")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("mineable");
                    data.effects = new[]
                    {
                        TryGet<StatusEffectData>("Building Immune To Everything"),
                        TryGet<StatusEffectData>("When Hit By Pickaxe Dies"),
                    };
                })
        );
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("MineableNoRequired")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("mineablenorequired");
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Mineable")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("MineableNoRequired");
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintHasStatus>(r =>
                        {
                            r.not = true;
                            r.status = TryGet<StatusEffectData>("Temporary Mineable");
                        }),
                    };
                })
        );
    }
}
