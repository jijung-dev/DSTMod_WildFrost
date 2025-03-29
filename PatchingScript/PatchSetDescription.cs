using System;
using System.Collections.Generic;
using HarmonyLib;
using static Text;

[HarmonyPatch(typeof(Text), nameof(Text.GetMentionedCards), typeof(string))]
internal static class PatchSetDescription
{
    internal static bool Prefix(ref HashSet<CardData> __result, string text)
    {
        HashSet<CardData> hashSet = new HashSet<CardData>();
        for (int i = 0; i < text.Length; i++)
        {
            if (!text[i].Equals('<'))
            {
                continue;
            }

            string text2 = FindTag(text, i);
            if (text2.Length > 0 && text2.Contains("="))
            {
                string[] array = text2.Split('=');
                if (array.Length == 2 && (array[0].Trim() == "card" || array[0].Trim() == "hiddencard"))
                {
                    CardData item = AddressableLoader.Get<CardData>("CardData", array[1].Trim());
                    hashSet.Add(item);
                }
            }
        }

        __result = hashSet;
        return false;
    }
}

[HarmonyPatch(typeof(Text), nameof(Text.Process), new Type[] { typeof(string), typeof(int), typeof(float), typeof(ColourProfileHex) })]
internal static class PatchSetDescription2
{
    internal static bool Prefix(ref string __result, string original, int effectBonus, float effectFactor, ColourProfileHex profile)
    {
        string text = Ext.RemoveHidden(original).Trim();
        int length = text.Length;
        for (int i = 0; i < length; i++)
        {
            if (text[i] != '<')
            {
                continue;
            }

            string text2 = FindTag(text, i);
            if (text2.Length > 0)
            {
                text = text.Remove(i, text2.Length + 2);
                string text3 = ProcessTag(text, text2, effectBonus, effectFactor, profile);
                if (text3.Length > 0)
                {
                    text = text.Insert(i, text3);
                    i += text3.Length;
                }

                length = text.Length;
                i--;
            }
        }

        __result = text;
        return false;
    }
}
