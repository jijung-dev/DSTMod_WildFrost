using System;
using System.Collections;
using System.Linq;
using DSTMod_WildFrost;
using DSTMod_WildFrost.PatchingScript;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(ShoveSystem))]
public class StatusEffectUnshovable : StatusEffectData
{
	// public override void Init()
	// {
	// 	Events.OnCheckAction += CheckAction;
	// }
	// public void OnDestroy()
	// {
	// 	Events.OnCheckAction -= CheckAction;
	// }
	public override bool RunBeginEvent()
	{
		target.positionPriority = 0;
		return false;
	}

	// private void CheckAction(ref PlayAction action, ref bool allow)
	// {
	// 	if (allow && !target.silenced && action is ActionMove actionMove && Battle.IsOnBoard(target) && Battle.IsOnBoard(actionMove.toContainers))
	// 	{
	// 		foreach (var container in actionMove.toContainers)
	// 		{
	// 			if (container.entities.Contains(target))
	// 			{
	// 				allow = false;
	// 				if (NoTargetTextSystem.Exists())
	// 				{
	// 					new Routine(NoTargetTextSystemExt.Run(target, NoTargetTypeExt.CantShove));
	// 				}
	// 			}
	// 		}
	// 	}
	// }
	bool isPlayed;

	[HarmonyPostfix]
	[HarmonyPatch(nameof(ShoveSystem.FindSlots))]
	static CardSlot[] FindSlots(CardSlot[] result, Entity shovee)
	{
		if (shovee.statusEffects.Any(s => s is StatusEffectUnshovable))
		{
			Debug.Log($"Preventing [{shovee}] from having slots to shove to?");
			return null;
		}
		return result;
	}

	public override void Init()
	{
		Events.OnCheckEntityShove += CheckEntityShove;
		Events.OnEntityRelease += EntityRelease;
	}

	private void EntityRelease(Entity arg0)
	{
		isPlayed = false;
	}

	public void OnDestroy()
	{
		Events.OnCheckEntityShove -= CheckEntityShove;
		Events.OnEntityRelease += EntityRelease;
	}

	public void CheckEntityShove(ref Entity entity, ref bool flag)
	{
		if (entity == target && target.enabled && !target.silenced)
		{
			if (NoTargetTextSystem.Exists() && !isPlayed)
			{
				isPlayed = true;
				new Routine(NoTargetTextSystemExt.Run(target, NoTargetTypeExt.CantShove));
			}
			flag = false;
		}
	}
}