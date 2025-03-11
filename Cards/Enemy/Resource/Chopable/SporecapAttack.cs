using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class SporecapAttack : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("sporecapAttack", "Sporecap Attack")
                .SetStats(null, null, 0)
                .SetSprites("Sporecap.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.isEnemyClunker = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ResourceChopable", 2),
                        SStack("While Active Increase Attack To Toadstool", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("While Active Barrage To Allies", "While Active Increase Attack To Toadstool")
                .WithText("While active, add <{a}><keyword=attack> to <card=dstmod.toadstoolEnraged>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("toadstoolOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Increase Attack");
                })
        );
    }
}
