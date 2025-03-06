using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXToUnitWhenDestroyedByCertainCards : StatusEffectApplyXWhenDestroyed
    {
        public TargetConstraint[] cardConstrains;
        public Entity chest;
        public string cardToRequire = "tgestudio.wildfrost.dstmod.chest";

        public override void Init()
        {
            base.OnEntityDestroyed += AddEffect;
        }

        private IEnumerator AddEffect(Entity entity, DeathType deathType)
        {
            if (!Check(entity))
                yield break;

            if (chest == null)
                foreach (Entity card in References.Battle.cards)
                {
                    if (card.data.name == cardToRequire)
                    {
                        Debug.Log("[Don't Frostbite] Found the Chest");
                        chest = card;
                        break;
                    }
                }

            Routine.Clump clump = new Routine.Clump();

            Hit hit = new Hit(applier, chest);
            hit.AddStatusEffect(effectToApply, GetAmount());

            clump.Add(hit.Process());

            yield return clump.WaitForEnd();
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
