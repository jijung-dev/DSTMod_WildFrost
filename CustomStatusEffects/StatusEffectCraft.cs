using System.Collections;
using System.Collections.Generic;
using DSTMod_WildFrost.PatchingScript;
using UnityEngine;

public class StatusEffectCraft : StatusEffectData
{
    public string cardToRequire = "tgestudio.wildfrost.dstmod.chest";
    public NoTargetTypeExt requireType;

    public bool running;
    public StatusEffectData removeEffect;
    public Entity chest = null;

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
        if (running || !target.enabled || target.silenced || !allow || !(action is ActionTrigger actionTrigger) || !(actionTrigger.entity == target))
        {
            return;
        }

        int amount = GetAmount();
        if (amount > 0 && !GetTargets(amount))
        {
            allow = false;
            if (NoTargetTextSystem.Exists())
            {
                new Routine(NoTargetTextSystemExt.Run(target, requireType));
            }
        }
    }

    public override bool RunPreTriggerEvent(Trigger trigger)
    {
        int amount = GetAmount();
        return amount > 0 && GetTargets(amount) && trigger.entity.name == target.name;
    }

    public IEnumerator EntityPreTrigger(Trigger trigger)
    {
        running = true;
        if (!(bool)chest)
            yield break;

        yield return chest.FindStatus(removeEffect).RemoveStacks(GetAmount(), false);

        running = false;
    }

    public bool GetTargets(int requiredAmount)
    {
        if (chest == null)
            foreach (Entity card in References.Battle.cards)
            {
                if (card.data.name == cardToRequire)
                {
                    Debug.Log("[ Don't Frostbite ] Found the Chest");
                    chest = card;
                    break;
                }
            }

        var effectData = chest.FindStatus(removeEffect);

        if (!(bool)effectData || effectData?.GetAmount() < requiredAmount)
        {
            return false;
        }

        return true;
    }
}
