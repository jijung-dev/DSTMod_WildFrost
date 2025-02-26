using System;
using HarmonyLib;

[HarmonyPatch(
    typeof(BoardDisplay),
    "SetUp",
    new Type[] { typeof(CampaignNode), typeof(CardController) }
)]
internal static class PatchSlots
{
    internal static void Prefix(BoardDisplay __instance)
    {
        __instance.enemyRowLength = 4;
        __instance.playerRowLength = 4;
    }
}
