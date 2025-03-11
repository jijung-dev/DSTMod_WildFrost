using System.Linq;
using System.Text.RegularExpressions;
using DSTMod_WildFrost;

public static class StringExtExt
{
    public static string Process(this string text)
    {
        return Regex.Replace(
            text,
            @"<(card|keyword)=dstmod\.(.*?)>",
            match =>
            {
                string prefix = match.Groups[1].Value;
                string name = match.Groups[2].Value;

                return $"<{prefix}=tgestudio.wildfrost.dstmod.{name}>";
            }
        );
    }
}
