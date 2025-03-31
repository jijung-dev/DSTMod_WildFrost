using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class GoldPickAxeUpgrades : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateItem("goldUpgrade", "Upgrade")
				.WithText("Upgrade <card=dstmod.pickaxe> and <card=dstmod.axe> to <card=dstmod.goldenPickaxe> and <card=dstmod.goldenAxe>".Process())
				.SetStats(null, null, 0)
				.SetSprites("GoldUpgrade.png", "Wendy_BG.png")
				.WithCardType("Item")
				.SubscribeToAfterAllBuildEvent<CardData>(
					delegate (CardData data)
					{
						data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gold Upgrade", 1) };
					}
				)
		);
	}
	protected override void CreateStatusEffect()
	{
		assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectGoldUpgrade>("Gold Upgrade"));
	}
}
