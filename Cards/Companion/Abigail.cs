using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Abigail : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("abigail", "Abigail")
                .SetSprites("Abigail.png", "Abigail_BG.png")
                .SetStats(5, 2, 0)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Trigger When Wendy Attacks", 1),
                        SStack("Gain Abigail Flower When Destroyed", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("abigailFlower", "Abigail's Flower")
                .SetSprites("abigailFlower.png", "Wendy_BG.png")
                .FreeModify(
                    delegate(CardData data)
                    {
                        data.playOnSlot = true;
                        data.canPlayOnEnemy = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                        data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("Summon Abigail", 1) };
                        data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade() };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectTriggerWhenCertainAllyAttacks>("Trigger When Wendy Attacks")
                .WithText("Trigger when <card=dstmod.wendy> attacks".Process())
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.isReaction = true;
                        data.stackable = false;
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenCertainAllyAttacks>(data =>
                {
                    data.allyInRow = false;
                    data.ally = TryGet<CardData>("wendy");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("Gain Abigail Flower When Destroyed")
                .WithText("Drop <card=dstmod.abigailFlower>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("drop") };
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Gain Abigail Flower");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Abigail Flower")
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.stackable = false;
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantGainCard>(
                    delegate(StatusEffectInstantGainCard data)
                    {
                        data.cardGain = TryGet<CardData>("abigailFlower");
                    }
                )
        );
    }
}
