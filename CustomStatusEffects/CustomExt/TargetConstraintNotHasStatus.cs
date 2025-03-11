using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetConstraintNotHasStatus : TargetConstraint
{
    [SerializeField]
    public StatusEffectData status;

    public override bool Check(Entity target)
    {
        if (!target.statusEffects.Any((StatusEffectData a) => a.name == status.name))
        {
            return !not;
        }

        return not;
    }

    public override bool Check(CardData targetData)
    {
        bool flag = false;
        CardData.StatusEffectStacks[] startWithEffects = targetData.startWithEffects;
        for (int i = 0; i < startWithEffects.Length; i++)
        {
            if (startWithEffects[i].data.name == status.name)
            {
                flag = true;
                break;
            }
        }

        if (!not)
        {
            return flag;
        }

        return !flag;
    }

    public bool CheckWillApply(Hit hit)
    {
        bool flag = false;
        List<CardData.StatusEffectStacks> statusEffects = hit.statusEffects;
        if (statusEffects != null && statusEffects.Count > 0)
        {
            foreach (CardData.StatusEffectStacks statusEffect in hit.statusEffects)
            {
                if (statusEffect.data.name == status.name)
                {
                    flag = true;
                    break;
                }
            }
        }

        if (!not)
        {
            return flag;
        }

        return !flag;
    }
}
