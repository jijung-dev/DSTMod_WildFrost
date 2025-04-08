using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BlueprintUpgrade : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateItem("blueprintUpgrade", "Upgrade")
				.WithText("Gain a random <keyword=dstmod.blueprint>".Process())
				.SetStats(null, null, 0)
				.SetSprites("RandomBlueprintUpgrade.png", "Wendy_BG.png")
				.WithCardType("Item")
				.SubscribeToAfterAllBuildEvent<CardData>(
					delegate (CardData data)
					{
						data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Blueprint Upgrade", 1) };
					}
				)
		);
	}

	protected override void CreateStatusEffect()
	{
		assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeBlueprint>("Blueprint Upgrade"));
	}
}
