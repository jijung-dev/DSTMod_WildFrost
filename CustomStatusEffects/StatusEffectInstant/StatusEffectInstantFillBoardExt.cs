using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatusEffectInstantFillBoardExt : StatusEffectInstant
{
    public enum Board
    {
        Enemy,
        Player,
        Full,
    }

    public CardData[] withCards;
    public readonly List<CardData> pool = new List<CardData>();
    public Board spawnBoard;
    public bool isEnemy;

    public override IEnumerator Process()
    {
        List<CardContainer> rows = GetRows();
        List<CardSlot> list = new List<CardSlot>();
        foreach (CardContainer item in rows)
        {
            if (item is CardSlotLane cardSlotLane)
            {
                list.AddRange(cardSlotLane.slots.Where((CardSlot slot) => slot.Empty));
            }
        }

        foreach (CardSlot slot2 in list)
        {
            CardData data = Pull().Clone();
            var owner = !isEnemy ? Battle.instance.player : Battle.instance.enemy;

            Card card = CardManager.Get(data, References.Battle.playerCardController, owner, inPlay: true, !isEnemy);
            yield return card.UpdateData();
            target.owner.reserveContainer.Add(card.entity);
            target.owner.reserveContainer.SetChildPosition(card.entity);
            ActionQueue.Stack(new ActionMove(card.entity, slot2), fixedPosition: true);
            ActionQueue.Stack(new ActionRunEnableEvent(card.entity), fixedPosition: true);
        }
    }

    public List<CardContainer> GetRows()
    {
        List<CardContainer> rows = new List<CardContainer>();
        switch (spawnBoard)
        {
            case Board.Enemy:
                return References.Battle.GetRows(Battle.instance.enemy);
            case Board.Player:
                return References.Battle.GetRows(Battle.instance.player);
            case Board.Full:
                return References.Battle.rows.Values.SelectMany((List<CardContainer> a) => a).ToList();
            default:
                return References.Battle.GetRows(Battle.instance.enemy);
        }
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
