using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;
using static StatusEffectInstantSummon;

namespace DSTMod_WildFrost
{
    [HarmonyPatch(typeof(StatusEffectInstantSummon), nameof(StatusEffectInstantSummon.CanSummon))]
    public class PatchCheckSummonSpot
    {
        static bool Prefix(ref bool __result, StatusEffectInstantSummon __instance)
        {
            List<CardContainer> rows = GetRows(__instance.summonPosition, __instance.target);
            if (rows == null)
            {
                return true;
            }

            List<CardSlot> list = new List<CardSlot>();
            foreach (CardContainer item in rows)
            {
                if (item is CardSlotLane cardSlotLane)
                {
                    list.AddRange(cardSlotLane.slots.Where((CardSlot slot) => slot.Empty));
                }
            }
            var actionCount = ActionQueue.GetActions().OfType<ActionSequence>().Count(r => r.Name.Contains("Instant Summon"));
            if (list.Count < actionCount)
            {
                Debug.LogWarning("No space so nuh uh");
                __result = false;
                return false;
            }

            return true;
        }

        // static bool Prefix(ref IEnumerator __result, StatusEffectInstantSummon __instance)
        // {
        //     List<CardContainer> rows = GetRows(__instance.summonPosition, __instance.target);
        //     if (rows == null)
        //     {
        //         return true;
        //     }
        //     Debug.LogWarning("What2");

        //     List<CardSlot> list = new List<CardSlot>();
        //     foreach (CardContainer item in rows)
        //     {
        //         if (item is CardSlotLane cardSlotLane)
        //         {
        //             Debug.LogWarning("What3");
        //             list.AddRange(cardSlotLane.slots.Where((CardSlot slot) => slot.Empty));
        //         }
        //     }
        //     Debug.LogWarning("What4");
        //     var actionCount = ActionQueue.GetActions().OfType<ActionSequence>().Count(r => r.Name.Contains("Instant Summon"));
        //     if (list.Count < actionCount)
        //     {
        //         Debug.LogWarning("No space so nuh uh");
        //         __result = TrySummon(__instance);
        //         return false;
        //     }
        //     Debug.LogWarning("What5");
        //     return true;
        // }
        // public static IEnumerator TrySummon(StatusEffectInstantSummon __instance)
        // {
        //     if (__instance.buildingToSummon)
        //     {
        //         yield return new WaitUntil(() => __instance.toSummon);
        //     }
        //     if (NoTargetTextSystem.Exists())
        //     {
        //         if ((bool)__instance.toSummon)
        //         {
        //             __instance.toSummon.RemoveFromContainers();
        //             UnityEngine.Object.Destroy(__instance.toSummon);
        //         }

        //         yield return NoTargetTextSystem.Run(__instance.target, NoTargetType.NoSpaceToSummon);
        //     }
        //     yield return null;
        // }

        public static List<CardContainer> GetRows(Position position, Entity target)
        {
            switch (position)
            {
                case Position.EnemyRow:
                    return target.owner == Battle.instance.player
                        ? References.Battle.GetRows(Battle.instance.enemy)
                        : References.Battle.GetRows(Battle.instance.player);
                case Position.InFrontOf:
                case Position.InFrontOfOrOtherRow:
                case Position.AppliersPosition:
                    return References.Battle.GetRows(target.owner);
                case Position.Hand:
                    return null;
                default:
                    throw new Exception("Row not found");
            }
        }
    }
}
