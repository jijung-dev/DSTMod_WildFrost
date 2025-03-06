using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(CardSlotLane), nameof(CardSlotLane.MoveChildrenForward))]
public class PatchUnshovable
{
	// private static bool Prefix(CardSlotLane __instance)
	// {
	// 	for (int i = 1; i < __instance.max; i++)
	// 	{
	// 		CardSlot cardSlot = __instance.slots[i];
	// 		Entity top = cardSlot.GetTop();
	// 		if (!top || top.positionPriority <= 0)
	// 		{
	// 			continue;
	// 		}
	// 		if (top.traits.Any(r => r.data == DSTMod.Instance.TryGet<TraitData>("Unshovable")))
	// 		{
	// 			top.positionPriority = 0;
	// 		}
	// 	}
	// 	return true;
	// }
}
