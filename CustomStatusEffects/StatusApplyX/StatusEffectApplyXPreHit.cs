using System.Collections;

public class StatusEffectApplyXPreHit : StatusEffectApplyX
{
    public TargetConstraint[] attackerConstraints;

    public override void Init()
    {
        base.OnHit += CheckHit;
    }

    public override bool RunHitEvent(Hit hit)
    {
        if (
            target.enabled
            && hit.target == target
            && hit.canRetaliate
            && (!targetMustBeAlive || (target.alive && Battle.IsOnBoard(target)))
            && hit.Offensive
            && hit.BasicHit
        )
        {
            return CheckAttackerConstraints(hit.attacker);
        }

        return false;
    }

    public IEnumerator CheckHit(Hit hit)
    {
        return Run(GetTargets(hit, GetTargetContainers(), GetTargetActualContainers()), hit.damage + hit.damageBlocked);
    }

    public bool CheckAttackerConstraints(Entity attacker)
    {
        if (attackerConstraints != null)
        {
            TargetConstraint[] array = attackerConstraints;
            for (int i = 0; i < array.Length; i++)
            {
                if (!array[i].Check(attacker))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
