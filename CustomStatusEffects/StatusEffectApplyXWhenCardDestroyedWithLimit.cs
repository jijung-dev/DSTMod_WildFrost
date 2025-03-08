using System.Linq;
using UnityEngine;

public class StatusEffectApplyXWhenCardDestroyedWithLimit : StatusEffectApplyXWhenCardDestroyed
{
    public int litmitCount;
    public TraitData traitToLimit;

    public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
    {
        var statusEffect = target.traits.FirstOrDefault(r => r.data = traitToLimit);

        if (statusEffect != null)
        {
            if (statusEffect.count < litmitCount)
            {
                return base.RunEntityDestroyedEvent(entity, deathType);
            }
        }
        else
        {
            return base.RunEntityDestroyedEvent(entity, deathType);
        }

        return false;
    }
}
