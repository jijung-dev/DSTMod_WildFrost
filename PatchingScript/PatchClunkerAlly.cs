using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using static JournalCardManagerPopulator;

namespace DSTMod_WildFrost
{
    [HarmonyPatch(typeof(JournalCardManagerPopulator), nameof(JournalCardManagerPopulator.Populate))]
    public class PatchClunkerAlly
    {
        static bool Prefix(JournalCardManagerPopulator __instance)
        {
            __instance.populated = true;
            StopWatch.Start();
            List<string> usedCards = new List<string>();
            List<string> playerCards = new List<string>();
            List<string> list = new List<string>();
            ClassData[] classes = References.Classes;
            foreach (ClassData classData in classes)
            {
                foreach (CardData item in classData.startingInventory.deck)
                {
                    StoreAsPlayerCard(item);
                }

                foreach (CardData item2 in classData.startingInventory.reserve)
                {
                    StoreAsPlayerCard(item2);
                }

                RewardPool[] rewardPools = classData.rewardPools;
                foreach (RewardPool rewardPool in rewardPools)
                {
                    if (list.Contains(rewardPool.name))
                    {
                        continue;
                    }

                    list.Add(rewardPool.name);
                    foreach (DataFile item3 in rewardPool.list)
                    {
                        if (item3 is CardData cardData2)
                        {
                            StoreAsPlayerCard(cardData2);
                        }
                    }
                }
            }

            foreach (CardData item4 in AddressableLoader.GetGroup<CardData>("CardData"))
            {
                if (!(item4.mainSprite == null) && !(item4.mainSprite.name == "Nothing"))
                {
                    string text = item4.cardType.name;
                    if (text == "Clunker" && !item4.isEnemyClunker)
                        StoreAsPlayerCard(item4);
                    if ((!(text == "Boss") && !(text == "BossSmall")) || !(item4.name != "FinalBoss2"))
                    {
                        text = item4.cardType.name;
                        bool flag = text == "Friendly" || text == "Item" || playerCards.Contains(item4.name);
                        ProcessCard(item4, !flag);
                    }
                }
            }

            foreach (BattleData item5 in AddressableLoader.GetGroup<BattleData>("BattleData"))
            {
                BattleWavePoolData[] pools = item5.pools;
                for (int i = 0; i < pools.Length; i++)
                {
                    BattleWavePoolData.Wave[] waves = pools[i].waves;
                    for (int j = 0; j < waves.Length; j++)
                    {
                        foreach (CardData unit in waves[j].units)
                        {
                            if ((bool)unit && unit.cardType.miniboss)
                            {
                                string text = unit.cardType.name;
                                if (text == "Boss" || text == "BossSmall")
                                {
                                    ProcessCard(unit, enemy: true);
                                }
                            }
                        }
                    }
                }
            }

            Debug.Log($"Journal Card Manager Population Done! ({StopWatch.Stop()}ms)");
            void ProcessCard(CardData cardData, bool enemy)
            {
                if (!usedCards.Contains(cardData.name))
                {
                    usedCards.Add(cardData.name);
                    Category[] array = __instance.categories;
                    for (int k = 0; k < array.Length && !array[k].CheckAdd(cardData, enemy); k++) { }
                }
            }

            void StoreAsPlayerCard(CardData cardData)
            {
                if (!cardData || playerCards.Contains(cardData.name))
                {
                    return;
                }

                playerCards.Add(cardData.name);
                foreach (string createdByThi in CreatedByLookup.GetCreatedByThis(cardData.name))
                {
                    playerCards.Add(createdByThi);
                }
            }
            return false;
        }
    }
}
