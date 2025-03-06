using System;
using System.Collections;
using UnityEngine;

public class StatusEffectApplyXAfterTurn : StatusEffectApplyX
{
	public bool turnTaken;
	public override void Init()
	{
		Events.OnEntityTriggered += TurnEnd;
		base.OnTurnEnd += CheckTurn;
	}
	public void OnDestroy()
	{
		Events.OnEntityTriggered += TurnEnd;
	}
	private IEnumerator CheckTurn(Entity entity)
	{
		yield return Run(GetTargets());
		turnTaken = true;
	}
	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (turnTaken && target.enabled && entity == target && Battle.IsOnBoard(target))
		{
			turnTaken = false;
			return true;
		}

		return false;
	}

	private void TurnEnd(ref Trigger trigger)
	{
		if (trigger.entity == target && !target.silenced && turnTaken)
		{
			turnTaken = false;
			RunCardPlayedEvent(trigger.entity, new Entity[] { target });
		}
	}
}