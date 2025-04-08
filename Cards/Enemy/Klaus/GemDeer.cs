using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class GemDeer : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("redGemDeer", "Red Gem Deer")
				.SetSprites("RedGemDeer.png", "Wendy_BG.png")
				.SetStats(10, 1, 3)
				.WithCardType("Enemy")
				.WithValue(6 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("On Turn Apply Spice To Klaus", 2),
						SStack("When Destroyed Double Klaus Health And Attack", 1),
						SStack("When Destroyed Red Gem", 1),
					};
				})
		);
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("blueGemDeer", "Blue Gem Deer")
				.SetSprites("BlueGemDeer.png", "Wendy_BG.png")
				.SetStats(10, 1, 3)
				.WithCardType("Enemy")
				.WithValue(6 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("On Turn Apply Snow To RandomEnemy", 2),
						SStack("When Destroyed Double Klaus Health And Attack", 1),
						SStack("When Destroyed Blue Gem", 1),
					};
				})
		);
	}

	protected override void CreateStatusEffect()
	{
		assets.Add(
			StatusCopy("On Turn Apply Demonize To RandomEnemy", "On Turn Apply Spice To Klaus")
				.WithText("Apply <{a}><keyword=spice> to <card=dstmod.klaus>".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
				{
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
					data.applyConstraints = new TargetConstraint[] { TryGetConstraint("klausOnly") };
					data.effectToApply = TryGet<StatusEffectData>("Spice");
				})
		);
		assets.Add(
			StatusCopy("While Active Snow Immune To Allies", "While Active Red Gem To Klaus")
				.WithText("Allies gain immune to <keyword=dstmod.overheat>".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
				{
					data.doPing = false;
					data.effectToApply = TryGet<StatusEffectData>("Temporary Red Gem");
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
					data.applyConstraints = new TargetConstraint[] { TryGetConstraint("klausOnly") };
				})
		);
		assets.Add(
			StatusCopy("While Active Snow Immune To Allies", "While Active Blue Gem To Klaus")
				.WithText("Allies gain immune to <keyword=dstmod.overheat>".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
				{
					data.doPing = false;
					data.effectToApply = TryGet<StatusEffectData>("Temporary Blue Gem");
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
					data.applyConstraints = new TargetConstraint[] { TryGetConstraint("klausOnly") };
				})
		);
		assets.Add(
			new StatusEffectDataBuilder(mod)
				.Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Double Klaus Health And Attack")
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
				{
					data.targetMustBeAlive = false;
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
					data.applyConstraints = new TargetConstraint[] { TryGetConstraint("klausOnly") };
					data.effectToApply = TryGet<StatusEffectData>("Apply Double Health & Double Attack");
				})
		);
		assets.Add(
			new StatusEffectDataBuilder(mod)
				.Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Blue Gem")
				.WithText("When destroyed <card=dstmod.klaus> gain <keyword=dstmod.bluegem>".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
				{
					data.doPing = false;
					data.targetMustBeAlive = false;
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
					data.applyConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsCardType>(r => r.allowedTypes = new CardType[] { TryGet<CardType>("Leader") }) };
					data.effectToApply = TryGet<StatusEffectData>("While Active Blue Gem To Klaus");
				})
		);
		assets.Add(
			new StatusEffectDataBuilder(mod)
				.Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Red Gem")
				.WithText("When destroyed <card=dstmod.klaus> gain <keyword=dstmod.redgem>".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
				{
					data.doPing = false;
					data.targetMustBeAlive = false;
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
					data.applyConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsCardType>(r => r.allowedTypes = new CardType[] { TryGet<CardType>("Leader") }) };
					data.effectToApply = TryGet<StatusEffectData>("While Active Red Gem To Klaus");
				})
		);
		assets.Add(
			new StatusEffectDataBuilder(mod)
				.Create<StatusEffectInstantIncreaseMaxHealthScriptable>("Double Max Health")
				.SubscribeToAfterAllBuildEvent<StatusEffectInstantIncreaseMaxHealthScriptable>(data =>
				{
					data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintHasHealth>() };
					data.scriptableAmount = new Scriptable<ScriptableCurrentHealth>(r =>
						{
							r.multiplier = 1f;
							r.roundUp = true;
						}
					);
				})
		);
		assets.Add(
			StatusCopy("Apply Haze & Double Attack", "Apply Double Health & Double Attack")
				.SubscribeToAfterAllBuildEvent<StatusEffectInstantMultiple>(data =>
				{
					data.effects = new StatusEffectInstant[]
					{
						TryGet<StatusEffectInstant>("Double Max Health"),
						TryGet<StatusEffectInstant>("Double Attack"),
					};
				})
		);
		assets.Add(
			StatusCopy("Temporary Aimless", "Temporary Blue Gem")
				.SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
				{
					data.trait = TryGet<TraitData>("Blue Gem");
				})
		);
		assets.Add(
			StatusCopy("Temporary Aimless", "Temporary Red Gem")
				.SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
				{
					data.trait = TryGet<TraitData>("Red Gem");
				})
		);
	}
	protected override void CreateKeyword()
	{
		assets.Add(
			new KeywordDataBuilder(mod)
				.Create("redgem")
				.WithTitle("Red Gem")
				.WithDescription("Double max <keyword=health>, double <keyword=attack>, when hit apply 1<keyword=spice> to self")
				.WithShowName(true)
				.WithTitleColour(new Color(0.56f, 0.168f, 0.184f))
				.WithBodyColour(new Color(1f, 1f, 1f))
				.WithCanStack(false)
		);
		assets.Add(
			new KeywordDataBuilder(mod)
				.Create("bluegem")
				.WithTitle("Blue Gem")
				.WithDescription("Double max <keyword=health>, double <keyword=attack>, when hit apply <1><keyword=snow> to attacker")
				.WithShowName(true)
				.WithTitleColour(new Color(0.2f, 0.32f, 0.59f))
				.WithBodyColour(new Color(1f, 1f, 1f))
				.WithCanStack(false)
		);
	}
	protected override void CreateTrait()
	{
		assets.Add(
			new TraitDataBuilder(mod)
				.Create("Red Gem")
				.SubscribeToAfterAllBuildEvent<TraitData>(data =>
				{
					data.keyword = TryGet<KeywordData>("redgem");
					data.effects = new StatusEffectData[]
					{
						TryGet<StatusEffectData>("When Hit Apply Spice To Self"),
					};
				})
		);
		assets.Add(
			new TraitDataBuilder(mod)
				.Create("Blue Gem")
				.SubscribeToAfterAllBuildEvent<TraitData>(data =>
				{
					data.keyword = TryGet<KeywordData>("bluegem");
					data.effects = new StatusEffectData[]
					{
						TryGet<StatusEffectData>("When Hit Apply Snow To Attacker"),
					};

				})
		);
	}
}
