using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public List<int> multiSummonCheck = new List<int>();
    public StatusEffectSummonNoAnimation targetSummonNoAnimation;
    public StatusEffectSummon targetSummon;
    public bool summonCopy;
    public bool buildingToSummon;
    public bool isRandom;
    public bool isEnemySide;
    public Range randomRange => new Range(minRandomRange, maxRandomRange);
    public int maxRandomRange;
    public int minRandomRange;
    public bool queue = true;
    public int slotID;
    public Entity toSummon;
    public StatusEffectData[] withEffects;
    int amountToSpawn = 0;

    public override IEnumerator Process()
    {
        amountToSpawn = 1;
        if (canSummonMultiple)
        {
            Routine.Clump clump = new Routine.Clump();
            int amount = GetAmount();
            amountToSpawn = Check(References.Battle.allSlots.ToArray());
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
                new Routine(
                    CreateCopy(
                        target,
                        delegate(Entity e)
                        {
                            toSummon = e;
                        }
                    )
                );
            }

            ActionQueue.Stack(new ActionSequence(TrySummon()) { note = "Instant Summon" }, fixedPosition: true);
        }
        else
        {
            yield return TrySummon();
        }
        multiSummonCheck.Clear();

        yield return base.Process();
    }

    public IEnumerator CreateCopyAndTrySummon()
    {
        yield return CreateCopy(
            target,
            delegate(Entity e)
            {
                toSummon = e;
            }
        );
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
            card =
                targetSummon != null
                    ? targetSummon.CreateCardCopy(target.data, container, applier.display.hover.controller)
                    : targetSummonNoAnimation.CreateCardCopy(target.data, container, applier.display.hover.controller);
            card.entity.startingEffectsApplied = true;
            yield return card.UpdateData();
            yield return targetSummon != null
                ? targetSummon.CopyStatsAndEffects(card.entity, toCopy)
                : targetSummonNoAnimation.CopyStatsAndEffects(card.entity, toCopy);
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
            if (targetSummon != null)
            {
                yield return toSummon
                    ? targetSummon.SummonPreMade(toSummon, container, applier.display.hover.controller, applier, withEffects, amount)
                    : (
                        summonCopy
                            ? targetSummon.SummonCopy(target, container, applier.display.hover.controller, applier, withEffects, amount)
                            : targetSummon.Summon(container, applier.display.hover.controller, applier, withEffects, amount)
                    );
            }
            else
            {
                yield return toSummon
                    ? targetSummonNoAnimation.SummonPreMade(toSummon, container, applier.display.hover.controller, applier, withEffects, amount)
                    : (
                        summonCopy
                            ? targetSummonNoAnimation.SummonCopy(target, container, applier.display.hover.controller, applier, withEffects, amount)
                            : targetSummonNoAnimation.Summon(container, applier.display.hover.controller, applier, withEffects, amount)
                    );
            }
        }
        else if (NoTargetTextSystem.Exists())
        {
            if ((bool)toSummon)
            {
                toSummon.RemoveFromContainers();
                UnityEngine.Object.Destroy(toSummon);
            }

            yield return NoTargetTextSystem.Run(target, NoTargetType.NoSpaceToSummon);
        }

        yield return null;
    }

    public static IEnumerator ApplyEffects(Entity applier, Entity entity, IEnumerable<StatusEffectData> effects, int count)
    {
        Hit hit = new Hit(applier, entity, 0) { countsAsHit = false, canRetaliate = false };
        foreach (StatusEffectData effect in effects)
        {
            hit.AddStatusEffect(effect, count);
        }

        yield return hit.Process();
    }

    public bool CanSummon(out CardContainer container, out Dictionary<Entity, List<CardSlot>> shoveData)
    {
        bool result = CanSummonInSlotID(out container, out shoveData);

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

    public bool CanSummonInSlotID(out CardContainer container, out Dictionary<Entity, List<CardSlot>> shoveData)
    {
        container = null;
        shoveData = null;
        CardSlot[] slots = References.Battle.allSlots.ToArray();
        if (amountToSpawn <= 0)
            return false;

        if (isRandom)
        {
            if (randomRange == null)
                throw new NullReferenceException("No Random Range Found!");
            do
            {
                slotID = GetRandomInt(randomRange.min, randomRange.max);
            } while (slotID == 3 || slotID == 7 || multiSummonCheck.Contains(slotID));
        }

        Entity top = slots[slotID].GetTop();

        if (top == null || ShoveSystem.CanShove(top, target.owner.entity, out shoveData))
        {
            amountToSpawn--;
            multiSummonCheck.Add(slotID);
            container = slots[slotID];
            return true;
        }
        return false;
    }

    public static int GetRandomInt(int min, int max)
    {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] data = new byte[4]; // 4 bytes for an int
            rng.GetBytes(data);
            int value = BitConverter.ToInt32(data, 0) & int.MaxValue; // Ensure positive number
            return min + (value % (max - min + 1)); // Scale to range
        }
    }

    public int Check(CardSlot[] slots)
    {
        int flag = 0;
        if (!isRandom && !isEnemySide)
        {
            minRandomRange = 0;
            maxRandomRange = 7;
        }

        for (int i = randomRange.min; i < randomRange.max; i++)
        {
            if (slots[i].Empty)
            {
                flag++;
            }
        }
        Debug.LogWarning($"{flag} free slot to spawn nice");
        return flag;
    }
}
