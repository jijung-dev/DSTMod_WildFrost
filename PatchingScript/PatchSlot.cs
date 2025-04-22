using System;
using DSTMod_WildFrost;
using HarmonyLib;

[HarmonyPatch(typeof(BoardDisplay), "SetUp", new Type[] { typeof(CampaignNode), typeof(CardController) })]
internal static class PatchSlots
{
    internal static void Prefix(BoardDisplay __instance)
    {
        if (DSTMod.Instance.isBattleOn)
        {
            __instance.enemyRowLength = 4;
            __instance.playerRowLength = 4;
        }
        else
        {
            __instance.enemyRowLength = 3;
            __instance.playerRowLength = 3;
        }
    }
}
