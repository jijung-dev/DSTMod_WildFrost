using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BeeMine : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("beeMine", "Bee Mine")
				.SetSprites("Dummy.png", "Wendy_BG.png")
				.WithCardType("Clunker")
				.SetStats(null, null, 0)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Require Wood", 1),
						SStack("When Deployed Lose Require Wood", 1),
						SStack("Scrap", 1) ,
						SStack("When Destroyed Summon Bee", 1) ,
					};
				})
		);
	}
	protected override void CreateStatusEffect()
	{
		assets.Add(
			StatusCopy("When Destroyed Add Health To Allies", "When Destroyed Summon Bee")
				.WithText("When destroyed summon <{a}> <card=dstmod.bee>".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
				{
					data.canBeBoosted = false;
					data.effectToApply = TryGet<StatusEffectData>("Instant Summon Bee");
					data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
				})
		);
		assets.Add(
			StatusCopy("When Deployed Lose Zoomlin", "When Deployed Lose Require Wood")
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
				{
					data.effectToApply = TryGet<StatusEffectData>("Lose Require Wood");
				})
		);
	}
}
