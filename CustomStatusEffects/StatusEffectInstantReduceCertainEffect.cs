using System.Collections;

namespace DSTMod_WildFrost
{
    public class StatusEffectInstantReduceCertainEffect : StatusEffectInstant
    {
        public StatusEffectData effectToReduce;

        public override IEnumerator Process()
        {
            target.FindStatus(effectToReduce).count -= GetAmount();

            if (target.FindStatus(effectToReduce).count <= 0)
                yield return target.FindStatus(effectToReduce).Remove();

            target.PromptUpdate();
            yield return base.Process();
        }
    }
}
