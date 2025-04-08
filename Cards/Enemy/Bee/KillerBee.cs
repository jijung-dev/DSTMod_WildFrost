using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class KillerBee : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("killerBee", "Killer Bee")
                .SetCardSprites("KillerBee.png", "Wendy_BG.png")
                .SetStats(6, 2, 4)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Bee", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Bee Is Killed Trigger To Self", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenCardDestroyed>("When Bee Is Killed Trigger To Self")
                .WithIsReaction(true)
                .WithText("Trigger when an ally <keyword=dstmod.bee> is killed".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.constraints = new TargetConstraint[] { TryGetConstraint("beeOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Trigger");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Killer Bee")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Killer Bee");
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Killer Bee")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.setCardType = TryGet<CardType>("Enemy");
                    data.summonCard = TryGet<CardData>("killerBee");
                })
        );
    }
}
