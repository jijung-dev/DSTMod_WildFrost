using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace DSTMod_WildFrost
{
    public class StatusEffectSanity : StatusEffectData
    {
        [SerializeField]
        public CardAnimation buildupAnimation = new Scriptable<CardAnimationShake>();
        public StatusEffectSummon[] shadowEnemy;
        public StatusEffectData summonRan;

        public bool Insaniting;

        public override void Init()
        {
            base.OnStack += Stack;
        }

        public IEnumerator Stack(int stacks)
        {
            yield return Check();
        }

        public IEnumerator Check()
        {
            var effect2 = target.FindStatus(DSTMod.Instance.TryGet<StatusEffectData>("Bloomness"));
            int current = effect2 != null ? effect2.count : target.hp.current;

            if (count >= current && !Insaniting)
            {
                yield return SummonEnemy();
                yield return Clear();
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

            VFXHelper.SFX.TryPlaySound("Sanity_Apply");

            clump.Add(Sequences.Wait(0.3f));
            yield return clump.WaitForEnd();

            if ((bool)buildupAnimation)
            {
                yield return buildupAnimation.Routine(target, 0.4f);
            }
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
