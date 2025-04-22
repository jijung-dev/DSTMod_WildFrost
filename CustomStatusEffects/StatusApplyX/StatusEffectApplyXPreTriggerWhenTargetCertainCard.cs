using System;
using System.Collections;
using System.Linq;
using DSTMod_WildFrost;
using WildfrostHopeMod.VFX;

public class StatusEffectTemporaryIncreaseAttackPreTriggerWhenTargetCertainCard : StatusEffectApplyXPreTrigger
{
    int amountIncrease = 0;
    public TargetConstraint[] constraints;
    bool isIncrease = false;
    public override void Init()
    {
        base.Init();
        base.PostAttack += Attack;
    }

    public override bool RunPreTriggerEvent(Trigger trigger)
    {
        if (trigger.entity != target || running)
            return false;

        if (constraints == null || trigger.targets.Any(r => constraints.Any(c => c.Check(r))))
        {
            isIncrease = true;
            if (effectToApply is StatusEffectInstantIncreaseAttack effect)
            {
                amountIncrease = effect.scriptableAmount.Get(target);
            }
            return base.RunPreTriggerEvent(trigger);
        }

        return false;
    }
    public override bool RunPostAttackEvent(Hit hit)
    {
        if (hit.attacker != null && hit.attacker == target && isIncrease)
        {
            return true;
        }
        return false;
    }
    private IEnumerator Attack(Hit hit)
    {
        if (effectToApply is StatusEffectInstantIncreaseAttack effect)
        {
            effect.scriptableAmount = new Scriptable<ScriptableFixedAmount>(r => r.amount = -amountIncrease);
        }
        isIncrease = false;
        yield return Run(GetTargets());
    }
}
