using System;
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
        public static List<(int, BattleDataEditor)> battleAssets =
            new List<(int, BattleDataEditor)>();
        private bool preLoaded = false;

        public DSTMod(string modDir)
            : base(modDir)
        {
            HarmonyInstance.PatchAll(typeof(PatchHarmony));
        }

        public override string GUID => "tgestudio.wildfrost.dstmod";

        public override string[] Depends => new string[] { "hope.wildfrost.vfx" };

        public override string Title => "Don't Frostbite";

        public override string Description => "A mod that get idea from don't starve";
        public override TMP_SpriteAsset SpriteAsset => spriteAsset;
        internal static TMP_SpriteAsset spriteAsset;

        private void CreateModAssets()
        {
            //Code for Status Effects

            #region Leader

            #region Wendy Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("wendy", "Wendy")
                    .WithCardType("Leader")
                    .SetSprites("Wendy.png", "Wendy_BG.png")
                    .SetStats(7, 2, 2)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("When Deployed Summon Abigail", 1),
                                SStack("When Abigail Destroyed Gain Card", 1),
                            };
                            data.createScripts = new CardScript[]
                            {
                                GiveUpgrade(),
                                AddRandomHealth(-1, 2),
                                AddRandomDamage(-1, 1),
                                AddRandomCounter(0, 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Abigail")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.abigail>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("abigail");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Abigail")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Abigail");
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Abigail")
                    .WithText("When deployed, summon {0}")
                    .WithTextInsert("<card=tgestudio.wildfrost.dstmod.abigail>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(
                        delegate (StatusEffectApplyXWhenDeployed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Abigail");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenCardDestroyed>("When Abigail Destroyed Gain Card")
                    .WithText($"<keyword={Extensions.PrefixGUID("mourningglory", this)}>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(
                        delegate (StatusEffectApplyXWhenCardDestroyed data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Abigail Flower In Hand"
                            );

                            TargetConstraintIsSpecificCard card =
                                ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
                            card.name = "Abigail";
                            card.allowedCards = new CardData[1] { TryGet<CardData>("abigail") };
                            data.constraints = new TargetConstraint[1] { card };
                        }
                    )
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("mourningglory")
                    .WithTitle("Mourning Glory")
                    .WithTitleColour(new Color(0.92f, 0.44f, 0.46f))
                    .WithShowName(true)
                    .WithDescription(
                        "When <card=tgestudio.wildfrost.dstmod.abigail> got destroyed, gain <card=tgestudio.wildfrost.dstmod.abigailFlower>"
                    )
                    .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            #endregion

            #region Wortox Card
            //Wortox Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("wortox", "Wortox")
                    .WithCardType("Leader")
                    .SetSprites("Wortox.png", "Wendy_BG.png")
                    .SetStats(11, 5, 2)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("When Enemy Is Killed Gain Card", 1),
                            };
                            data.createScripts = new CardScript[]
                            {
                                GiveUpgrade(),
                                AddRandomHealth(-1, 2),
                                AddRandomDamage(-1, 1),
                                AddRandomCounter(0, 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Trigger When Enemy Is Killed", "When Enemy Is Killed Gain Card")
                    .WithText(
                        "When an Enemy is killed, Add <card=tgestudio.wildfrost.dstmod.soul> or <card=tgestudio.wildfrost.dstmod.souls> to hand"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(
                        delegate (StatusEffectApplyXWhenCardDestroyed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Soul In Hand"
                            );
                        }
                    )
            );

            //Wortox Effect
            assets.Add(
                StatusCopy("Summon Junk", "Summon Soul")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("soul");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Souls")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("souls");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandom>("Instant Summon Soul In Hand")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectInstantSummonRandom data)
                        {
                            data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                            data.randomCards = new StatusEffectSummon[2]
                            {
                                TryGet<StatusEffectSummon>("Summon Soul"),
                                TryGet<StatusEffectSummon>("Summon Souls"),
                            };
                        }
                    )
            );

            //Soul Effect
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Heal To Allies")
                    .WithText("Restore {0} to all allies")
                    .WithTextInsert("<{a}><keyword=health>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(
                        delegate (StatusEffectApplyXOnCardPlayed data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                            data.effectToApply = TryGet<StatusEffectData>("Heal");
                        }
                    )
            );

            //Soul Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("souls", "Souls")
                    .SetSprites("Souls.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1), TStack("Consume", 1))
                    .WithPlayType(Card.PlayType.Play)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.needsTarget = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Card Played Heal To Allies", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("soul", "Soul")
                    .SetSprites("Soul.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1), TStack("Consume", 1))
                    .SetAttackEffect(SStack("Heal", 3))
            );
            #endregion

            #endregion

            #region Companion

            #region Abigail Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("abigail", "Abigail")
                    .SetSprites("Abigail.png", "Abigail_BG.png")
                    .SetStats(5, 3, 0)
                    .WithCardType("Summoned")
                    .SetTraits(new CardData.TraitStacks(Get<TraitData>("Barrage"), 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Trigger When Wendy Attacks", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectTriggerWhenCertainAllyAttacks>("Trigger When Wendy Attacks")
                    .WithCanBeBoosted(false)
                    .WithText("Trigger when {0} attacks")
                    .WithTextInsert("<card=tgestudio.wildfrost.dstmod.wendy>")
                    .FreeModify(
                        delegate (StatusEffectData data)
                        {
                            data.isReaction = true;
                            data.stackable = false;
                        }
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenCertainAllyAttacks>(
                        delegate (StatusEffectTriggerWhenCertainAllyAttacks data)
                        {
                            data.allyInRow = false;
                            data.ally = TryGet<CardData>("wendy");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Abigail Flower")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.abigail>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("abigailFlower");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Abigail Flower In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Abigail Flower");
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("abigailFlower", "Abigail's Flower")
                    .SetSprites("abigailFlower.png", "Wendy_BG.png")
                    .SetTraits(TStack("Consume", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.playOnSlot = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Summon Abigail", 1),
                            };
                        }
                    )
            );
            #endregion

            #region Chester Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("chester", "Chester")
                    .SetSprites("Chester.png", "Wendy_BG.png")
                    .SetStats(20, null, 0)
                    .WithCardType("Summoned")
            );
            assets.Add(
                StatusCopy("Summon Plep", "Summon Chester")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.chester>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("chester");
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("eyeBone", "Eye Bone")
                    .WithCardType("Item")
                    .SetSprites("EyeBone.png", "Wendy_BG.png")
                    .SetTraits(TStack("Consume", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.playOnSlot = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Summon Chester", 1),
                            };
                        }
                    )
            );
            #endregion

            #region IceChester Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("iceChester", "Ice Chester")
                    .SetSprites("IceChester.png", "Wendy_BG.png")
                    .SetStats(15, null, 0)
                    .WithCardType("Friendly")
                    .SetStartWithEffect(SStack("When Hit Apply Snow To Attacker", 2))
            );
            #endregion

            #region ShadowChester Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("shadowChester", "Shadow Chester")
                    .SetSprites("ShadowChester.png", "Wendy_BG.png")
                    .SetStats(12, null, 0)
                    .WithCardType("Friendly")
                    .SetStartWithEffect(SStack("When Hit Apply Demonize To Attacker", 1))
            );
            #endregion

            #region Glommer Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("glommer", "Glommer")
                    .SetSprites("Glommer.png", "Wendy_BG.png")
                    .SetStats(3, null, 5)
                    .WithCardType("Friendly")
                    .SetStartWithEffect(SStack("On Turn Add Attack To Allies", 2))
            );
            #endregion

            #region Tallbird Card

            #region Tallbird Egg
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("tallbirdEgg", "Tallbird Egg")
                    .SetSprites("TallBirdEgg.png", "Wendy_BG.png")
                    .SetStats(1, null, 5)
                    .WithText($"<keyword={Extensions.PrefixGUID("growth", this)}>")
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("On Turn Summon Smallbird", 1),
                                SStack("Destroy Self After Counter Turn", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Smallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("smallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Smallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon =
                                TryGet<StatusEffectData>("Summon Smallbird") as StatusEffectSummon;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        "On Turn Summon Smallbird"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate (StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>(
                                "Instant Summon Smallbird"
                            );
                        }
                    )
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("growth")
                    .WithTitle("Growth")
                    .WithTitleColour(new Color(0.49f, 0.42f, 0.54f))
                    .WithShowName(true)
                    .WithDescription(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.smallbird> then destroy self"
                    )
                    .WithNoteColour(new Color(0.91f, 0.26f, 0.27f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );

            #endregion

            #region BabyTallBird
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("smallbird", "Smallbird")
                    .SetSprites("SmallBird.png", "Wendy_BG.png")
                    .WithText($"<keyword={Extensions.PrefixGUID("growth1", this)}>")
                    .SetStats(5, 3, 5)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[3]
                            {
                                SStack("On Turn Summon Smallish Tallbird", 1),
                                SStack("Destroy Self After Counter Turn", 1),
                                SStack("Trigger When Ally Is Hit", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        "On Turn Summon Smallish Tallbird"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate (StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>(
                                "Instant Summon Smallish Tallbird"
                            );
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this).Create<StatusEffectDestroySelfAfterCounterTurn>(
                    "Destroy Self After Counter Turn"
                )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Smallish Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("smallishTallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Smallish Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>(
                                "Summon Smallish Tallbird"
                            );
                        }
                    )
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("growth1")
                    .WithTitle("Growth I")
                    .WithTitleColour(new Color(0.49f, 0.42f, 0.54f))
                    .WithShowName(true)
                    .WithDescription(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.smallishTallbird> then destroy self"
                    )
                    .WithNoteColour(new Color(0.91f, 0.26f, 0.27f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            #endregion

            #region SmallishTallBird
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("smallishTallbird", "Smallish Tallbird")
                    .WithText($"<keyword={Extensions.PrefixGUID("growth2", this)}>")
                    .SetSprites("SmallishBird.png", "Wendy_BG.png")
                    .SetStats(8, 5, 5)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[3]
                            {
                                SStack("On Turn Summon Tallbird", 1),
                                SStack("Destroy Self After Counter Turn", 1),
                                SStack("Trigger When Ally Is Hit", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("tallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Tallbird");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        "On Turn Summon Tallbird"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate (StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>(
                                "Instant Summon Tallbird"
                            );
                        }
                    )
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("growth2")
                    .WithTitle("Growth II")
                    .WithTitleColour(new Color(0.49f, 0.42f, 0.54f))
                    .WithShowName(true)
                    .WithDescription(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.tallbird> then destroy self"
                    )
                    .WithNoteColour(new Color(0.91f, 0.26f, 0.27f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            #endregion

            #region TallBird
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("tallbird", "Tallbird")
                    .SetSprites("TallBird.png", "Wendy_BG.png")
                    .SetStats(18, 10, 4)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("MultiHit", 1),
                            };
                        }
                    )
            );
            #endregion

            #endregion

            #region FriendlyFly Card
            //Gloomer Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("friendlyFly", "Friendly Fly")
                    .SetSprites("FriendFly.png", "Wendy_BG.png")
                    .SetStats(2, null, 5)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Turn Boost Allies Effect", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Boost Allies Effect")
                    .WithText("Boost allies effect by <{a}>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate (StatusEffectApplyXOnTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                            data.effectToApply = TryGet<StatusEffectData>("Increase Effects");
                        }
                    )
            );
            #endregion

            #region Bunnyman Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("bunnyman", "Bunnyman")
                    .WithText(
                        "When hit escape to hand and becomes <card=tgestudio.wildfrost.dstmod.bunnymanInjured>"
                    )
                    .SetSprites("Bunnyman.png", "Wendy_BG.png")
                    .SetStats(8, 5, 3)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("After Hit Escape", 1),
                                SStack("After Hit Summon Bunnyman Injured In Hand", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("bunnymanInjured", "Bunnyman Injured")
                    .SetSprites("BunnymanInjured.png", "Wendy_BG.png")
                    .SetTraits(TStack("Smackback", 1))
                    .SetStats(4, 2, 4)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Turn Increase Health And Damage", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Bunnyman Injured")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("bunnymanInjured");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Bunnyman Injured In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>(
                                "Summon Bunnyman Injured"
                            );
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenHit>("After Hit Escape")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(
                        delegate (StatusEffectApplyXWhenHit data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Escape");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenHit>("After Hit Summon Bunnyman Injured In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(
                        delegate (StatusEffectApplyXWhenHit data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Bunnyman Injured In Hand"
                            );
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Increase Health And Damage")
                    .WithText("Increase <keyword=attack> and <keyword=health> by <{a}>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate (StatusEffectApplyXOnTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Increase Attack & Health"
                            );
                        }
                    )
            );
            #endregion

            #region PigMan Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("pigman", "Pig")
                    .SetSprites("Pig.png", "Wendy_BG.png")
                    .SetTraits(TStack("Smackback", 1), TStack("Pigheaded", 1))
                    .SetStats(10, 6, 0)
                    .WithCardType("Friendly")
            );
            #endregion

            #region Pearl Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("pearl", "Pearl")
                    .SetSprites("Pearl.png", "Wendy_BG.png")
                    .SetTraits(TStack("Fragile", 1))
                    .SetStats(1, null, 4)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Turn Reduce Counter To Allies", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("On Turn Add Attack To Allies", "On Turn Reduce Counter To Allies")
                    .WithText("Reduce <keyword=counter> by <{a}> to all allies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate (StatusEffectApplyXOnTurn data)
                        {
                            data.canBeBoosted = false;
                            data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
                        }
                    )
            );
            #endregion

            #region Beefalo Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("beefalo", "Beefalo")
                    .SetSprites("Beefalo.png", "Wendy_BG.png")
                    .SetTraits(TStack("Knockback", 1), TStack("Frontline", 1))
                    .SetStats(10, 6, 4)
                    .WithCardType("Friendly")
            );
            #endregion

            #region RabbitKing
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("fortuitousRabbit", "Fortuitous Rabbit")
                    .SetSprites("scareRabbit.png", "Wendy_BG.png")
                    .SetTraits(TStack("Fragile", 1))
                    .WithText($"<keyword={Extensions.PrefixGUID("neutral", this)}>")
                    .SetStats(1, null, 7)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("Neutral On Turn Summon BRK On Destroyed Summon WRB", 1),
                                SStack("Destroy Self After Counter Turn", 1),
                            };
                        }
                    )
            );

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        "Neutral On Turn Summon BRK On Destroyed Summon WRB"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate (StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>(
                                "Instant Summon Benevolent Rabbit King"
                            );
                            data.effectToApplyWhenNotOnCounterTurn = TryGet<StatusEffectData>(
                                "Instant Summon Wrathful Rabbit King"
                            );
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("wrathfulRabbitKing", "Wrathful Rabbit King")
                    .SetSprites("evilRabbit.png", "Wendy_BG.png")
                    .SetTraits(TStack("Spark", 1))
                    .SetStats(8, 5, 3)
                    .WithCardType("Enemy")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("While Active Frenzy To AlliesInRow", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("benevolentRabbitKing", "Benevolent Rabbit King")
                    .SetSprites("friendlyRabbit.png", "Wendy_BG.png")
                    .SetTraits(TStack("Spark", 1))
                    .SetStats(8, 5, 3)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("While Active Frenzy To AlliesInRow", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Benevolent Rabbit King")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.benevolentRabbitKing>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("benevolentRabbitKing");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Benevolent Rabbit King")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>(
                                "Summon Benevolent Rabbit King"
                            );
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Wrathful Rabbit King")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.wrathfulRabbitKing>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("wrathfulRabbitKing");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Wrathful Rabbit King")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>(
                                "Summon Wrathful Rabbit King"
                            );
                            data.summonPosition = StatusEffectInstantSummon.Position.EnemyRow;
                        }
                    )
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("neutral")
                    .WithTitle("Neutral")
                    .WithTitleColour(new Color(1f, 0.79f, 0.34f))
                    .WithShowName(true)
                    .WithDescription(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.benevolentRabbitKing> "
                            + "else if got destroyed before then summon <card=tgestudio.wildfrost.dstmod.wrathfulRabbitKing> on the enemy side"
                    )
                    .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            #endregion

            #endregion

            #region Pet

            #region DFly Pet
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("broodling", "Broodling")
                    .IsPet("", true)
                    .SetSprites("Broodling.png", "Wendy_BG.png")
                    .SetStartWithEffect(SStack("When Hit Apply Spice To AlliesInRow", 2))
                    .SetStats(6, 3, 5)
                    .WithCardType("Friendly")
            );
            #endregion

            #region Vargling Pet
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("vargling", "Vargling")
                    .IsPet("", true)
                    .SetSprites("Vargling.png", "Wendy_BG.png")
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .SetStats(4, 2, 3)
                    .WithCardType("Friendly")
            );
            #endregion

            #region Kittykit Pet
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("kittykit", "Kittykit")
                    .IsPet("", true)
                    .SetSprites("Kittykit.png", "Wendy_BG.png")
                    .SetTraits(TStack("Greed", 1))
                    .SetStats(5, 1, 3)
                    .WithCardType("Friendly")
            );
            #endregion

            #endregion

            #region Items

            #region Spear Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("spear", "Spear")
                    .SetSprites("Spear.png", "Wendy_BG.png")
                    .SetStats(null, 3, 0)
                    .WithCardType("Item")
            );
            #endregion

            #region HamBat Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("hamBat", "Ham Bat")
                    .SetStats(null, 4, 0)
                    .SetSprites("HamBat.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Card Played Reduce Attack", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Reduce Attack")
                    .WithText("Reduce <keyword=attack> by <{a}> when played")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(
                        delegate (StatusEffectApplyXOnCardPlayed data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Reduce Attack");
                        }
                    )
            );
            #endregion

            #region IceStaff Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("iceStaff", "Ice Staff")
                    .SetSprites("IceStaff.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Snow", 1),
                            };
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Card Played Increase Snow", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Increase Snow")
                    .WithText("Increase <keyword=snow> by 1 when played")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(
                        delegate (StatusEffectApplyXOnCardPlayed data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Increase Effects");
                        }
                    )
            );
            #endregion

            #region WalkingCane Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("walkingCane", "Walking Cane")
                    .SetDamage(1)
                    .SetSprites("WalkingCane.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Reduce Counter", 3),
                            };
                        }
                    )
            );
            #endregion

            #region LogSuit Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("logSuit", "Log Suit")
                    .SetSprites("LogSuit.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Shell", 2),
                            };
                        }
                    )
            );
            #endregion

            #region BoosterShot Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("boosterShot", "Booster Shot")
                    .SetSprites("BoosterShot.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Increase Max Health", 2),
                            };
                        }
                    )
            );
            #endregion

            #region Pickaxe Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("pickaxe", "Pickaxe")
                    .SetStats(null, 1, 0)
                    .WithText($"<keyword={Extensions.PrefixGUID("pickaxetype", this)}>")
                    .SetSprites("Pickaxe.png", "Wendy_BG.png")
                    .WithCardType("Item")
            );
            #endregion

            #region Axe Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("axe", "Axe")
                    .SetStats(null, 2, 0)
                    .WithText($"<keyword={Extensions.PrefixGUID("axetype", this)}>")
                    .SetSprites("Axe.png", "Wendy_BG.png")
                    .WithCardType("Item")
            );
            #endregion

            #region Hammer Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("hammer", "Hammer")
                    .SetStats(null, 1, 0)
                    .WithText($"<keyword={Extensions.PrefixGUID("hammertype", this)}>")
                    .SetSprites("Hammer.png", "Wendy_BG.png")
                    .WithCardType("Item")
            );
            #endregion

            #endregion

            #region Enemy

            #region Resources
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("stone", "Stone")
                    .SetStats(null, null, 0)
                    .WithText($"<keyword={Extensions.PrefixGUID("mineable", this)}>")
                    .SetSprites("Stone.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("ResourceMineable", 1),
                                SStack("When Destroyed Instant Summon Rock In Hand", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("goldOre", "Gold Ore")
                    .SetStats(null, null, 0)
                    .WithText($"<keyword={Extensions.PrefixGUID("mineable", this)}>")
                    .SetSprites("GoldOre.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("ResourceMineable", 2),
                                SStack("When Destroyed Instant Summon Gold In Hand", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("smallTree", "Small Tree")
                    .SetStats(null, null, 0)
                    .WithText($"<keyword={Extensions.PrefixGUID("chopable", this)}>")
                    .SetSprites("SmallTree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("When Destroyed Instant Summon Wood In Hand", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("tree", "Tree")
                    .SetStats(null, null, 0)
                    .WithText($"<keyword={Extensions.PrefixGUID("chopable", this)}>")
                    .SetSprites("Tree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("ResourceChopable", 3),
                                SStack("When Destroyed Instant Summon Wood In Hand", 2),
                            };
                        }
                    )
            );

            #region Resource Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("rock", "Rock")
                    .SetStats(null, null, 0)
                    .SetSprites("Rock.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.canPlayOnBoard = false;
                            data.canPlayOnEnemy = false;
                            data.canPlayOnHand = false;
                            data.canPlayOnFriendly = false;
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("gold", "Gold")
                    .SetStats(null, null, 0)
                    .SetSprites("Gold.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.canPlayOnBoard = false;
                            data.canPlayOnEnemy = false;
                            data.canPlayOnHand = false;
                            data.canPlayOnFriendly = false;
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("wood", "Wood")
                    .SetStats(null, null, 0)
                    .SetSprites("Wood.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.canPlayOnBoard = false;
                            data.canPlayOnEnemy = false;
                            data.canPlayOnHand = false;
                            data.canPlayOnFriendly = false;
                        }
                    )
            );
            #endregion

            #region Summon Resource Effect
            assets.Add(
                StatusCopy("Summon Junk", "Summon Wood")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("wood");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Rock")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("rock");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Gold")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("gold");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Wood In Hand")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Wood");
                            data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Rock In Hand")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Rock");
                            data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Gold In Hand")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Gold");
                            data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                        }
                    )
            );
            assets.Add(
                StatusCopy(
                        "When Destroyed Add Health To Allies",
                        "When Destroyed Instant Summon Wood In Hand"
                    )
                    .WithText("Gain <{a}> <card=tgestudio.wildfrost.dstmod.wood> when destroyed")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectApplyXWhenDestroyed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Wood In Hand"
                            );
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                        }
                    )
            );
            assets.Add(
                StatusCopy(
                        "When Destroyed Add Health To Allies",
                        "When Destroyed Instant Summon Rock In Hand"
                    )
                    .WithText("Gain <{a}> <card=tgestudio.wildfrost.dstmod.rock> when destroyed")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectApplyXWhenDestroyed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Rock In Hand"
                            );
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                        }
                    )
            );
            assets.Add(
                StatusCopy(
                        "When Destroyed Add Health To Allies",
                        "When Destroyed Instant Summon Gold In Hand"
                    )
                    .WithText("Gain <{a}> <card=tgestudio.wildfrost.dstmod.gold> when destroyed")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectApplyXWhenDestroyed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Gold In Hand"
                            );
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                        }
                    )
            );
            #endregion

            #endregion

            #region Spider

            #region Spider Queen
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("spiderQueen", "Spider Queen")
                    .SetSprites("SpiderQueen.png", "Wendy_BG.png")
                    .SetStats(20, 2, 4)
                    .SetTraits(TStack("Smackback", 1))
                    .WithCardType("Miniboss")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Counter Turn Summon Spiders With Nurse", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>(
                        "On Counter Turn Summon Spiders With Nurse"
                    )
                    .WithText(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.spiderWarrior> or <card=tgestudio.wildfrost.dstmod.spiderNurse>"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(
                        delegate (StatusEffectApplyXOnCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Summon Spiders With Nurse"
                            );
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandom>("Summon Spiders With Nurse")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(
                        delegate (StatusEffectInstantSummonRandom data)
                        {
                            data.summonPosition = StatusEffectInstantSummon
                                .Position
                                .InFrontOfOrOtherRow;
                            data.randomCards = new StatusEffectSummon[2]
                            {
                                TryGet<StatusEffectSummon>("Summon Spider Nurse"),
                                TryGet<StatusEffectSummon>("Summon Spider Warrior"),
                            };
                        }
                    )
            );
            #endregion

            #region SpiderNormal

            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("spider", "Spider")
                    .SetSprites("Spider.png", "Wendy_BG.png")
                    .SetTraits(TStack("Aimless", 1))
                    .SetStats(4, 1, 3)
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .WithCardType("Enemy")
            );

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Spider")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.spider>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("spider");
                            data.setCardType = TryGet<CardType>("Enemy");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Spider")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Spider");
                        }
                    )
            );

            #endregion

            #region SpiderWarrior
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("spiderWarrior", "Spider Warrior")
                    .SetSprites("SpiderWarrior.png", "Wendy_BG.png")
                    .SetStats(8, 4, 3)
                    .SetTraits(TStack("Frontline", 1))
                    .WithCardType("Enemy")
            );

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Spider Warrior")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.spiderWarrior>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("spiderWarrior");
                            data.setCardType = TryGet<CardType>("Enemy");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Spider Warrior")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Spider Warrior");
                        }
                    )
            );
            #endregion

            #region SpiderNurse
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("spiderNurse", "Spider Nurse")
                    .SetSprites("SpiderNurse.png", "Wendy_BG.png")
                    .SetStats(5, 1, 3)
                    .SetStartWithEffect(SStack("On Turn Heal Allies", 1))
                    .WithCardType("Enemy")
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Spider Nurse")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.spiderNurse>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("spiderNurse");
                            data.setCardType = TryGet<CardType>("Enemy");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Spider Nurse")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate (StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Spider Nurse");
                        }
                    )
            );
            #endregion

            #region SpiderNest
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("spiderNest", "Spider Nest")
                    .SetSprites("SpiderNest.png", "Wendy_BG.png")
                    .SetStats(8, null, 3)
                    .SetTraits(TStack("Backline", 1), TStack("Unmovable",1))
                    .WithCardType("Enemy")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Counter Turn Summon Spiders", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Spiders")
                    .WithText(
                        "Summon <card=tgestudio.wildfrost.dstmod.spider> or <card=tgestudio.wildfrost.dstmod.spiderWarrior>"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(
                        delegate (StatusEffectApplyXOnCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Summon Spiders");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandom>("Summon Spiders")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(
                        delegate (StatusEffectInstantSummonRandom data)
                        {
                            data.summonPosition = StatusEffectInstantSummon
                                .Position
                                .InFrontOf;
                            data.randomCards = new StatusEffectSummon[2]
                            {
                                TryGet<StatusEffectSummon>("Summon Spider"),
                                TryGet<StatusEffectSummon>("Summon Spider Warrior"),
                            };
                        }
                    )
            );
            #endregion

            #endregion

            #endregion

            #region Tribe
            //Add Tribe
            assets.Add(
                TribeCopy("Magic", "DST")
                    .WithFlag("Images/DSTFlag.png")
                    .WithSelectSfxEvent(
                        FMODUnity.RuntimeManager.PathToEventReference("event:/sfx/card/summon")
                    )
                    .SubscribeToAfterAllBuildEvent(
                        (data) =>
                        {
                            GameObject gameObject =
                                data.characterPrefab.gameObject.InstantiateKeepName();
                            UnityEngine.Object.DontDestroyOnLoad(gameObject);
                            gameObject.name = "Player (dstmod.DST)";
                            data.characterPrefab = gameObject.GetComponent<Character>();
                            data.id = "dstmod.DST";

                            data.leaders = DataList<CardData>(
                                "wendy",
                                "wortox",
                                "Leader1_heal_on_kill"
                            );

                            Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
                            inventory.deck.list = DataList<CardData>(
                                    "eyeBone",
                                    "scienceMachineBlueprint",
                                    "spear",
                                    "pickaxe",
                                    "pickaxe",
                                    "axe",
                                    "axe",
                                    "hammer",
                                    "hamBat",
                                    "iceStaff",
                                    "logSuit",
                                    "boosterShot",
                                    "walkingCane"
                                )
                                .ToList();
                            //inventory.upgrades.Add(TryGet<CardUpgradeData>("CardUpgradeCritical"));
                            data.startingInventory = inventory;

                            RewardPool unitPool = CreateRewardPool(
                                "DrawUnitPool",
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
                                    "fortuitousRabbit",
                                    "tallbirdEgg"
                                //"Flash",
                                //"TinyTyko"
                                )
                            );

                            //RewardPool itemPool = CreateRewardPool("DrawItemPool", "Items", DataList<CardData>(
                            //    "ShellShield", "StormbearSpirit", "PepperFlag", "SporePack", "Woodhead",
                            //    "BeepopMask", "Dittostone", "Putty", "Dart", "SharkTooth",
                            //    "Bumblebee", "Badoo", "Juicepot", "PomDispenser", "LuminShard",
                            //    "Wrenchy", "Vimifier", "OhNo", "Madness", "Joob"));

                            //RewardPool charmPool = CreateRewardPool("DrawCharmPool", "Charms", DataList<CardUpgradeData>(
                            //    "CardUpgradeTrash",
                            //    "CardUpgradeInk", "CardUpgradeOverload",
                            //    "CardUpgradeMime", "CardUpgradeShellBecomesSpice",
                            //    "CardUpgradeAimless"));

                            data.rewardPools = new RewardPool[]
                            {
                                unitPool,
                                //itemPool,
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

            #region StatusType

            #region Sanity
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("sanity")
                    .WithTitle("Sanity")
                    .WithDescription(
                        "Summon <Shadow Creature> at enemy side when more than or equal to health"
                    )
            );

            assets.Add(
                new StatusIconBuilder(this)
                    .Create(
                        name: "sanity icon",
                        statusType: "dst.sanity",
                        ImagePath("Icons/Sanity.png")
                    )
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0.2f, 0f, 0f, 0.8f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f), offsetY: -1f)
                    .WithTextboxSprite()
                    //.WithEffectDamageVFX(ImagePath("Icons/Sanity_Hit.gif"))
                    .WithApplyVFX(ImagePath("Icons/Sanity_Apply.gif"))
                    .WithApplySFX(ImagePath("Sanity_Apply.wav"))
                    .WithKeywords(iconKeywordOrNull: "sanity")
            );

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectSanity>("Sanity")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSanity>(
                        delegate (StatusEffectSanity data)
                        {
                            data.shadowEnemy = new StatusEffectSummon[2]
                            {
                                TryGet<StatusEffectSummon>("Summon Crawling Horror"),
                                TryGet<StatusEffectSummon>("Summon Terrorbeak"),
                            };
                        }
                    )
                    .Subscribe_WithStatusIcon("sanity icon")
            );

            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("crawlingHorror", "Crawling Horror")
                    .SetSprites("CrawlingHorror.png", "Wendy_BG.png")
                    .SetStats(8, 2, 4)
                    .WithCardType("Enemy")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Sanity", 2),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("terrorbeak", "Terrorbeak")
                    .SetSprites("TerrorBeak.png", "Wendy_BG.png")
                    .SetStats(5, 4, 3)
                    .WithCardType("Enemy")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Sanity", 3),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Crawling Horror")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("crawlingHorror");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Terrorbeak")
                    .SubscribeToAfterAllBuildEvent(
                        delegate (StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("terrorbeak");
                        }
                    )
            );

            #endregion

            #region Resource
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("resource")
                    .WithTitle("Resource")
                    .WithDescription("Can only be damaged by <Pickaxe> or <Axe> cards")
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("mineable")
                    .WithTitle("Mineable")
                    .WithShowName(true)
                    .WithDescription("Can only be damaged by <Pickaxe> cards")
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("chopable")
                    .WithTitle("Chopable")
                    .WithShowName(true)
                    .WithDescription("Can only be damaged by <Axe> cards")
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("pickaxetype")
                    .WithTitle("Pickaxe")
                    .WithShowName(true)
                    .WithDescription(
                        "Can damage <card=tgestudio.wildfrost.dstmod.stone> and <card=tgestudio.wildfrost.dstmod.goldOre>"
                    )
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("axetype")
                    .WithTitle("Axe")
                    .WithShowName(true)
                    .WithDescription(
                        "Can damage <card=tgestudio.wildfrost.dstmod.smallTree> and <card=tgestudio.wildfrost.dstmod.tree>"
                    )
            );

            assets.Add(
                new StatusIconBuilder(this)
                    .Create(
                        name: "resource icon",
                        statusType: "dst.resource",
                        ImagePath("Icons/Resource.png")
                    )
                    .WithTextColour(new Color(0.2f, 0f, 0f, 0.8f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f), offsetY: -1f)
                    .WithTextboxSprite()
                    //.WithEffectDamageVFX(ImagePath("Icons/Sanity_Hit.gif"))
                    //.WithApplyVFX(ImagePath("Icons/Sanity_Apply.gif"))
                    .WithWhenHitSFX(ImagePath("Rock_Apply.wav"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithKeywords(iconKeywordOrNull: "resource")
            );

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectResource>("ResourceMineable")
                    .WithIsStatus(true)
                    .SubscribeToAfterAllBuildEvent<StatusEffectResource>(
                        delegate (StatusEffectResource data)
                        {
                            data.allowedCards = new CardData[1] { TryGet<CardData>("pickaxe") };
                            data.preventDeath = true;
                            data.affectedBySnow = false;
                            data.canBeBoosted = false;
                            data.stackable = true;
                            data.type = "dst.resource";
                        }
                    )
                    .Subscribe_WithStatusIcon("resource icon")
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectResource>("ResourceChopable")
                    .WithIsStatus(true)
                    .SubscribeToAfterAllBuildEvent<StatusEffectResource>(
                        delegate (StatusEffectResource data)
                        {
                            data.allowedCards = new CardData[1] { TryGet<CardData>("axe") };
                            data.preventDeath = true;
                            data.affectedBySnow = false;
                            data.canBeBoosted = false;
                            data.stackable = true;
                            data.type = "dst.resource";
                        }
                    )
                    .Subscribe_WithStatusIcon("resource icon")
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedByCertainCards>(
                        "When Destroyed By Hammer Gain Rock"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedByCertainCards>(
                        delegate (StatusEffectApplyXWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.cardsToApply = new CardData[1] { TryGet<CardData>("hammer") };
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Rock In Hand"
                            );
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedByCertainCards>(
                        "When Destroyed By Hammer Gain Gold"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedByCertainCards>(
                        delegate (StatusEffectApplyXWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.cardsToApply = new CardData[1] { TryGet<CardData>("hammer") };
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Gold In Hand"
                            );
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedByCertainCards>(
                        "When Destroyed By Hammer Gain Wood"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedByCertainCards>(
                        delegate (StatusEffectApplyXWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.cardsToApply = new CardData[1] { TryGet<CardData>("hammer") };
                            data.effectToApply = TryGet<StatusEffectData>(
                                "Instant Summon Wood In Hand"
                            );
                        }
                    )
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("building")
                    .WithTitle("Building")
                    .WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
                    .WithShowName(true)
                    .WithDescription(
                        "Use <card=tgestudio.wildfrost.dstmod.hammer> to destroy and return resources used| Can't be recalled"
                    )
                    .WithNoteColour(new Color(65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("hammertype")
                    .WithTitle("Hammer")
                    .WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
                    .WithShowName(true)
                    .WithDescription("Can use to destroy <Building> to gain back resource used")
                    .WithCanStack(false)
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectCraft>("Require Rock")
                    .WithText("Require <{a}> <card=tgestudio.wildfrost.dstmod.rock>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(
                        delegate (StatusEffectCraft data)
                        {
                            data.cardToRequire = TryGet<CardData>("rock").name;
                            data.requireType = PatchingScript.NoTargetTypeExt.RequireRock;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectCraft>("Require Wood")
                    .WithText("Require <{a}> <card=tgestudio.wildfrost.dstmod.wood>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(
                        delegate (StatusEffectCraft data)
                        {
                            data.cardToRequire = TryGet<CardData>("wood").name;
                            data.requireType = PatchingScript.NoTargetTypeExt.RequireWood;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectCraft>("Require Gold")
                    .WithText("Require <{a}> <card=tgestudio.wildfrost.dstmod.gold>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(
                        delegate (StatusEffectCraft data)
                        {
                            data.cardToRequire = TryGet<CardData>("gold").name;
                            data.requireType = PatchingScript.NoTargetTypeExt.RequireGold;
                        }
                    )
            );
            #endregion

            #endregion

            #region Science Machine
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("scienceMachine", "Science Machine")
                    .SetStats(null, null, 3)
                    .WithText($"<keyword={Extensions.PrefixGUID("building", this)}>")
                    .SetSprites("ScienceMachine.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[4]
                            {
                                SStack("Scrap", 2),
                                SStack("On Turn Reduce Counter For Allies In Row", 2),
                                SStack("When Destroyed By Hammer Gain Rock", 1),
                                SStack("When Destroyed By Hammer Gain Wood", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("scienceMachineBlueprint", "Science Machine Blueprint")
                    .SetSprites("Blueprint.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetTraits(TStack("Consume", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.playOnSlot = true;
                            data.canPlayOnEnemy = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[3]
                            {
                                SStack("Summon Science Machine", 1),
                                SStack("Require Rock", 1),
                                SStack("Require Wood", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Science Machine")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.scienceMachine>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate (StatusEffectSummon data)
                        {
                            CardType cardType = TryGet<CardType>("Clunker");
                            cardType.canRecall = false;
                            data.setCardType = cardType;
                            data.summonCard = TryGet<CardData>("scienceMachine");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Reduce Counter For Allies In Row")
                    .WithText("Reduce <keyword=counter> by <{a}> for an ally behind")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate (StatusEffectApplyXOnTurn data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AlliesInRow;
                        }
                    )
            );

            #endregion


            #region Testing Stuffs
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("sanityStick99", "Sanity Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Dummy.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Sanity", 99),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("sanityStick1", "Sanity Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Dummy.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetTraits(TStack("Zoomlin", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Sanity", 1),
                            };
                        }
                    )
            );

            #endregion
            preLoaded = true;
        }

        private void CreateBattleAssets()
        {
            battleAssets.Add(
                (
                    0,
                    new BattleDataEditor(this, "Spider Den", 0)
                        .SetSprite("Nodes/SpiderNode.png")
                        .SetNameRef("Spider Den")
                        .EnemyDictionary(
                            ('S', "spider"),
                            ('W', "spiderWarrior"),
                            ('N', "spiderNest"),
                            ('Q', "spiderQueen"),
                            ('R', "stone"),
                            ('T', "smallTree")
                        )
                        .StartWavePoolData(0, "Wave 1: Stone In")
                        .ConstructWaves(3, 0, "TSR", "RST")
                        .StartWavePoolData(1, "Wave 2: Tree In")
                        .ConstructWaves(3, 2, "STS", "SSN", "RSS")
                        .StartWavePoolData(2, "Wave 3: Spider In")
                        .ConstructWaves(3, 5, "SWT", "SWR", "TNS")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(4, 7, "NRST", "SNSR", "TSNS")
                        .StartWavePoolData(4, "Wave 5: Queen In")
                        .ConstructWaves(4, 9, "TNQ", "QNR", "QW")
                        .AddBattleToLoader()
                        .LoadBattle(
                            0,
                            resetAllOnClear: true,
                            "GameModeNormal",
                            BattleStack.Exclusivity.removeUnmodded
                        )
                )
            );
        }

        public override List<T> AddAssets<T, Y>()
        {
            if (assets.OfType<T>().Any())
                Debug.LogWarning(
                    $"[{Title}] adding {typeof(Y).Name}s: {assets.OfType<T>().Select(a => a._data.name).Join()}"
                );
            return assets.OfType<T>().ToList();
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

        public CardDataBuilder CardCopy(string oldName, string newName) =>
            DataCopy<CardData, CardDataBuilder>(oldName, newName);

        public ClassDataBuilder TribeCopy(string oldName, string newName) =>
            DataCopy<ClassData, ClassDataBuilder>(oldName, newName);

        public CardData.TraitStacks TStack(string name, int amount) =>
            new CardData.TraitStacks(TryGet<TraitData>(name), amount);

        public CardData.StatusEffectStacks SStack(string name, int amount) =>
            new CardData.StatusEffectStacks(TryGet<StatusEffectData>(name), amount);

        private StatusEffectDataBuilder StatusCopy(string oldName, string newName)
        {
            StatusEffectData data = TryGet<StatusEffectData>(oldName).InstantiateKeepName();
            data.name = GUID + "." + newName;
            data.targetConstraints = new TargetConstraint[0];
            StatusEffectDataBuilder builder = data.Edit<
                StatusEffectData,
                StatusEffectDataBuilder
            >();
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
                spriteAsset = HopeUtils.CreateSpriteAsset(
                    Title,
                    directoryWithPNGs: ImagePath("Icons")
                );
                CreateModAssets();
            }

            SpriteAsset.RegisterSpriteAsset();
            base.Load();
            CreateBattleAssets();

            foreach (var (num, battleDataEditor) in battleAssets)
            {
                battleDataEditor.ToggleBattle(true);
            }

            CreateLocalizedStrings();
            Events.OnEntityCreated += FixImage;

            GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            gameMode.classes = gameMode.classes.Append(TryGet<ClassData>("DST")).ToArray();
        }

        public override void Unload()
        {
            base.Unload();
            Events.OnEntityCreated -= FixImage;

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

        internal CardScript GiveUpgrade(string name = "Crown") //Give a crown
        {
            CardScriptGiveUpgrade script = ScriptableObject.CreateInstance<CardScriptGiveUpgrade>(); //This is the standard way of creating a ScriptableObject
            script.name = $"Give {name}"; //Name only appears in the Unity Inspector. It has no other relevance beyond that.
            script.upgradeData = TryGet<CardUpgradeData>(name);
            return script;
        }

        internal CardScript AddRandomHealth(int min, int max) //Boost health by a random amount
        {
            CardScriptAddRandomHealth health =
                ScriptableObject.CreateInstance<CardScriptAddRandomHealth>();
            health.name = "Random Health";
            health.healthRange = new Vector2Int(min, max);
            return health;
        }

        internal CardScript AddRandomDamage(int min, int max) //Boost damage by a ranom amount
        {
            CardScriptAddRandomDamage damage =
                ScriptableObject.CreateInstance<CardScriptAddRandomDamage>();
            damage.name = "Give Damage";
            damage.damageRange = new Vector2Int(min, max);
            return damage;
        }

        internal CardScript AddRandomCounter(int min, int max) //Increase counter by a random amount
        {
            CardScriptAddRandomCounter counter =
                ScriptableObject.CreateInstance<CardScriptAddRandomCounter>();
            counter.name = "Give Counter";
            counter.counterRange = new Vector2Int(min, max);
            return counter;
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
            StringTable uiText = LocalizationHelper.GetCollection(
                "UI Text",
                SystemLanguage.English
            );
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
