using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost;
using UnityEngine;

public class StatusEffectUpgradeCharm : StatusEffectData, IUpgrade
{
    public void Run()
    {
        CharacterRewards component = References.Player.GetComponent<CharacterRewards>();
        var cardUpgradeData = component.Pull<CardUpgradeData>(target, "Charms");
        cardUpgradeData.Assign(References.LeaderData);
    }
}
