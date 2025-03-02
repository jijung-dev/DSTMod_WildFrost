using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StatusEffectInstantSummonOnCertainSlot : StatusEffectInstant
{
    public class Range
    {
        public int min;
        public int max;
        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        public Range() { }
    }
    public bool canSummonMultiple;
    public StatusEffectSummon targetSummon;
    public bool summonCopy;
    public bool buildingToSummon;
    public bool isRandom;
    public Range randomRange;
    public bool queue = true;
    public int slotID;
    public Entity toSummon;
    public StatusEffectData[] withEffects;

    public override IEnumerator Process()
    {
        if (canSummonMultiple)
        {
            Routine.Clump clump = new Routine.Clump();
            int amount = GetAmount();
            for (int i = 0; i < amount; i++)
            {
                if (summonCopy)
                {
                    clump.Add(CreateCopyAndTrySummon());
                }
                else
                {
                    clump.Add(TrySummon());
                }
            }

            yield return clump.WaitForEnd();
        }
        else if (queue)
        {
            if (summonCopy)
            {
                new Routine(CreateCopy(target, delegate (Entity e)
                {
                    toSummon = e;
                }));
            }

            ActionQueue.Stack(new ActionSequence(TrySummon())
            {
                note = "Instant Summon"
            }, fixedPosition: true);
        }
        else
        {
            yield return TrySummon();
        }

        yield return base.Process();
    }
    public IEnumerator CreateCopyAndTrySummon()
    {
        yield return CreateCopy(target, delegate (Entity e)
        {
            toSummon = e;
        });
        if ((bool)toSummon)
        {
            yield return TrySummon();
        }
    }
    public IEnumerator CreateCopy(Entity toCopy, UnityAction<Entity> onComplete)
    {
        buildingToSummon = true;
        Card card = null;
        if (CanSummon(out var container, out var _))
        {
            card = targetSummon.CreateCardCopy(target.data, container, applier.display.hover.controller);
            card.entity.startingEffectsApplied = true;
            yield return card.UpdateData();
            yield return targetSummon.CopyStatsAndEffects(card.entity, toCopy);
        }

        buildingToSummon = false;
        onComplete?.Invoke(card ? card.entity : null);
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
        container = null;
        shoveData = null;
        CardSlot[] slots = References.Battle.allSlots.ToArray();
        List<int> freeSlots = new List<int>();

        if (isRandom)
        {
            if (randomRange == null) randomRange = new Range(0, slots.Count());

            for (int i = randomRange.min; i < randomRange.max; i++)
            {
                if (slots[i].GetTop() != null) continue;
                freeSlots.Add(i);
            }
            slotID = freeSlots[Random.Range(0, freeSlots.Count)];
        }

        if (slots[slotID].GetTop() != null)
        {
            return false;
        }

        container = slots[slotID];
        return true;
    }
}
