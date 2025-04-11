using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost;
using UnityEngine;

public class StatusEffectUpgradeBling : StatusEffectData, IUpgrade
{
    public void Run()
    {
        Events.InvokeDropGold(100, "BlingsUpgrade", References.Player, target.GetContainerWorldPosition());
    }
}
