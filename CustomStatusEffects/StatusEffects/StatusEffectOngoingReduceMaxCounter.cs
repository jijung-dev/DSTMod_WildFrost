using System.Collections;
using UnityEngine;

public class StatusEffectOngoingReduceMaxCounter : StatusEffectOngoing
{
    public override IEnumerator Add(int add)
    {
        target.counter.max -= add;
        if (target.counter.current > target.counter.max)
            target.counter.current = target.counter.max;
        target.PromptUpdate();
        yield break;
    }

    public override IEnumerator Remove(int remove)
    {
        target.counter.max += remove;
        target.PromptUpdate();
        yield break;
    }
}
