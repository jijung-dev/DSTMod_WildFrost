using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class TwintailedHeart : DataBase
{
	public override void CreateCard()
	{
		assets.Add(
			new CardDataBuilder(mod)
				.CreateItem("twintailedHeart", "Twintailed Heart")
				.SetStats(null, null, 0)
				.WithText("Apply <keyword=noomlin> to a companion card in hand")
				.SetSprites("TwintailedHeart.png", "Wendy_BG.png")
				.WithCardType("Item")
				.SubscribeToAfterAllBuildEvent<CardData>(
					delegate (CardData data)
					{
						data.canPlayOnHand = true;
						data.canPlayOnBoard = false;
						data.canPlayOnEnemy = false;
						data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Free Action", 1) };
						data.targetConstraints = new TargetConstraint[] { TryGetConstraint("companionOnly") };
						data.traits = new List<CardData.TraitStacks>() { TStack("Noomlin", 1) };
					}
				)
		);
	}
}
