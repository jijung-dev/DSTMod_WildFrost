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
                while (count > 0)
                {
                    var effect = target.FindStatus(effectToReduce);

                    if (!(bool)effect)
                        break;

                    var count2 = effect.count;
                    effect.count -= count;
                    count -= count2;
                    if (effect.count <= 0)
                        yield return effect.Remove();
                }
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
