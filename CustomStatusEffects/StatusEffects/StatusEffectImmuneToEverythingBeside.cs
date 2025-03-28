using System;
using System.Linq;
using DSTMod_WildFrost;

public class StatusEffectImmuneToEverythingBeside : StatusEffectData
{
	public StatusEffectData[] bypass;
	public Type bypassType;

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if ((bool)apply.effectData && apply.target == target && !bypass.Contains(apply.effectData) && bypassType != null && apply.effectData.GetType() != bypassType)
		{
			apply.effectData = null;
			apply.count = 0;
		}
		return false;
	}
}
