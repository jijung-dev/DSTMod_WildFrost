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
            Events.OnCheckAction += CheckAction;
        }

        public void OnDestroy()
        {
            Events.OnCheckAction -= CheckAction;
        }

        public override bool RunHitEvent(Hit hit)
        {
            if (hit.target == target)
            {
                return hit.counterReduction > 0;
            }

            return false;
        }
        public void CheckAction(ref PlayAction action, ref bool allow)
        {
            if (allow && !target.silenced && action is ActionTrigger actionTrigger && Battle.IsOnBoard(target) && target == actionTrigger.entity && count > 0)
            {
                NoTargetTextSystem noText = NoTargetTextSystem.instance;
                if (noText != null)
                {
                    float num = noText.shakeDurationRange.Random();
                    actionTrigger.entity.curveAnimator.Move(noText.shakeAmount.WithX(noText.shakeAmount.x.WithRandomSign()), noText.shakeCurve, 1f, num);
                }
                allow = false;
            }
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
    }
}
