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
using UnityEngine.UI;
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
        public static GameObject prefabHolder;
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

        public RewardPool itemPool;
        public RewardPool unitPool;

        private void CreateTargetConstraint()
        {
            allConstraint.Add("noChopable", new Scriptable<TargetConstraintHasTrait>(x => x.not = true));
            allConstraint.Add("noMineable", new Scriptable<TargetConstraintHasTrait>(x => x.not = true));
            allConstraint.Add("hammerOnly", new Scriptable<TargetConstraintHasTrait>());
            allConstraint.Add("pickaxeOnly", new Scriptable<TargetConstraintHasTrait>());
            allConstraint.Add("axeOnly", new Scriptable<TargetConstraintHasTrait>());
            allConstraint.Add("beeOnly", new Scriptable<TargetConstraintHasTrait>());

            allConstraint.Add("companionOnly", new Scriptable<TargetConstraintIsCardType>());

            allConstraint.Add("buildingOnly", new Scriptable<TargetConstraintHasStatus>());
            allConstraint.Add("clunkerOnly", new Scriptable<TargetConstraintHasStatus>());
            allConstraint.Add("noChestHealth", new Scriptable<TargetConstraintHasStatus>(x => x.not = true));
            allConstraint.Add("noBuilding", new Scriptable<TargetConstraintHasStatus>(x => x.not = true));

            allConstraint.Add("abigailOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("floorOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("dflyOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("toadstoolOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("sandCastleOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("fuelweaverOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("chestOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("catapultOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("wolfgangOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("wormwoodOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("wendyOnly", new Scriptable<TargetConstraintIsSpecificCard>());
            allConstraint.Add("noDfly", new Scriptable<TargetConstraintIsSpecificCard>(x => x.not = true));
            allConstraint.Add("noToadstool", new Scriptable<TargetConstraintIsSpecificCard>(x => x.not = true));
            allConstraint.Add("noBoomshroom", new Scriptable<TargetConstraintIsSpecificCard>(x => x.not = true));
        }

        private void ApplyConstraint()
        {
            ((TargetConstraintHasTrait)allConstraint["noChopable"]).trait = TryGet<TraitData>("Chopable");
            ((TargetConstraintHasTrait)allConstraint["noMineable"]).trait = TryGet<TraitData>("Mineable");
            ((TargetConstraintHasTrait)allConstraint["hammerOnly"]).trait = TryGet<TraitData>("HammerType");
            ((TargetConstraintHasTrait)allConstraint["pickaxeOnly"]).trait = TryGet<TraitData>("PickaxeType");
            ((TargetConstraintHasTrait)allConstraint["axeOnly"]).trait = TryGet<TraitData>("AxeType");
            ((TargetConstraintHasTrait)allConstraint["beeOnly"]).trait = TryGet<TraitData>("Bee");

            ((TargetConstraintIsCardType)allConstraint["companionOnly"]).allowedTypes = new[] { TryGet<CardType>("Friendly") };

            ((TargetConstraintHasStatus)allConstraint["buildingOnly"]).status = TryGet<StatusEffectData>("Building Health");
            ((TargetConstraintHasStatus)allConstraint["clunkerOnly"]).status = TryGet<StatusEffectData>("Scrap");
            ((TargetConstraintHasStatus)allConstraint["noBuilding"]).status = TryGet<StatusEffectData>("Building Health");
            ((TargetConstraintHasStatus)allConstraint["noChestHealth"]).status = TryGet<StatusEffectData>("Chest Health");

            ((TargetConstraintIsSpecificCard)allConstraint["abigailOnly"]).allowedCards = new CardData[] { TryGet<CardData>("abigail") };
            ((TargetConstraintIsSpecificCard)allConstraint["floorOnly"]).allowedCards = new CardData[] { TryGet<CardData>("floor") };
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
            ((TargetConstraintIsSpecificCard)allConstraint["noDfly"]).allowedCards = new CardData[]
            {
                TryGet<CardData>("dragonfly"),
                TryGet<CardData>("dragonflyEnraged"),
            };
            ((TargetConstraintIsSpecificCard)allConstraint["noToadstool"]).allowedCards = new CardData[]
            {
                TryGet<CardData>("toadstool"),
                TryGet<CardData>("toadstoolEnraged"),
            };
            ((TargetConstraintIsSpecificCard)allConstraint["noBoomshroom"]).allowedCards = new CardData[] { TryGet<CardData>("boomshroom") };
            ((TargetConstraintIsSpecificCard)allConstraint["catapultOnly"]).allowedCards = new CardData[] { TryGet<CardData>("catapult") };
            ((TargetConstraintIsSpecificCard)allConstraint["wolfgangOnly"]).allowedCards = new CardData[] { TryGet<CardData>("wolfgang") };
            ((TargetConstraintIsSpecificCard)allConstraint["wormwoodOnly"]).allowedCards = new CardData[] { TryGet<CardData>("wormwood") };
            ((TargetConstraintIsSpecificCard)allConstraint["wendyOnly"]).allowedCards = new CardData[] { TryGet<CardData>("wendy") };
        }

        private void CreateModAssets()
        {
            var subclass = DataBase.subclasses;
            foreach (Type type in subclass)
            {
                if (Activator.CreateInstance(type) is DataBase instance)
                {
                    assets.AddRange(instance.CreateAssest());
                }
            }

            itemPool = CreateRewardPool("DstItemPool", "Items", DataList<CardData>());
            unitPool = CreateRewardPool("DstUnitPool", "Units", DataList<CardData>());
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

                            data.leaders = DataList<CardData>("wendy", "wortox", "winona", "wolfgang", "wormwood");

                            Inventory inventory = new Scriptable<Inventory>();
                            inventory.deck.list = DataList<CardData>(
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
            var subclass = DataBase.subclasses;
            foreach (Type type in subclass)
            {
                if (Activator.CreateInstance(type) is DataBase instance)
                {
                    battleAssets.AddRange(instance.CreateBattleAsset());
                }
            }
        }

        private void InsertNodeViaPreset(ref string[] preset)
        {
            InsertAfterLetter(ref preset, 'S', '=', 1);
            InsertAfterLetter(ref preset, 'B', '^', 1);
        }

        private void InsertAfterLetter(ref string[] preset, char letter, char insertChar, int targetAmount)
        {
            for (int i = 0; i < preset[0].Length; i++)
            {
                if (preset[0][i] == letter)
                {
                    targetAmount--;
                    if (targetAmount == 0)
                    {
                        for (int j = 0; j < preset.Length; j++)
                        {
                            preset[j] = preset[j].Insert(i + 1, j == 0 ? insertChar.ToString() : preset[j][i].ToString());
                        }
                        break;
                    }
                }
            }
        }

        // TODO: Custom starting deck for leader
        public IEnumerator CampaignInit()
        {
            if (References.PlayerData?.classData.ModAdded != this)
                yield break;

            References.LeaderData.startWithEffects = References
                .LeaderData.startWithEffects.Concat(new[] { SStack("Summon Chest Before Battle", 1), SStack("Summon Floor Before Battle", 1) })
                .ToArray();

            if (References.LeaderData.original == TryGet<CardData>("wendy"))
            {
                References.PlayerData.inventory.deck.list.AddRange(DataList<CardData>("abigailFlower").Select(c => c.Clone()));
            }
            if (References.LeaderData.original == TryGet<CardData>("winona"))
            {
                References.PlayerData.inventory.deck.list.AddRange(DataList<CardData>("catapult", "catapult").Select(c => c.Clone()));
            }
            if (References.LeaderData.original == TryGet<CardData>("wolfgang"))
            {
                References.PlayerData.inventory.deck.list.AddRange(
                    DataList<CardData>("dumbbell", "dumbbell", "goldenDumbbell", "marbell").Select(c => c.Clone())
                );
            }
            if (References.LeaderData.original == TryGet<CardData>("wormwood"))
            {
                References.PlayerData.inventory.deck.list.AddRange(DataList<CardData>("compostWrap").Select(c => c.Clone()));
            }
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

        public CardTypeBuilder CardTypeCopy(string oldName, string newName) => DataCopy<CardType, CardTypeBuilder>(oldName, newName);

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

        internal Sprite ScaledSprite(string fileName, int pixelsPerUnit = 100)
        {
            Texture2D tex = ImagePath(fileName).ToTex();
            return Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, (20f * pixelsPerUnit) / (tex.height * 100f)),
                pixelsPerUnit
            );
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

            prefabHolder = new GameObject(GUID);
            UnityEngine.Object.DontDestroyOnLoad(prefabHolder);
            prefabHolder.SetActive(false);

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
            Events.OnCampaignLoadPreset += InsertNodeViaPreset;

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

            prefabHolder.Destroy();

            Events.OnEntityCreated -= FixImage;
            Events.OnCampaignInit -= CampaignInit;
            Events.OnCampaignLoadPreset -= InsertNodeViaPreset;

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
            RewardPool pool = new Scriptable<RewardPool>();
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
            uiText.SetString(TribeTitleKey, "The Survivor"); //Create the title
            uiText.SetString(
                TribeDescKey,
                "The night devours the unprepared. The earth offers life but demands struggle. We gather, hunt, and build to see another day. "
                    + "Beasts lurk, trees whisper, and the sky brings fire and frost. Fire is safety; darkness is death. "
                    + "We craft tools, weave armor, and wield spears. Kin and foes walk this land, but hunger is our greatest enemy."
                    + "To survive, we must be wise. To falter is to be forgotten."
            ); //Create the description.

            StringTable tooltipText = LocalizationHelper.GetCollection("Tooltips", SystemLanguage.English);
            tooltipText.SetString("tgestudio.wildfrost.dstmod.requirewood", "Require Wood");
            tooltipText.SetString("tgestudio.wildfrost.dstmod.requiregold", "Require Gold");
            tooltipText.SetString("tgestudio.wildfrost.dstmod.requirerock", "Require Rock");
            tooltipText.SetString("tgestudio.wildfrost.dstmod.requirerabbit", "Require Rabbit");
            tooltipText.SetString("tgestudio.wildfrost.dstmod.cannotshove", "Cannot Shove");
        }
        #endregion
    }
}
