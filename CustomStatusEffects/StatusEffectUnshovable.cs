using System;
using System.Collections;
using DSTMod_WildFrost.PatchingScript;
using UnityEngine;

public class StatusEffectUnshovable : StatusEffectData
{
	public override void Init()
	{
		Events.OnCheckAction += CheckAction;
	}

	public void OnDestroy()
	{
		Events.OnCheckAction -= CheckAction;
	}

	public void CheckAction(ref PlayAction action, ref bool allow)
	{
		if (allow && !target.silenced && action is ActionMove actionMove && Battle.IsOnBoard(target) && Battle.IsOnBoard(actionMove.toContainers))
		{
			foreach (var container in actionMove.toContainers)
			{
				if (container.entities.Contains(target))
				{
					allow = false;
					if (NoTargetTextSystem.Exists())
					{
						new Routine(NoTargetTextSystemExt.Run(target, NoTargetTypeExt.CantShove));
					}
				}
			}
		}
	}
}