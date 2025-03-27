using System.Collections;
using System.Collections.Generic;

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
            CardData data = cardGain.Clone(runCreateScripts: false);
            Card card = CardManager.Get(data, References.Battle.playerCardController, References.Player, inPlay: true, true);
            yield return card.UpdateData();
            References.Player.drawContainer.Add(card.entity);
            References.Player.drawContainer.SetChildPosition(card.entity);
            ActionQueue.Stack(new ActionMove(card.entity, References.Player.handContainer), fixedPosition: true);
            ActionQueue.Stack(new ActionRunEnableEvent(card.entity), fixedPosition: true);
            ActionQueue.Stack(new ActionReveal(card.entity), fixedPosition: true);
            card.entity.flipper.FlipUp();

            card.entity.curveAnimator.Ping();
        }
        yield return base.Process();
    }
}
