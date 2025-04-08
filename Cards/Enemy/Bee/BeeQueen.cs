using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class BeeQueen : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("beeQueen", "Bee Queen")
                .WithText("The Queens Of All <keyword=dstmod.bee>".Process())
                .SetBossSprites("BeeQueen.png", "Wendy_BG.png")
                .SetStats(20, 4, 3)
                .WithCardType("Miniboss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Queen Enraged", 1), SStack("ImmuneToSnow", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("beeQueenEnraged", "Bee Queen Enraged")
                .SetBossSprites("BeeQueenEnraged.png", "Wendy_BG.png")
                .SetStats(25, 2, 5)
                .WithCardType("Miniboss")
                .WithValue(23 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("While Active Enraged All Bee", 1),
                        SStack("When Deployed Fill Board With Grumble Bee", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "Queen Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("beeQueenEnraged");
                })
        );
        assets.Add(
            StatusCopy("While Active Increase Attack To Allies (No Desc)", "While Active Enraged All Bee")
                .WithText("While active, <keyword=dstmod.enraged> all <keyword=dstmod.bee>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("beeOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Temporary Enraged");
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("Enraged");
                })
        );
        assets.Add(
            StatusCopy("When Deployed Lose Zoomlin", "When Deployed Fill Board With Grumble Bee")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.textKey = null;
                    data.effectToApply = TryGet<StatusEffectData>("Fill Board With Grumble Bee");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Board With Grumble Bee")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("grumbleBee") };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                })
        );
    }
}
