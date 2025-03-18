using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using UnityEngine.UI;

public static class Ext
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

    public static T CopyAdd<T>(this GameObject desitation, T org)
        where T : Component
    {
        Type type = org.GetType();
        Component copy = desitation.AddComponent(type);

        FieldInfo[] fields = type.GetFields();
        foreach (var field in fields)
        {
            field.SetValue(copy, field.GetValue(org));
        }

        return copy as T;
    }

    public static T Copy<T>(this T desitation, T org)
        where T : Component
    {
        FieldInfo[] fields = org.GetType().GetFields();
        foreach (var field in fields)
        {
            field.SetValue(desitation, field.GetValue(org));
        }

        return desitation;
    }

    public static void FitToChildren2(this VerticalLayoutGroup layout)
    {
        layout.enabled = false;
        var main = new GameObject("=");
        main.transform.SetParent(layout.transform);
        if (!main.TryGetComponent<RectTransform>(out RectTransform rect))
            rect = main.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);
        rect.pivot = new Vector2(0, 0.5f);
        rect.anchoredPosition = new Vector2(0, 0);
        var hor = main.AddComponent<HorizontalLayoutGroup>();

        hor.padding = layout.padding;
        hor.spacing = layout.spacing;
        hor.childAlignment = TextAnchor.UpperCenter;
        hor.reverseArrangement = layout.reverseArrangement;
        hor.childControlWidth = layout.childControlWidth;
        hor.childControlHeight = layout.childControlHeight;
        hor.childScaleWidth = layout.childScaleWidth;
        hor.childScaleHeight = layout.childScaleHeight;
        hor.childForceExpandWidth = layout.childForceExpandWidth;
        hor.childForceExpandHeight = layout.childForceExpandHeight;

        Texture2D texture2D = DSTMod.Instance.ImagePath("test.png").ToTex();
        Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 200);
        main.AddComponent<Image>().sprite = sprite;

        float xSize = 0;
        float ySize = 0;
        float colummSize = 0;
        xSize += layout.spacing + ((RectTransform)layout.transform.GetChild(0)).sizeDelta.x;
        foreach (RectTransform item in layout.transform)
        {
            if (item.name.Contains("="))
                continue;

            Vector2 sizeDelta = item.sizeDelta;
            if (colummSize + layout.spacing + sizeDelta.y >= 10)
            {
                if (ySize < colummSize)
                    ySize = colummSize;
                xSize += layout.spacing + sizeDelta.x;
                colummSize = 0;
            }
            item.SetParent(rect);

            colummSize += layout.spacing + sizeDelta.y;
        }
        rect.sizeDelta = new Vector2(xSize, ySize);
        layout.enabled = true;

        // var spacing = layout.spacing;

        // var g2 = new GameObject("@1").CopyAdd(layout);
        // //((RectTransform)g2.transform).Copy((RectTransform)layout.transform);
        // g2.gameObject.transform.SetParent(main.transform);

        // float num = 0;
        // float num2 = 0;
        // float num3 = 0;
        // float test = 2;
        // RectTransform now = (RectTransform)g2.transform;
        // foreach (RectTransform item in layout.transform)
        // {
        //     //if (item.name.Contains("=")) continue;
        //     if (num + spacing + item.sizeDelta.y > 10)
        //     {
        //         if (num3 < num) num3 = num;
        //         now.sizeDelta = new Vector2(item.sizeDelta.x, num);
        //         Debug.LogWarning(num);
        //         var g3 = new GameObject("@" + test.ToString()).CopyAdd(layout);
        //         //((RectTransform)g2.transform).Copy((RectTransform)layout.transform);
        //         g3.gameObject.transform.SetParent(main.transform);
        //         now = (RectTransform)g3.transform;
        //         num2 += item.sizeDelta.x + spacing;
        //         num = 0;
        //         test++;
        //     }
        //     item.SetParent(now);
        //     Debug.LogWarning($"{item.name} Added to {now.name}");

        //     if (num > 0f)
        //     {
        //         num += spacing;
        //     }
        //     num += item.sizeDelta.y;
        //     Debug.LogWarning($"{num} wat");
        // }
        // rect.sizeDelta = new Vector2(num2, num3);
        //((RectTransform)main.transform).anchoredPosition = new Vector3(0, 0, 0);
        //hor.FitToChildren();
    }
}
