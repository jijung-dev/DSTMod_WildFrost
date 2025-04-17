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
                .SetCardSprites("Floor.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.cardType = TryGet<CardType>("Clunker");
                    data.canShoveToOtherRow = false;
                    data.isEnemyClunker = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Chest Health", 1),
                        SStack("Floor Immune To Everything", 1),
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
                .Create<StatusEffectImmuneToEverythingBeside>("Floor Immune To Everything")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmuneToEverythingBeside>(data =>
                {
                    data.bypassType = new string[] { "dst.build" };
                    data.bypass = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Instant Summon Hammer In Hand"),
                        TryGet<StatusEffectData>("Reduce Chest Health"),
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
                    data.type = "dst.summon";
                    data.withCards = new CardData[] { TryGet<CardData>("floor") };
                    data.slotID = 3;
                })
        );
    }
}
