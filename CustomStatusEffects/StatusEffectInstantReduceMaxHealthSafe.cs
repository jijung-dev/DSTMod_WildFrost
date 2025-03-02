using System.Collections;

public class StatusEffectInstantReduceMaxHealthSafe : StatusEffectInstantReduceMaxHealth
{
	public override IEnumerator Process()
	{
		if (target.hp.max - GetAmount() <= 1)
		{
			target.hp.max = GetAmount() + 1;
			target.hp.current = GetAmount() + 1;
			target.PromptUpdate();
		}
		yield return base.Process();
	}
}