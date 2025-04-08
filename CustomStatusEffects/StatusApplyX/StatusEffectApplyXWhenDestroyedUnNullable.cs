using UnityEngine;

public class StatusEffectApplyXWhenDestroyedUnNullable : StatusEffectApplyXWhenDestroyed
{
    public override bool TargetSilenced() => false;

    public override bool CanTrigger() => target.enabled;

    public override int GetAmount()
    {
        if (!target)
        {
            return 0;
        }

        if (!canBeBoosted)
        {
            return count;
        }

        return Mathf.Max(0, Mathf.RoundToInt((count + target.effectBonus) * target.effectFactor));
    }
}
public class StatusEffectApplyXWhenHitUnNullable : StatusEffectApplyXWhenHit
{
    public override bool TargetSilenced() => false;

    public override bool CanTrigger() => target.enabled;

    public override int GetAmount()
    {
        if (!target)
        {
            return 0;
        }

        if (!canBeBoosted)
        {
            return count;
        }

        return Mathf.Max(0, Mathf.RoundToInt((count + target.effectBonus) * target.effectFactor));
    }
}
