using System.Collections;
using UnityEngine;

public class StatusEffectResource : StatusEffectData
{
    public CardData[] allowedCards;

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
        if (allowedCards == null)
            yield break;

        foreach (CardData card in allowedCards)
        {
            if (hit.attacker?.name == card.name)
            {
                count -= hit.damage;
            }
            else
            {
                hit.damageBlocked = hit.damage;
                hit.damage = 0;
            }
        }

        if (count <= 0)
        {
            yield return Remove();
        }

        target.PromptUpdate();
    }
}
