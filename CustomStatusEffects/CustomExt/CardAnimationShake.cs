using System.Collections;
using UnityEngine;
using static HitFlashSystem;

public class CardAnimationShake : CardAnimation
{
    public override IEnumerator Routine(object data, float startDelay = 0f)
    {
        if (data is Entity target)
        {
            yield return new WaitForSeconds(startDelay);
            CurveAnimator curveAnimator = target.curveAnimator;

            var mat = new Material(HitFlashSystem.instance.damageMaterial);
            mat.color = new Color(0f, 0f, 0f);

            HitFlashSystem.instance.list.Add(new HitFlash(target, mat, HitFlashSystem.instance.flashDuration * 3));

            if ((object)curveAnimator != null)
            {
                curveAnimator.Move(Vector3.one * 0.5f, Curves.Get("TakeHit"), 0.3f);
            }
        }
    }
}
