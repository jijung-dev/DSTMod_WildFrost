using System.Collections;

public class StatusEffectStealthy : StatusEffectData
{
    public bool cardPlayed;

    public override void Init()
    {
        base.OnActionPerformed += ActionPerformed;
    }

    public override bool RunBeginEvent()
    {
        target.cannotBeHitCount++;
        return false;
    }

    public override bool RunEndEvent()
    {
        target.cannotBeHitCount--;
        return false;
    }

    public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
    {
        if (!cardPlayed && entity == target && count > 0)
        {
            cardPlayed = true;
        }

        return false;
    }

    public override bool RunActionPerformedEvent(PlayAction action)
    {
        if (cardPlayed)
        {
            return ActionQueue.Empty;
        }

        return false;
    }

    public IEnumerator ActionPerformed(PlayAction action)
    {
        yield return CountDown();
    }

    public IEnumerator CountDown()
    {
        int amount = 1;
        Events.InvokeStatusEffectCountDown(this, ref amount);
        cardPlayed = false;
        if (amount != 0)
        {
            yield return CountDown(target, amount);
        }
    }
}
