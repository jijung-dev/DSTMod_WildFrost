using System.Collections;

public class StatusEffectInstantGainCard : StatusEffectInstant
{
    public CardData cardGain;
    public bool canSummonMultiple;

    public override IEnumerator Process()
    {
        int amount = 1;
        if (canSummonMultiple)
            amount = GetAmount();

        for (int i = 0; i < amount; i++)
        {
            CardData data = cardGain.Clone();
            Card card = CardManager.Get(data, References.Battle.playerCardController, References.Player, inPlay: true, true);
            yield return card.UpdateData();
            ActionQueue.Stack(new ActionMove(card.entity, References.Player.handContainer), fixedPosition: true);
            ActionQueue.Stack(new ActionRunEnableEvent(card.entity), fixedPosition: true);

            card.entity.curveAnimator.Ping();
        }
        yield return base.Process();
    }
}
