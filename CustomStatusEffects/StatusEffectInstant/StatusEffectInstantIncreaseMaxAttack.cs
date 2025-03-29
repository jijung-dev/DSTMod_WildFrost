using System.Collections;

public class StatusEffectInstantIncreaseMaxAttack : StatusEffectInstant
{
    public override IEnumerator Process()
    {
        target.damage.current += GetAmount();
        target.damage.max += GetAmount();
        target.PromptUpdate();
        yield return base.Process();
    }
}
