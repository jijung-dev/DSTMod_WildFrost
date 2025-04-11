using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatusEffectApplyXWhenBoardIsCleared : StatusEffectApplyX
{
    public override void Init()
    {
        base.OnTurnEnd += TurnEnd;
    }

    private IEnumerator TurnEnd(Entity entity)
    {
        return Run(GetTargets(), GetAmount());
    }

    public override bool RunTurnEndEvent(Entity entity)
    {
        var rows = References.Battle.GetRows(Battle.instance.enemy);
        float freeSlots = 0;
        foreach (CardContainer item in rows)
        {
            if (item is CardSlotLane cardSlotLane)
            {
                freeSlots += cardSlotLane.slots.Count((CardSlot slot) => slot.Empty);
            }
        }
        if (freeSlots >= 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
