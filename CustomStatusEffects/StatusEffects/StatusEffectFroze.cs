using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectFroze : StatusEffectData
    {
        public CardAnimation buildupAnimation = ScriptableObject.CreateInstance<CardAnimationOverburn>();

        public override void Init()
        {
            base.OnHit += Hit;
            base.OnEntityDestroyed += RemoveEffect;
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
                if ((bool)buildupAnimation)
                {
                    yield return buildupAnimation.Routine(target);
                }
                hit.counterReduction--;
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
