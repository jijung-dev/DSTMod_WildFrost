using System.Linq;
using DSTMod_WildFrost;


public class StatusEffectImmune : StatusEffectData
{
    public StatusEffectData[] immuneTo;

    public override bool RunApplyStatusEvent(StatusEffectApply apply)
    {
        if ((bool)apply.effectData && apply.target == target)
            foreach (StatusEffectData item in immuneTo)
            {
                bool isBypass = false;
                for (int i = 0; i < target.statusEffects.Count; i++)
                {
                    if (target.statusEffects[i] is StatusEffectBypass bypass && bypass.effect == item)
                    {
                        isBypass = true;
                        break;
                    }
                }

                if (item == apply.effectData && !isBypass)
                {
                    apply.effectData = null;
                    apply.count = 0;
                }
            }
        return false;
    }
}
