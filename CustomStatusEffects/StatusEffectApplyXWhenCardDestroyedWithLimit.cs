public class StatusEffectApplyXWhenCardDestroyedWithLimit : StatusEffectApplyXWhenCardDestroyed
{
	public int litmitCount;
	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		var statusEffect = target.FindStatus(effectToApply);

		if ((bool)statusEffect)
		{
			if (statusEffect.count <= litmitCount)
			{
				return base.RunEntityDestroyedEvent(entity, deathType);
			}
		}
		else
		{
			return base.RunEntityDestroyedEvent(entity, deathType);
		}

		return false;
	}
}