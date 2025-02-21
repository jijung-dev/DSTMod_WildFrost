using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DSTMod_WildFrost.PatchingScript
{
    public class NoTargetTextSystemExt
    {
        private Vector2 shakeDurationRange;
        private Vector3 shakeAmount;
        private AnimationCurve shakeCurve;
        private TMP_Text textElement;

        public NoTargetTextSystemExt() { }

        public IEnumerator _Run(Entity entity, NoTargetTypeExt type, int args)
        {
            shakeDurationRange = NoTargetTextSystem.instance.shakeDurationRange;
            shakeAmount = NoTargetTextSystem.instance.shakeAmount;
            shakeCurve = NoTargetTextSystem.instance.shakeCurve;
            textElement = NoTargetTextSystem.instance.textElement;

            if (NoTargetTextSystem.instance.enabled)
            {
                yield return Sequences.WaitForAnimationEnd(entity);
                float num = shakeDurationRange.Random();
                entity.curveAnimator.Move(
                    shakeAmount.WithX(shakeAmount.x.WithRandomSign()),
                    shakeCurve,
                    1f,
                    num
                );
                textElement.text = (type == NoTargetTypeExt.None) ? "" : GetStringType(type, args);
                NoTargetTextSystem.instance.PopText(entity.transform.position);
                yield return new WaitForSeconds(num);
            }
        }

        public string GetStringType(NoTargetTypeExt type, int args)
        {
            string text;
            switch (type)
            {
                case NoTargetTypeExt.RequireGold:
                    text = $"Require {args} Gold";
                    break;
                case NoTargetTypeExt.RequireRock:
                    text = $"Require {args} Rock";
                    break;
                case NoTargetTypeExt.RequireWood:
                    text = $"Require {args} Wood";
                    break;
                default:
                    text = "";
                    break;
            }
            return text;
        }
    }

    public enum NoTargetTypeExt
    {
        None,
        RequireRock,
        RequireGold,
        RequireWood,
    }
}
