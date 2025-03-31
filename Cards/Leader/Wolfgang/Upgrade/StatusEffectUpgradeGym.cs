using System;

public class StatusEffectUpgradeGym : StatusEffectData, IUpgrade
{
	public void Run()
	{
		var effect = Array.Find(References.LeaderData.startWithEffects, r => r.data is StatusEffectMightiness).data as StatusEffectMightiness;
		effect.cap = 15;
	}
}