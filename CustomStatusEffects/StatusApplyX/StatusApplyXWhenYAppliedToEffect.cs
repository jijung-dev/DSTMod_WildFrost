using System.Collections;
using UnityEngine;

public class StatusEffectApplyXWhenYAppliedToEffect : StatusEffectApplyX
{
    public bool instead;
    public bool whenAnyApplied;
    public StatusEffectData[] whenAppliedEffect;
    public ApplyToFlags whenAppliedToFlags;
    public bool mustReachAmount;
    public bool adjustAmount;
    public int addAmount;
    public float multiplyAmount = 1f;

    public override void Init()
    {
        base.PostApplyStatus += Run;
    }

    public bool CheckType(StatusEffectData effectData)
    {
        if (!whenAnyApplied)
        {
            return whenAppliedEffect.Contains(effectData);
        }
        return true;
    }

    public override bool RunApplyStatusEvent(StatusEffectApply apply)
    {
        if ((adjustAmount || instead) && target.enabled && !TargetSilenced() && (target.alive || !targetMustBeAlive) && (bool)apply.effectData && apply.count > 0 && CheckType(apply.effectData) && CheckTarget(apply.target))
        {
            if (instead)
            {
                apply.effectData = effectToApply;
            }

            if (adjustAmount)
            {
                apply.count += addAmount;
                apply.count = Mathf.RoundToInt((float)apply.count * multiplyAmount);
            }
        }

        return false;
    }

    public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
    {
        if (target.enabled && !TargetSilenced() && (bool)apply.effectData && apply.count > 0 && CheckType(apply.effectData) && CheckTarget(apply.target))
        {
            return CheckAmount(apply);
        }

        return false;
    }

    public IEnumerator Run(StatusEffectApply apply)
    {
        return Run(GetTargets(), apply.count);
    }

    public bool CheckFlag(ApplyToFlags whenAppliedTo)
    {
        return (whenAppliedToFlags & whenAppliedTo) != 0;
    }

    public bool CheckTarget(Entity entity)
    {
        if (!Battle.IsOnBoard(target))
        {
            return false;
        }

        if (entity == target)
        {
            return CheckFlag(ApplyToFlags.Self);
        }

        if (entity.owner == target.owner)
        {
            return CheckFlag(ApplyToFlags.Allies);
        }

        if (entity.owner != target.owner)
        {
            return CheckFlag(ApplyToFlags.Enemies);
        }

        return false;
    }

    public bool CheckAmount(StatusEffectApply apply)
    {
        if (!mustReachAmount)
        {
            return true;
        }

        StatusEffectData statusEffectData = apply.target.FindStatus(apply.effectData.type);
        if ((bool)statusEffectData)
        {
            return statusEffectData.count >= GetAmount();
        }

        return false;
    }
}