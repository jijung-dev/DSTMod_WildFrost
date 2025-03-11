using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Antlion : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("antlion", "Antlion")
                .SetSprites("Antlion.png", "Wendy_BG.png")
                .SetStats(30, 6, 4)
                .WithCardType("Miniboss")
                .WithValue(13 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Deployed Summon Sand Castle", 2),
                        SStack("On Turn Kill All Sand Castle", 1),
                        SStack("After Turn Summon Sand Castle", 2),
                        SStack("ImmuneToSnow", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Super Bombard", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("sandCastle", "Sand Castle")
                .SetSprites("SandCastle.png", "Wendy_BG.png")
                .SetStats(null, null, 0)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Chest Health", 1), SStack("Immune To Everything", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Super Unmovable", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Sand Castle")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    CardType cardType = TryGet<CardType>("Clunker");
                    cardType.canRecall = false;
                    data.setCardType = cardType;
                    data.summonCard = TryGet<CardData>("sandCastle");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonOnCertainSlot>("Instant Summon Sand Castle")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonOnCertainSlot>(data =>
                {
                    data.isRandom = true;
                    data.canSummonMultiple = true;
                    data.maxRandomRange = 7;
                    data.minRandomRange = 0;
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Sand Castle");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXPreTurn>("On Turn Kill All Sand Castle")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXPreTurn>(data =>
                {
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("sandCastleOnly") };
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Chest Health");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("After Turn Summon Sand Castle")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Sand Castle");
                })
        );

        assets.Add(
            StatusCopy("When Deployed Lose Zoomlin", "When Deployed Summon Sand Castle")
                .WithText("When deployed, summon <{a}> {0} at enemy side")
                .WithTextInsert("<card=dstmod.sandCastle>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Sand Castle");
                })
        );
    }
}
