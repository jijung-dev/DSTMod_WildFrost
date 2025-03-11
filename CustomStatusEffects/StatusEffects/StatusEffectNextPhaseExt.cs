using System;
using System.Linq;
using UnityEngine;
using static HitFlashSystem;

public class StatusEffectNextPhaseExt : StatusEffectData
{
    public CardData nextPhase;

    public CardAnimation animation;

    public bool goToNextPhase;

    public bool activated;

    public override void Init()
    {
        Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
    }

    public void OnDestroy()
    {
        Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
    }

    public void EntityDisplayUpdated(Entity entity)
    {
        if (!activated && target.hp.current <= 0 && entity == target)
        {
            TryActivate();
        }
    }

    public override bool RunPostHitEvent(Hit hit)
    {
        if (!activated && hit.target == target && target.hp.current <= 0)
        {
            TryActivate();
        }

        return false;
    }

    public void TryActivate()
    {
        bool flag = true;
        foreach (StatusEffectData statusEffect in target.statusEffects)
        {
            if (statusEffect != this && statusEffect.preventDeath)
            {
                flag = false;
                break;
            }
        }

        if (!flag)
        {
            return;
        }

        activated = true;

        if ((bool)nextPhase)
        {
            ActionQueue.Stack(new ActionChangePhaseExt(target, nextPhase.Clone(), animation) { priority = 10 }, fixedPosition: true);
            return;
        }

        throw new ArgumentException("Next phase not given!");
    }
}
