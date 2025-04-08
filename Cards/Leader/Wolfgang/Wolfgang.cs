using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Wolfgang : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("wolfgang", "Wolfgang")
                .SetSprites("Wolfgang.png", "Wendy_BG.png")
                .SetStats(8, 4, 5)
                .WithCardType("Leader")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Smackback", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Counter Turn Increase Max Damage", 1),
                        SStack("Mightiness", 5),
                    };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade(), LeaderExt.AddRandomHealth(-1, 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("dumbbell", "Dumbbell")
                .SetSprites("Dumbbell.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Mightiness", 3) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wolfgangOnly") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("goldenDumbbell", "Golden Dumbbell")
                .SetSprites("GoldenDumbbell.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Snow", 2), SStack("Mightiness", 5) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wolfgangOnly") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("marbell", "Marbell")
                .SetSprites("Marbell.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Spice", 3), SStack("Mightiness", 5) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Require Rock", 1) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wolfgangOnly") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("firebell", "Firebell")
                .SetSprites("Firebell.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Overheat", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Card Played Add Mightiness To Wolfgang", 2) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("icebell", "Icebell")
                .SetSprites("Icebell.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Card Played Add Mightiness To Wolfgang", 2) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("gembell", "Gembell")
                .SetSprites("Gembell.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Mightiness", 5) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wolfgangOnly") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Increase Max Damage")
                .WithText("Increase max <keyword=attack> by 1")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.canBeBoosted = false;
                    data.stackable = false;
                    data.effectToApply = TryGet<StatusEffectData>("Increase Max Attack");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Increase Max Damage All Allies")
                .WithText("Increase max <keyword=attack> by 1 for allies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.canBeBoosted = false;
                    data.stackable = false;
                    data.effectToApply = TryGet<StatusEffectData>("Increase Max Attack");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantIncreaseMaxAttack>("Increase Max Attack")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantIncreaseMaxAttack>(data =>
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintDoesDamage>() }
                )
        );
        assets.Add(
            StatusCopy("On Card Played Add Scrap To Allies", "On Card Played Add Mightiness To Wolfgang")
                .WithText("Apply <{a}><keyword=dstmod.mightiness> to <card=dstmod.wolfgang>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wolfgangOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Mightiness");
                })
        );
    }
}
