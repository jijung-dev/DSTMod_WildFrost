using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class SporecapSnow : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("sporecapSnow", "Sporecap Snow")
                .SetStats(null, null, 3)
                .SetSprites("Sporecap.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceChopable", 2),
                        SStack("On Turn Apply Snow To RandomEnemy", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("On Turn Apply Demonize To RandomEnemy", "On Turn Apply Snow To RandomEnemy")
                .WithText("Apply <{a}><keyword=snow> to a random enemy")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("noBuilding"), TryGetConstraint("noChestHealth") };
                    data.effectToApply = TryGet<StatusEffectData>("Snow");
                })
        );
    }
}
