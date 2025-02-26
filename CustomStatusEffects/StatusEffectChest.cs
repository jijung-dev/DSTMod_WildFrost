using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost.PatchingScript;
using UnityEngine;

public class StatusEffectChest : StatusEffectData
{
    public bool isRemoved;
    public TraitData traitData;

    public override void Init()
    {
        base.OnBegin += Begin;
        Events.OnEntityDisplayUpdated += OnEntityDisplayUpdated;
    }

    public void OnDestroy()
    {
        Events.OnEntityDisplayUpdated -= OnEntityDisplayUpdated;
    }

    public void OnEntityDisplayUpdated(Entity entity)
    {
        if (entity == target && target.enabled)
        {
            Begin();
        }
    }

    public IEnumerator Begin()
    {
        StatusEffectData statusEffectData = target.FindStatus("Temporary Summoned");
        if ((bool)statusEffectData && isRemoved)
        {
            yield return statusEffectData.Remove();
            // var traits = target.traits.FirstOrDefault(r => r.data == traitData);

            // // Debug.Log("> [" + base.name + " " + traits.data.name + "] Removed! Removing effects [" + string.Join(", ", traits.passiveEffects) + "]");
            // // target.traits.Remove(traits);

            // // yield return traits.DisableEffects();

            // target.traits.FirstOrDefault(r => r.data == traitData).count --;

            // yield return target.UpdateTraits(traits);

            // //yield return target.Reset();
        }
    }
}
