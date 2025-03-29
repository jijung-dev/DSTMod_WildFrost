using HarmonyLib;

namespace DSTMod_WildFrost
{
    [HarmonyPatch(typeof(StatusEffectIncreaseAttackWhileAlone), nameof(StatusEffectIncreaseAttackWhileAlone.CountOthersInMyRows))]
    static class PatchFury
    {
        static bool Prefix(StatusEffectIncreaseAttackWhileAlone __instance, ref int __result)
        {
            int num = 0;
            CardContainer[] containers = __instance.target.containers;
            for (int i = 0; i < containers.Length; i++)
            {
                foreach (Entity item in containers[i])
                {
                    if (item.IsAliveAndExists() && item != __instance.target && !item.statusEffects.Find(effect => effect is StatusEffectStealth))
                    {
                        num++;
                    }
                }
            }

            __result = num;
            return false;
        }
    }
}
