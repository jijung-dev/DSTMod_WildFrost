using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Mosling : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("mosling", "Mosling")
                .SetSprites("Mosling.png", "Wendy_BG.png")
                .SetStats(6, 1, 3)
                .WithCardType("Miniboss")
                .WithValue(4 * 36)
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("mooseEgg", "Moose Egg")
                .SetSprites("MooseEgg.png", "Wendy_BG.png")
                .SetStats(null, null, 4)
                .WithCardType("Enemy")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Chest Health", 1),
                        SStack("On Counter Turn Fill Board With Mosling", 1),
                        SStack("Destroy Self After Counter Turn", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Board With Mosling")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.isEnemy = true;
                    data.eventPriority = -1;
                    data.withCards = new CardData[] { TryGet<CardData>("mosling") };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Counter Turn Fill Board With Mosling")
                .WithText("Fill board with <card=dstmod.mosling> then destroy self".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Fill Board With Mosling");
                })
        );
    }
}
