using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Glacier : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("glacier", "Glacier")
				.SetCardSprites("Glacier.png", "Wendy_BG.png")
				.SetStats(null, null, 0)
				.WithCardType("Clunker")
				.WithValue(2 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.isEnemyClunker = true;
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("When Hit Apply Freezing To Attacker",1),
						SStack("Scrap",2),
					};
				})
		);
	}

	protected override void CreateStatusEffect()
	{
		assets.Add(
			StatusCopy("When Hit Apply Snow To Attacker", "When Hit Apply Freezing To Attacker")
				.WithText("When hit, apply <{a}><keyword=dstmod.freeze> to the attacker".Process())
				.SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(data =>
				{
					data.effectToApply = TryGet<StatusEffectData>("Freezing");
				})
		);
	}
}
