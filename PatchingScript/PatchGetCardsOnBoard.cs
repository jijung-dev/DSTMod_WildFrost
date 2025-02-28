using System;
using System.Collections.Generic;
using HarmonyLib;

[HarmonyPatch(typeof(Battle), "GetCardsOnBoard", new Type[] { typeof(Character) })]
internal static class PatchGetCardsOnBoard
{
    internal static bool Prefix(Character character, ref List<Entity> __result)
    {
        bool flag = false;
        __result = new List<Entity>();
        int count = Battle.instance.rows[character].Count;
        for (int i = 0; i < 99; i++)
        {
            flag = true;
            for (int j = 0; j < count; j++)
            {
                CardContainer row = Battle.instance.GetRow(character, j);
                CardSlotLane val = (CardSlotLane)(object)((row is CardSlotLane) ? row : null);
                if (val != null && val.slots.Count > i)
                {
                    flag = false;
                    Entity top = ((CardContainer)val.slots[i]).GetTop();
                    if (top != null && !__result.Contains(top))
                    {
                        __result.Add(top);
                    }
                }
            }

            if (flag)
            {
                break;
            }
        }

        return false;
    }
}
