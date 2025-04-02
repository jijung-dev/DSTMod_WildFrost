using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXToUnitWhenDestroyedByCertainCards : StatusEffectApplyXWhenDestroyedUnNullable
    {
        public TargetConstraint[] cardConstrains;

        public override bool RunBeginEvent()
        {
            applyToFlags = ApplyToFlags.Allies | ApplyToFlags.Enemies | ApplyToFlags.Self;
            applyConstraints = new TargetConstraint[] { DSTMod.Instance.TryGetConstraint("chestOnly") };

            return true;
        }
        public override bool RunApplyStatusEvent(StatusEffectApply apply)
        {
            if (apply.applier == target && apply.effectData == effectToApply)
                apply.target.curveAnimator.Ping();
            return base.RunApplyStatusEvent(apply);
        }


        public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
        {
            if (!Check(entity))
                return false;

            return base.RunEntityDestroyedEvent(entity, deathType);
        }

        public bool Check(Entity entity)
        {
            if (entity == null || entity != target || entity?.lastHit?.attacker == null)
                return false;

            foreach (var cardData in cardConstrains)
            {
                return cardData.Check(entity?.lastHit.attacker);
            }

            return true;
        }
    }
}
