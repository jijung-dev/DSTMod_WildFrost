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
        private static Vector2 shakeDurationRange;
        private static Vector3 shakeAmount;
        private static AnimationCurve shakeCurve;
        private static TMP_Text textElement;

        public NoTargetTextSystemExt() { }

        public static IEnumerator Run(Entity entity, NoTargetTypeExt type)
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
                textElement.text = (type == NoTargetTypeExt.None) ? "" : GetStringType(type);
                NoTargetTextSystem.instance.PopText(entity.transform.position);
                yield return new WaitForSeconds(num);
            }
        }

        public static string GetStringType(NoTargetTypeExt type)
        {
            string text;
            switch (type)
            {
                case NoTargetTypeExt.RequireGold:
                    text = $"Require Gold";
                    break;
                case NoTargetTypeExt.RequireRock:
                    text = $"Require Rock";
                    break;
                case NoTargetTypeExt.RequireWood:
                    text = $"Require Wood";
                    break;
                case NoTargetTypeExt.CantShove:
                    text = $"Cannot Shove";
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
        CantShove,
        RequireRock,
        RequireGold,
        RequireWood,
    }
}
