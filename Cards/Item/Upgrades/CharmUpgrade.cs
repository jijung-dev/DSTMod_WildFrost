using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class CharmUpgrade : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateItem("charmUpgrade", "Upgrade")
				.WithText("Leader gain a random charm".Process())
				.SetStats(null, null, 0)
				.SetSprites("RandomCharmUpgrade.png", "Wendy_BG.png")
				.WithCardType("Item")
				.SubscribeToAfterAllBuildEvent<CardData>(
					delegate (CardData data)
					{
						data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Charm Upgrade", 1) };
					}
				)
		);
	}

	protected override void CreateStatusEffect()
	{
		assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeCharm>("Charm Upgrade"));
	}
}
