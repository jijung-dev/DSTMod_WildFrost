using System.Collections;
using UnityEngine;

public class StatusEffectImmuneToDamage : StatusEffectData
{
    public override bool RunHitEvent(Hit hit)
    {
        if (hit.target == target && hit.damage > 0)
        {
            hit.nullified = true;
            return false;
        }

        return false;
    }
}
