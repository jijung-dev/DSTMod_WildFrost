using System.Collections;
using UnityEngine;

public class StatusEffectInstantRandom : StatusEffectInstant
{
    public StatusEffectData[] effects;

    public override bool CanStackActions => false;

    public override IEnumerator Process()
    {
        int amount = GetAmount();
        Routine.Clump clump = new Routine.Clump();
        int ran = Random.Range(0, effects.Length);
        StatusEffectData ranEffect = effects[ran];

        if (!ranEffect.canBeBoosted || amount > 0)
        {
            clump.Add(StatusEffectSystem.Apply(target, applier, ranEffect, amount, temporary: false));
        }

        yield return clump.WaitForEnd();
        yield return base.Process();
    }
}
