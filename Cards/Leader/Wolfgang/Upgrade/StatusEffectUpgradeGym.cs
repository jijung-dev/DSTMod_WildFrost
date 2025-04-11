using System;
using DSTMod_WildFrost;

public class StatusEffectUpgradeGym : StatusEffectData, IUpgrade
{
    public void Run()
    {
        References.LeaderData.startWithEffects = Ext.AddStartEffect("Upgrade Gym", 1);
    }

    public override bool RunBeginEvent()
    {
        var effect = target.FindStatus(DSTMod.Instance.TryGet<StatusEffectData>("Mightiness")) as StatusEffectMightiness;
        if (effect)
        {
            effect.cap = 15;
        }
        return true;
    }
}
