using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class EggOfTerror : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("eggOfTerror", "Egg Of Terror")
                .SetCardSprites("EggOfTerror.png", "Wendy_BG.png")
                .WithText("<hiddencard=dstmod.suspiciousPeeper>".Process())
                .SetStats(2, null, 1)
                .WithCardType("Enemy")
                .WithValue(1 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Counter Turn Suspicious Peeper", 1),
                        SStack("Destroy Self After Counter Turn", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("suspiciousPeeper", "Suspicious Peeper")
                .SetCardSprites("SuspiciousPeeper.png", "Wendy_BG.png")
                .SetStats(6, 0, 4)
                .WithCardType("Enemy")
                .WithValue(1 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("While Active Increase Attack To Allies", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Suspicious Peeper")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Suspicious Peeper");
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Suspicious Peeper")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("suspiciousPeeper");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Egg Of Terror")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Egg Of Terror");
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Egg Of Terror")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("eggOfTerror");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Counter Turn Suspicious Peeper")
                .WithText("When <keyword=counter> reaches 0, summon <card=dstmod.suspiciousPeeper> then kill self".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Suspicious Peeper");
                })
        );
    }
}
