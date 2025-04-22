using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Winona : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("winona", "Winona")
                .SetLeaderSprites("Winona.png", "Wendy_BG.png")
                .WithText("<hiddencard=dstmod.catapult>".Process())
                .SetStats(10, 0, 4)
                .WithCardType("Leader")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Instant Gain Handy Remote", 1) };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade(), LeaderExt.AddRandomDamage(0, 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("handyRemote", "Handy Remote")
                .SetCardSprites("HandyRemote.png", "Wendy_BG.png")
                .WithCardType("Item")
                .FreeModify(
                    delegate(CardData data)
                    {
                        data.needsTarget = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Card Played Trigger All Catapult", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("handyRemoteSlow", "Handy Remote")
                .SetCardSprites("HandyRemote.png", "Wendy_BG.png")
                .WithCardType("Item")
                .FreeModify(
                    delegate(CardData data)
                    {
                        data.needsTarget = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Card Played Trigger All Catapult", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Consumable Slow", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Handy Remote")
                .WithText("Gain <card=dstmod.handyRemote>".Process())
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
                        data.cardGain = TryGet<CardData>("handyRemote");
                    }
                )
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Handy Remote Slow")
                .WithText("Gain <card=dstmod.handyRemoteSlow>".Process())
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
                        data.cardGain = TryGet<CardData>("handyRemoteSlow");
                    }
                )
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Trigger All Catapult")
                .WithText("Trigger All <card=dstmod.catapult>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.effectToApply = TryGet<StatusEffectData>("Trigger (High Prio)");
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("catapultOnly") };
                })
        );
    }

    protected override void CreateFinalSwapAsset()
    {
        var scripts = new List<CardScript>
        {
            new Scriptable<CardScriptRemoveAttackEffect>(r =>
            {
                r.toRemove = new StatusEffectData[] { TryGet<StatusEffectData>("Instant Gain Handy Remote") };
            }),
        };
        finalSwapAsset = (TryGet<CardData>("winona"), scripts.ToArray());
    }
}
