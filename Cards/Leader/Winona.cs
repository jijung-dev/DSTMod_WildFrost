using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Winona : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("winona", "Winona")
				.SetSprites("Winona.png", "Wendy_BG.png")
				.SetStats(10, 0, 4)
				.WithCardType("Leader")
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.attackEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Instant Gain Handy Remote", 1),
					};
					data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade(), LeaderExt.AddRandomDamage(0, 1) };
				})
		);
		assets.Add(
			new CardDataBuilder(mod)
				.CreateItem("handyRemote", "Handy Remote")
				.SetSprites("HandyRemote.png", "Wendy_BG.png")
				.WithCardType("Item")
				.FreeModify(
					delegate (CardData data)
					{
						data.needsTarget = false;
					}
				)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("On Card Played Trigger All Catapult", 1),
					};
					data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
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
					delegate (StatusEffectData data)
					{
						data.stackable = false;
						data.canBeBoosted = false;
					}
				)
				.SubscribeToAfterAllBuildEvent<StatusEffectInstantGainCard>(
					delegate (StatusEffectInstantGainCard data)
					{
						data.cardGain = TryGet<CardData>("handyRemote");
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
}
