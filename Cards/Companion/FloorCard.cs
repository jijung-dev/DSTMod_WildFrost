using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class FloorCard : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("floor", "Building Floor")
                .SetStats(null, null, 0)
                .WithText("Place <keyword=tgestudio.wildfrost.dstmod.building> on this platform")
                .SetSprites("Floor.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    CardType cardType = TryGet<CardType>("Clunker");
                    cardType.canRecall = false;
                    data.cardType = cardType;
                    data.canShoveToOtherRow = false;
                    data.isEnemyClunker = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Chest Health", 1), SStack("Immune To Everything", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1), TStack("Super Unmovable", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyed>("When Destroyed Summon Floor")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Floor");
                })
        );

        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXBeforeBattle>("Summon Floor Before Battle")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXBeforeBattle>(data =>
                {
                    data.doPing = false;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Floor");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.checkBeforeSpawn = true;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillSlots>("Instant Summon Floor")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(data =>
                {
                    //data.targetSummonNoAnimation = TryGet<StatusEffectSummonNoAnimation>("Summon Floor");
                    data.withCards = new CardData[] { TryGet<CardData>("floor") };
                    data.slotID = 3;
                })
        );
    }
}
