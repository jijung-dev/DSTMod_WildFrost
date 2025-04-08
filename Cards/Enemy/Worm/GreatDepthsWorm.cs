using Deadpan.Enums.Engine.Components.Modding;

public class GreatDepthsWorm : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("greatDepthsWormHead", "Great Depths Worm Head")
				.SetBossSprites("GreatDepthsWormHead.png", "Wendy_BG.png")
				.SetStats(30, 3, 8)
				.WithCardType("Miniboss")
				.WithValue(7 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Teeth", 3),
						SStack("On Turn Eat A Random Enemy", 1),
						SStack("ImmuneToSnow", 1),
					};
				})
		);
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("greatDepthsWormBody", "Great Depths Worm Body")
				.SetBossSprites("GreatDepthsWormBody.png", "Wendy_BG.png")
				.SetStats(20, null, 0)
				.WithCardType("Miniboss")
				.WithValue(7 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Teeth", 2),
						SStack("While Active Head Immune To Damage", 1),
					};
				})
		);
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("greatDepthsWormTail", "Great Depths Worm Tail")
				.SetBossSprites("GreatDepthsWormTail.png", "Wendy_BG.png")
				.SetStats(30, null, 6)
				.WithCardType("Miniboss")
				.WithValue(7 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Teeth", 1),
						SStack("On Turn Apply Teeth To Allies", 1),
						SStack("While Active Head Immune To Damage", 1),
					};
				})
		);
	}

	protected override void CreateStatusEffect()
	{
		assets.Add(
			new StatusEffectDataBuilder(mod)
				.Create<StatusEffectResource>("Immune To Damage")
				.SubscribeToAfterAllBuildEvent<StatusEffectResource>(data =>
				{
					data.allowedCards = new TargetConstraint[]
					{
						new Scriptable<TargetConstraintIsSpecificCard>(card =>
						{
							card.allowedCards = new CardData[] {TryGet<CardData>("greatDepthsWormHead")};
						}),
					};
				})
		);
		assets.Add(
			StatusCopy("While Active Teeth To Allies", "While Active Head Immune To Damage")
			.WithText("While active <card=dstmod.greatDepthsWormHead> is immune to damage".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
				{
					data.effectToApply = TryGet<StatusEffectData>("Immune To Damage");
					data.applyConstraints = new TargetConstraint[]
					{
						new Scriptable<TargetConstraintIsSpecificCard>(card =>
						{
							card.allowedCards = new CardData[] {TryGet<CardData>("greatDepthsWormHead")};
						}),
					};
				})
		);
		assets.Add(
			StatusCopy("On Turn Apply Attack To Self", "On Turn Eat A Random Enemy")
			.WithText("Eat and take <keyword=health> and <keyword=attack> a random enemy")
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
				{
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomEnemy;
					data.effectToApply = TryGet<StatusEffectData>("Eat (Health & Attack)");
					data.applyConstraints = new TargetConstraint[]
					{
						TryGetConstraint("noBuilding"), TryGetConstraint("noChestHealth"),
						new Scriptable<TargetConstraintIsCardType>(r =>
						{
							r.not = true;
							r.allowedTypes = new CardType[] {TryGet<CardType>("Leader")};
						}
						)
					};
				})
		);
	}
}
