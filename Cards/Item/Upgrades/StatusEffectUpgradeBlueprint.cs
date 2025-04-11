using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost;
using UnityEngine;

public class StatusEffectUpgradeBlueprint : StatusEffectData, IUpgrade
{
    public void Run()
    {
        var deck = References.Player.data.inventory.deck.list;
        var ran = Ext.blueprints[Ext.blueprints.RandomIndex()];
        deck.Add(ran.Clone());
    }
}
