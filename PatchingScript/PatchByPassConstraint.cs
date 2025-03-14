using System;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace DSTMod_WildFrost
{
	[HarmonyPatch(typeof(TargetConstraintHasHealth), nameof(TargetConstraintHasHealth.Check), new Type[] { typeof(Entity) })]
	public class PatchByConstraintHasHealth
	{
		static bool Prefix(ref bool __result, TargetConstraint __instance, Entity target)
		{
			if ((bool)target.FindStatus("dstmod.ByPass"))
			{
				__result = true;
				return false;
			}
			return true;
		}
	}
}