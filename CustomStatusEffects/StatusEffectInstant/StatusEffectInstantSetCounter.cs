using System.Collections;

public class StatusEffectInstantSetCounter : StatusEffectInstant
{
    public override IEnumerator Process()
    {
        int amount = GetAmount();
        target.counter.max = amount;
        target.counter.current = amount;
        yield return base.Process();
    }
}
