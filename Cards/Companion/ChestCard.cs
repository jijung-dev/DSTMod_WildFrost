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
                .SetCardSprites("Chest.png", "Abigail_BG.png")
                .SetStats(null, null, 0)
                .WithText(
                    "Store <keyword=tgestudio.wildfrost.dstmod.rock> and <keyword=tgestudio.wildfrost.dstmod.wood> and <keyword=tgestudio.wildfrost.dstmod.gold>"
                )
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.cardType = TryGet<CardType>("Clunker");
                    data.canShoveToOtherRow = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Chest Health", 1),
                        SStack("Chest Immune To Everything", 1),
                        SStack("Cannot Recall", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1), TStack("Super Unmovable", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmuneToEverythingBeside>("Chest Immune To Everything")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmuneToEverythingBeside>(data =>
                {
                    data.bypass = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Rock"),
                        TryGet<StatusEffectData>("Gold"),
                        TryGet<StatusEffectData>("Wood"),
                        TryGet<StatusEffectData>("Cannot Recall"),
                        TryGet<StatusEffectData>("Chest Health"),
                        TryGet<StatusEffectData>("Low Priority Position"),
                        TryGet<StatusEffectData>("Unshovable"),
                        TryGet<StatusEffectData>("Unmovable"),
                    };
                })
        );
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
                    data.withCards = new CardData[] { TryGet<CardData>("chest") };
                    data.slotID = 7;
                })
        );
    }
}
