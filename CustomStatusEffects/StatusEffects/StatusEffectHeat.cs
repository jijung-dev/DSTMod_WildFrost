using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace DSTMod_WildFrost
{
    public class StatusEffectHeat : StatusEffectData
    {
        public StatusEffectData overheatingEffect;
        public StatusEffectData freezeEffect;
        public StatusEffectData frozeEffect;

        public bool Overheating;

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
            StatusEffectData freezeEffectData = target.FindStatus(freezeEffect);
            if ((bool)freezeEffectData)
            {
                yield return freezeEffectData.Remove();
            }

            StatusEffectData frozeEffectData = target.FindStatus(frozeEffect);
            if ((bool)frozeEffectData)
            {
                yield return frozeEffectData.Remove();
            }
        }

        public IEnumerator Stack(int stacks)
        {
            Check();
            yield return null;
        }

        public void Check()
        {
            if (count >= target.hp.max && !Overheating)
            {
                ActionQueue.Stack(
                    new ActionSequence(Overheat())
                    {
                        fixedPosition = true,
                        priority = eventPriority,
                        note = "Overheating",
                    }
                );
                ActionQueue.Stack(
                    new ActionSequence(Clear())
                    {
                        fixedPosition = true,
                        priority = eventPriority,
                        note = "Clear Heating",
                    }
                );
                Overheating = true;
            }
        }

        public IEnumerator RemoveFreeze(StatusEffectData statusEffect)
        {
            yield return statusEffect.Remove();
        }

        public IEnumerator Overheat()
        {
            if (!this || !target || !target.alive)
            {
                yield break;
            }

            VFXHelper.SFX.TryPlaySound("Heat_Apply");
            Routine.Clump clump = new Routine.Clump();

            Hit hit = new Hit(applier, target, 0) { damageType = "dst.overheating" };

            hit.AddStatusEffect(overheatingEffect, 1);

            clump.Add(hit.Process());
            clump.Add(Sequences.Wait(0.3f));
            yield return clump.WaitForEnd();
        }

        public IEnumerator Clear()
        {
            if ((bool)this && (bool)target && target.alive)
            {
                yield return Remove();
                Overheating = false;
            }
        }
    }
}
