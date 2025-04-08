using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Klaus : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("klaus", "Klaus")
                .SetBossSprites("Klaus.png", "Wendy_BG.png")
                .SetStats(20, 5, 4)
                .WithCardType("Boss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Klaus Enraged", 1),
                        SStack("ImmuneToSnow", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("klausEnraged", "Klaus Enraged")
                .SetBossSprites("KlausEnraged.png", "Wendy_BG.png")
                .SetStats(15, 5, 4)
                .WithCardType("Boss")
                .WithValue(25 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Hit All Enemies", 1),
                        SStack("ImmuneToSnow", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("FrenzyBossPhase2", "Klaus Enraged")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("klausEnraged");
                })
        );
    }
}
