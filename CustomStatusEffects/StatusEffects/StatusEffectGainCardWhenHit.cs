using System.Collections;

public class StatusEffectGainCardWhenHit : StatusEffectInstantGainCard
{
    public override void Init()
    {
        base.PostHit += CheckHit;
    }

    public override bool RunPostHitEvent(Hit hit)
    {
        if (target.enabled && hit.target == target && hit.canRetaliate && hit.Offensive && hit.BasicHit)
        {
            return true;
        }

        return false;
    }

    public IEnumerator CheckHit(Hit hit)
    {
        if (hit.target == target)
        {
            yield return Process();
        }
        yield break;
    }
}
