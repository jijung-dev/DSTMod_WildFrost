using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace DSTMod_WildFrost
{
    public class StatusEffectSanity : StatusEffectData
    {
        [SerializeField]
        public CardAnimation buildupAnimation = new Scriptable<CardAnimationOverburn>();
        public StatusEffectSummon[] shadowEnemy;
        public StatusEffectData summonRan;

        public bool Insaniting;

        public override void Init()
        {
            base.OnStack += Stack;
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

        public IEnumerator Stack(int stacks)
        {
            Check();
            yield return null;
        }

        public void Check()
        {
            if (count >= target.hp.current && !Insaniting)
            {
                ActionQueue.Stack(
                    new ActionSequence(SummonEnemy())
                    {
                        fixedPosition = true,
                        priority = eventPriority,
                        note = "Sanity",
                    }
                );
                ActionQueue.Stack(
                    new ActionSequence(Clear())
                    {
                        fixedPosition = true,
                        priority = eventPriority,
                        note = "Clear Sanity",
                    }
                );
                Insaniting = true;
            }
        }

        public IEnumerator SummonEnemy()
        {
            if (!this || !target || !target.alive)
            {
                yield break;
            }

            Routine.Clump clump = new Routine.Clump();

            Hit hit = new Hit(applier, target, 0) { damageType = "dst.sanity" };

            hit.AddStatusEffect(summonRan, 1);
            clump.Add(hit.Process());

            VFXMod.instance.SFX.TryPlaySoundFromPath(DSTMod.Instance.ImagePath("Sanity_Apply.wav"));

            if ((bool)buildupAnimation)
            {
                yield return buildupAnimation.Routine(target);
            }
            clump.Add(Sequences.Wait(0.3f));
            yield return clump.WaitForEnd();
        }

        public IEnumerator Clear()
        {
            if ((bool)this && (bool)target && target.alive)
            {
                yield return Remove();
                Insaniting = false;
            }
        }
    }
}
