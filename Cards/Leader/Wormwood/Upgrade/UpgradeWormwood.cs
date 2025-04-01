using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class UpgradeWormwood : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("huskUpgrade", "Bramble Husk Upgrade")
                .WithText("Increase <keyword=teeth> applied by 1".Process())
                .SetStats(null, null, 0)
                .SetSprites("HuskUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Husk", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("saladmander", "Saladmander")
                .SetSprites("Saladmander.png", "Wendy_BG.png")
                .SetStats(8, 3, 0)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Trigger When Wormwood Is Hit", 1),
                        SStack("When Deployed Apply Reduce Bloomness To Wormwood", 8),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("carrat", "Carrat")
                .SetSprites("Carrat.png", "Wendy_BG.png")
                .SetStats(2, null, 5)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Turn Gain Random Consumable", 1),
                        SStack("When Deployed Apply Reduce Bloomness To Wormwood", 4),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("bulbousLightbug", "Bulbous Lightbug")
                .SetSprites("BulbousLightbug.png", "Wendy_BG.png")
                .SetStats(1, null, 0)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Fragile", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("While Active Sanity Immune To Wormwood", 1),
                        SStack("When Deployed Apply Reduce Bloomness To Wormwood", 6),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeHusk>("Upgrade Husk"));
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenCertainAllyIsHit>("Trigger When Wormwood Is Hit")
                .WithText("Trigger when <card=dstmod.wormwood> is hit".Process())
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.isReaction = true;
                        data.stackable = false;
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCertainAllyIsHit>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Trigger");
                    data.ally = TryGet<CardData>("wormwood");
                })
        );
        assets.Add(
            StatusCopy("When Deployed Apply Ink To Allies", "When Deployed Apply Reduce Bloomness To Wormwood")
                .WithText("Cost <{a}><keyword=dstmod.bloomness>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Bloomness");
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("wormwoodOnly") };
                })
        );
        assets.Add(
            StatusCopy("While Active Snow Immune To Allies", "While Active Sanity Immune To Wormwood")
                .WithText("While active, <card=dstmod.wormwood> gain <keyword=dstmod.sanityresist>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Immune To Sanity");
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("wormwoodOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonRandom>("Instant Summon Random Consumable In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                    data.randomCards = new StatusEffectSummon[]
                    {
                        TryGet<StatusEffectSummon>("Summon Juicy Berries"),
                        TryGet<StatusEffectSummon>("Summon Berries"),
                        TryGet<StatusEffectSummon>("Summon Honey"),
                        TryGet<StatusEffectSummon>("Summon Blue Cap"),
                        TryGet<StatusEffectSummon>("Summon Green Cap"),
                        TryGet<StatusEffectSummon>("Summon Red Cap"),
                        TryGet<StatusEffectSummon>("Summon Cactus Flesh"),
                        TryGet<StatusEffectSummon>("Summon Light Bulb"),
                        TryGet<StatusEffectSummon>("Summon Foliage"),
                        TryGet<StatusEffectSummon>("Summon Lesser Glow Berry"),
                    };
                })
        );
        assets.Add(
            StatusCopy("On Turn Apply Attack To Self", "On Turn Gain Random Consumable")
                .WithText("Gain random <keyword=dstmod.food> or <keyword=dstmod.consumable>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Random Consumable In Hand");
                })
        );
    }
}
