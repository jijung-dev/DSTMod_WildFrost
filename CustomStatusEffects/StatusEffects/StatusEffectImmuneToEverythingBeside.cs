using System;
using System.Linq;
using DSTMod_WildFrost;

public class StatusEffectImmuneToEverythingBeside : StatusEffectData
{
    public StatusEffectData[] bypass;
    public string[] bypassType;
    public bool isAllStatus;

    public override bool RunApplyStatusEvent(StatusEffectApply apply)
    {
        if ((bool)apply.effectData && apply.target == target)
        {
            if ((isAllStatus && !apply.effectData.isStatus) || bypass.Contains(apply.effectData) || bypassType.Contains(apply.effectData.type))
            {
                return true;
            }
            else
            {
                apply.effectData = null;
                apply.count = 0;
            }
        }
        return false;
    }
}
