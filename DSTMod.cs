using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BattleEditor;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Localization.Tables;
using UnityEngine.U2D;
using UnityEngine.UI;
using WildfrostHopeMod.SFX;
using WildfrostHopeMod.Utils;
using WildfrostHopeMod.VFX;
using static SelectLeader;
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

        public override string Description =>
            "What did Maxwell do this time? The survivors got brought to the infinite freezing winter??!! "
            + "\n"
            + "\n"
            + "\n"
            + "\n"
            + "[h1] DON'T FROSTBITE [/h1]"
            + "A mod that based of the game Don't Starve Together\n"
            + "\n"
            + "\n"
            + "[h3] This mod added [/h3]"
            + "[list]\n"
            + "[*] 5 Leaders with different starting items\n"
            + "[*] New Custom Battles\n"
            + "[*] Pets, Companions, Items, Charms\n"
            + "[*] New Resource System\n"
            + "[/list]"
            + "And more to comes!!"
            + "\n"
            + "\n"
            + "If you found any bug/ glitch/ crash please tell me down the comment or the game discord #mod-development @nubboiz\n"
            + "Thanks for playing :3";
        public override TMP_SpriteAsset SpriteAsset => spriteAsset;
        internal static TMP_SpriteAsset spriteAsset;

        public static string CatalogFolder => Path.Combine(Instance.ModDirectory, "Windows");
        public static string CatalogPath => Path.Combine(CatalogFolder, "catalog.json");

        public static SpriteAtlas Cards;
        public static SpriteAtlas Bosses;
        public static SpriteAtlas Leaders;
        public static SpriteAtlas Other;

        public Dictionary<string, TargetConstraint> allConstraint = new Dictionary<string, TargetConstraint>();

        public static string[] spriteStrings = { "Frame", "NameTag", "Mask", "FrameOutline", "DescriptionBox" };

        public RewardPool itemPool;
        public RewardPool unitPool;

        private void CreateTargetConstraint()
        {
            void AddTraitConstraint(string key, bool not = false) =>
                allConstraint.Add(key, new Scriptable<TargetConstraintHasTrait>(x => x.not = not));

            void AddStatusConstraint(string key, bool not = false) =>
                allConstraint.Add(key, new Scriptable<TargetConstraintHasStatus>(x => x.not = not));

            void AddSpecificCardConstraint(string key, bool not = false) =>
                allConstraint.Add(key, new Scriptable<TargetConstraintIsSpecificCard>(x => x.not = not));

            void AddCardTypeConstraint(string key, bool not = false) =>
                allConstraint.Add(key, new Scriptable<TargetConstraintIsCardType>(x => x.not = not));


            AddTraitConstraint("noChopable", true);
            AddTraitConstraint("noMineable", true);
            AddTraitConstraint("hammerOnly");
            AddTraitConstraint("pickaxeOnly");
            AddTraitConstraint("axeOnly");
            AddTraitConstraint("beeOnly");
            AddTraitConstraint("buildingOnly");

            AddCardTypeConstraint("companionOnly");

            AddStatusConstraint("clunkerOnly");
            AddStatusConstraint("noChestHealth", true);
            AddStatusConstraint("noBuilding", true);

            string[] specificOnlyKeys = {
                "abigailOnly", "floorOnly", "dflyOnly", "toadstoolOnly", "sandCastleOnly",
                "fuelweaverOnly", "klausOnly", "chestOnly", "catapultOnly", "moslingOnly",
                "wolfgangOnly", "wormwoodOnly", "wendyOnly", "charlieOnly"
            };
            foreach (var key in specificOnlyKeys)
                AddSpecificCardConstraint(key);

            AddSpecificCardConstraint("noCharlie", true);
            AddSpecificCardConstraint("noDfly", true);
            AddSpecificCardConstraint("noToadstool", true);
            AddSpecificCardConstraint("noBoomshroom", true);
        }

        private void ApplyConstraint()
        {
            void SetTrait(string key, string traitName) =>
        ((TargetConstraintHasTrait)allConstraint[key]).trait = TryGet<TraitData>(traitName);

            void SetStatus(string key, string statusName) =>
                ((TargetConstraintHasStatus)allConstraint[key]).status = TryGet<StatusEffectData>(statusName);

            void SetCards(string key, params string[] cardNames) =>
                ((TargetConstraintIsSpecificCard)allConstraint[key]).allowedCards = cardNames.Select(TryGet<CardData>).ToArray();

            void SetCardType(string key, params string[] cardTypes) =>
                ((TargetConstraintIsCardType)allConstraint[key]).allowedTypes = cardTypes.Select(TryGet<CardType>).ToArray();

            SetTrait("noChopable", "Chopable");
            SetTrait("noMineable", "Mineable");
            SetTrait("hammerOnly", "HammerType");
            SetTrait("pickaxeOnly", "PickaxeType");
            SetTrait("axeOnly", "AxeType");
            SetTrait("beeOnly", "Bee");
            SetTrait("buildingOnly", "Building");

            SetCardType("companionOnly", "Friendly");

            SetStatus("clunkerOnly", "Scrap");
            SetStatus("noChestHealth", "Chest Health");
            SetStatus("noBuilding", "Building Health");

            SetCards("abigailOnly", "abigail");
            SetCards("floorOnly", "floor");
            SetCards("dflyOnly", "dragonfly", "dragonflyEnraged");
            SetCards("toadstoolOnly", "toadstool", "toadstoolEnraged");
            SetCards("sandCastleOnly", "sandCastle");
            SetCards("fuelweaverOnly", "ancientFuelweaver");
            SetCards("klausOnly", "klaus", "klausEnraged", "winterKlaus");
            SetCards("chestOnly", "chest");
            SetCards("catapultOnly", "catapult");
            SetCards("moslingOnly", "mosling");
            SetCards("wolfgangOnly", "wolfgang");
            SetCards("wormwoodOnly", "wormwood");
            SetCards("wendyOnly", "wendy");
            SetCards("charlieOnly", "charlie");
            SetCards("noCharlie", "charlie");
            SetCards("noDfly", "dragonfly", "dragonflyEnraged");
            SetCards("noToadstool", "toadstool", "toadstoolEnraged");
            SetCards("noBoomshroom", "boomshroom");
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
            Sprite flagSprite = DSTMod.Other.GetSprite("DSTFlag");
            assets.Add(
                TribeCopy("Magic", "DST")
                    .WithFlag(flagSprite)
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
                                    "spear",
                                    "pickaxe",
                                    "axe",
                                    "hamBat",
                                    "iceStaff",
                                    "boosterShot",
                                    "walkingCane",
                                    "torch"
                                )
                                .ToList();
                            data.startingInventory = inventory;

                            data.rewardPools = new RewardPool[]
                            {
                                unitPool,
                                itemPool,
                                Extensions.GetRewardPool("GeneralUnitPool"),
                                Extensions.GetRewardPool("GeneralItemPool"),
                                Extensions.GetRewardPool("GeneralCharmPool"),
                                Extensions.GetRewardPool("GeneralModifierPool"),
                            };
                        }
                    )
            );
            #endregion

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
        public void CreateCustomCardFrames()
        {
            //Somewhere else, possibleSprites is defined like this:
            //public string[] possibleSprites = new string[] { "Frame", "NameTag", "Mask", "FrameOutline", "DescriptionBox" };
            // The full list of changeables can be found in CardCustomFrameSetter as well  
            Dictionary<string, Sprite> dictionary = new Dictionary<string, Sprite>();
            foreach (var names in spriteStrings)
            {
                //Using a very specific naming convention here. You don't have to, but the code will look slightly longer.
                dictionary[names] = ImagePath($"Leader{names}.png").ToSprite();
            }
            CustomCardFrameSystem.AddCustomFrame("dst.leader", "Friendly", dictionary);
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

        public IEnumerator CampaignInit()
        {
            References.LeaderData.startWithEffects = References
                .LeaderData.startWithEffects.Concat(new[] { SStack("Summon Chest Before Battle", 1), SStack("Summon Floor Before Battle", 1) })
                .ToArray();

            if (References.PlayerData?.classData.ModAdded != this)
                yield break;

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
            if (!Addressables.ResourceLocators.Any(r => r is ResourceLocationMap map && map.LocatorId == CatalogPath))
            {
                Addressables.LoadContentCatalogAsync(CatalogPath).WaitForCompletion();
            }
            Cards = (SpriteAtlas)Addressables.LoadAssetAsync<UnityEngine.Object>($"Assets/{GUID}/Cards.spriteatlas").WaitForCompletion();
            Leaders = (SpriteAtlas)Addressables.LoadAssetAsync<UnityEngine.Object>($"Assets/{GUID}/Leaders.spriteatlas").WaitForCompletion();
            Bosses = (SpriteAtlas)Addressables.LoadAssetAsync<UnityEngine.Object>($"Assets/{GUID}/Bosses.spriteatlas").WaitForCompletion();
            Other = (SpriteAtlas)Addressables.LoadAssetAsync<UnityEngine.Object>($"Assets/{GUID}/Other.spriteatlas").WaitForCompletion();

            if (!preLoaded)
            {
                spriteAsset = HopeUtils.CreateSpriteAsset(Title, directoryWithPNGs: ImagePath("Icons"));
                CreateTargetConstraint();
                CreateCustomCardFrames();
                CreateModAssets();
            }

            prefabHolder = new GameObject(GUID);
            UnityEngine.Object.DontDestroyOnLoad(prefabHolder);
            prefabHolder.SetActive(false);

            SpriteAsset.RegisterSpriteAsset();
            base.Load();

            CreateBattleAssets();
            foreach (var (num, battleDataEditor) in battleAssets)
            {
                battleDataEditor.ToggleBattle(true);
            }
            
            ApplyConstraint();

            VFXHelper.SFX = new SFXLoader(ImagePath("Sounds"));
            VFXHelper.SFX.RegisterAllSoundsToGlobal();
            VFXHelper.VFX = new GIFLoader(this, ImagePath("Animations"));
            VFXHelper.VFX.RegisterAllAsApplyEffect();

            

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
            gameMode.classes = RemoveNulls(gameMode.classes);
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
