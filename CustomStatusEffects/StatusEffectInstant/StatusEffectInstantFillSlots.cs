using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatusEffectInstantFillSlots : StatusEffectInstant
{
    public bool doPing = true;
    public int slotID;
    public CardData[] withCards;
    public readonly List<CardData> pool = new List<CardData>();
    public bool isEnemy;

    public override IEnumerator Process()
    {
        CardSlot[] slots = References.Battle.allSlots.ToArray();

        if (!slots[slotID].Empty)
            yield return new ArgumentNullException("Please only fill on empty slot");

        var owner = !isEnemy ? Battle.instance.player : Battle.instance.enemy;

        CardData data = Pull().Clone();
        Card card = CardManager.Get(data, References.Battle.playerCardController, owner, inPlay: true, !isEnemy);
        yield return card.UpdateData();
        target.owner.reserveContainer.Add(card.entity);
        target.owner.reserveContainer.SetChildPosition(card.entity);
        ActionQueue.Stack(new ActionMove(card.entity, slots[slotID]), fixedPosition: true);
        ActionQueue.Stack(new ActionRunEnableEvent(card.entity), fixedPosition: true);

        if (doPing)
        {
            card.entity.curveAnimator.Ping();
        }

        yield return base.Process();
    }

    public CardData Pull()
    {
        if (pool.Count <= 0)
        {
            pool.AddRange(withCards);
        }

        return pool.TakeRandom();
    }
}
