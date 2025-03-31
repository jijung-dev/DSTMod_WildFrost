using System;
using System.Collections;
using Dead;
using DSTMod_WildFrost;
using UnityEngine;

public class StatusEffectMightiness : StatusEffectData
{
    int attack;
    public bool primed;
    public StatusEffectData tempTrait;
    public int cap = 10;

    public override void Init()
    {
        base.OnTurnEnd += TurnEnd;
    }

    public override bool RunTurnStartEvent(Entity entity)
    {
        if (!primed && entity == target && Battle.IsOnBoard(entity))
        {
            primed = true;
        }

        return false;
    }

    public override bool RunTurnEndEvent(Entity entity)
    {
        if (entity == target)
        {
            return primed;
        }

        return false;
    }

    public IEnumerator TurnEnd(Entity entity)
    {
        int amount = 1;
        Events.InvokeStatusEffectCountDown(this, ref amount);
        attack = target.damage.max;
        if (amount != 0 && (bool)target && target.enabled && entity == target && count > 1)
        {
            count -= amount;
            Check(count);
            target.PromptUpdate();
        }
        yield return null;
    }

    public override bool RunApplyStatusEvent(StatusEffectApply apply)
    {
        attack = target.damage.max;
        if (!(bool)apply.effectData || apply.target != target || !(apply.effectData is StatusEffectMightiness))
            return false;
        int current = apply.count + count;
        if (current >= cap)
        {
            apply.count = cap - count;
        }
        Check(current);
        return true;
    }

    void Check(int current)
    {
        if (current >= 7)
        {
            target.damage.current = attack * 2;
            ActionQueue.Stack(
                new ActionSequence(AddUnmove())
                {
                    fixedPosition = true,
                    priority = eventPriority,
                    note = "Add Unmove",
                }
            );
        }
        else if (current > 3)
        {
            target.damage.current = attack;
            ActionQueue.Stack(
                new ActionSequence(RemoveUnmove())
                {
                    fixedPosition = true,
                    priority = eventPriority,
                    note = "Remove Unmove",
                }
            );
        }
        else
        {
            target.damage.current = attack / 2;
            ActionQueue.Stack(
                new ActionSequence(RemoveUnmove())
                {
                    fixedPosition = true,
                    priority = eventPriority,
                    note = "Remove Unmove",
                }
            );
        }
    }

    public IEnumerator AddUnmove()
    {
        if (!this || !target || !target.alive)
        {
            yield break;
        }
        Routine.Clump clump = new Routine.Clump();
        Hit hit = new Hit(applier, target, 0) { damageType = "dst.mightiness" };
        hit.AddStatusEffect(tempTrait, 1);
        clump.Add(hit.Process());
        yield return clump.WaitForEnd();
    }

    public IEnumerator RemoveUnmove()
    {
        if (!this || !target || !target.alive)
        {
            yield break;
        }
        var effect = target.FindStatus(tempTrait);
        yield return effect?.Remove();
    }
}
