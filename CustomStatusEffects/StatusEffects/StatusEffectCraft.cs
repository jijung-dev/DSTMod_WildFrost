using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ext;

public class StatusEffectCraft : StatusEffectData
{
    public string cardToRequire = "tgestudio.wildfrost.dstmod.chest";
    public NoTargetTypeExt requireType;
    public CardData requireCard;
    public readonly List<Entity> toDestroy = new List<Entity>();

    public bool running;
    public StatusEffectData removeEffect;
    public Entity chest = null;

    public override void Init()
    {
        Events.OnCheckAction += CheckAction;
        base.PreTrigger += EntityPreTrigger;
        base.OnCardMove += EntityCardMove;
    }

    public void OnDestroy()
    {
        Events.OnCheckAction -= CheckAction;
    }

    public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
    {
        if (target.enabled)
        {
            return entity == target;
        }

        return false;
    }

    public void CheckAction(ref PlayAction action, ref bool allow)
    {
        if (running || !target.enabled || target.silenced || !allow || (!(action is ActionTrigger) && !(action is ActionMove)))
        {
            return;
        }

        if (action is ActionTrigger trigger && trigger.entity != target)
        {
            return;
        }

        if (action is ActionMove move && (Battle.IsOnBoard(move.entity) || move.entity != target))
        {
            return;
        }

        allow = true;
        int amount = GetAmount();
        if (amount > 0)
        {
            if (!GetTargets(amount))
            {
                allow = false;
                if (NoTargetTextSystem.Exists())
                {
                    Ext.PopupText(target, requireType);
                }
            }
            if (!GetTargetCards(amount))
            {
                allow = false;
                if (NoTargetTextSystem.Exists())
                {
                    Ext.PopupText(target, requireType);
                }
            }
        }
    }

    public override bool RunPreTriggerEvent(Trigger trigger)
    {
        int amount = GetAmount();
        return amount > 0 && trigger.entity.name == target.name;
    }

    public override bool RunCardMoveEvent(Entity entity)
    {
        int amount = GetAmount();
        return amount > 0 && entity.name == target.name;
    }

    private IEnumerator EntityCardMove(Entity entity)
    {
        yield return entity.FindStatus(this)?.Remove();
        yield return Clear();
        entity.display.promptUpdateDescription = true;
        entity.PromptUpdate();
    }

    public IEnumerator EntityPreTrigger(Trigger trigger)
    {
        return Clear();
    }

    public IEnumerator Clear()
    {
        running = true;
        if (toDestroy.Count > 0)
        {
            foreach (Entity item in toDestroy)
            {
                target.curveAnimator.Ping();
                yield return item.Kill(DeathType.Consume);
            }

            toDestroy.Clear();
        }

        if (!(bool)chest)
            yield break;

        if (removeEffect != null)
        {
            chest.curveAnimator.Ping();
            yield return chest.FindStatus(removeEffect).RemoveStacks(GetAmount(), false);
        }

        running = false;
    }

    public bool GetTargets(int requiredAmount)
    {
        if (removeEffect == null)
            return true;

        if (chest == null)
            foreach (Entity card in References.Battle.cards)
            {
                if (card.data.name == cardToRequire)
                {
                    Debug.Log("[Don't Frostbite] Found the Chest");
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

    public bool GetTargetCards(int requiredAmount)
    {
        if (requireCard == null)
            return true;

        foreach (Entity item in References.Player.handContainer)
        {
            if (item.data.name == requireCard.name)
            {
                toDestroy.Add(item);
                requiredAmount--;
            }
        }

        if (requiredAmount <= 0)
        {
            return true;
        }

        return false;
    }
}
