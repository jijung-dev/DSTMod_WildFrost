using System.Collections;
using UnityEngine;

public class StatusEffectImmuneToDamageFromCertainCard : StatusEffectData
{
    public TargetConstraint[] notAllowedCards;

    public override void Init()
    {
        base.OnHit += Check;
    }

    public override bool RunHitEvent(Hit hit)
    {
        if (hit.target == target && hit.damage > 0)
        {
            return !hit.nullified;
        }

        return false;
    }

    public IEnumerator Check(Hit hit)
    {
        if (notAllowedCards == null)
            yield break;

        foreach (var card in notAllowedCards)
        {
            if (hit.attacker == null)
                yield break;

            if (!card.Check(hit.attacker))
            {
                hit.damageBlocked = hit.damage;
                hit.damage = 0;
            }
        }

        target.PromptUpdate();
    }
}
