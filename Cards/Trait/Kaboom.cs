using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Kaboom : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("kaboom")
                .WithTitle("Kaboom")
                .WithShowName(true)
                .WithDescription("When <keyword=counter> reaches <0> destroy self, When destroyed apply <1><keyword=shroom> to an random unit")
                .WithTitleColour(new Color(0.63f, 0.95f, 0.55f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Kaboom")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("kaboom");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("When Destroyed Apply Shroom To Random Unit") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("When Destroyed Apply Damage To Attacker", "When Destroyed Apply Shroom To Random Unit")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.stackable = false;
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomUnit;
                    data.applyConstraints = new TargetConstraint[]
                    {
                        TryGetConstraint("noBuilding"),
                        TryGetConstraint("noChestHealth"),
                        TryGetConstraint("noChopable"),
                        TryGetConstraint("noToadstool"),
                        TryGetConstraint("noMineable"),
                        TryGetConstraint("noBoomshroom"),
                    };
                    data.effectToApply = TryGet<StatusEffectData>("Shroom");
                })
        );
    }
}
