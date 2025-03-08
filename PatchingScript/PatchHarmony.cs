using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;

namespace DSTMod_WildFrost
{
    [HarmonyPatch(typeof(WildfrostMod.DebugLoggerTextWriter), nameof(WildfrostMod.DebugLoggerTextWriter.WriteLine))]
    class PatchHarmony
    {
        static bool Prefix()
        {
            Postfix();
            return false;
        }

        static void Postfix() =>
            HarmonyLib.Tools.Logger.ChannelFilter = HarmonyLib.Tools.Logger.LogChannel.Warn | HarmonyLib.Tools.Logger.LogChannel.Error;
    }
}
