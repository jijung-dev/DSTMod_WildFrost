using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using static CardData;

public static class Ext
{
    public static List<CardData> blueprints = new List<CardData>();

    public static string Process(this string text)
    {
        return Regex.Replace(
            text,
            @"<(card|keyword|hiddencard)=dstmod\.(.*?)>",
            match =>
            {
                string prefix = match.Groups[1].Value;
                string name = match.Groups[2].Value;

                return $"<{prefix}=tgestudio.wildfrost.dstmod.{name}>";
            }
        );
    }
    public static CardDataBuilder AddLeaderFrame(this CardDataBuilder cardData)
    {
        CustomCardFrameSystem.LeaderCards.Add(cardData._data.name);
        return cardData;
    }

    public static string RemoveHidden(string text)
    {
        StringBuilder sb = new StringBuilder(text);
        int start;

        while ((start = sb.ToString().IndexOf("<hiddencard=")) != -1)
        {
            int end = sb.ToString().IndexOf(">", start);
            if (end == -1)
                break; // Safety check

            sb.Remove(start, end - start + 1);
        }

        return string.Join("\n", sb.ToString().Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)));
    }

    public static StatusEffectStacks[] AddStartEffect(string effectName, int value)
    {
        return References.LeaderData.startWithEffects.Concat(new[] { DSTMod.Instance.SStack(effectName, value) }).ToArray();
    }

    public static StatusEffectStacks[] AddAttackEffect(string effectName, int value)
    {
        return References.LeaderData.attackEffects.Concat(new[] { DSTMod.Instance.SStack(effectName, value) }).ToArray();
    }

    public static StatusEffectStacks[] RemoveStartEffect(string effectName)
    {
        return References.LeaderData.startWithEffects.Where(effect => effect.data != DSTMod.Instance.TryGet<StatusEffectData>(effectName)).ToArray();
    }

    public static StatusEffectStacks[] RemoveAttackEffect(string effectName)
    {
        return References.LeaderData.attackEffects.Where(effect => effect.data != DSTMod.Instance.TryGet<StatusEffectData>(effectName)).ToArray();
    }

    public static CardDataBuilder SetCardSprites(this CardDataBuilder builder, string mainSpriteName, string backgroundSpriteName)
    {
        Sprite mainSprite = DSTMod.Cards.GetSprite(mainSpriteName.Replace(".png", ""));
        Sprite backgroundSprite = DSTMod.Other.GetSprite(backgroundSpriteName.Replace(".png", ""));

        return builder.SetSprites(mainSprite, backgroundSprite);
    }

    public static CardDataBuilder SetLeaderSprites(this CardDataBuilder builder, string mainSpriteName, string backgroundSpriteName)
    {
        Sprite mainSprite = DSTMod.Leaders.GetSprite(mainSpriteName.Replace(".png", ""));
        Sprite backgroundSprite = DSTMod.Other.GetSprite(backgroundSpriteName.Replace(".png", ""));

        return builder.SetSprites(mainSprite, backgroundSprite);
    }

    public static CardDataBuilder SetBossSprites(this CardDataBuilder builder, string mainSpriteName, string backgroundSpriteName)
    {
        Sprite mainSprite = DSTMod.Bosses.GetSprite(mainSpriteName.Replace(".png", ""));
        Sprite backgroundSprite = DSTMod.Other.GetSprite(backgroundSpriteName.Replace(".png", ""));

        return builder.SetSprites(mainSprite, backgroundSprite);
    }

    public static void PopupText(Entity entity, NoTargetTypeExt requireType)
    {
        NoTargetTextSystem noText = NoTargetTextSystem.instance;
        if (noText != null)
        {
            TMP_Text text = noText.textElement;

            float num = noText.shakeDurationRange.Random();
            entity.curveAnimator.Move(noText.shakeAmount.WithX(noText.shakeAmount.x.WithRandomSign()), noText.shakeCurve, 1f, num);

            StringTable tooltips = LocalizationHelper.GetCollection("Tooltips", SystemLanguage.English);
            text.text = tooltips.GetString(GetStringType(requireType)).GetLocalizedString();
            noText.PopText(entity.transform.position);
        }
    }

    public static string GetStringType(NoTargetTypeExt type)
    {
        string text;
        switch (type)
        {
            case NoTargetTypeExt.RequireWood:
                text = "tgestudio.wildfrost.dstmod.requirewood";
                break;
            case NoTargetTypeExt.RequireGold:
                text = $"tgestudio.wildfrost.dstmod.requiregold";
                break;
            case NoTargetTypeExt.RequireRock:
                text = $"tgestudio.wildfrost.dstmod.requirerock";
                break;
            case NoTargetTypeExt.CantShove:
                text = $"tgestudio.wildfrost.dstmod.cannotshove";
                break;
            case NoTargetTypeExt.RequireRabbit:
                text = $"tgestudio.wildfrost.dstmod.requirerabbit";
                break;
            default:
                text = "";
                break;
        }
        return text;
    }

    public enum NoTargetTypeExt
    {
        None,
        CantShove,
        RequireRock,
        RequireGold,
        RequireWood,
        RequireRabbit,
    }
}
