using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Pengull : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("pengull", "Pengull")
				.SetCardSprites("Pengull.png", "Wendy_BG.png")
				.SetStats(4, 2, 2)
				.WithCardType("Enemy")
				.WithValue(2 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 2) };
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("Gain Egg When Destroyed", 1),
					};
				})
		);
	}
}
