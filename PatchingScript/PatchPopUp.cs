// using System;
// using System.Collections;
// using System.Linq;
// using Deadpan.Enums.Engine.Components.Modding;
// using HarmonyLib;
// using UnityEngine;
// using UnityEngine.UI;

// namespace DSTMod_WildFrost
// {
// 	[HarmonyPatch(typeof(InspectSystem), nameof(InspectSystem.FixLayouts))]
// 	public class PatchFixLayout
// 	{
// 		static bool Prefix(ref IEnumerator __result, InspectSystem __instance)
// 		{
// 			__result = FixLayouts(__instance);
// 			return false;
// 		}
// 		public static IEnumerator FixLayouts(InspectSystem __instance)
// 		{
// 			yield return null;
// 			LayoutGroup[] array = __instance.layoutsToFix;
// 			foreach (LayoutGroup layoutGroup in array)
// 			{
// 				if (!(layoutGroup is VerticalLayoutGroup layout))
// 				{
// 					if (layoutGroup is HorizontalLayoutGroup layout2)
// 					{
// 						layout2.FitToChildren();
// 					}
// 				}
// 				else
// 				{
// 					layout.FitToChildren();
// 				}
// 			}

// 			if (__instance.CheckOverflow(__instance.bottomPopGroup))
// 			{
// 				yield return FixLayouts(__instance);
// 			}
// 			//__instance.rightPopGroup.GetComponent<VerticalLayoutGroup>().FitToChildren2();
// 		}
// 	}
// 	[HarmonyPatch(typeof(InspectSystem), nameof(InspectSystem.CreatePopups))]
// 	public class PatchPopUp
// 	{
// 		static bool Prefix(InspectSystem __instance)
// 		{
// 			__instance.CreateIconPopups(__instance.inspect.display.healthLayoutGroup, __instance.leftPopGroup);
// 			__instance.CreateIconPopups(__instance.inspect.display.damageLayoutGroup, __instance.rightPopGroup);
// 			__instance.CreateIconPopups(__instance.inspect.display.counterLayoutGroup, __instance.bottomPopGroup);

// 			if (!__instance.rightPopGroup.GetAllChildren().Any(r => r.name.Contains("=")))
// 			{
// 				GameObject main = GameObject.Instantiate(new GameObject("="), __instance.rightPopGroup);
// 				//GameObject g1 = GameObject.Instantiate(new GameObject("@1"), main.transform);
// 				main.CopyAdd(__instance.bottomPopGroup.GetComponent<HorizontalLayoutGroup>());
// 				//g1.CopyAdd(__instance.rightPopGroup.GetComponent<VerticalLayoutGroup>());
// 				if (!main.TryGetComponent<RectTransform>(out RectTransform rect))
// 					rect = main.AddComponent<RectTransform>();
// 				rect.anchoredPosition = new Vector2(0, 0);
// 				rect.sizeDelta = new Vector2(1, 10);

// 				Texture2D texture2D = DSTMod.Instance.ImagePath("test.png").ToTex();
// 				Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 200);
// 				main.AddComponent<Image>().sprite = sprite;
// 			}

// 			CoroutineManager.Start(__instance.FixLayoutsAfterFrame());
// 			return false;
// 		}
// 	}

// }
