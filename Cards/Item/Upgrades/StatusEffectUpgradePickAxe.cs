using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost;
using UnityEngine;

public class StatusEffectUpgradePickAxe : StatusEffectData, IUpgrade
{
    public void Run()
    {
        var deck = References.Player.data.inventory.deck.list;
        deck.AddRange(DSTMod.Instance.DataList<CardData>("pickaxe", "axe").Select(c => c.Clone()));
    }
}
