using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleEditor;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Tables;
using WildfrostHopeMod.Utils;
using WildfrostHopeMod.VFX;
using Extensions = Deadpan.Enums.Engine.Components.Modding.Extensions;

namespace DSTMod_WildFrost
{
    public class DSTMod : WildfrostMod
    {
        public static DSTMod Instance;

        public static List<object> assets = new List<object>();
        public static List<(int, BattleDataEditor)> battleAssets = new List<(int, BattleDataEditor)>();
        private bool preLoaded = false;

        public DSTMod(string modDir)
            : base(modDir)
        {
            HarmonyInstance.PatchAll(typeof(PatchHarmony));
        }

        public override string GUID => "tgestudio.wildfrost.dstmod";

        public override string[] Depends => new string[] { "hope.wildfrost.vfx", "mhcdc9.wildfrost.battle" };

        public override string Title => "Don't Frostbite";

        public override string Description => "A mod that get idea from don't starve";
        public override TMP_SpriteAsset SpriteAsset => spriteAsset;
        internal static TMP_SpriteAsset spriteAsset;

        public Dictionary<string, TargetConstraint> allConstraint = new Dictionary<string, TargetConstraint>();

        private void CreateTargetConstraint()
        {
            allConstraint.Add("noChopable", ScriptableObject.CreateInstance<TargetConstraintNotHasTrait>());
            allConstraint.Add("noMineable", ScriptableObject.CreateInstance<TargetConstraintNotHasTrait>());
            allConstraint.Add("hammerOnly", ScriptableObject.CreateInstance<TargetConstraintHasTrait>());
            allConstraint.Add("pickaxeOnly", ScriptableObject.CreateInstance<TargetConstraintHasTrait>());
            allConstraint.Add("axeOnly", ScriptableObject.CreateInstance<TargetConstraintHasTrait>());
            allConstraint.Add("beeOnly", ScriptableObject.CreateInstance<TargetConstraintHasTrait>());

            allConstraint.Add("buildingOnly", ScriptableObject.CreateInstance<TargetConstraintHasStatus>());
            allConstraint.Add("clunkerOnly", ScriptableObject.CreateInstance<TargetConstraintHasStatus>());
            allConstraint.Add("noChestHealth", ScriptableObject.CreateInstance<TargetConstraintNotHasStatus>());
            allConstraint.Add("noBuilding", ScriptableObject.CreateInstance<TargetConstraintNotHasStatus>());

            allConstraint.Add("abigailOnly", ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>());
            allConstraint.Add("dflyOnly", ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>());
            allConstraint.Add("toadstoolOnly", ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>());
            allConstraint.Add("sandCastleOnly", ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>());
            allConstraint.Add("fuelweaverOnly", ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>());
            allConstraint.Add("chestOnly", ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>());
            allConstraint.Add("noDfly", ScriptableObject.CreateInstance<TargetConstraintIsNotSpecificCard>());
            allConstraint.Add("noToadstool", ScriptableObject.CreateInstance<TargetConstraintIsNotSpecificCard>());
            allConstraint.Add("noBoomshroom", ScriptableObject.CreateInstance<TargetConstraintIsNotSpecificCard>());
        }

        private void ApplyConstraint()
        {
            ((TargetConstraintNotHasTrait)allConstraint["noChopable"]).trait = TryGet<TraitData>("Chopable");
            ((TargetConstraintNotHasTrait)allConstraint["noMineable"]).trait = TryGet<TraitData>("Mineable");
            ((TargetConstraintHasTrait)allConstraint["hammerOnly"]).trait = TryGet<TraitData>("HammerType");
            ((TargetConstraintHasTrait)allConstraint["pickaxeOnly"]).trait = TryGet<TraitData>("PickaxeType");
            ((TargetConstraintHasTrait)allConstraint["axeOnly"]).trait = TryGet<TraitData>("AxeType");
            ((TargetConstraintHasTrait)allConstraint["beeOnly"]).trait = TryGet<TraitData>("Bee");

            ((TargetConstraintHasStatus)allConstraint["buildingOnly"]).status = TryGet<StatusEffectData>("Building Health");
            ((TargetConstraintHasStatus)allConstraint["clunkerOnly"]).status = TryGet<StatusEffectData>("Scrap");
            ((TargetConstraintNotHasStatus)allConstraint["noBuilding"]).status = TryGet<StatusEffectData>("Building Health");
            ((TargetConstraintNotHasStatus)allConstraint["noChestHealth"]).status = TryGet<StatusEffectData>("Chest Health");

            ((TargetConstraintIsSpecificCard)allConstraint["abigailOnly"]).allowedCards = new CardData[] { TryGet<CardData>("abigail") };
            ((TargetConstraintIsSpecificCard)allConstraint["dflyOnly"]).allowedCards = new CardData[]
            {
                TryGet<CardData>("dragonfly"),
                TryGet<CardData>("dragonflyEnraged"),
            };
            ((TargetConstraintIsSpecificCard)allConstraint["toadstoolOnly"]).allowedCards = new CardData[]
            {
                TryGet<CardData>("toadstool"),
                TryGet<CardData>("toadstoolEnraged"),
            };
            ((TargetConstraintIsSpecificCard)allConstraint["sandCastleOnly"]).allowedCards = new CardData[] { TryGet<CardData>("sandCastle") };
            ((TargetConstraintIsSpecificCard)allConstraint["fuelweaverOnly"]).allowedCards = new CardData[] { TryGet<CardData>("ancientFuelweaver") };
            ((TargetConstraintIsSpecificCard)allConstraint["chestOnly"]).allowedCards = new CardData[] { TryGet<CardData>("chest") };
            ((TargetConstraintIsNotSpecificCard)allConstraint["noDfly"]).allowedCards = new CardData[]
            {
                TryGet<CardData>("dragonfly"),
                TryGet<CardData>("dragonflyEnraged"),
            };
            ((TargetConstraintIsNotSpecificCard)allConstraint["noToadstool"]).allowedCards = new CardData[]
            {
                TryGet<CardData>("toadstool"),
                TryGet<CardData>("toadstoolEnraged"),
            };
            ((TargetConstraintIsNotSpecificCard)allConstraint["noBoomshroom"]).allowedCards = new CardData[] { TryGet<CardData>("boomshroom") };
        }

        private void CreateModAssets()
        {
            foreach (Type type in DataBase.subclasses)
            {
                if (Activator.CreateInstance(type) is DataBase instance)
                {
                    assets.AddRange(instance.Create());
                }
            }

            #region Tribe
            //Add Tribe
            assets.Add(
                TribeCopy("Magic", "DST")
                    .WithFlag("Images/DSTFlag.png")
                    .WithSelectSfxEvent(FMODUnity.RuntimeManager.PathToEventReference("event:/sfx/card/summon"))
                    .SubscribeToAfterAllBuildEvent(
                        (data) =>
                        {
                            GameObject gameObject = data.characterPrefab.gameObject.InstantiateKeepName();
                            UnityEngine.Object.DontDestroyOnLoad(gameObject);
                            gameObject.name = "Player (dstmod.DST)";
                            data.characterPrefab = gameObject.GetComponent<Character>();
                            data.id = "dstmod.DST";

                            data.leaders = DataList<CardData>("wendy", "wortox", "Leader1_heal_on_kill");

                            Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
                            inventory.deck.list = DataList<CardData>(
                                    "scienceMachineBlueprint",
                                    "spear",
                                    "pickaxe",
                                    "pickaxe",
                                    "axe",
                                    "axe",
                                    "hamBat",
                                    "iceStaff",
                                    "boosterShot",
                                    "walkingCane",
                                    "torch"
                                )
                                .ToList();
                            //inventory.upgrades.Add(TryGet<CardUpgradeData>("CardUpgradeCritical"));
                            data.startingInventory = inventory;

                            RewardPool unitPool = CreateRewardPool(
                                "DstUnitPool",
                                "Units",
                                DataList<CardData>(
                                    "iceChester",
                                    "shadowChester",
                                    "friendlyFly",
                                    "glommer",
                                    "bunnyman",
                                    "pigman",
                                    "pearl",
                                    "beefalo",
                                    "fortuitousRabbit"
                                )
                            );

                            RewardPool itemPool = CreateRewardPool(
                                "DrawItemPool",
                                "Items",
                                DataList<CardData>("trident", "fireStaff", "wateringCan", "sewingKit", "panFlute", "garland", "logSuit", "darkSword")
                            );

                            //RewardPool charmPool = CreateRewardPool("DrawCharmPool", "Charms", DataList<CardUpgradeData>(
                            //    "CardUpgradeTrash",
                            //    "CardUpgradeInk", "CardUpgradeOverload",
                            //    "CardUpgradeMime", "CardUpgradeShellBecomesSpice",
                            //    "CardUpgradeAimless"));

                            data.rewardPools = new RewardPool[]
                            {
                                unitPool,
                                itemPool,
                                //charmPool,
                                Extensions.GetRewardPool("GeneralUnitPool"),
                                Extensions.GetRewardPool("GeneralItemPool"),
                                Extensions.GetRewardPool("GeneralCharmPool"),
                                Extensions.GetRewardPool("GeneralModifierPool"),
                                //Extensions.GetRewardPool("SnowUnitPool"),
                                //Extensions.GetRewardPool("SnowCharmPool"),
                            };
                        }
                    )
            );
            #endregion

            #region Science Machine
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("scienceMachine", "Science Machine")
                    .SetStats(null, null, 5)
                    .SetSprites("ScienceMachine.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.isEnemyClunker = false;
                        data.startWithEffects = new CardData.StatusEffectStacks[]
                        {
                            SStack("On Turn Reduce Counter For Allies", 1),
                            SStack("When Destroyed By Hammer Gain Rock", 1),
                            SStack("When Destroyed By Hammer Gain Wood", 1),
                        };
                        data.traits = new List<CardData.TraitStacks>() { TStack("Building", 1), TStack("Super Unmovable", 1) };
                    })
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("scienceMachineBlueprint", "Science Machine Blueprint")
                    .WithText("Place <card=tgestudio.wildfrost.dstmod.scienceMachine>")
                    .SetSprites("Blueprint.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        var floorOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
                        floorOnly.name = "floorOnly";
                        floorOnly.allowedCards = new CardData[] { TryGet<CardData>("floor") };
                        data.targetConstraints = new TargetConstraint[] { floorOnly };

                        data.traits = new List<CardData.TraitStacks>() { TStack("Blueprint", 1), TStack("Consume", 1) };
                        data.attackEffects = new CardData.StatusEffectStacks[]
                        {
                            SStack("Instant Summon Hammer In Hand", 1),
                            SStack("Build Science Machine", 1),
                            SStack("Reduce Chest Health", 1),
                        };
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Require Wood", 1), SStack("Require Rock", 1) };
                    })
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectNextPhaseExt>("Build Science Machine")
                    .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                    {
                        data.preventDeath = true;
                        data.nextPhase = TryGet<CardData>("scienceMachine");
                        data.animation = TryGet<StatusEffectNextPhase>("SoulboundBossPhase2").animation;
                    })
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Science Machine")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.scienceMachine>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                    {
                        CardType cardType = TryGet<CardType>("Clunker");
                        cardType.canRecall = false;
                        data.setCardType = cardType;
                        data.summonCard = TryGet<CardData>("scienceMachine");
                    })
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Reduce Counter For Allies")
                    .WithText("Reduce <keyword=counter> by <{a}> for allies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                    {
                        data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
                        data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    })
            );

            #endregion

            #region Testing Stuffs
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("sanityStick99", "Sanity Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Sanity", 99) };
                    })
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("freezingStick99", "Freezing Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Freezing", 99) };
                    })
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("freezingStick1", "Freezing Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Freezing", 1) };
                    })
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("sanityStick1", "Sanity Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetTraits(TStack("Zoomlin", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Sanity", 1) };
                    })
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("overheatingStick99", "Overheat Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Overheat", 99) };
                    })
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("overheatingStick1", "Overheat Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Overheat", 1) };
                    })
            );
            #endregion

            //TODO: Make the map node more immersive?

            // Texture2D texture2D = ImagePath("Nodes/FuelweaverNode.png").ToTex();
            // Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100);

            // assets.Add(
            //     NodeCopy("CampaignNodeFinalBoss", "CampaignNodeFinalBossTest")
            //     .FreeModify(
            //         delegate (CampaignNodeType data)
            //         {
            //             data.mapNodePrefab.spriteOptions[0] = sprite;
            //             data.mapNodeSprite = sprite;
            //         }
            //     )
            // );

            preLoaded = true;
        }

        private void CreateBattleAssets()
        {
            #region Spider Den
            battleAssets.Add(
                (
                    0,
                    new BattleDataEditor(this, "Spider Den", 0)
                        .SetSprite("Nodes/SpiderNode.png")
                        .SetNameRef("Spider Den")
                        .EnemyDictionary(
                            ('B', "berryBush"),
                            ('S', "spider"),
                            ('W', "spiderWarrior"),
                            ('N', "spiderNest"),
                            ('Q', "spiderQueen"),
                            ('R', "stone"),
                            ('T', "smallTree")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(5, 0, "SRTSB", "BTRSS")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(4, 2, "SWSB", "SSNB")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(4, 7, "STSN", "SRSW", "STSW", "SRSN")
                        .StartWavePoolData(4, "Wave 5: Queen In")
                        .ConstructWaves(4, 9, "TSNQ", "RSQN", "WSQ")
                        .StartWavePoolData(5, "Wave 6: Queen In")
                        .ConstructWaves(4, 9, "WB", "SSB", "NB")
                        .AddBattleToLoader()
                        .LoadBattle(0, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion

            #region Hound Mound
            battleAssets.Add(
                (
                    1,
                    new BattleDataEditor(this, "Dog Spawner", 0)
                        .SetSprite("Nodes/HoundNode.png")
                        .SetNameRef("Hound Mound")
                        .EnemyDictionary(
                            ('H', "hound"),
                            ('R', "redHound"),
                            ('B', "blueHound"),
                            ('L', "varglet"),
                            ('V', "varg"),
                            ('S', "stone"),
                            ('T', "smallTree")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(3, 0, "THS", "SHT")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(3, 2, "TRH", "SBH", "SRH", "TBH")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(4, 7, "LHST")
                        .StartWavePoolData(4, "Wave 5: Queen In")
                        .ConstructWaves(4, 9, "VSTH", "VSB", "VTR", "VTB", "VSR")
                        .AddBattleToLoader()
                        .LoadBattle(1, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion

            #region Deerclops
            battleAssets.Add(
                (
                    2,
                    new BattleDataEditor(this, "DeerClops", 0)
                        .SetSprite("Nodes/DeerclopsNode.png")
                        .SetNameRef("DeerClops")
                        .EnemyDictionary(
                            ('H', "hound"),
                            ('B', "blueHound"),
                            ('S', "spider"),
                            ('N', "spiderNest"),
                            ('D', "deerclops"),
                            ('R', "stone"),
                            ('T', "smallTree"),
                            ('G', "goldOre")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(6, 0, "HTDRB", "STDRN")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(4, 2, "BSG", "HBG", "NSG", "SBG")
                        .StartWavePoolData(2, "Wave 3: Spider In")
                        .ConstructWaves(4, 5, "BSRN", "HBTH")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(4, 7, "T", "R", "G")
                        .AddBattleToLoader()
                        .LoadBattle(2, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion

            #region Antlion
            battleAssets.Add(
                (
                    3,
                    new BattleDataEditor(this, "Antlion", 0)
                        .SetSprite("Nodes/AntlionNode.png")
                        .SetNameRef("Desert")
                        .EnemyDictionary(
                            ('H', "hound"),
                            ('B', "buzzard"),
                            ('A', "antlion"),
                            ('C', "cactus"),
                            ('R', "boulder"),
                            ('S', "spikyTree"),
                            ('G', "goldOre")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(4, 0, "RCSB", "RCSH")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(3, 2, "HHG", "BBG", "BHG")
                        .StartWavePoolData(2, "Wave 3: Spider In")
                        .ConstructWaves(4, 5, "CBSC")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(3, 7, "SCA")
                        .StartWavePoolData(4, "Wave 4: Nest In")
                        .ConstructWaves(1, 9, "C", "H", "B")
                        .AddBattleToLoader()
                        .LoadBattle(3, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion

            #region BeeQueen
            battleAssets.Add(
                (
                    4,
                    new BattleDataEditor(this, "BeeQueen", 0)
                        .SetSprite("Nodes/BeeQueenHiveNode.png")
                        .SetNameRef("Giant Beehive")
                        .EnemyDictionary(
                            ('B', "bee"),
                            ('K', "killerBee"),
                            ('N', "beehive"),
                            ('S', "smallTree"),
                            ('R', "stone"),
                            ('T', "tree"),
                            ('Q', "beeQueen"),
                            ('G', "grumbleBee")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(4, 0, "RBBS", "SBBR")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(4, 2, "BTN", "BRNS", "BSNS")
                        .StartWavePoolData(2, "Wave 3: Spider In")
                        .ConstructWaves(3, 5, "BKN")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(4, 7, "GGQG")
                        .StartWavePoolData(4, "Wave 4: Nest In")
                        .ConstructWaves(1, 9, "G", "N")
                        .AddBattleToLoader()
                        .LoadBattle(4, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion

            #region Dragonfly
            battleAssets.Add(
                (
                    5,
                    new BattleDataEditor(this, "DragonFly", 0)
                        .SetSprite("Nodes/DragonflyNestNode.png")
                        .SetNameRef("Dragonfly Nest")
                        .EnemyDictionary(
                            ('H', "hound"),
                            ('R', "redHound"),
                            ('G', "goldOre"),
                            ('B', "boulder"),
                            ('S', "spikyTree"),
                            ('C', "cactus"),
                            ('T', "buzzard"),
                            ('D', "dragonfly")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(3, 0, "SCDB", "BCDT")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(3, 2, "RHT", "HHR")
                        .StartWavePoolData(2, "Wave 3: Spider In")
                        .ConstructWaves(3, 5, "CSC")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(4, 7, "GS", "SB", "GH")
                        .AddBattleToLoader()
                        .LoadBattle(5, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion

            #region Toadstool
            battleAssets.Add(
                (
                    6,
                    new BattleDataEditor(this, "ToadStool", 0)
                        .SetSprite("Nodes/ToadstoolNode.png")
                        .SetNameRef("Toadstool Cap")
                        .EnemyDictionary(
                            ('C', "caveSpider"),
                            ('B', "batilisk"),
                            ('M', "blueMushtree"),
                            ('R', "redMushtree"),
                            ('G', "greenMushtree"),
                            ('S', "stalagmite"),
                            ('A', "tallStalagmite"),
                            ('T', "toadstool")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(3, 0, "MCS", "RCS", "GCS")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(4, 2, "CABB")
                        .StartWavePoolData(2, "Wave 3: Spider In")
                        .ConstructWaves(3, 5, "MRG", "RMG", "GMR")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(2, 7, "TB", "TC")
                        .StartWavePoolData(4, "Wave 5: Nest In")
                        .ConstructWaves(2, 9, "CB")
                        .AddBattleToLoader()
                        .LoadBattle(6, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion

            #region Fuelweaver
            battleAssets.Add(
                (
                    7,
                    new BattleDataEditor(this, "Fuelweaver", 0)
                        .SetSprite("Nodes/FuelweaverNode.png", 200)
                        .SetNameRef("Odd Skeleton")
                        .EnemyDictionary(
                            ('S', "smallTree"),
                            ('T', "tree"),
                            ('R', "stone"),
                            ('G', "goldOre"),
                            ('A', "stalagmite"),
                            ('L', "tallStalagmite"),
                            ('B', "blueMushtree"),
                            ('M', "redMushtree"),
                            ('E', "greenMushtree"),
                            ('F', "forestFuelweaver")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(3, 0, "SRF", "RSF")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(2, 2, "TG")
                        .StartWavePoolData(2, "Wave 3: Spider In")
                        .ConstructWaves(4, 5, "LTS", "GRST")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(3, 7, "EBM", "BME")
                        .AddBattleToLoader()
                        .LoadBattle(7, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
                )
            );
            #endregion
        }

        // TODO: Custom starting deck for leader
        public IEnumerator CampaignInit()
        {
            if (References.PlayerData?.classData.ModAdded != this)
                yield break;

            References.LeaderData.startWithEffects = References
                .LeaderData.startWithEffects.Concat(new[] { SStack("Summon Chest Before Battle", 1), SStack("Summon Floor Before Battle", 1) })
                .ToArray();
        }

        // {
        //     // Only applies if the selected tribe is from this mod
        //     if (References.PlayerData?.classData.ModAdded != this)
        //         yield break;

        //     if (References.LeaderData.original == TryGet<CardData>("NakedGnomeFriendly"))
        //     {
        //         Debug.LogError("Forgot someone?");
        //         References.PlayerData.inventory.deck.list.AddRange(
        //             DataList<CardData>(
        //                 "NakedGnomeFriendly",
        //                 "NakedGnomeFriendly",
        //                 "NakedGnomeFriendly"
        //                 ).Select(c => c.Clone())
        //             );
        //     }
        //     if (References.LeaderData.original == TryGet<CardData>("sasha idk"))
        //     {
        //         References.PlayerData.inventory.deck.list.AddRange(
        //             DataList<CardData>(
        //                 "Nova",
        //                 "SunRod",
        //                 "Foxee"
        //                 ).Select(c => c.Clone())
        //             );
        //     }
        // }
        public override List<T> AddAssets<T, Y>()
        {
            if (assets.OfType<T>().Any())
                Debug.LogWarning($"[{Title}] adding {typeof(Y).Name}s: {assets.OfType<T>().Select(a => a._data.name).Join()}");
            return assets.OfType<T>().ToList();
        }

        public TargetConstraint TryGetConstraint(string name)
        {
            if (!allConstraint.Keys.Contains(name))
                throw new Exception($"TryGetConstraint Error: Could not find a [{typeof(TargetConstraint).Name}] with the name [{name}]");
            return allConstraint[name];
        }

        public T[] DataList<T>(params string[] names)
            where T : DataFile => names.Select((s) => TryGet<T>(s)).ToArray();

        public T TryGet<T>(string name)
            where T : DataFile
        {
            T data;
            if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
                data = base.Get<StatusEffectData>(name) as T;
            else if (typeof(KeywordData).IsAssignableFrom(typeof(T)))
                data = base.Get<KeywordData>(name.ToLower()) as T;
            else
                data = base.Get<T>(name);

            if (data == null)
                throw new Exception(
                    $"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{name}] or [{Extensions.PrefixGUID(name, this)}]"
                );
            return data;
        }

        public CardDataBuilder CardCopy(string oldName, string newName) => DataCopy<CardData, CardDataBuilder>(oldName, newName);

        public CampaignNodeTypeBuilder NodeCopy(string oldName, string newName) =>
            DataCopy<CampaignNodeType, CampaignNodeTypeBuilder>(oldName, newName);

        public ClassDataBuilder TribeCopy(string oldName, string newName) => DataCopy<ClassData, ClassDataBuilder>(oldName, newName);

        public CardData.TraitStacks TStack(string name, int amount) => new CardData.TraitStacks(TryGet<TraitData>(name), amount);

        public CardData.StatusEffectStacks SStack(string name, int amount) => new CardData.StatusEffectStacks(TryGet<StatusEffectData>(name), amount);

        public StatusEffectDataBuilder StatusCopy(string oldName, string newName)
        {
            StatusEffectData data = TryGet<StatusEffectData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            data.targetConstraints = new TargetConstraint[0];
            StatusEffectDataBuilder builder = data.Edit<StatusEffectData, StatusEffectDataBuilder>();
            builder.Mod = this;
            return builder;
        }

        public T DataCopy<Y, T>(string oldName, string newName)
            where Y : DataFile
            where T : DataFileBuilder<Y, T>, new()
        {
            Y data = Get<Y>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            T builder = data.Edit<Y, T>();
            builder.Mod = this;
            return builder;
        }

        public override void Load()
        {
            Instance = this;
            if (!preLoaded)
            {
                spriteAsset = HopeUtils.CreateSpriteAsset(Title, directoryWithPNGs: ImagePath("Icons"));
                CreateTargetConstraint();
                CreateModAssets();
            }

            SpriteAsset.RegisterSpriteAsset();
            base.Load();
            CreateBattleAssets();
            ApplyConstraint();

            foreach (var (num, battleDataEditor) in battleAssets)
            {
                battleDataEditor.ToggleBattle(true);
            }

            CreateLocalizedStrings();
            Events.OnEntityCreated += FixImage;
            Events.OnCampaignInit += CampaignInit;

            GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            gameMode.classes = gameMode.classes.Append(TryGet<ClassData>("DST")).ToArray();
        }

        public override void Unload()
        {
            base.Unload();
            foreach (var (num, battleDataEditor) in battleAssets)
            {
                battleDataEditor.ToggleBattle(false);
            }

            Events.OnEntityCreated -= FixImage;
            Events.OnCampaignInit -= CampaignInit;

            SpriteAsset.UnRegisterSpriteAsset();

            GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            gameMode.classes = RemoveNulls(gameMode.classes); //Without this, a non-restarted game would crash on tribe selection
            UnloadFromClasses();
        }

        #region Tribe Stuffs
        public void UnloadFromClasses()
        {
            List<ClassData> tribes = AddressableLoader.GetGroup<ClassData>("ClassData");
            foreach (ClassData tribe in tribes)
            {
                if (tribe == null || tribe.rewardPools == null)
                {
                    continue;
                } //This isn't even a tribe; skip it.

                foreach (RewardPool pool in tribe.rewardPools)
                {
                    if (pool == null)
                    {
                        continue;
                    }
                    ; //This isn't even a reward pool; skip it.

                    pool.list.RemoveAllWhere((item) => item == null || item.ModAdded == this); //Find and remove everything that needs to be removed.
                }
            }
        }

        internal T[] RemoveNulls<T>(T[] data)
            where T : DataFile
        {
            List<T> list = data.ToList();
            list.RemoveAll(x => x == null || x.ModAdded == this);
            return list.ToArray();
        }

        private RewardPool CreateRewardPool(string name, string type, DataFile[] list)
        {
            RewardPool pool = ScriptableObject.CreateInstance<RewardPool>();
            pool.name = name;
            pool.type = type;
            pool.list = list.ToList();
            return pool;
        }

        private void FixImage(Entity entity)
        {
            if (entity.display is Card card && !card.hasScriptableImage) //These cards should use the static image
            {
                card.mainImage.gameObject.SetActive(true); //And this line turns them on
            }
        }

        public string TribeTitleKey => GUID + ".TribeTitle";
        public string TribeDescKey => GUID + ".TribeDesc";

        //Call this method in Load()

        private void CreateLocalizedStrings()
        {
            StringTable uiText = LocalizationHelper.GetCollection("UI Text", SystemLanguage.English);
            uiText.SetString(TribeTitleKey, "The Survival"); //Create the title
            uiText.SetString(
                TribeDescKey,
                "The night devours the unprepared. The earth offers life but demands struggle. We gather, hunt, and build to see another day. "
                    + "Beasts lurk, trees whisper, and the sky brings fire and frost. Fire is safety; darkness is death. "
                    + "We craft tools, weave armor, and wield spears. Kin and foes walk this land, but hunger is our greatest enemy."
                    + "To survive, we must be wise. To falter is to be forgotten."
            ); //Create the description.
        }
        #endregion
    }
}
