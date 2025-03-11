using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class SuperBombard : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("superbombard")
                .WithTitle("Super Bombard")
                .WithShowName(true)
                .WithDescription("Hits all targets in <sprite=target> areas then resummon all <card=tgestudio.wildfrost.dstmod.sandCastle>")
                .WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Super Bombard")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("superbombard");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Bombard 3") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Bombard 2", "Bombard 3")
                .SubscribeToAfterAllBuildEvent<StatusEffectBombard>(data =>
                {
                    data.hitFriendlyChance = 0;
                    data.targetCountRange = new Vector2Int();
                    data.maxFrontTargets = 1;
                    data.delayAfter = 0.05f;
                })
        );
    }
}
