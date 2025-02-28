using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StatusEffectInstantSummonOnCertainSlot : StatusEffectInstant
{
    public StatusEffectSummon targetSummon;
    public bool summonCopy;

    public bool buildingToSummon;

    public int slotID;
    public Entity toSummon;
    public StatusEffectData[] withEffects;

    public override IEnumerator Process()
    {
        ActionQueue.Stack(
            new ActionSequence(TrySummon()) { note = "Instant Summon" },
            fixedPosition: true
        );

        yield return base.Process();
    }

    public IEnumerator TrySummon()
    {
        if (buildingToSummon)
        {
            yield return new WaitUntil(() => toSummon);
        }

        if (CanSummon(out var container, out var shoveData))
        {
            if (shoveData != null)
            {
                yield return ShoveSystem.DoShove(shoveData, updatePositions: true);
            }

            int amount = GetAmount();
            yield return toSummon
                ? targetSummon.SummonPreMade(
                    toSummon,
                    container,
                    applier.display.hover.controller,
                    applier,
                    withEffects,
                    amount
                )
                : (
                    summonCopy
                        ? targetSummon.SummonCopy(
                            target,
                            container,
                            applier.display.hover.controller,
                            applier,
                            withEffects,
                            amount
                        )
                        : targetSummon.Summon(
                            container,
                            applier.display.hover.controller,
                            applier,
                            withEffects,
                            amount
                        )
                );
        }
        else if (NoTargetTextSystem.Exists())
        {
            if ((bool)toSummon)
            {
                toSummon.RemoveFromContainers();
                Object.Destroy(toSummon);
            }

            yield return NoTargetTextSystem.Run(target, NoTargetType.NoSpaceToSummon);
        }

        yield return null;
    }

    public static IEnumerator ApplyEffects(
        Entity applier,
        Entity entity,
        IEnumerable<StatusEffectData> effects,
        int count
    )
    {
        Hit hit = new Hit(applier, entity, 0) { countsAsHit = false, canRetaliate = false };
        foreach (StatusEffectData effect in effects)
        {
            hit.AddStatusEffect(effect, count);
        }

        yield return hit.Process();
    }

    public bool CanSummon(
        out CardContainer container,
        out Dictionary<Entity, List<CardSlot>> shoveData
    )
    {
        bool result = false;
        container = null;
        shoveData = null;
        result = CanSummonInEnemyRow(out container, out shoveData);

        return result;
    }

    public static CardContainer GetSlotToShove(Entity entity)
    {
        if (entity.actualContainers.Count <= 0)
        {
            if (entity.preActualContainers.Length == 0)
            {
                return null;
            }

            return entity.preActualContainers.RandomItem();
        }

        return entity.actualContainers.RandomItem();
    }

    public bool CanSummonInEnemyRow(
        out CardContainer container,
        out Dictionary<Entity, List<CardSlot>> shoveData
    )
    {
        bool result = false;
        container = null;
        shoveData = null;
        CardSlot[] slots = References.Battle.allSlots.ToArray();

        if (slots != null)
        {
            container = slots[slotID];
            result = true;
        }

        return result;
    }
}
