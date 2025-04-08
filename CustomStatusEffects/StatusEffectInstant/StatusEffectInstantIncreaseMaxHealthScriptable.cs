using System.Collections;

public class StatusEffectInstantIncreaseMaxHealthScriptable : StatusEffectInstant
{
	public bool alsoIncreaseCurrentHealth = true;
	public ScriptableAmount scriptableAmount;

	public override IEnumerator Process()
	{
		target.hp.max += (scriptableAmount ? scriptableAmount.Get(target) : GetAmount());
		if (alsoIncreaseCurrentHealth)
		{
			target.hp.current += (scriptableAmount ? scriptableAmount.Get(target) : GetAmount());
		}
		target.PromptUpdate();
		yield return base.Process();
	}
}