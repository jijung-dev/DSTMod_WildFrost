using System.Collections;

public class StatusEffectInstantIncreaseCounter : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		target.counter.current += GetAmount();
		target.PromptUpdate();
		yield return base.Process();
	}
}