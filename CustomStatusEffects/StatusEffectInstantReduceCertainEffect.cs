using System.Collections;

namespace DSTMod_WildFrost
{
    public class StatusEffectInstantReduceCertainEffect : StatusEffectInstant
    {
        public StatusEffectData effectToReduce;
        public bool isHit;

        public override IEnumerator Process()
        {
            if (!isHit)
            {
                var effect = target.FindStatus(effectToReduce);

                if (!(bool)effect) yield break;

                target.FindStatus(effectToReduce).count -= GetAmount();

                if (target.FindStatus(effectToReduce).count <= 0)
                    yield return target.FindStatus(effectToReduce).Remove();

                target.PromptUpdate();
            }
            else
            {
                Routine.Clump clump = new Routine.Clump();
                Hit hit = new Hit(applier, target, GetAmount());

                clump.Add(hit.Process());
                yield return clump.WaitForEnd();
            }
            yield return base.Process();
        }
    }
}
