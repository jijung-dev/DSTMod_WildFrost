public class StatusEffectImmune : StatusEffectData
{
    public StatusEffectData[] immuneTo;

    public override bool RunApplyStatusEvent(StatusEffectApply apply)
    {
        if ((bool)apply.effectData && apply.target == target)
            if (immuneTo.Contains(apply.effectData))
            {
                apply.effectData = null;
                apply.count = 0;
            }

        return false;
    }
}
