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
    public class StatusEffectFreeze : StatusEffectData
    {
        [SerializeField]
        public CardAnimation buildupAnimation = ScriptableObject.CreateInstance<CardAnimationOverburn>();
        public StatusEffectData tempTrait;
        public StatusEffectData heatEffect;
        public StatusEffectData frozeEffect;
        public StatusEffectData freezeEffect;

        public bool Freezing;

        public override void Init()
        {
            base.OnStack += Stack;
            base.OnBegin += Begin;

            Events.OnEntityDisplayUpdated += EntityDisplayUpdated;
        }

        public void OnDestroy()
        {
            Events.OnEntityDisplayUpdated -= EntityDisplayUpdated;
        }

        public void EntityDisplayUpdated(Entity entity)
        {
            if (entity == target && target.enabled)
            {
                Check();
            }
        }

        public IEnumerator Begin()
        {
            StatusEffectData statusEffectFrozeData = target.FindStatus(frozeEffect);
            StatusEffectData statusEffectFreezeData = target.FindStatus(freezeEffect);

            if ((bool)statusEffectFrozeData)
            {
                yield return statusEffectFreezeData.Remove();
            }

            StatusEffectData statusEffectData = target.FindStatus(heatEffect);
            if ((bool)statusEffectData && statusEffectData.count > 0)
            {
                yield return statusEffectData.Remove();
            }
        }

        public IEnumerator Stack(int stacks)
        {
            Check();
            yield return null;
        }

        public void Check()
        {
            if (count >= target.hp.max && !Freezing)
            {
                ActionQueue.Stack(
                    new ActionSequence(Freeze())
                    {
                        fixedPosition = true,
                        priority = eventPriority,
                        note = "Freeze",
                    }
                );
                ActionQueue.Stack(
                    new ActionSequence(Clear())
                    {
                        fixedPosition = true,
                        priority = eventPriority,
                        note = "Clear Freeze",
                    }
                );
                Freezing = true;
            }
        }

        public IEnumerator RemoveHeat(StatusEffectData statusEffect)
        {
            yield return statusEffect.Remove();
        }

        public IEnumerator Freeze()
        {
            if (!this || !target || !target.alive)
            {
                yield break;
            }

            if ((bool)buildupAnimation)
            {
                yield return buildupAnimation.Routine(target);
            }

            Routine.Clump clump = new Routine.Clump();

            // var instantSummonRandomEffect = ScriptableObject.CreateInstance<instant>();
            // Hit hit = new Hit(applier, target, 0)
            // {
            //     damageType = "the statusType"
            // };
            // hit.AddStatusEffect(instantSummonRandomEffect, 1);

            Hit hit = new Hit(applier, target, 0) { damageType = "dst.freezing" };
            hit.AddStatusEffect(tempTrait, 1);

            clump.Add(hit.Process());

            // if ((bool)buildupAnimation)
            // {
            //     yield return buildupAnimation.Routine(target);
            // }
            clump.Add(Sequences.Wait(0.3f));
            yield return clump.WaitForEnd();
        }

        public IEnumerator Clear()
        {
            if ((bool)this && (bool)target && target.alive)
            {
                yield return Remove();
                Freezing = false;
            }
        }
    }
}
