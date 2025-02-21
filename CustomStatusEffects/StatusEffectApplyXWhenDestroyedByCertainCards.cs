using UnityEditor;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXWhenDestroyedByCertainCards : StatusEffectApplyXWhenDestroyed
    {
        public CardData[] cardsToApply;

        public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
        {
            foreach (CardData card in cardsToApply)
            {
                if (entity != target || entity?.lastHit.attacker?.name != card.name)
                    continue;

                return base.RunEntityDestroyedEvent(entity, deathType);
            }

            return false;
        }
    }
}
