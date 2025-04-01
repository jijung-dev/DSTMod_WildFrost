using System.Linq;
using DSTMod_WildFrost;

public class StatusEffectUpgradeTape : StatusEffectData, IUpgrade
{
    public void Run()
    {
        References.PlayerData.inventory.deck.list.AddRange(DSTMod.Instance.DataList<CardData>("tape", "tape").Select(c => c.Clone()));
    }
}
