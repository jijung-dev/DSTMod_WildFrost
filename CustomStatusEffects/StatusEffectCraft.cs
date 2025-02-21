using System.Collections;
using System.Collections.Generic;
using DSTMod_WildFrost.PatchingScript;
using UnityEngine;

public class StatusEffectCraft : StatusEffectData
{
    public string cardToRequire = "";
    public NoTargetTypeExt requireType;

    public bool running;

    public readonly List<Entity> toDestroy = new List<Entity>();

    public override void Init()
    {
        Events.OnCheckAction += CheckAction;
        base.PreTrigger += EntityPreTrigger;
    }

    public void OnDestroy()
    {
        Events.OnCheckAction -= CheckAction;
    }

    public void CheckAction(ref PlayAction action, ref bool allow)
    {
        if (
            running
            || !target.enabled
            || target.silenced
            || !allow
            || !(action is ActionTrigger actionTrigger)
            || !(actionTrigger.entity == target)
        )
        {
            return;
        }

        int amount = GetAmount();
        Events.CheckRecycleAmount(target, ref amount);
        if (amount > 0 && !GetTargets(amount))
        {
            NoTargetTextSystemExt noTargetTextSystemExt = new NoTargetTextSystemExt();
            allow = false;
            if (NoTargetTextSystem.Exists())
            {
                new Routine(noTargetTextSystemExt._Run(target, requireType, amount));
            }
        }
    }

    public override bool RunPreTriggerEvent(Trigger trigger)
    {
        return toDestroy.Count > 0;
    }

    public IEnumerator EntityPreTrigger(Trigger trigger)
    {
        running = true;
        foreach (Entity item in toDestroy)
        {
            target.curveAnimator.Ping();
            yield return item.Kill();
        }

        toDestroy.Clear();
        running = false;
    }

    public bool GetTargets(int requiredAmount)
    {
        bool flag = false;
        toDestroy.Clear();
        foreach (Entity item in References.Player.handContainer)
        {
            if (item.data.name == cardToRequire)
            {
                toDestroy.Add(item);
                if (--requiredAmount <= 0)
                {
                    flag = true;
                    break;
                }
            }
        }

        if (!flag)
        {
            toDestroy.Clear();
        }

        return flag;
    }

    public bool IsEnoughJunkInHand()
    {
        int num = GetAmount();
        foreach (Entity item in References.Player.handContainer)
        {
            if (item.data.name == cardToRequire && --num <= 0)
            {
                return true;
            }
        }

        return false;
    }
}
