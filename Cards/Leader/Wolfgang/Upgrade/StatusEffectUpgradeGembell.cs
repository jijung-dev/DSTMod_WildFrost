using System.Linq;
using DSTMod_WildFrost;

public class StatusEffectUpgradeGembell : StatusEffectData, IUpgrade
{
	public void Run()
	{
		References.PlayerData.inventory.deck.list.AddRange(DSTMod.Instance.DataList<CardData>("firebell", "icebell", "gembell").Select(c => c.Clone()));
	}
}