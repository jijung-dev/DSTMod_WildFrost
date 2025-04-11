using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Charlie : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("charlie", "Charlie")
                .SetBossSprites("Charlie.png", "Wendy_BG.png")
                .SetStats(null, null, 15)
                .WithCardType("Boss")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Darkness", 1), TStack("Backline", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Shadow Health", 3),
                        SStack("Building Immune To Everything", 1),
                        SStack("When Deployed Fill Board With Resource And Enemies", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectInstantSetCounter>("Set Counter"));
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Kill All Allies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.effectToApply = TryGet<StatusEffectData>("Kill");
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("noCharlie") };
                })
        );
        assets.Add(
            StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Board With Resource And Enemies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Fill Board With Resource And Enemies");
                })
        );
    }
}
