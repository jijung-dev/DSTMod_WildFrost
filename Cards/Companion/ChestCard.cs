using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class ChestCard : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("chest", "Chest")
                .SetSprites("Chest.png", "Abigail_BG.png")
                .SetStats(null, null, 0)
                .WithText(
                    "Store <keyword=tgestudio.wildfrost.dstmod.rock> and <keyword=tgestudio.wildfrost.dstmod.wood> and <keyword=tgestudio.wildfrost.dstmod.gold>"
                )
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    CardType cardType = TryGet<CardType>("Clunker");
                    cardType.canRecall = false;
                    data.cardType = cardType;
                    data.canShoveToOtherRow = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Chest Health", 1), SStack("Immune To Everything", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1), TStack("Super Unmovable", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXBeforeBattle>("Summon Chest Before Battle")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXBeforeBattle>(data =>
                {
                    data.doPing = false;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Chest");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.checkBeforeSpawn = true;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillSlots>("Instant Summon Chest")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(data =>
                {
                    data.isEnemy = true;
                    //data.targetSummonNoAnimation = TryGet<StatusEffectSummonNoAnimation>("Summon Chest");
                    data.withCards = new CardData[] { TryGet<CardData>("chest") };
                    data.slotID = 7;
                })
        );
    }
}
