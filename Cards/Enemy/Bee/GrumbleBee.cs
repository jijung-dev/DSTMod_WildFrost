using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class GrumbleBee : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("grumbleBee", "Grumble Bee")
                .SetSprites("GrumbleBee.png", "Wendy_BG.png")
                .SetStats(8, 2, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Bee", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectTriggerWhenCertainAllyAttacks>("Trigger When Enraged Bee Queen Attacks")
                .WithCanBeBoosted(false)
                .FreeModify(data =>
                {
                    data.isReaction = true;
                    data.stackable = false;
                })
                .SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenCertainAllyAttacks>(data =>
                {
                    data.allyInRow = false;
                    data.ally = TryGet<CardData>("beeQueenEnraged");
                })
        );
    }
}
