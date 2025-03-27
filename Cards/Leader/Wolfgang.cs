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
				.SetStats(10, 4, 5)
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
					data.attackEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Mightiness", 3),
					};
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
					data.attackEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Snow", 2),
						SStack("Mightiness", 5),
					};
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
					data.attackEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Spice", 3),
						SStack("Mightiness", 5),
					};
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Require Rock", 1)
					};
					data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wolfgangOnly") };
				})
		);
	}
	protected override void CreateStatusEffect()
	{
		assets.Add(
			new StatusEffectDataBuilder(mod)
				.Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Increase Max Damage")
				.WithText("Increase max <keyword=attack> by <{a}>")
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>
				(
					data =>
					{
						data.effectToApply = TryGet<StatusEffectData>("Increase Max Attack");
						data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
					}
				)
		);
		assets.Add(
			new StatusEffectDataBuilder(mod)
				.Create<StatusEffectInstantIncreaseMaxAttack>("Increase Max Attack")
				.SubscribeToAfterAllBuildEvent<StatusEffectInstantIncreaseMaxAttack>
				(
					data =>
					data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintDoesDamage>() }
				)
		);
	}
}
