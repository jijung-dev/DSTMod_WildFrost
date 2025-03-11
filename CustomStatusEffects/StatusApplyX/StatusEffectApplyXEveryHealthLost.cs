using System.Collections;
using UnityEngine;

public class StatusEffectApplyXEveryYHealthLost : StatusEffectApplyX
{
    public int healthLost;
    public bool active;

    public int currentHealth;

    public override void Init()
    {
        Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
    }

    public void OnDestroy()
    {
        Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
    }

    public override bool RunBeginEvent()
    {
        active = true;
        currentHealth = target.hp.current;
        return false;
    }

    public void EntityDisplayUpdated(Entity entity)
    {
        if (active && target.hp.current != currentHealth && entity == target)
        {
            int num = currentHealth - target.hp.current;
            if (num >= healthLost && target.enabled && !target.silenced && (!targetMustBeAlive || (target.alive && Battle.IsOnBoard(target))))
            {
                currentHealth -= healthLost;
                ActionQueue.Stack(new ActionSequence(HealthLost(-num)) { note = base.name, priority = eventPriority }, fixedPosition: true);
            }
        }
    }

    public IEnumerator HealthLost(int amount)
    {
        if ((bool)this && target.IsAliveAndExists())
        {
            yield return Run(GetTargets(), amount);
        }
    }
}
