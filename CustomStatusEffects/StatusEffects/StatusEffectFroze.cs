using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deadpan.Enums.Engine.Components.Modding;
using TMPro;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectFroze : StatusEffectData
    {
        public override void Init()
        {
            base.OnHit += Hit;
            base.OnEntityDestroyed += RemoveEffect;
        }

        public override bool RunPreTriggerEvent(Trigger trigger)
        {
            if (trigger.entity == target && count > 0)
            {
                trigger.nullified = true;
                NoTargetTextSystem noText = NoTargetTextSystem.instance;
                if (noText != null)
                {
                    float num = noText.shakeDurationRange.Random();
                    target.curveAnimator.Move(noText.shakeAmount.WithX(noText.shakeAmount.x.WithRandomSign()), noText.shakeCurve, 1f, num);
                }
            }
            return false;
        }

        public override bool RunHitEvent(Hit hit)
        {
            if (hit.target == target)
            {
                return hit.counterReduction > 0;
            }

            return false;
        }

        public IEnumerator Hit(Hit hit)
        {
            while (hit.counterReduction > 0 && count > 0 && Battle.IsOnBoard(hit.target))
            {
                NoTargetTextSystem noText = NoTargetTextSystem.instance;
                if (noText != null)
                {
                    float num = noText.shakeDurationRange.Random();
                    hit.target.curveAnimator.Move(noText.shakeAmount.WithX(noText.shakeAmount.x.WithRandomSign()), noText.shakeCurve, 1f, num);
                    hit.counterReduction--;
                }
            }
            if (count <= 0)
            {
                yield return Remove();
            }
        }

        public IEnumerator RemoveEffect(Entity entity, DeathType deathType)
        {
            if (entity == target)
            {
                yield return Remove();
            }
        }

        public override bool CanTrigger() => target.enabled;

        public override int GetAmount()
        {
            if (!target)
            {
                return 0;
            }

            if (!canBeBoosted)
            {
                return count;
            }

            return Mathf.Max(0, Mathf.RoundToInt((count + target.effectBonus) * target.effectFactor));
        }
    }
}
