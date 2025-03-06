using System.Collections;
using UnityEngine;

public class SideFlipCreateCardAnimation : CreateCardAnimation
{
    public static readonly Vector3 startPos = new Vector3(-10f, 0f, 0f);

    public override IEnumerator Run(Entity entity, params CardData.StatusEffectStacks[] withEffects)
    {
        entity.transform.localScale = Vector3.zero;
        entity.transform.position = startPos;
        if (entity.display is Card card)
        {
            card.canvasGroup.alpha = 1f;
        }

        //entity.flipper.FlipDownInstant();
        entity.curveAnimator.Ping();
        yield return CreateCardAnimation.GainEffects(entity, withEffects);
        //entity.flipper.FlipUp();
        entity.wobbler.WobbleRandom();
        base.gameObject.Destroy();
    }
}