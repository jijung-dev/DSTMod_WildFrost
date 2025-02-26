using System;
using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using HarmonyLib;

[HarmonyPatch(typeof(ShoveSystem), "CanShoveTo")]
public class ShoveToFrontline
{
    private static bool Prefix(
        Entity shovee,
        Entity shover,
        int dir,
        CardSlot[] slots,
        out Dictionary<Entity, List<CardSlot>> shoveData,
        ref bool __result
    )
    {
        shoveData = new Dictionary<Entity, List<CardSlot>>();
        bool flag = false;
        if (
            !(DSTMod.Instance).HasLoaded
            || (Object)(object)shovee.owner == (Object)(object)Battle.instance.enemy
        )
        {
            return true;
        }

        int num = 1;
        Queue<KeyValuePair<Entity, CardSlot[]>> queue =
            new Queue<KeyValuePair<Entity, CardSlot[]>>();
        queue.Enqueue(new KeyValuePair<Entity, CardSlot[]>(shovee, slots));
        List<Entity> list = new List<Entity>();
        bool flag2 = false;
        while (queue.Count > 0)
        {
            KeyValuePair<Entity, CardSlot[]> keyValuePair = queue.Dequeue();
            Entity key = keyValuePair.Key;
            list.Add(key);
            CardSlot[] value = keyValuePair.Value;
            if (
                value == null
                || value.Length == 0
                || shovee.positionPriority < shover.positionPriority
            )
            {
                break;
            }

            List<CardSlot> list2 = new List<CardSlot>();
            CardSlot[] array = value;
            foreach (CardSlot val in array)
            {
                if (shoveData.ContainsKey(key))
                {
                    shoveData[key].Add(val);
                }
                else
                {
                    shoveData[key] = new List<CardSlot> { val };
                }

                Entity top = ((CardContainer)val).GetTop();
                if (
                    (Object)(object)top != (Object)null
                    && (Object)(object)top != (Object)(object)shover
                )
                {
                    list2.Add(val);
                }
            }

            num--;
            foreach (CardSlot item in list2)
            {
                Entity blockingEntity = ((CardContainer)item).GetTop();
                if (
                    list.Contains(blockingEntity)
                    || queue.Any(
                        (KeyValuePair<Entity, CardSlot[]> p) =>
                            (Object)(object)p.Key == (Object)(object)blockingEntity
                    )
                )
                {
                    continue;
                }

                CardSlot[] array2 = ShoveSystem.FindSlots(blockingEntity, dir);
                if (
                    (array2 == null || array2.Length == 0)
                    && blockingEntity._containers.Count == -1
                    && shover.positionPriority > 1
                    && !flag
                )
                {
                    CardContainer obj = blockingEntity.containers[0];
                    CardSlotLane val2 = (CardSlotLane)(object)((obj is CardSlotLane) ? obj : null);
                    int index = ((CardContainer)val2).IndexOf(blockingEntity);
                    if (
                        ((CardContainer)val2).shoveTo != null
                        && ((CardContainer)val2).shoveTo.Count > 0
                    )
                    {
                        CardContainer obj2 = ((CardContainer)val2).shoveTo[0];
                        CardSlotLane val3 = (CardSlotLane)
                            (object)((obj2 is CardSlotLane) ? obj2 : null);
                        if (
                            (Object)(object)val3 != (Object)null
                            && (
                                !((CardContainer)val2).Contains(shovee)
                                || !((CardContainer)val2).Contains(shover)
                            )
                            && (
                                !((CardContainer)val3).Contains(shovee)
                                || !((CardContainer)val3).Contains(shover)
                            )
                        )
                        {
                            CardSlot val4 = val3.slots[index];
                            if (
                                (Object)(object)val4 != (Object)null
                                && (
                                    ((CardContainer)val4).entities.Count == 0
                                    || ((CardContainer)val4).GetTop().positionPriority
                                        <= shover.positionPriority
                                )
                            )
                            {
                                flag = true;
                                dir *= -1;
                                array2 = (CardSlot[])(object)new CardSlot[1] { val4 };
                            }
                        }
                    }
                }

                queue.Enqueue(new KeyValuePair<Entity, CardSlot[]>(blockingEntity, array2));
                num++;
            }
        }

        if (num <= 0)
        {
            flag2 = true;
        }

        __result = flag2;
        return false;
    }
}
