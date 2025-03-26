using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class SporecapEffect : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("sporecapIncreaseEffect", "Sporecap Effect")
                .SetStats(null, null, 0)
                .SetSprites("Sporecap.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceChopable", 2),
                        SStack("While Active Increase Effects To Toadstool", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("While Active Barrage To Allies", "While Active Increase Effects To Toadstool")
                .WithText("While active, increase effect by <{a}> to <card=dstmod.toadstoolEnraged>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("toadstoolOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Increase Effects");
                })
        );
    }
}
