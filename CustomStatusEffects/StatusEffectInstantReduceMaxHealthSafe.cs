using System.Collections;

public class StatusEffectInstantReduceMaxHealthSafe : StatusEffectInstantReduceMaxHealth
{
	public override IEnumerator Process()
	{
		if (target.hp.max > 1)
			yield return base.Process();
	}
}