using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DSTMod_WildFrost
{
    [HarmonyPatch(typeof(EventRoutineCurseItems), nameof(EventRoutineCurseItems.TakeCard), new Type[] { typeof(Entity) })]
    class PatchNewNode
    {
        static bool Prefix(ref IEnumerator __result, EventRoutineCurseItems __instance, Entity entity)
        {
            if (!DSTMod.Instance.isBattleOn)
                return true;
            if (__instance.node.type.letter == "=" || __instance.node.type.letter == "^")
            {
                __result = TakeCard(__instance, entity);
                return false;
            }
            return true;
        }

        static IEnumerator TakeCard(EventRoutineCurseItems __instance, Entity entity)
        {
            SfxSystem.OneShot(__instance.takeSfxEvent);
            __instance.cardController.Disable();
            __instance.backButton.SetActive(value: false);
            int index = __instance.cards.IndexOf(entity);
            Transform transform = entity.transform;
            Entity curse = __instance.curses[index];
            if ((bool)curse)
            {
                Transform obj = curse.transform;
                obj.position = transform.position;
                obj.localScale = Vector3.one * __instance.cardScale;
                obj.localRotation = Quaternion.identity;
                curse.gameObject.SetActive(value: true);
                curse.flipper.FlipDownInstant();
            }

            __instance.cards.Clear();
            SaveCollection<string> saveCollection = __instance.data.Get<SaveCollection<string>>("cards");

            saveCollection.Remove(index);
            __instance.data["cards"] = saveCollection;
            __instance.curses.RemoveAt(index);
            SaveCollection<string> saveCollection2 = __instance.data.Get<SaveCollection<string>>("curses");
            saveCollection2.Remove(index);
            __instance.data["curses"] = saveCollection2;

            var status = entity.statusEffects.Find(r => r is IUpgrade) as IUpgrade;
            if (status != null)
            {
                status.Run();
            }
            else
            {
                __instance.cardSelector.TakeCard(entity);
            }

            Events.InvokeEntityChosen(entity);

            __instance.talker.Say("thanks", 0f, entity.data.title);
            if ((bool)curse)
            {
                yield return new WaitForSeconds(0.5f);
                curse.flipper.FlipUp();
                yield return new WaitForSeconds(0.5f);
                __instance.cardSelector.TakeCard(curse);
                Events.InvokeEntityChosen(curse);
            }

            yield return new WaitForSeconds(0.3f);
            __instance.cardController.Enable();

            __instance.Back();
            __instance.node.SetCleared();
        }
    }

    [HarmonyPatch(typeof(CampaignNodeTypeCurseItems), nameof(CampaignNodeTypeCurseItems.SetUp), typeof(CampaignNode))]
    class PatchNewNodePopulate
    {
        static bool Prefix(ref IEnumerator __result, CampaignNodeTypeCurseItems __instance, CampaignNode node)
        {
            if (!DSTMod.Instance.isBattleOn)
                return true;
            if (node.type.letter == "=")
            {
                var list2 = DSTMod
                    .Instance.DataList<CardData>("trapBlueprint", "scienceMachineBlueprint", "firePitBlueprint", "crockPotBlueprint", "tentBlueprint")
                    .Select(c => c.Clone());
                CardData[] picked = list2.RandomItems(3);

                __instance.force = new List<CardData>(picked);
            }
            else if (node.type.letter == "^")
            {
                var charList = GetCharacterCards();
                var generalList = DSTMod.Instance.DataList<CardData>(
                    "goldUpgrade",
                    "blingsUpgrade",
                    "pickAxeUpgrade",
                    "charmUpgrade",
                    "blueprintUpgrade"
                );

                List<CardData> list = new List<CardData>();
                list.AddRange(charList);
                list.AddRange(generalList);

                __instance.force = list.RandomItems(3).ToList();
            }
            return true;
        }

        static CardData[] GetCharacterCards()
        {
            var leader = References.LeaderData.original;
            switch (leader.title)
            {
                case "Wendy":
                    return DSTMod.Instance.DataList<CardData>(
                        "unyieldingDraught",
                        "distilledVengeance",
                        "nightshadeNostrum",
                        "vigorMortis",
                        "revenantRestorative",
                        "ghastlyExperience"
                    );
                case "Wortox":
                    return DSTMod.Instance.DataList<CardData>("twintailedHeart", "soulJar", "immuneSanityUpgarde", "knabsack");
                case "Wolfgang":
                    return DSTMod.Instance.DataList<CardData>("gymUpgrade", "gembellUpgrade", "whistleUpgrade");
                case "Wormwood":
                    return DSTMod.Instance.DataList<CardData>("huskUpgrade", "saladmander", "carrat", "bulbousLightbug");
                case "Winona":
                    return DSTMod.Instance.DataList<CardData>("slowUpgrade", "2tapeUpgrade", "generator", "spotlight");
                default:
                    return DSTMod.Instance.DataList<CardData>();
            }
        }

        static T TryGet<T>(string name)
            where T : DataFile => DSTMod.Instance.TryGet<T>(name);
    }

    [HarmonyPatch(typeof(EventRoutineCurseItems), nameof(EventRoutineCurseItems.Populate))]
    class PatchNewNodeCard
    {
        static bool Prefix(EventRoutineCurseItems __instance)
        {
            if (!DSTMod.Instance.isBattleOn)
                return true;
            if (__instance.node.type.letter == "=" || __instance.node.type.letter == "^")
                __instance.cardScale = 0.8f;
            return true;
        }
    }

    [HarmonyPatch(typeof(EventRoutineCurseItems), nameof(EventRoutineCurseItems.Run))]
    class PatchNewNodeSprite
    {
        static bool Prefix(ref IEnumerator __result, EventRoutineCurseItems __instance)
        {
            if (!DSTMod.Instance.isBattleOn)
                return true;
            if (__instance.node.type.letter == "=" || __instance.node.type.letter == "^")
            {
                __result = Run(__instance);
                return false;
            }
            return true;
        }

        static IEnumerator Run(EventRoutineCurseItems __instance)
        {
            Texture2D texture2D = DSTMod.Instance.ImagePath("Icons/Wagstaff.png").ToTex();
            Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 50);

            __instance.backButton.SetActive(value: false);

            var poser = GameObject.FindObjectOfType<AvatarPoser>();
            poser.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);

            var poser2 = poser.transform.GetChild(0).transform.GetChild(0);
            poser2.gameObject.GetComponent<Image>().sprite = sprite;
            poser2.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            poser2.transform.localPosition += new Vector3(1f, 0f, 0f);

            foreach (Transform item in poser2)
            {
                item.gameObject.SetActive(false);
            }

            int num = __instance.data.Get("enterCount", 0) + 1;
            __instance.data["enterCount"] = num;
            if (num == 1)
            {
                __instance.talker.Say("greet", PettyRandom.Range(0.5f, 1f));
            }

            __instance.sequence.owner = __instance.player;
            __instance.cardController.owner = __instance.player;
            __instance.cardSelector.character = __instance.player;
            CinemaBarSystem.Top.SetScript(GetTitle(__instance));
            if (!__instance.data.Get("analyticsEventSent", @default: false))
            {
                foreach (Entity item in __instance.cardContainer)
                {
                    Events.InvokeEntityOffered(item);
                }

                __instance.data["analyticsEventSent"] = true;
            }

            yield return __instance.sequence.Run();
            CinemaBarSystem.Clear();
            if (__instance.data.Get<SaveCollection<string>>("cards").Count <= 0)
            {
                __instance.node.SetCleared();
            }
        }

        static string GetTitle(EventRoutineCurseItems __instance)
        {
            switch (__instance.node.type.letter)
            {
                case "=":
                    return "Choose a blueprint";
                case "^":
                    return "Choose an upgrade or an item";
                default:
                    return "";
            }
        }
    }
}
