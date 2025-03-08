using System.Collections;
using UnityEngine;

public class NoCreateCardAnimation : CreateCardAnimation
{
    public static readonly Vector3 startPos = new Vector3(-10f, 0f, 0f);

    public override IEnumerator Run(Entity entity, params CardData.StatusEffectStacks[] withEffects)
    {
        if (entity.display is Card card)
        {
            card.canvasGroup.alpha = 1f;
        }
        yield return CreateCardAnimation.GainEffects(entity, withEffects);
        base.gameObject.Destroy();
    }
}
