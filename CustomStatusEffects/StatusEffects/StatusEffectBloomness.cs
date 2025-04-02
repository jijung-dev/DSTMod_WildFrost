using System;
using System.Collections;
using System.Collections.Generic;
using Dead;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class StatusEffectBloomness : StatusEffectData
{
    public bool primed;
    public CardData.StatusEffectStacks[] stage0;
    public CardData.StatusEffectStacks[] stage1;
    public CardData.StatusEffectStacks[] stage2;
    public CardData.StatusEffectStacks[] stage3;
    public int currentStage = 0;
    public int healAmount = 1;

    public override void Init()
    {
        base.OnTurnEnd += TurnEnd;
        base.OnHit += Check;
    }

    public override bool RunHitEvent(Hit hit)
    {
        if (hit.target == target)
        {
            return hit.damage > 0;
        }

        return false;
    }

    public IEnumerator Check(Hit hit)
    {
        while (hit.damage > 0 && count > 0)
        {
            count--;
            hit.damage--;
            hit.damageBlocked++;
            Check(count);
        }

        if (count <= 0)
        {
            yield return Remove();
        }

        target.PromptUpdate();
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
        int amount = healAmount;
        if (amount != 0 && (bool)target && target.enabled && entity == target && count + healAmount < 20)
        {
            SfxSystem.OneShot("event:/sfx/status_icon/counter_decrease");
            count += amount;
            Check(count);
            target.PromptUpdate();
        }
        yield return null;
    }

    public override bool RunApplyStatusEvent(StatusEffectApply apply)
    {
        if (!(bool)apply.effectData || apply.target != target || !(apply.effectData is StatusEffectBloomness))
            return false;
        int current = apply.count + count;
        if (current >= 20)
        {
            apply.count = 20 - count;
        }
        Check(current);
        return true;
    }

    void Check(int current)
    {
        ActionSequence actionSequence;
        if (current >= 17)
        {
            healAmount = 2;
            actionSequence = new ActionSequence(SetStage(3));
        }
        else if (current >= 10)
        {
            healAmount = 1;
            actionSequence = new ActionSequence(SetStage(2));
        }
        else if (current >= 5)
        {
            healAmount = 1;
            actionSequence = new ActionSequence(SetStage(1));
        }
        else
        {
            healAmount = 1;
            actionSequence = new ActionSequence(SetStage(0));
        }

        actionSequence.fixedPosition = true;
        actionSequence.priority = eventPriority;
        actionSequence.note = "Set Stage";

        ActionQueue.Stack(actionSequence);
    }

    public IEnumerator SetStage(int stageID)
    {
        if (!this || !target || !target.alive || currentStage == stageID)
        {
            yield break;
        }

        foreach (var item in GetStageArray(currentStage))
        {
            var effect = target.FindStatus(item.data);
            if (effect != null)
            {
                if (effect is StatusEffectWhileActiveX effect2)
                    yield return effect2.Deactivate();
                yield return effect.Remove();
            }
        }

        Routine.Clump clump = new Routine.Clump();
        Hit hit = new Hit(applier, target, 0) { damageType = "dst.bloomness" };
        if (stageID > currentStage)
        {
            VFXHelper.SFX.TryPlaySound("Bloomness_Apply");
        }

        foreach (var item in GetStageArray(stageID))
        {
            hit.AddStatusEffect(item);
        }

        currentStage = stageID;

        clump.Add(hit.Process());
        yield return clump.WaitForEnd();
    }

    private CardData.StatusEffectStacks[] GetStageArray(int stageID)
    {
        switch (stageID)
        {
            case 1:
                return stage1;
            case 2:
                return stage2;
            case 3:
                return stage3;
            default:
                return new CardData.StatusEffectStacks[] { }; // Return empty array if out of range
        }
    }
}
