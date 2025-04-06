using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Spitter : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateUnit("spitter", "Spitter")
				.SetSprites("Spitter.png", "Wendy_BG.png")
				.SetStats(6, 1, 3)
				.WithCardType("Enemy")
				.WithValue(3 * 36)
				.SubscribeToAfterAllBuildEvent<CardData>(data =>
				{
					data.startWithEffects = new CardData.StatusEffectStacks[]
					{
						SStack("MultiHit", 2),
						SStack("Gain Monster Meat When Destroyed", 1),
					};
					data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1) };
				})
		);
	}
}
