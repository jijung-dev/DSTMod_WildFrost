using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void CreateModAssets()
        {
            //Code for Status Effects
            #region TargetConstrain
            var allyOnly = ScriptableObject.CreateInstance<TargetConstraintIsCardType>();

            var noChopable = ScriptableObject.CreateInstance<TargetConstraintNotHasTrait>();
            var noMineable = ScriptableObject.CreateInstance<TargetConstraintNotHasTrait>();
            var hammerOnly = ScriptableObject.CreateInstance<TargetConstraintHasTrait>();
            var pickaxeOnly = ScriptableObject.CreateInstance<TargetConstraintHasTrait>();
            var axeOnly = ScriptableObject.CreateInstance<TargetConstraintHasTrait>();
            var beeOnly = ScriptableObject.CreateInstance<TargetConstraintHasTrait>();

            var buildingOnly = ScriptableObject.CreateInstance<TargetConstraintHasStatus>();
            var clunkerOnly = ScriptableObject.CreateInstance<TargetConstraintHasStatus>();
            var noChestHealth = ScriptableObject.CreateInstance<TargetConstraintNotHasStatus>();
            var noBuilding = ScriptableObject.CreateInstance<TargetConstraintNotHasStatus>();

            var abigailOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var stoneOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var boulderOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var goldOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var smallTreeOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var treeOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var spikyTreeOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var beeQueenOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var dflyOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var toadstoolOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var sandCastleOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var fuelweaverOnly = ScriptableObject.CreateInstance<TargetConstraintIsSpecificCard>();
            var noDfly = ScriptableObject.CreateInstance<TargetConstraintIsNotSpecificCard>();
            var noToadstool = ScriptableObject.CreateInstance<TargetConstraintIsNotSpecificCard>();
            var noBoomshroom = ScriptableObject.CreateInstance<TargetConstraintIsNotSpecificCard>();

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectResistX>("TargetConstrainBuilder")
                    .SubscribeToAfterAllBuildEvent<StatusEffectResistX>(
                        delegate(StatusEffectResistX data)
                        {
                            allyOnly.allowedTypes = new CardType[] { TryGet<CardType>("Friendly"), TryGet<CardType>("Leader") };

                            noChopable.trait = TryGet<TraitData>("Chopable");
                            noMineable.trait = TryGet<TraitData>("Mineable");
                            hammerOnly.trait = TryGet<TraitData>("HammerType");
                            pickaxeOnly.trait = TryGet<TraitData>("PickaxeType");
                            axeOnly.trait = TryGet<TraitData>("AxeType");
                            beeOnly.trait = TryGet<TraitData>("Bee");

                            buildingOnly.status = TryGet<StatusEffectData>("Building Health");
                            clunkerOnly.status = TryGet<StatusEffectData>("Scrap");
                            noBuilding.status = TryGet<StatusEffectData>("Building Health");
                            noChestHealth.status = TryGet<StatusEffectData>("Chest Health");

                            abigailOnly.allowedCards = new CardData[] { TryGet<CardData>("abigail") };
                            stoneOnly.allowedCards = new CardData[] { TryGet<CardData>("stone") };
                            boulderOnly.allowedCards = new CardData[] { TryGet<CardData>("boulder") };
                            goldOnly.allowedCards = new CardData[] { TryGet<CardData>("goldOre") };
                            smallTreeOnly.allowedCards = new CardData[] { TryGet<CardData>("smallTree") };
                            treeOnly.allowedCards = new CardData[] { TryGet<CardData>("tree") };
                            spikyTreeOnly.allowedCards = new CardData[] { TryGet<CardData>("spikyTree") };
                            dflyOnly.allowedCards = new CardData[] { TryGet<CardData>("dragonfly"), TryGet<CardData>("dragonflyEnraged") };
                            toadstoolOnly.allowedCards = new CardData[] { TryGet<CardData>("toadstool"), TryGet<CardData>("toadstoolEnraged") };
                            sandCastleOnly.allowedCards = new CardData[] { TryGet<CardData>("sandCastle") };
                            noDfly.notAllowedCards = new CardData[] { TryGet<CardData>("dragonfly"), TryGet<CardData>("dragonflyEnraged") };
                            noToadstool.notAllowedCards = new CardData[] { TryGet<CardData>("toadstool"), TryGet<CardData>("toadstoolEnraged") };
                            noBoomshroom.notAllowedCards = new CardData[] { TryGet<CardData>("boomshroom") };
                            beeQueenOnly.allowedCards = new CardData[] { TryGet<CardData>("beeQueen"), TryGet<CardData>("beeQueenEnraged") };
                            fuelweaverOnly.allowedCards = new CardData[] { TryGet<CardData>("ancientFuelweaver") };
                        }
                    )
            );
            #endregion

            #region Leader

            #region Wendy

            #region Wendy Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("wendy", "Wendy")
                    .WithCardType("Leader")
                    .SetSprites("Wendy.png", "Wendy_BG.png")
                    .SetStats(8, 2, 3)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Mourning Glory", 1) };
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("When Deployed Summon Abigail", 1),
                                SStack("When Abigail Destroyed Gain Card", 1),
                                SStack("When Abigail Destroyed Mourning Glory", 1),
                                SStack("Summon Chest Before Battle", 1),
                                SStack("Summon Floor Before Battle", 1),
                            };
                            data.createScripts = new CardScript[]
                            {
                                GiveUpgrade(),
                                AddRandomHealth(0, 1),
                                //AddRandomDamage(0, 1),
                                // AddRandomCounter(0, 1),
                            };
                        }
                    )
            );
            #endregion

            #region WendyStatusEffect
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Abigail")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.abigail>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("abigail");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Abigail")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
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
                        delegate(StatusEffectApplyXWhenDeployed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Abigail");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenCardDestroyed>("When Abigail Destroyed Gain Card")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(
                        delegate(StatusEffectApplyXWhenCardDestroyed data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Abigail Flower In Hand");
                            data.constraints = new TargetConstraint[] { abigailOnly };
                        }
                    )
            );
            #endregion

            #region MourningGloryStatusEffect
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectWhileActiveX>("While Active Reduce Abigail Max Health")
                    .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(
                        delegate(StatusEffectWhileActiveX data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                            data.effectToApply = TryGet<StatusEffectData>("Reduce Max Health Safe");
                            data.applyConstraints = new TargetConstraint[] { abigailOnly };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Temporary Aimless", "Temporary Mourning Glory")
                    .WithCanBeBoosted(false)
                    .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(
                        delegate(StatusEffectTemporaryTrait data)
                        {
                            data.trait = TryGet<TraitData>("Mourning Glory");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenCardDestroyedWithLimit>("When Abigail Destroyed Mourning Glory")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyedWithLimit>(
                        delegate(StatusEffectApplyXWhenCardDestroyedWithLimit data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Temporary Mourning Glory");
                            data.constraints = new TargetConstraint[] { abigailOnly };

                            data.litmitCount = 4;
                            data.traitToLimit = TryGet<TraitData>("Mourning Glory");
                        }
                    )
            );

            #endregion

            #endregion

            #region Wortox Card
            //Wortox Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("wortox", "Wortox")
                    .WithCardType("Leader")
                    .SetSprites("Wortox.png", "Wendy_BG.png")
                    .SetStats(10, 3, 4)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("When Enemy Is Killed Gain Card", 1),
                                SStack("Summon Chest Before Battle", 1),
                                SStack("Summon Floor Before Battle", 1),
                            };
                            data.createScripts = new CardScript[]
                            {
                                GiveUpgrade(),
                                AddRandomHealth(0, 1),
                                AddRandomDamage(0, 1),
                                // AddRandomCounter(0, 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenCardDestroyed>("When Enemy Is Killed Gain Card")
                    .WithText(
                        "When an Enemy is killed, Add <card=tgestudio.wildfrost.dstmod.soul> or <card=tgestudio.wildfrost.dstmod.souls> to hand"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(
                        delegate(StatusEffectApplyXWhenCardDestroyed data)
                        {
                            data.canBeAlly = false;
                            data.stackable = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.constraints = new TargetConstraint[] { noChopable, noMineable };
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Soul In Hand");
                        }
                    )
            );

            //Wortox Effect
            assets.Add(
                StatusCopy("Summon Junk", "Summon Soul")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("soul");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Souls")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("souls");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandom>("Instant Summon Soul In Hand")
                    .SubscribeToAfterAllBuildEvent(
                        delegate(StatusEffectInstantSummonRandom data)
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
                        delegate(StatusEffectApplyXOnCardPlayed data)
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
                        delegate(CardData data)
                        {
                            data.needsTarget = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Card Played Heal To Allies", 1) };
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
                    .SetStats(5, 2, 0)
                    .WithCardType("Summoned")
                    .SetTraits(TStack("Barrage", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("Trigger When Wendy Attacks", 1) };
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
                        delegate(StatusEffectData data)
                        {
                            data.isReaction = true;
                            data.stackable = false;
                        }
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenCertainAllyAttacks>(
                        delegate(StatusEffectTriggerWhenCertainAllyAttacks data)
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
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("abigailFlower");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Abigail Flower In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
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
                        delegate(CardData data)
                        {
                            data.playOnSlot = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("Summon Abigail", 1) };
                        }
                    )
            );
            #endregion

            #region IceChester Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("iceChester", "Ice Chester")
                    .SetSprites("IceChester.png", "Wendy_BG.png")
                    .SetStats(12, null, 0)
                    .WithCardType("Friendly")
                    .SetStartWithEffect(SStack("When Hit Apply Snow To Attacker", 2))
            );
            #endregion

            #region ShadowChester Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("shadowChester", "Shadow Chester")
                    .SetSprites("ShadowChester.png", "Wendy_BG.png")
                    .SetStats(10, null, 0)
                    .WithCardType("Friendly")
                    .SetStartWithEffect(SStack("When Hit Apply Demonize To Attacker", 1))
            );
            #endregion

            #region Glommer Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("glommer", "Glommer")
                    .SetSprites("Glommer.png", "Wendy_BG.png")
                    .SetStats(3, null, 6)
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
                    .IsPet("", true)
                    .SetStats(1, null, 5)
                    .WithText($"<keyword={Extensions.PrefixGUID("growth", this)}>")
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
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
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("smallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Smallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectData>("Summon Smallbird") as StatusEffectSummon;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Turn Summon Smallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate(StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Smallbird");
                        }
                    )
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
                        delegate(CardData data)
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
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Turn Summon Smallish Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate(StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Smallish Tallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Smallish Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("smallishTallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Smallish Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Smallish Tallbird");
                        }
                    )
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
                        delegate(CardData data)
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
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("tallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Tallbird");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Turn Summon Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate(StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Tallbird");
                        }
                    )
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
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("MultiHit", 1) };
                        }
                    )
            );
            #endregion

            #endregion

            #region FriendlyFly Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("friendlyFly", "Friendly Fly")
                    .SetSprites("FriendFly.png", "Wendy_BG.png")
                    .SetStats(2, null, 6)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Turn Boost Random Ally Effect", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Boost Random Ally Effect")
                    .WithText("Boost random ally effect by <{a}>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.applyConstraints = new TargetConstraint[] { noBuilding, noChestHealth };
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAlly;
                            data.effectToApply = TryGet<StatusEffectData>("Increase Effects");
                        }
                    )
            );
            #endregion

            #region Bunnyman Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("bunnyman", "Bunnyman")
                    .WithText("When hit escape to hand and becomes <card=tgestudio.wildfrost.dstmod.bunnymanInjured>")
                    .SetSprites("Bunnyman.png", "Wendy_BG.png")
                    .SetStats(8, 4, 3)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
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
                    .SetStats(3, 1, 0)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Turn Increase Health And Damage", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Bunnyman Injured")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("bunnymanInjured");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Bunnyman Injured In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.stackable = false;
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Bunnyman Injured");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenHit>("After Hit Escape")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(
                        delegate(StatusEffectApplyXWhenHit data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Escape");
                        }
                    )
            );
            //FIXME: Multi hit spawn multi copy
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenHit>("After Hit Summon Bunnyman Injured In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(
                        delegate(StatusEffectApplyXWhenHit data)
                        {
                            data.stackable = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Bunnyman Injured In Hand");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Increase Health And Damage")
                    .WithText("Increase <keyword=attack> and <keyword=health> by <{a}>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Increase Attack & Health");
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
                    .SetStats(6, 4, 0)
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
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Turn Reduce Counter To Allies", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("On Turn Add Attack To Allies", "On Turn Reduce Counter To Allies")
                    .WithText("Reduce <keyword=counter> by <{a}> to all allies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
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
                    .SetTraits(TStack("Knockback", 1), TStack("Pigheaded", 1))
                    .SetStats(8, 3, 5)
                    .WithCardType("Friendly")
            );
            #endregion

            #region RabbitKing

            #region RabbitKingCard
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("fortuitousRabbit", "Fortuitous Rabbit")
                    .SetSprites("scareRabbit.png", "Wendy_BG.png")
                    .SetTraits(TStack("Fragile", 1))
                    .WithText($"<keyword={Extensions.PrefixGUID("neutral", this)}>")
                    .SetStats(1, null, 8)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
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
                new CardDataBuilder(this)
                    .CreateUnit("wrathfulRabbitKing", "Wrathful Rabbit King")
                    .SetSprites("evilRabbit.png", "Wendy_BG.png")
                    .SetTraits(TStack("Spark", 1))
                    .SetStats(5, 3, 3)
                    .WithCardType("Enemy")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("While Active Frenzy To AlliesInRow", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("benevolentRabbitKing", "Benevolent Rabbit King")
                    .SetSprites("friendlyRabbit.png", "Wendy_BG.png")
                    .SetTraits(TStack("Spark", 1))
                    .SetStats(5, 3, 3)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("While Active Frenzy To AlliesInRow", 1) };
                        }
                    )
            );
            #endregion

            #region RabbitKingStatusEffectData
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("Neutral On Turn Summon BRK On Destroyed Summon WRB")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(
                        delegate(StatusEffectApplyXWhenDestroyedWithCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.targetMustBeAlive = false;
                            data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Benevolent Rabbit King");
                            data.effectToApplyWhenNotOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Wrathful Rabbit King");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Benevolent Rabbit King")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.benevolentRabbitKing>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("benevolentRabbitKing");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Benevolent Rabbit King")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Benevolent Rabbit King");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Wrathful Rabbit King")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.wrathfulRabbitKing>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("wrathfulRabbitKing");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Wrathful Rabbit King")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Wrathful Rabbit King");
                            data.summonPosition = StatusEffectInstantSummon.Position.EnemyRow;
                        }
                    )
            );
            #endregion

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

            #region Chester Pet
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("chester", "Chester")
                    .SetSprites("Chester.png", "Wendy_BG.png")
                    .SetStats(15, null, 0)
                    .WithCardType("Summoned")
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("eyeBone", "Eye Bone")
                    .IsPet("", true)
                    .WithCardType("Item")
                    .SetSprites("EyeBone.png", "Wendy_BG.png")
                    .SetTraits(TStack("Consume", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.playOnSlot = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("Summon Chester", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Plep", "Summon Chester")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.chester>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("chester");
                        }
                    )
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
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Card Played Reduce Attack", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Reduce Attack")
                    .WithText("Reduce <keyword=attack> by <{a}> when played")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(
                        delegate(StatusEffectApplyXOnCardPlayed data)
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
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Snow", 1) };
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Card Played Increase Snow", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Increase Snow")
                    .WithText("Increase <keyword=snow> by 1 when played")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(
                        delegate(StatusEffectApplyXOnCardPlayed data)
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
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Reduce Counter", 2) };
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
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Shell", 5) };
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
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Increase Max Health", 2) };
                        }
                    )
            );
            #endregion

            #region Pickaxe Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("pickaxe", "Pickaxe")
                    .SetStats(null, 1, 0)
                    .SetSprites("Pickaxe.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("PickaxeType", 1) };
                        }
                    )
            );
            #endregion

            #region Axe Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("axe", "Axe")
                    .SetStats(null, 2, 0)
                    .SetSprites("Axe.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("AxeType", 1) };
                        }
                    )
            );
            #endregion

            #region Hammer Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("hammer", "Hammer")
                    .SetStats(null, null, 0)
                    .SetSprites("Hammer.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1), TStack("HammerType", 1) };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Building Health", 1) };
                            data.targetConstraints = new TargetConstraint[] { buildingOnly };
                        }
                    )
            );
            #endregion

            #region Trident Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("trident", "Strident Trident")
                    .SetStats(null, 2, 0)
                    .SetSprites("Trident.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetStartWithEffect(SStack("Hit All Enemies", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.needsTarget = false;
                        }
                    )
            );
            #endregion

            #region Torch Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("torch", "Torch")
                    .SetSprites("Torch.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetTraits(TStack("Noomlin", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Overheat", 1) };
                        }
                    )
            );
            #endregion

            #region Fire Staff Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("fireStaff", "Fire Staff")
                    .SetSprites("FireStaff.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetTraits(TStack("Consume", 1), TStack("Aimless", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 4) };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Overheat", 2) };
                        }
                    )
            );
            #endregion

            #region Watering Can Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("wateringCan", "Watering Can")
                    .SetSprites("WateringCan.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetTraits(TStack("Noomlin", 1), TStack("Aimless", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 3) };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 1) };
                        }
                    )
            );
            #endregion

            #region Sewing Kit Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("sewingKit", "Sewing Kit")
                    .SetSprites("SewingKit.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { clunkerOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Instant Add Scrap", 1) };
                        }
                    )
            );
            #endregion

            #region Pan Flute Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("panFlute", "Pan Flute")
                    .SetSprites("PanFlute.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Snow All Enemies", 1) };
                            data.needsTarget = false;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("Snow All Enemies")
                    .WithText("Apply <keyword=snow> by <{a}> to all enemies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(
                        delegate(StatusEffectApplyXOnCardPlayed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Snow");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                        }
                    )
            );
            #endregion

            #region Garland Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("garland", "Garland")
                    .SetSprites("Garland.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Sanity", 3) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantReduceCertainEffect>("Reduce Sanity")
                    .WithText("Reduce <keyword=tgestudio.wildfrost.dstmod.sanity> by <{a}>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(
                        delegate(StatusEffectInstantReduceCertainEffect data)
                        {
                            data.effectToReduce = TryGet<StatusEffectData>("Sanity");
                        }
                    )
            );
            #endregion

            #region Dark Sword Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("darkSword", "Dark Sword")
                    .SetStats(null, 7, 0)
                    .SetSprites("DarkSword.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.canPlayOnFriendly = false;
                            data.canPlayOnHand = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("When Played Apply Sanity To Allies In Row", 4) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("When Played Apply Sanity To Allies In Row")
                    .WithText("Apply <{a}> <keyword=tgestudio.wildfrost.dstmod.sanity> to allies in row")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(
                        delegate(StatusEffectApplyXOnCardPlayed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Sanity");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AlliesInRow;
                            data.targetMustBeAlive = false;
                        }
                    )
            );
            #endregion

            #endregion

            #region Enemy

            #region Resources

            #region Mineable
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("stone", "Stone")
                    .SetStats(null, null, 0)
                    .SetSprites("Stone.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceMineable", 1),
                                SStack("When Destroyed By Pickaxe Gain Rock", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("boulder", "Boulder")
                    .SetStats(null, null, 0)
                    .SetSprites("Boulder.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceMineable", 2),
                                SStack("When Destroyed By Pickaxe Gain Rock", 2),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("stalagmite", "Stalagmite")
                    .SetStats(null, null, 0)
                    .SetSprites("Stalagmite.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceMineable", 1),
                                SStack("When Destroyed By Pickaxe Gain Rock", 1),
                                SStack("When Destroyed By Pickaxe Gain Gold", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("tallStalagmite", "Tall Stalagmite")
                    .SetStats(null, null, 0)
                    .SetSprites("TallStalagmite.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceMineable", 2),
                                SStack("When Destroyed By Pickaxe Gain Rock", 2),
                                SStack("When Destroyed By Pickaxe Gain Gold", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("goldOre", "Gold Ore")
                    .SetStats(null, null, 0)
                    .SetSprites("GoldOre.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceMineable", 2),
                                SStack("When Destroyed By Pickaxe Gain Gold", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Mineable", 1) };
                        }
                    )
            );
            #endregion

            #region Chopable
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("smallTree", "Small Tree")
                    .SetStats(null, null, 0)
                    .SetSprites("SmallTree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("When Destroyed By Axe Gain Wood", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("blueMushtree", "Blue Mushtree")
                    .SetStats(null, null, 0)
                    .SetSprites("BlueMushtree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("When Destroyed By Axe Gain Wood", 1),
                                SStack("Gain Blue Cap When Destroyed", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("redMushtree", "Red Mushtree")
                    .SetStats(null, null, 0)
                    .SetSprites("RedMushtree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("When Destroyed By Axe Gain Wood", 1),
                                SStack("Gain Red Cap When Destroyed", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("greenMushtree", "Green Mushtree")
                    .SetStats(null, null, 0)
                    .SetSprites("GreenMushtree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("When Destroyed By Axe Gain Wood", 1),
                                SStack("Gain Green Cap When Destroyed", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("tree", "Tree")
                    .SetStats(null, null, 0)
                    .SetSprites("Tree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 3),
                                SStack("When Destroyed By Axe Gain Wood", 2),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("spikyTree", "Spiky Tree")
                    .SetStats(null, null, 0)
                    .SetSprites("SpikyTree.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("When Destroyed By Axe Gain Wood", 1),
                                SStack("Immune To Everything", 1),
                                SStack("Teeth", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            #endregion

            #region Consumable

            #region Cactus
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("cactus", "Cactus")
                    .SetStats(3, null, 0)
                    .SetSprites("Cactus.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Teeth", 1),
                                SStack("Gain Cactus Flesh When Destroyed", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("cactusFlesh", "Cactus Flesh")
                    .SetSprites("CactusFlesh.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 2), SStack("Reduce Sanity", 2) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Food", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Cactus Flesh")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.cactus>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("cactusFlesh");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Cactus Flesh In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Cactus Flesh");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Cactus Flesh When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.cactusFlesh>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Cactus Flesh In Hand");
                        }
                    )
            );
            #endregion

            #region Honey
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("honey", "Honey")
                    .SetSprites("Honey.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Honey")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.honey>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("honey");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Honey In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Honey");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Honey When Destroyed")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Honey In Hand");
                        }
                    )
            );
            #endregion

            #region Blue Cap
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("blueCap", "Blue Cap")
                    .SetSprites("BlueCap.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { allyOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 2), SStack("Sanity", 2) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Food", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Blue Cap")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.blueCap>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("blueCap");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Blue Cap In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Blue Cap");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Blue Cap When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.blueCap>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Blue Cap In Hand");
                        }
                    )
            );
            #endregion

            #region Red Cap
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("redCap", "Red Cap")
                    .SetSprites("RedCap.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { allyOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Food", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Red Cap")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.redCap>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("redCap");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Red Cap In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Red Cap");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Red Cap When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.redCap>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Red Cap In Hand");
                        }
                    )
            );
            #endregion

            #region Green Cap
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("greenCap", "Green Cap")
                    .SetSprites("GreenCap.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { allyOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Sanity", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Food", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Green Cap")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.greenCap>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("greenCap");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Green Cap In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Green Cap");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Green Cap When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.greenCap>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Green Cap In Hand");
                        }
                    )
            );
            #endregion

            #region Light Bulb
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("lightBulb", "Light Bulb")
                    .SetSprites("LightBulb.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { allyOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Light Bulb")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.lightBulb>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("lightBulb");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Light Bulb In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Light Bulb");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Light Bulb When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.lightBulb>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Light Bulb In Hand");
                        }
                    )
            );
            #endregion

            #region Foliage
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("foliage", "Foliage")
                    .SetSprites("Foliage.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { allyOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Foliage")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.foliage>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("foliage");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Foliage In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Foliage");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Foliage When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.foliage>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Foliage In Hand");
                        }
                    )
            );
            #endregion

            #region LesserGlowBerry
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("lesserGlowBerry", "Lesser Glow Berry")
                    .SetSprites("LesserGlowBerry.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { allyOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 2), SStack("Sanity", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Consumable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon LGlowBerry")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.lesserGlowBerry>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("lesserGlowBerry");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon LGlowBerry In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon LGlowBerry");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain LGlowBerry When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.lesserGlowBerry>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon LGlowBerry In Hand");
                        }
                    )
            );
            #endregion

            #region Berries

            #region BerryBush
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("berryBush", "Berry Bush")
                    .SetStats(2, null, 0)
                    .SetSprites("BerryBush.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Berries When Destroyed", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("berries", "Berries")
                    .SetSprites("Berries.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Food", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Berries")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("berries");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Berries In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Berries");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Berries When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.berries>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Berries In Hand");
                        }
                    )
            );
            #endregion

            #region Juicy Berry Bush
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("juicyBerryBush", "Juicy Berry Bush")
                    .SetStats(5, null, 0)
                    .SetSprites("JuicyBerryBush.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Juicy Berries When Destroyed", 3) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("juicyBerries", "Juicy Berries")
                    .SetSprites("JuicyBerries.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Food", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Juicy Berries")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("juicyBerries");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Juicy Berries In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Juicy Berries");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Juicy Berries When Destroyed")
                    .WithText("When destroyed gain <card=tgestudio.wildfrost.dstmod.juicyBerries>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Juicy Berries In Hand");
                        }
                    )
            );
            #endregion

            #endregion

            #region Meat

            #region Monster Meat
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("monsterMeat", "Monster Meat")
                    .SetSprites("MonsterMeat.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.targetConstraints = new TargetConstraint[] { allyOnly };
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 1), SStack("Sanity", 2) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Food", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Monster Meat")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.monsterMeat>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("monsterMeat");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Monster Meat In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Monster Meat");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("Gain Monster Meat When Destroyed")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Monster Meat In Hand");
                        }
                    )
            );
            #endregion

            #endregion
            
            #endregion

            #endregion

            #region Spider

            #region Spider Queen
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("spiderQueen", "Spider Queen")
                    .SetSprites("SpiderQueen.png", "Wendy_BG.png")
                    .SetStats(12, 2, 4)
                    .SetTraits(TStack("Smackback", 1))
                    .WithCardType("Miniboss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Counter Turn Summon Spiders With Nurse", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Spiders With Nurse")
                    .WithText(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.spiderWarrior> or <card=tgestudio.wildfrost.dstmod.spiderNurse>"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(
                        delegate(StatusEffectApplyXOnCounterTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Summon Spiders With Nurse");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandom>("Summon Spiders With Nurse")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(
                        delegate(StatusEffectInstantSummonRandom data)
                        {
                            data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
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
                    .SetStats(4, 1, 3)
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1), TStack("Aimless", 1) };
                        }
                    )
            );

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Spider")
                    .WithText($"Summon <card=tgestudio.wildfrost.dstmod.spider>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("spider");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Spider")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
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
                    .SetStats(6, 3, 3)
                    .WithCardType("Enemy")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1), TStack("Frontline", 1) };
                        }
                    )
            );

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Spider Warrior")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.spiderWarrior>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("spiderWarrior");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Spider Warrior")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
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
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Spider Nurse")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.spiderNurse>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("spiderNurse");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Spider Nurse")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
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
                    .SetStats(6, null, 4)
                    .SetTraits(TStack("Backline", 1), TStack("Unmovable", 1))
                    .WithCardType("Enemy")
                    .WithValue(4 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.canShoveToOtherRow = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Counter Turn Summon Spiders", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Spiders")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.spider> or <card=tgestudio.wildfrost.dstmod.spiderWarrior>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(
                        delegate(StatusEffectApplyXOnCounterTurn data)
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
                        delegate(StatusEffectInstantSummonRandom data)
                        {
                            data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
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

            #region Dogs

            #region Varg
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("varg", "Varg")
                    .SetSprites("Varg.png", "Wendy_BG.png")
                    .SetStats(14, 6, 5)
                    .SetTraits(TStack("Wild", 1))
                    .WithCardType("Miniboss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("MultiHit", 1),
                                SStack("On Counter Turn Summon Hounds", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Hounds")
                    .WithText(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.hound> or <card=tgestudio.wildfrost.dstmod.redHound> or <card=tgestudio.wildfrost.dstmod.blueHound>"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(
                        delegate(StatusEffectApplyXOnCounterTurn data)
                        {
                            //data.canApplyMultiple = true;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Summon Hounds");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandom>("Summon Hounds")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(
                        delegate(StatusEffectInstantSummonRandom data)
                        {
                            data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                            data.randomCards = new StatusEffectSummon[]
                            {
                                TryGet<StatusEffectSummon>("Summon Hound"),
                                TryGet<StatusEffectSummon>("Summon Red Hound"),
                                TryGet<StatusEffectSummon>("Summon Blue Hound"),
                            };
                        }
                    )
            );
            #endregion

            #region Hound Normal

            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("hound", "Hound")
                    .SetSprites("Hound.png", "Wendy_BG.png")
                    .SetStats(4, 1, 3)
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1), TStack("Wild", 1) };
                        }
                    )
            );

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Hound")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.hound>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("hound");
                        }
                    )
            );
            #endregion

            #region RedHound
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("redHound", "Red Hound")
                    .SetSprites("HoundFire.png", "Wendy_BG.png")
                    .SetStats(5, 1, 3)
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Destroyed Overheat All Enemies", 3) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1), TStack("Wild", 1) };
                        }
                    )
                    .WithCardType("Enemy")
                    .WithValue(4 * 50)
            );

            assets.Add(
                StatusCopy("Summon Fallow", "Summon Red Hound")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.redHound>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("redHound");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("When Destroyed Overheat All Enemies")
                    .WithText("When destroyed apply <{a}> <keyword=tgestudio.wildfrost.dstmod.overheat> to enemies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.effectToApply = TryGet<StatusEffectData>("Overheat");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                        }
                    )
            );
            #endregion

            #region Blue Hound
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("blueHound", "Blue Hound")
                    .SetSprites("HoundIce.png", "Wendy_BG.png")
                    .SetStats(5, 1, 3)
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Destroyed Freezing All Enemies", 3) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1), TStack("Wild", 1) };
                        }
                    )
                    .WithCardType("Enemy")
                    .WithValue(4 * 50)
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Blue Hound")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.blueHound>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("blueHound");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("When Destroyed Freezing All Enemies")
                    .WithText("When destroyed apply <{a}> <keyword=tgestudio.wildfrost.dstmod.freeze> to enemies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.effectToApply = TryGet<StatusEffectData>("Freezing");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                        }
                    )
            );
            #endregion

            #region Varglet
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("varglet", "Varglet")
                    .SetSprites("Varglet.png", "Wendy_BG.png")
                    .SetStats(6, 2, 5)
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Counter Turn Summon Hounds", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1), TStack("Wild", 1), TStack("Smackback", 1) };
                        }
                    )
                    .WithCardType("Enemy")
                    .WithValue(8 * 50)
            );
            #endregion

            #endregion

            #region DeerClops
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("deerclops", "DeerClops")
                    .SetSprites("Deerclops.png", "Wendy_BG.png")
                    .SetStats(20, 4, 4)
                    .SetTraits(TStack("Knockback", 1))
                    .WithCardType("Boss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 2), SStack("Sanity", 2) };
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("ResistSnow", 1) };
                        }
                    )
            );
            #endregion

            #region Antlion

            #region Antlion Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("antlion", "Antlion")
                    .SetSprites("Antlion.png", "Wendy_BG.png")
                    .SetStats(30, 6, 4)
                    .WithCardType("Miniboss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("When Deployed Summon Sand Castle", 2),
                                SStack("On Turn Kill All Sand Castle", 1),
                                SStack("After Turn Summon Sand Castle", 2),
                                SStack("ResistSnow", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Super Bombard", 1) };
                        }
                    )
            );
            #endregion

            #region SandCastle Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("sandCastle", "Sand Castle")
                    .SetSprites("SandCastle.png", "Wendy_BG.png")
                    .SetStats(null, null, 0)
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Chest Health", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Super Unmovable", 1) };
                        }
                    )
            );
            #endregion

            #region AntlionStatusEffect
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Sand Castle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            CardType cardType = TryGet<CardType>("Clunker");
                            cardType.canRecall = false;
                            data.setCardType = cardType;
                            data.summonCard = TryGet<CardData>("sandCastle");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonOnCertainSlot>("Instant Summon Sand Castle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonOnCertainSlot>(
                        delegate(StatusEffectInstantSummonOnCertainSlot data)
                        {
                            data.isRandom = true;
                            data.canSummonMultiple = true;
                            data.maxRandomRange = 7;
                            data.minRandomRange = 0;
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Sand Castle");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXPreTurn>("On Turn Kill All Sand Castle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXPreTurn>(
                        delegate(StatusEffectApplyXPreTurn data)
                        {
                            data.applyConstraints = new TargetConstraint[] { sandCastleOnly };
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                            data.effectToApply = TryGet<StatusEffectData>("Reduce Chest Health");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("After Turn Summon Sand Castle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Sand Castle");
                        }
                    )
            );

            assets.Add(
                StatusCopy("When Deployed Lose Zoomlin", "When Deployed Summon Sand Castle")
                    .WithText("When deployed, summon <{a}> {0} at enemy side")
                    .WithTextInsert("<card=tgestudio.wildfrost.dstmod.sandCastle>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(
                        delegate(StatusEffectApplyXWhenDeployed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Sand Castle");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Bombard 2", "Bombard 3")
                    .SubscribeToAfterAllBuildEvent<StatusEffectBombard>(
                        delegate(StatusEffectBombard data)
                        {
                            data.hitFriendlyChance = 0;
                            data.targetCountRange = new Vector2Int(4, 4);
                            data.delayAfter = 0.1f;
                        }
                    )
            );
            #endregion

            #region Buzzard Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("buzzard", "Buzzard")
                    .SetSprites("Buzzard.png", "Wendy_BG.png")
                    .SetTraits(TStack("Aimless", 1))
                    .SetStats(5, 2, 4)
                    .SetStartWithEffect(SStack("MultiHit", 1))
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
            );
            #endregion

            #endregion

            #region Bees

            #region BeeQueen Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("beeQueen", "Bee Queen")
                    .WithText("The Queens Of All <keyword=tgestudio.wildfrost.dstmod.bee>")
                    .SetSprites("BeeQueen.png", "Wendy_BG.png")
                    .SetStats(20, 4, 3)
                    .WithCardType("Miniboss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Queen Enraged", 1), SStack("ResistSnow", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("beeQueenEnraged", "Bee Queen Enraged")
                    .SetSprites("BeeQueenEnraged.png", "Wendy_BG.png")
                    .SetStats(15, 2, 5)
                    .WithCardType("Miniboss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("While Active Enraged All Bee", 1),
                                SStack("When Deployed Summon Grumble Bee", 2),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1) };
                        }
                    )
            );
            #endregion

            #region BeeQueenStatusEffect
            assets.Add(
                StatusCopy("FrenzyBossPhase2", "Queen Enraged")
                    .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(
                        delegate(StatusEffectNextPhase data)
                        {
                            data.preventDeath = true;
                            data.nextPhase = TryGet<CardData>("beeQueenEnraged");
                            //data.animation = TryGet<StatusEffectNextPhase>("FrenzyBossPhase2").animation;
                        }
                    )
            );
            #endregion

            #region EnragedBeeQueenStatusEffect
            assets.Add(
                StatusCopy("While Active Increase Attack To Allies (No Desc)", "While Active Enraged All Bee")
                    .WithText("While active <keyword=tgestudio.wildfrost.dstmod.enraged> all <keyword=tgestudio.wildfrost.dstmod.bee>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(
                        delegate(StatusEffectWhileActiveX data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                            data.applyConstraints = new TargetConstraint[] { beeOnly };
                            data.effectToApply = TryGet<StatusEffectData>("Temporary Enraged");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Temporary Aimless", "Temporary Enraged")
                    .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(
                        delegate(StatusEffectTemporaryTrait data)
                        {
                            data.trait = TryGet<TraitData>("Enraged");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummon>("Instant Summon Grumble Bee")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.canSummonMultiple = true;
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Grumble Bee");
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Deployed Lose Zoomlin", "When Deployed Summon Grumble Bee")
                    .WithText("When deployed, Summon {0}")
                    .WithTextInsert("<card=tgestudio.wildfrost.dstmod.grumbleBee>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(
                        delegate(StatusEffectApplyXWhenDeployed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Grumble Bee");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Grumble Bee")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.setCardType = TryGet<CardType>("Enemy");
                            data.summonCard = TryGet<CardData>("grumbleBee");
                        }
                    )
            );
            #endregion

            #region GuardBeeCard
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("grumbleBee", "Grumble Bee")
                    .SetSprites("GrumbleBee.png", "Wendy_BG.png")
                    .SetStats(8, 2, 3)
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Bee", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectTriggerWhenCertainAllyAttacks>("Trigger When Enraged Bee Queen Attacks")
                    .WithCanBeBoosted(false)
                    .FreeModify(
                        delegate(StatusEffectData data)
                        {
                            data.isReaction = true;
                            data.stackable = false;
                        }
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectTriggerWhenCertainAllyAttacks>(
                        delegate(StatusEffectTriggerWhenCertainAllyAttacks data)
                        {
                            data.allyInRow = false;
                            data.ally = TryGet<CardData>("beeQueenEnraged");
                        }
                    )
            );
            #endregion

            #region BeeNormal
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("bee", "Bee")
                    .SetSprites("Bee.png", "Wendy_BG.png")
                    .SetStats(4, 2, 2)
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Bee", 1) };
                        }
                    )
            );
            #endregion

            #region BeeKiller
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("killerBee", "Killer Bee")
                    .SetSprites("KillerBee.png", "Wendy_BG.png")
                    .SetStats(6, 2, 4)
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.traits = new List<CardData.TraitStacks>() { TStack("Bee", 1) };
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Bee Is Killed Trigger To Self", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenCardDestroyed>("When Bee Is Killed Trigger To Self")
                    .WithIsReaction(true)
                    .WithText("Trigger when an ally <keyword=tgestudio.wildfrost.dstmod.bee> is killed")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(
                        delegate(StatusEffectApplyXWhenCardDestroyed data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.constraints = new TargetConstraint[] { beeOnly };
                            data.effectToApply = TryGet<StatusEffectData>("Trigger");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Killer Bee")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Killer Bee");
                            data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Killer Bee")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.setCardType = TryGet<CardType>("Enemy");
                            data.summonCard = TryGet<CardData>("killerBee");
                        }
                    )
            );
            #endregion

            #region Beehive Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("beehive", "Beehive")
                    .SetSprites("Beehive.png", "Wendy_BG.png")
                    .SetStats(8, null, 0)
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Trigger When Ally Bee Is Hit", 1),
                                SStack("Summon Killer Bee PreTrigger", 1),
                                SStack("On Turn Apply Ink To Self", 4),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenUnitIsHit>("Trigger When Ally Bee Is Hit")
                    .WithIsReaction(true)
                    .WithText("After ally <keyword=tgestudio.wildfrost.dstmod.bee> is hit summon <card=tgestudio.wildfrost.dstmod.killerBee>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenUnitIsHit>(
                        delegate(StatusEffectApplyXWhenUnitIsHit data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Trigger");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.unitConstraints = new TargetConstraint[] { beeOnly };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXPreTrigger>("Summon Killer Bee PreTrigger")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXPreTrigger>(
                        delegate(StatusEffectApplyXPreTrigger data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Killer Bee");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Apply Ink To Self")
                    .WithIsReaction(true)
                    .WithText("then apply <{a}><keyword=null> to self")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Null");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                        }
                    )
            );

            #endregion

            #endregion

            #region DFly

            #region DFly Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("dragonfly", "Dragonfly")
                    .SetSprites("Dragonfly.png", "Wendy_BG.png")
                    .SetStats(25, 3, 4)
                    .WithCardType("Boss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Dragonfly Enraged", 1),
                                SStack("When Health Lost Summon Lavae", 1),
                                SStack("Immune To Overheat", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("dragonflyEnraged", "Dragonfly Enraged")
                    .SetSprites("DragonflyEnraged.png", "Wendy_BG.png")
                    .SetStats(20, 2, 4)
                    .WithCardType("Boss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Overheat", 2) };
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Hit All Units", 1),
                                SStack("When Overheat Applied To Self Gain Spice Instead", 2),
                                SStack("Immune To Damage From DFly", 1),
                                SStack("MultiHit", 1),
                            };
                        }
                    )
            );
            #endregion

            #region DFlyStatusEffect
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXEveryYHealthLost>("When Health Lost Summon Lavae")
                    .WithText("When every <5><keyword=health> is lost, summon <card=tgestudio.wildfrost.dstmod.lavae>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXEveryYHealthLost>(
                        delegate(StatusEffectApplyXEveryYHealthLost data)
                        {
                            data.healthLost = 5;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Lavae");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectImmuneToDamageFromCertainCard>("Immune To Damage From DFly")
                    .SubscribeToAfterAllBuildEvent<StatusEffectImmuneToDamageFromCertainCard>(
                        delegate(StatusEffectImmuneToDamageFromCertainCard data)
                        {
                            data.notAllowedCards = new TargetConstraint[] { noDfly };
                        }
                    )
            );
            assets.Add(
                StatusCopy("FrenzyBossPhase2", "Dragonfly Enraged")
                    .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(
                        delegate(StatusEffectNextPhase data)
                        {
                            data.preventDeath = true;
                            data.nextPhase = TryGet<CardData>("dragonflyEnraged");
                            //data.animation = TryGet<StatusEffectNextPhase>("FrenzyBossPhase2").animation;
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Shell Applied To Self Gain Spice Instead", "When Overheat Applied To Self Gain Spice Instead")
                    .WithText("When <keyword=tgestudio.wildfrost.dstmod.overheat>'d, gain 2*<{a}><keyword=spice> instead")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(
                        delegate(StatusEffectApplyXWhenYAppliedTo data)
                        {
                            data.whenAppliedTypes = new string[] { "dst.overheat" };
                            data.effectToApply = TryGet<StatusEffectData>("Spice");
                            data.adjustAmount = true;
                            data.multiplyAmount = 2;
                        }
                    )
            );
            #endregion

            #region Lavae Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("lavae", "Lavae")
                    .SetSprites("Lavae.png", "Wendy_BG.png")
                    .SetStats(5, 2, 3)
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("When Destroyed Apply Overheat To Enemies", 1),
                                SStack("When Destroyed Apply Spice To DFly", 2),
                                SStack("Immune To Summoned", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Destroyed Apply Ink To RandomEnemy", "When Destroyed Apply Overheat To Enemies")
                    .WithText("When destroyed, apply <{a}><keyword=tgestudio.wildfrost.dstmod.overheat> to enemies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                            data.effectToApply = TryGet<StatusEffectData>("Overheat");
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Destroyed Apply Ink To Allies", "When Destroyed Apply Spice To DFly")
                    .WithText("then apply <{a}><keyword=spice> to <card=tgestudio.wildfrost.dstmod.dragonfly>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.applyConstraints = new TargetConstraint[] { dflyOnly };
                            data.effectToApply = TryGet<StatusEffectData>("Spice");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Lavae")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Lavae");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Lavae")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.setCardType = TryGet<CardType>("Enemy");
                            data.summonCard = TryGet<CardData>("lavae");
                        }
                    )
            );
            #endregion

            #endregion

            #region Toadstool

            #region Toadstool Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("toadstool", "Toadstool")
                    .SetSprites("Toadstool.png", "Wendy_BG.png")
                    .SetStats(25, null, 5)
                    .WithCardType("Miniboss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Toadstool Enraged", 1),
                                SStack("On Turn Lose Fill Board With Boomshroom", 1),
                                SStack("On Turn Fill Board With Boomshroom", 1),
                                SStack("Immune To Shroom", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("toadstoolEnraged", "Toadstool Enraged")
                    .SetSprites("ToadstoolEnraged.png", "Wendy_BG.png")
                    .SetStats(15, 1, 5)
                    .WithCardType("Miniboss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Shroom", 2) };
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Hit All Units", 1),
                                SStack("When Shroom Applied To Self Heal Instead", 1),
                                SStack("When Deployed Fill Enemy Board With Sporecaps", 1),
                                SStack("Immune To Damage From Toadstool", 1),
                            };
                        }
                    )
            );

            #endregion

            #region ToadstoolStatusEffect
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectImmuneToDamageFromCertainCard>("Immune To Damage From Toadstool")
                    .SubscribeToAfterAllBuildEvent<StatusEffectImmuneToDamageFromCertainCard>(
                        delegate(StatusEffectImmuneToDamageFromCertainCard data)
                        {
                            data.notAllowedCards = new TargetConstraint[] { noToadstool };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantFillBoardExt>("Fill Board With Boomshroom")
                    .WithText("Fill board with <card=tgestudio.wildfrost.dstmod.boomshroom>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(
                        delegate(StatusEffectInstantFillBoardExt data)
                        {
                            data.eventPriority = -1;
                            data.withCards = new CardData[] { TryGet<CardData>("boomshroom") };
                            data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Full;
                        }
                    )
            );
            assets.Add(
                StatusCopy("Lose Scrap", "Lose Fill Board With Boomshroom")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantLoseX>(
                        delegate(StatusEffectInstantLoseX data)
                        {
                            data.statusToLose = TryGet<StatusEffectData>("Fill Board With Boomshroom");
                        }
                    )
            );
            assets.Add(
                StatusCopy("On Turn Apply Attack To Self", "On Turn Lose Fill Board With Boomshroom")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.doPing = false;
                            data.textKey = null;
                            data.eventPriority = 9999;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Lose Fill Board With Boomshroom");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantFillBoardExt>("Fill Enemy Board With Sporecaps")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(
                        delegate(StatusEffectInstantFillBoardExt data)
                        {
                            data.withCards = new CardData[]
                            {
                                TryGet<CardData>("sporecapAttack"),
                                TryGet<CardData>("sporecapCounter"),
                                TryGet<CardData>("sporecapSnow"),
                                TryGet<CardData>("sporecapIncreaseEffect"),
                            };
                            data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Enemy Board With Sporecaps")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(
                        delegate(StatusEffectApplyXWhenDeployed data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Fill Enemy Board With Sporecaps");
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Shell Applied To Self Gain Spice Instead", "When Shroom Applied To Self Heal Instead")
                    .WithText("When <keyword=shroom>'d, restore <{2*a}><2*{a}><keyword=health> instead")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(
                        delegate(StatusEffectApplyXWhenYAppliedTo data)
                        {
                            data.whenAppliedTypes = new string[] { "shroom" };
                            data.effectToApply = TryGet<StatusEffectData>("Heal");
                            data.adjustAmount = true;
                            data.multiplyAmount = 2;
                        }
                    )
            );
            assets.Add(
                StatusCopy("FrenzyBossPhase2", "Toadstool Enraged")
                    .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(
                        delegate(StatusEffectNextPhase data)
                        {
                            data.preventDeath = true;
                            data.nextPhase = TryGet<CardData>("toadstoolEnraged");
                            //data.animation = TryGet<StatusEffectNextPhase>("FrenzyBossPhase2").animation;
                        }
                    )
            );
            assets.Add(
                StatusCopy("On Turn Apply Attack To Self", "On Turn Fill Board With Boomshroom")
                    .WithText("Fill board with <card=tgestudio.wildfrost.dstmod.boomshroom>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.stackable = false;
                            data.effectToApply = TryGet<StatusEffectData>("Fill Board With Boomshroom");
                        }
                    )
            );
            #endregion

            #region Boomshroom Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("boomshroom", "Boomshroom")
                    .SetSprites("Boomshroom.png", "Wendy_BG.png")
                    .SetStats(1, null, 3)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            var type = TryGet<CardType>("Clunker");
                            type.canRecall = false;
                            data.cardType = type;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("When Destroyed Apply Shroom To Random Unit", 1),
                                SStack("Destroy Self After Counter Turn", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Kaboom", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Destroyed Apply Damage To Attacker", "When Destroyed Apply Shroom To Random Unit")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.stackable = false;
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomUnit;
                            data.applyConstraints = new TargetConstraint[]
                            {
                                noBuilding,
                                noChestHealth,
                                noChopable,
                                noToadstool,
                                noMineable,
                                noBoomshroom,
                            };
                            data.effectToApply = TryGet<StatusEffectData>("Shroom");
                        }
                    )
            );
            #endregion

            #region Sporecaps

            #region SporecapAttack
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("sporecapAttack", "Sporecap Attack")
                    .SetStats(null, null, 0)
                    .SetSprites("Sporecap.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("Immune To Everything", 1),
                                SStack("While Active Increase Attack To Toadstool", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("While Active Barrage To Allies", "While Active Increase Attack To Toadstool")
                    .WithText("While active, add <{a}><keyword=attack> to <card=tgestudio.wildfrost.dstmod.toadstoolEnraged>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(
                        delegate(StatusEffectWhileActiveX data)
                        {
                            data.applyConstraints = new TargetConstraint[] { toadstoolOnly };
                            data.effectToApply = TryGet<StatusEffectData>("Increase Attack");
                        }
                    )
            );
            #endregion

            #region SporecapCounter
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("sporecapCounter", "Sporecap Counter")
                    .SetStats(null, null, 0)
                    .SetSprites("Sporecap.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("Immune To Everything", 1),
                                SStack("While Active Reduce Max Counter To Toadstool", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("While Active Barrage To Allies", "While Active Reduce Max Counter To Toadstool")
                    .WithText("While active, reduce max <keyword=counter> by <{a}> to <card=tgestudio.wildfrost.dstmod.toadstoolEnraged>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(
                        delegate(StatusEffectWhileActiveX data)
                        {
                            data.applyConstraints = new TargetConstraint[] { toadstoolOnly };
                            data.effectToApply = TryGet<StatusEffectData>("Reduce Max Counter");
                        }
                    )
            );
            #endregion

            #region SporecapIncreaseEffect
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("sporecapIncreaseEffect", "Sporecap Effect")
                    .SetStats(null, null, 0)
                    .SetSprites("Sporecap.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("Immune To Everything", 1),
                                SStack("While Active Increase Effects To Toadstool", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("While Active Barrage To Allies", "While Active Increase Effects To Toadstool")
                    .WithText("While active, increase effect by <{a}> to <card=tgestudio.wildfrost.dstmod.toadstoolEnraged>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(
                        delegate(StatusEffectWhileActiveX data)
                        {
                            data.applyConstraints = new TargetConstraint[] { toadstoolOnly };
                            data.effectToApply = TryGet<StatusEffectData>("Increase Effects");
                        }
                    )
            );
            #endregion

            #region SporecapSnow
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("sporecapSnow", "Sporecap Snow")
                    .SetStats(null, null, 3)
                    .SetSprites("Sporecap.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = true;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("ResourceChopable", 2),
                                SStack("Immune To Everything", 1),
                                SStack("On Turn Apply Snow To RandomEnemy", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Chopable", 1) };
                        }
                    )
            );
            assets.Add(
                StatusCopy("On Turn Apply Demonize To RandomEnemy", "On Turn Apply Snow To RandomEnemy")
                    .WithText("Apply <{a}><keyword=snow> to a random enemy")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.applyConstraints = new TargetConstraint[] { noBuilding, noChestHealth };
                            data.effectToApply = TryGet<StatusEffectData>("Snow");
                        }
                    )
            );
            #endregion

            #endregion

            #endregion

            #region Caves

            #region CaveSpider Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("caveSpider", "Cave Spider")
                    .SetSprites("CaveSpider.png", "Wendy_BG.png")
                    .SetStats(8, 1, 3)
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 1) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Frontline", 1), TStack("Monster", 1) };
                        }
                    )
            );
            #endregion

            #region Batilisk Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("batilisk", "Batilisk")
                    .WithText("Take <2><keyword=health>")
                    .SetSprites("Batilisk.png", "Wendy_BG.png")
                    .SetStats(6, 2, 3)
                    .WithCardType("Enemy")
                    .WithValue(2 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Take Health", 2) };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Aimless", 1), TStack("Monster", 1) };
                        }
                    )
            );
            #endregion

            #endregion

            #region Fuelweaver

            #region Forest
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("forestFuelweaver", "Forest Fuelweaver")
                    .SetSprites("ForestFuelweaver.png", "Wendy_BG.png")
                    .SetStats(30, null, 5)
                    .WithCardType("Boss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("CaveFuelweaver", 1),
                                SStack("Pre Turn Fill Player Board With Floral", 1),
                                SStack("On Turn Apply Sanity To Everything", 4),
                                SStack("Immune To Sanity", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("FrenzyBossPhase2", "CaveFuelweaver")
                    .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(
                        delegate(StatusEffectNextPhase data)
                        {
                            data.preventDeath = true;
                            data.nextPhase = TryGet<CardData>("caveFuelweaver");
                            //data.animation = TryGet<StatusEffectNextPhase>("FrenzyBossPhase2").animation;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantFillBoardExt>("Fill Player Board With Floral")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(
                        delegate(StatusEffectInstantFillBoardExt data)
                        {
                            data.withCards = new CardData[]
                            {
                                TryGet<CardData>("fern"),
                                TryGet<CardData>("mysteriousPlant"),
                                TryGet<CardData>("lightFlower"),
                            };
                            data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Player;
                        }
                    )
            );
            assets.Add(
                StatusCopy("Pre Trigger Copy Effects Of RandomAlly", "Pre Turn Fill Player Board With Floral")
                    .WithText("Before attacking, fill player board with <Florals>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXPreTrigger>(
                        delegate(StatusEffectApplyXPreTrigger data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Fill Player Board With Floral");
                        }
                    )
            );
            assets.Add(
                StatusCopy("On Turn Apply Attack To Self", "On Turn Apply Sanity To Everything")
                    .WithText("Apply <{a}><keyword=tgestudio.wildfrost.dstmod.sanity> to everything")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                            data.effectToApply = TryGet<StatusEffectData>("Sanity");
                        }
                    )
            );
            #endregion

            #region Cave
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("caveFuelweaver", "Cave Fuelweaver")
                    .SetSprites("CaveFuelweaver.png", "Wendy_BG.png")
                    .SetStats(30, 3, 3)
                    .WithCardType("Boss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 3) };
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("AncientFuelweaver", 1),
                                SStack("Immune To Sanity", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("FrenzyBossPhase2", "AncientFuelweaver")
                    .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(
                        delegate(StatusEffectNextPhase data)
                        {
                            data.preventDeath = true;
                            data.nextPhase = TryGet<CardData>("ancientFuelweaver");
                            //data.animation = TryGet<StatusEffectNextPhase>("FrenzyBossPhase2").animation;
                        }
                    )
            );
            #endregion

            #region Ancient
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("ancientFuelweaver", "Ancient Fuelweaver")
                    .SetSprites("AncientFuelweaver.png", "Wendy_BG.png")
                    .SetStats(40, 6, 5)
                    .WithCardType("Boss")
                    .WithValue(13 * 50)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 5) };
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("On Turn Fill Board With Woven Shadow", 1),
                                SStack("Immune To Sanity", 1),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("On Turn Apply Attack To Self", "On Turn Fill Board With Woven Shadow")
                    .WithText("Fill board with <Woven Shadows>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.stackable = false;
                            data.effectToApply = TryGet<StatusEffectData>("Fill Board With Woven Shadow");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantFillBoardExt>("Fill Board With Woven Shadow")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(
                        delegate(StatusEffectInstantFillBoardExt data)
                        {
                            data.withCards = new CardData[]
                            {
                                TryGet<CardData>("skullWovenShadow"),
                                TryGet<CardData>("handWovenShadow"),
                                TryGet<CardData>("unseenHand"),
                            };
                            data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Full;
                        }
                    )
            );
            #endregion

            #region Plant Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("mysteriousPlant", "Mysterious Plant")
                    .SetSprites("MysteriousPlant.png", "Wendy_BG.png")
                    .WithCardType("Friendly")
                    .SetStats(1, null, 0)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain LGlowBerry When Destroyed", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("fern", "Fern")
                    .SetSprites("Fern.png", "Wendy_BG.png")
                    .WithCardType("Friendly")
                    .SetStats(1, null, 0)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Foliage When Destroyed", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("lightFlower", "Light Flower")
                    .SetSprites("LightFlower.png", "Wendy_BG.png")
                    .WithCardType("Friendly")
                    .SetStats(1, null, 0)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Light Bulb When Destroyed", 1) };
                        }
                    )
            );
            #endregion

            #region Woven Shadow
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("skullWovenShadow", "Skull Woven Shadow")
                    .SetSprites("SkullWovenShadow.png", "Wendy_BG.png")
                    .WithCardType("Enemy")
                    .SetStats(1, null, 4)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("On Counter Turn Heal Ancient Fuelweaver", 4),
                                SStack("Destroy Self After Counter Turn", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("handWovenShadow", "Hand Woven Shadow")
                    .SetSprites("HandWovenShadow.png", "Wendy_BG.png")
                    .WithCardType("Enemy")
                    .SetStats(1, null, 4)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("On Counter Turn Heal Ancient Fuelweaver", 4),
                                SStack("Destroy Self After Counter Turn", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Heal Ancient Fuelweaver")
                    .WithText("Restore <{a}><keyword=health> to <card=tgestudio.wildfrost.dstmod.ancientFuelweaver> then destroy self")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(
                        delegate(StatusEffectApplyXOnCounterTurn data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Heal");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                            data.applyConstraints = new TargetConstraint[] { fuelweaverOnly };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("unseenHand", "Unseen Hand")
                    .SetSprites("UnseenHand.png", "Wendy_BG.png")
                    .WithCardType("Enemy")
                    .SetStats(1, null, 0)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("While Active Apply Shadow Shield To Fuelweaver", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectResource>("Shadow Shield")
                    .SubscribeToAfterAllBuildEvent<StatusEffectResource>(
                        delegate(StatusEffectResource data)
                        {
                            data.allowedCards = new TargetConstraint[] { fuelweaverOnly };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Temporary Aimless", "Temporary Shadow Shield")
                    .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(
                        delegate(StatusEffectTemporaryTrait data)
                        {
                            data.trait = TryGet<TraitData>("Shadow Shield");
                        }
                    )
            );
            assets.Add(
                StatusCopy("While Active Increase Attack To Allies (No Desc)", "While Active Apply Shadow Shield To Fuelweaver")
                    .WithText(
                        "While active, apply <keyword=tgestudio.wildfrost.dstmod.shadowshield> to <card=tgestudio.wildfrost.dstmod.ancientFuelweaver>"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(
                        delegate(StatusEffectWhileActiveX data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Temporary Shadow Shield");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                            data.applyConstraints = new TargetConstraint[] { fuelweaverOnly };
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

            #region StatusType

            #region Sanity

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectSanity>("Sanity")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSanity>(
                        delegate(StatusEffectSanity data)
                        {
                            data.type = "dst.sanity";
                            data.summonRan = TryGet<StatusEffectData>("Instant Summon Random Shadow Creature");
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
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 2), SStack("Immune To Sanity", 1) };
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
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 3), SStack("Immune To Sanity", 1) };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandom>("Instant Summon Random Shadow Creature")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(
                        delegate(StatusEffectInstantSummonRandom data)
                        {
                            data.summonPosition = StatusEffectInstantSummon.Position.EnemyRow;
                            data.randomCards = new StatusEffectSummon[]
                            {
                                TryGet<StatusEffectSummon>("Summon Crawling Horror"),
                                TryGet<StatusEffectSummon>("Summon Terrorbeak"),
                            };
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Crawling Horror")
                    .SubscribeToAfterAllBuildEvent(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("crawlingHorror");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Terrorbeak")
                    .SubscribeToAfterAllBuildEvent(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("terrorbeak");
                        }
                    )
            );

            #endregion

            #region Resource

            #region ResourceStatusEffect

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectResource>("ResourceMineable")
                    .WithIsStatus(true)
                    .SubscribeToAfterAllBuildEvent<StatusEffectResource>(
                        delegate(StatusEffectResource data)
                        {
                            data.allowedCards = new TargetConstraint[] { pickaxeOnly };
                            data.preventDeath = true;
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
                        delegate(StatusEffectResource data)
                        {
                            data.allowedCards = new TargetConstraint[] { axeOnly };
                            data.preventDeath = true;
                            data.type = "dst.resource";
                        }
                    )
                    .Subscribe_WithStatusIcon("resource icon")
            );
            #endregion

            #region GainResourceWhenDestroyedByHammer

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Hammer Gain Rock")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(
                        delegate(StatusEffectApplyXToUnitWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                            data.cardConstrains = new TargetConstraint[] { hammerOnly };
                            data.effectToApply = TryGet<StatusEffectData>("Rock");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Hammer Gain Gold")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(
                        delegate(StatusEffectApplyXToUnitWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                            data.effectToApply = TryGet<StatusEffectData>("Gold");
                            data.cardConstrains = new TargetConstraint[] { hammerOnly };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Hammer Gain Wood")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(
                        delegate(StatusEffectApplyXToUnitWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                            data.effectToApply = TryGet<StatusEffectData>("Wood");
                            data.cardConstrains = new TargetConstraint[] { hammerOnly };
                        }
                    )
            );
            #endregion

            #region Floor Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("floor", "Building Floor")
                    .SetStats(null, null, 0)
                    .WithText("Place <keyword=tgestudio.wildfrost.dstmod.building> on this platform")
                    .SetSprites("Floor.png", "Wendy_BG.png")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            CardType cardType = TryGet<CardType>("Clunker");
                            cardType.canRecall = false;
                            data.cardType = cardType;
                            data.canShoveToOtherRow = false;
                            data.isEnemyClunker = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Chest Health", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1), TStack("Super Unmovable", 1) };
                        }
                    )
            );
            #endregion

            #region FloorStatusEffectSummon

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenDestroyed>("When Destroyed Summon Floor")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(
                        delegate(StatusEffectApplyXWhenDestroyed data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Floor");
                        }
                    )
            );

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXBeforeBattle>("Summon Floor Before Battle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXBeforeBattle>(
                        delegate(StatusEffectApplyXBeforeBattle data)
                        {
                            data.doPing = false;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Floor");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.checkBeforeSpawn = true;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantFillSlots>("Instant Summon Floor")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(
                        delegate(StatusEffectInstantFillSlots data)
                        {
                            //data.targetSummonNoAnimation = TryGet<StatusEffectSummonNoAnimation>("Summon Floor");
                            data.withCards = new CardData[] { TryGet<CardData>("floor") };
                            data.slotID = 3;
                        }
                    )
            );
            #endregion

            #region RequireResourceStatusEffect

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectCraft>("Require Rock")
                    .WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.rock>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(
                        delegate(StatusEffectCraft data)
                        {
                            data.requireType = PatchingScript.NoTargetTypeExt.RequireRock;
                            data.removeEffect = TryGet<StatusEffectData>("Rock");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectCraft>("Require Wood")
                    .WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.wood>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(
                        delegate(StatusEffectCraft data)
                        {
                            data.requireType = PatchingScript.NoTargetTypeExt.RequireWood;
                            data.removeEffect = TryGet<StatusEffectData>("Wood");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectCraft>("Require Gold")
                    .WithText("Require <{a}> <keyword=tgestudio.wildfrost.dstmod.gold>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectCraft>(
                        delegate(StatusEffectCraft data)
                        {
                            data.requireType = PatchingScript.NoTargetTypeExt.RequireGold;
                            data.removeEffect = TryGet<StatusEffectData>("Gold");
                        }
                    )
            );
            #endregion

            #region Resource StatusEffect
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectResistX>("Gold").Subscribe_WithStatusIcon("gold icon"));
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectResistX>("Rock").Subscribe_WithStatusIcon("rock icon"));
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectResistX>("Wood").Subscribe_WithStatusIcon("wood icon"));
            #endregion

            #region Chest Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("chest", "Chest")
                    .SetSprites("Chest.png", "Abigail_BG.png")
                    .SetStats(null, null, 0)
                    .WithText(
                        "Store <keyword=tgestudio.wildfrost.dstmod.rock> and <keyword=tgestudio.wildfrost.dstmod.wood> and <keyword=tgestudio.wildfrost.dstmod.gold>"
                    )
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            CardType cardType = TryGet<CardType>("Clunker");
                            cardType.canRecall = false;
                            data.cardType = cardType;
                            data.canShoveToOtherRow = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("Chest Health", 1),
                                SStack("Immune To Everything", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Backline", 1), TStack("Super Unmovable", 1) };
                        }
                    )
            );
            #endregion

            #region ChestStatusEffectSummon

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXBeforeBattle>("Summon Chest Before Battle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXBeforeBattle>(
                        delegate(StatusEffectApplyXBeforeBattle data)
                        {
                            data.doPing = false;
                            data.effectToApply = TryGet<StatusEffectData>("Instant Summon Chest");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                            data.checkBeforeSpawn = true;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantFillSlots>("Instant Summon Chest")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(
                        delegate(StatusEffectInstantFillSlots data)
                        {
                            //data.targetSummonNoAnimation = TryGet<StatusEffectSummonNoAnimation>("Summon Chest");
                            data.withCards = new CardData[] { TryGet<CardData>("chest") };
                            data.slotID = 7;
                        }
                    )
            );
            #endregion

            #region Chest Health
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantReduceCertainEffect>("Reduce Chest Health")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(
                        delegate(StatusEffectInstantReduceCertainEffect data)
                        {
                            data.effectToReduce = TryGet<StatusEffectData>("Chest Health");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectStealth>("Chest Health")
                    .SubscribeToAfterAllBuildEvent<StatusEffectStealth>(
                        delegate(StatusEffectStealth data)
                        {
                            data.preventDeath = true;
                        }
                    )
                    .Subscribe_WithStatusIcon("chest icon")
            );
            #endregion

            #region Building Health
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantReduceCertainEffect>("Reduce Building Health")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(
                        delegate(StatusEffectInstantReduceCertainEffect data)
                        {
                            data.effectToReduce = TryGet<StatusEffectData>("Building Health");
                            data.isHit = true;
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectResource>("Building Health")
                    .SubscribeToAfterAllBuildEvent<StatusEffectResource>(
                        delegate(StatusEffectResource data)
                        {
                            data.allowedCards = new TargetConstraint[] { hammerOnly };
                            data.preventDeath = true;
                            data.type = "dst.resource";
                        }
                    )
                    .Subscribe_WithStatusIcon("building icon")
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Hammer")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
                        {
                            data.summonCard = TryGet<CardData>("hammer");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Junk In Hand", "Instant Summon Hammer In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.targetSummon = TryGet<StatusEffectSummon>("Summon Hammer");
                        }
                    )
            );
            #endregion

            #region Gain Resource When Destroyed
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Pickaxe Gain Rock")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(
                        delegate(StatusEffectApplyXToUnitWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                            data.effectToApply = TryGet<StatusEffectData>("Rock");
                            data.cardConstrains = new TargetConstraint[] { pickaxeOnly };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Pickaxe Gain Gold")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(
                        delegate(StatusEffectApplyXToUnitWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                            data.effectToApply = TryGet<StatusEffectData>("Gold");
                            data.cardConstrains = new TargetConstraint[] { pickaxeOnly };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>("When Destroyed By Axe Gain Wood")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXToUnitWhenDestroyedByCertainCards>(
                        delegate(StatusEffectApplyXToUnitWhenDestroyedByCertainCards data)
                        {
                            data.targetMustBeAlive = false;
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;

                            data.effectToApply = TryGet<StatusEffectData>("Wood");
                            data.cardConstrains = new TargetConstraint[] { axeOnly };
                        }
                    )
            );
            #endregion

            #endregion

            #region Freeze

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectFreeze>("Freezing")
                    .SubscribeToAfterAllBuildEvent<StatusEffectFreeze>(
                        delegate(StatusEffectFreeze data)
                        {
                            data.tempTrait = TryGet<StatusEffectData>("Temporary Froze");
                            data.frozeEffect = TryGet<StatusEffectData>("Temporary Froze");
                            data.freezeEffect = TryGet<StatusEffectData>("Freezing");

                            data.heatEffect = TryGet<StatusEffectData>("Overheat");
                        }
                    )
                    .Subscribe_WithStatusIcon("freeze icon")
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectTemporaryTrait>("Temporary Froze")
                    .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(
                        delegate(StatusEffectTemporaryTrait data)
                        {
                            data.trait = TryGet<TraitData>("Froze");
                            data.affectedBySnow = false;
                            data.stackable = false;
                            data.type = "dst.froze";
                        }
                    )
            );
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectFroze>("Froze").Subscribe_WithStatusIcon("froze icon"));
            #endregion

            #region Overheat

            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectHeat>("Overheat")
                    .SubscribeToAfterAllBuildEvent<StatusEffectHeat>(
                        delegate(StatusEffectHeat data)
                        {
                            data.type = "dst.overheat";
                            data.overheatingEffect = TryGet<StatusEffectData>("Lose Half Health");
                            data.freezeEffect = TryGet<StatusEffectData>("Freezing");
                            data.frozeEffect = TryGet<StatusEffectData>("Temporary Froze");
                        }
                    )
                    .Subscribe_WithStatusIcon("overheat icon")
            );
            #endregion

            #endregion

            #region Science Machine
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("scienceMachine", "Science Machine")
                    .SetStats(null, null, 3)
                    .SetSprites("ScienceMachine.png", "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.isEnemyClunker = false;
                            data.startWithEffects = new CardData.StatusEffectStacks[]
                            {
                                SStack("On Turn Reduce Counter For Allies", 2),
                                SStack("When Destroyed By Hammer Gain Rock", 1),
                                SStack("When Destroyed By Hammer Gain Wood", 1),
                            };
                            data.traits = new List<CardData.TraitStacks>() { TStack("Building", 1), TStack("Super Unmovable", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("scienceMachineBlueprint", "Science Machine Blueprint")
                    .WithText("Place <card=tgestudio.wildfrost.dstmod.scienceMachine>")
                    .SetSprites("Blueprint.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
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
                        }
                    )
            );
            assets.Add(
                StatusCopy("FrenzyBossPhase2", "Build Science Machine")
                    .SubscribeToAfterAllBuildEvent<StatusEffectNextPhase>(
                        delegate(StatusEffectNextPhase data)
                        {
                            data.nextPhase = TryGet<CardData>("scienceMachine");
                            //data.animation = TryGet<StatusEffectNextPhase>("FrenzyBossPhase2").animation;
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Science Machine")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.scienceMachine>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(
                        delegate(StatusEffectSummon data)
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
                    .Create<StatusEffectApplyXOnTurn>("On Turn Reduce Counter For Allies")
                    .WithText("Reduce <keyword=counter> by <{a}> for allies")
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(
                        delegate(StatusEffectApplyXOnTurn data)
                        {
                            data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
                            data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
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
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Sanity", 99) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("freezingStick99", "Freezing Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Freezing", 99) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("freezingStick1", "Freezing Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Freezing", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("sanityStick1", "Sanity Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .SetTraits(TStack("Zoomlin", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Sanity", 1) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("overheatingStick99", "Overheat Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Overheat", 99) };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("overheatingStick1", "Overheat Stick")
                    .SetStats(null, null, 0)
                    .SetSprites("Stick.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1))
                    .WithCardType("Item")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Overheat", 1) };
                        }
                    )
            );
            #endregion

            #region Traits
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Monster")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("monster");
                            data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Gain Monster Meat When Destroyed") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Consumable")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("consumable");
                            data.effects = new StatusEffectData[]
                            {
                                TryGet<StatusEffectData>("Destroy After Use"),
                                TryGet<StatusEffectData>("Free Action"),
                            };
                            data.overrides = new TraitData[] { TryGet<TraitData>("Food") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Food")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("food");
                            data.effects = new StatusEffectData[]
                            {
                                TryGet<StatusEffectData>("Destroy After Use"),
                                TryGet<StatusEffectData>("Free Action"),
                            };
                            data.overrides = new TraitData[] { TryGet<TraitData>("Food") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Shadow Shield")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("shadowshield");
                            data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Shadow Shield") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("PickaxeType")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("pickaxetype");
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("AxeType")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("axetype");
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("HammerType")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("hammertype");
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Chopable")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("chopable");
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Mineable")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("mineable");
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Froze")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("froze");
                            data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Froze") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Building")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = TryGet<KeywordData>("building");
                            data.effects = new StatusEffectData[]
                            {
                                TryGet<StatusEffectData>("Low Priority Position"),
                                TryGet<StatusEffectData>("When Destroyed Summon Floor"),
                                TryGet<StatusEffectData>("Building Health"),
                                TryGet<StatusEffectData>("Immune To Everything"),
                                TryGet<StatusEffectData>("StealthSafe"),
                            };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Blueprint")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("blueprint");
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Mourning Glory")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("mourningglory");
                            data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("While Active Reduce Abigail Max Health") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Super Bombard")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("superbombard");
                            data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Bombard 3") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Super Unmovable")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("superunmovable");
                            data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Unshovable"), TryGet<StatusEffectData>("Unmovable") };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Kaboom")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("kaboom");
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Bee")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("bee");
                            data.effects = new StatusEffectData[]
                            {
                                TryGet<StatusEffectData>("Immune To Summoned"),
                                TryGet<StatusEffectData>("Gain Honey When Destroyed"),
                            };
                        }
                    )
            );
            assets.Add(
                new TraitDataBuilder(this)
                    .Create("Enraged")
                    .SubscribeToAfterAllBuildEvent<TraitData>(
                        delegate(TraitData data)
                        {
                            data.keyword = Get<KeywordData>("enraged");
                            data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Trigger When Enraged Bee Queen Attacks") };
                        }
                    )
            );
            #endregion

            #region Keywords
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("monster")
                    .WithTitle("Monster")
                    .WithShowName(true)
                    .WithDescription("When destroyed gain <card=tgestudio.wildfrost.dstmod.monsterMeat>")
                    .WithTitleColour(new Color(0.57f, 0.40f, 1.00f))
                    .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("food")
                    .WithTitle("Food")
                    .WithShowName(true)
                    .WithDescription("Consume, Free Action|Can be cooked")
                    .WithTitleColour(new Color(1f, 0.57f, 0.21f))
                    .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("consumable")
                    .WithTitle("Consumable")
                    .WithShowName(true)
                    .WithDescription("Consume, Free Action|Cannot be cooked")
                    .WithTitleColour(new Color(1f, 0.57f, 0.21f))
                    .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("mourningglory")
                    .WithTitle("Mourning Glory")
                    .WithShowName(true)
                    .WithDescription(
                        "When <card=tgestudio.wildfrost.dstmod.abigail> got destroyed, gain <card=tgestudio.wildfrost.dstmod.abigailFlower> and gain 1 <Mourning Glory>,"
                            + " each stack reduce <card=tgestudio.wildfrost.dstmod.abigail> max <keyword=health>|Max 5 stacks"
                    )
                    .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                    .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(true)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("growth")
                    .WithTitle("Growth")
                    .WithShowName(true)
                    .WithDescription("When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.smallbird> then destroy self")
                    .WithTitleColour(new Color(0.1f, 0f, 0.2f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("growth1")
                    .WithTitle("Growth I")
                    .WithShowName(true)
                    .WithDescription("When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.smallishTallbird> then destroy self")
                    .WithTitleColour(new Color(0.1f, 0f, 0.2f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("growth2")
                    .WithTitle("Growth II")
                    .WithShowName(true)
                    .WithDescription("When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.tallbird> then destroy self")
                    .WithTitleColour(new Color(0.1f, 0f, 0.2f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("neutral")
                    .WithTitle("Neutral")
                    .WithShowName(true)
                    .WithDescription(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.benevolentRabbitKing>"
                            + ", if got destroyed before then summon <card=tgestudio.wildfrost.dstmod.wrathfulRabbitKing> on the enemy side"
                    )
                    .WithTitleColour(new Color(0.96f, 0.7f, 0.1f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("sanity")
                    .WithTitle("Sanity")
                    .WithShowName(false)
                    .WithDescription("When more than or equal to health summon <Shadow Creature> at the enemy side")
                    .WithTitleColour(new Color(0.34f, 0f, 0.63f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(true)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("resource")
                    .WithTitle("Resource")
                    .WithShowName(false)
                    .WithDescription("Can only be damaged by <Pickaxes> or <Axes> cards")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("mineable")
                    .WithTitle("Mineable")
                    .WithShowName(true)
                    .WithDescription("Can only be damaged by <Pickaxes> cards")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("chopable")
                    .WithTitle("Chopable")
                    .WithShowName(true)
                    .WithDescription("Can only be damaged by <Axes> cards")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("pickaxetype")
                    .WithTitle("Pickaxes")
                    .WithShowName(true)
                    .WithDescription($"Can damage <keyword=tgestudio.wildfrost.dstmod.mineable>")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("axetype")
                    .WithTitle("Axes")
                    .WithShowName(true)
                    .WithDescription("Can damage <keyword=tgestudio.wildfrost.dstmod.chopable>")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("hammertype")
                    .WithTitle("Hammers")
                    .WithShowName(true)
                    .WithDescription("Can use to destroy <keyword=tgestudio.wildfrost.dstmod.building> to gain back resources used")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("blueprint")
                    .WithTitle("Blueprint")
                    .WithShowName(true)
                    .WithDescription("When played gain <card=tgestudio.wildfrost.dstmod.hammer>")
                    .WithTitleColour(new Color(0.0627f, 0.0941f, 0.4706f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("building")
                    .WithTitle("Buildings")
                    .WithShowName(true)
                    .WithDescription(
                        "Can be place on <card=tgestudio.wildfrost.dstmod.floor>, only destroyable by <Hammers>|Can't be recalled, backline, stealth, immune to everything"
                    )
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithNoteColour(new Color(0.88f, 0.33f, 0.96f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("chest")
                    .WithTitle("Chest")
                    .WithShowName(true)
                    .WithDescription("Can't be recalled, stealth, immune to everything i guess?")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithNoteColour(new Color(0.88f, 0.33f, 0.96f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("floor")
                    .WithTitle("Floor")
                    .WithShowName(true)
                    .WithDescription(
                        "Place <keyword=tgestudio.wildfrost.dstmod.building> here|Can't be recalled, stealth, immune to everything i guess?"
                    )
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithNoteColour(new Color(0.88f, 0.33f, 0.96f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("gold")
                    .WithTitle("Gold")
                    .WithShowName(false)
                    .WithDescription("Use to summon <keyword=tgestudio.wildfrost.dstmod.building>")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );

            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("rock")
                    .WithTitle("Rock")
                    .WithShowName(false)
                    .WithDescription("Use to summon <keyword=tgestudio.wildfrost.dstmod.building>")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );

            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("wood")
                    .WithTitle("Wood")
                    .WithShowName(false)
                    .WithDescription("Use to summon <keyword=tgestudio.wildfrost.dstmod.building>")
                    .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("overheat")
                    .WithTitle("Overheat")
                    .WithShowName(false)
                    .WithDescription(
                        "Reduce max <keyword=health> by half when more than or equal to max <keyword=health>, Can be remove by <keyword=tgestudio.wildfrost.dstmod.freeze> or <keyword=tgestudio.wildfrost.dstmod.froze>"
                    )
                    .WithTitleColour(new Color(0.94f, 0.58f, 0.24f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("freeze")
                    .WithTitle("Freeze")
                    .WithShowName(false)
                    .WithDescription(
                        "<keyword=tgestudio.wildfrost.dstmod.froze> when more than or equal to max <keyword=health>, Can be remove by <keyword=tgestudio.wildfrost.dstmod.overheat>"
                    )
                    .WithTitleColour(new Color(0.60f, 0.81f, 0.98f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("froze")
                    .WithTitle("Froze")
                    .WithShowName(true)
                    .WithDescription("Can't attack. Can be remove by <keyword=tgestudio.wildfrost.dstmod.overheat>")
                    .WithTitleColour(new Color(0.60f, 0.81f, 0.98f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("superunmovable")
                    .WithTitle("Super Unmovable")
                    .WithShowName(true)
                    .WithDescription("Cannot move, like... at all")
                    .WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("superbombard")
                    .WithTitle("Super Bombard")
                    .WithShowName(true)
                    .WithDescription("Hits all targets in <sprite=target> areas then resummon all <card=tgestudio.wildfrost.dstmod.sandCastle>")
                    .WithTitleColour(new Color(1.00f, 0.79f, 0.34f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("bee")
                    .WithTitle("Bee")
                    .WithShowName(true)
                    .WithDescription("When destroyed gain <card=tgestudio.wildfrost.dstmod.honey>")
                    .WithTitleColour(new Color(1.00f, 0.6353f, 0f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("kaboom")
                    .WithTitle("Kaboom")
                    .WithShowName(true)
                    .WithDescription("When <keyword=counter> reaches <0> destroy self, When destroyed apply <1><keyword=shroom> to an random unit")
                    .WithTitleColour(new Color(0.63f, 0.95f, 0.55f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("enraged")
                    .WithTitle("Enraged")
                    .WithShowName(true)
                    .WithDescription("Trigger when <card=tgestudio.wildfrost.dstmod.beeQueenEnraged> attacks")
                    .WithTitleColour(new Color(1.00f, 0.4f, 0f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            assets.Add(
                new KeywordDataBuilder(this)
                    .Create("shadowshield")
                    .WithTitle("Shadow Shield")
                    .WithShowName(true)
                    .WithDescription("Immune to 100% all damage")
                    .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                    .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                    .WithBodyColour(new Color(1f, 1f, 1f))
                    .WithCanStack(false)
            );
            #endregion

            #region Icons
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "sanity icon", statusType: "dst.sanity", ImagePath("Icons/Sanity.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0f, 0f, 0f, 1f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                    .WithTextboxSprite()
                    .WithEffectDamageVFX(ImagePath("Icons/Heat_Apply.gif"))
                    .WithEffectDamageSFX(ImagePath("Sanity_Apply.wav"), 0.1f)
                    .WithApplyVFX(ImagePath("Icons/Sanity_Apply.gif"))
                    .WithApplySFX(ImagePath("Sanity_Attack.wav"), 0.1f)
                    //.WithApplySFX(ImagePath("Sanity_Attack.wav"), 0.1f)
                    .WithKeywords(iconKeywordOrNull: "sanity")
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "resource icon", statusType: "dst.resource", ImagePath("Icons/Resource.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0f, 0f, 0f, 1f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                    .WithTextboxSprite()
                    .WithWhenHitSFX(ImagePath("Rock_Apply.wav"))
                    .WithKeywords(iconKeywordOrNull: "resource")
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "gold icon", statusType: "dst.gold", ImagePath("Icons/Gold.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0f, 0f, 0f, 1f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                    .WithTextboxSprite()
                    .WithWhenHitSFX(ImagePath("Rock_Apply.wav"))
                    .WithKeywords(iconKeywordOrNull: "gold")
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "rock icon", statusType: "dst.rock", ImagePath("Icons/Rock.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0f, 0f, 0f, 1f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                    .WithTextboxSprite()
                    .WithWhenHitSFX(ImagePath("Rock_Apply.wav"))
                    .WithKeywords(iconKeywordOrNull: "rock")
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "wood icon", statusType: "dst.wood", ImagePath("Icons/Wood.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0f, 0f, 0f, 1f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                    .WithTextboxSprite()
                    .WithWhenHitSFX(ImagePath("Rock_Apply.wav"))
                    .WithKeywords(iconKeywordOrNull: "wood")
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "chest icon", statusType: "dst.chest", ImagePath("Icons/Chest.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.counter)
                    .WithTextboxSprite()
                    .WithKeywords(iconKeywordOrNull: "chest")
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "building icon", statusType: "dst.building", ImagePath("Icons/Building.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextboxSprite()
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "freeze icon", statusType: "dst.freeze", ImagePath("Icons/Freezing.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0f, 0f, 0f, 1f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                    .WithTextboxSprite()
                    //TODO: Freeze VFX
                    .WithApplyVFX(ImagePath("Icons/Freeze_Apply.gif"))
                    .WithApplySFX(ImagePath("Freeze_Apply.wav"))
                    .WithKeywords(iconKeywordOrNull: "freeze")
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "froze icon", statusType: "dst.froze", ImagePath("Icons/Freeze.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.crown)
                    //TODO: Froze VFX
                    .WithApplySFX(ImagePath("Froze_Apply.wav"))
            );
            assets.Add(
                new StatusIconBuilder(this)
                    .Create(name: "overheat icon", statusType: "dst.overheat", ImagePath("Icons/Heat.png"))
                    .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                    .WithTextColour(new Color(0f, 0f, 0f, 1f))
                    .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                    .WithTextboxSprite()
                    .WithApplyVFX(ImagePath("Icons/Heat_Apply.gif"))
                    .WithApplySFX(ImagePath("Heat_Apply.wav"))
                    .WithEffectDamageSFX(ImagePath("Heat_Apply.wav"))
                    .WithKeywords(iconKeywordOrNull: "overheat")
            );
            #endregion

            #region General StatusEffect
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectInstantReduceMaxHealthSafe>("Reduce Max Health Safe"));
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectUnshovable>("Unshovable"));
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectDestroySelfAfterCounterTurn>("Destroy Self After Counter Turn"));
            assets.Add(new StatusEffectDataBuilder(this).Create<StatusEffectStealth>("StealthSafe"));
            assets.Add(
                StatusCopy("Hit All Enemies", "Hit All Units")
                    .WithText("Hit All Units On Board")
                    .SubscribeToAfterAllBuildEvent<StatusEffectChangeTargetMode>(
                        delegate(StatusEffectChangeTargetMode data)
                        {
                            data.targetMode = ScriptableObject.CreateInstance<TargetModeAllUnit>();
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectImmune>("Immune To Summoned")
                    .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(
                        delegate(StatusEffectImmune data)
                        {
                            data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Temporary Summoned") };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectImmune>("Immune To Overheat")
                    .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(
                        delegate(StatusEffectImmune data)
                        {
                            data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Overheat") };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectImmune>("Immune To Shroom")
                    .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(
                        delegate(StatusEffectImmune data)
                        {
                            data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Shroom") };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectImmune>("Immune To Sanity")
                    .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(
                        delegate(StatusEffectImmune data)
                        {
                            data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Sanity") };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectImmune>("Immune To Everything")
                    .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(
                        delegate(StatusEffectImmune data)
                        {
                            data.immuneTo = new StatusEffectData[]
                            {
                                TryGet<StatusEffectData>("Overheat"),
                                TryGet<StatusEffectData>("Freezing"),
                                TryGet<StatusEffectData>("Froze"),
                                TryGet<StatusEffectData>("Sanity"),
                                TryGet<StatusEffectData>("Weakness"),
                                TryGet<StatusEffectData>("Snow"),
                                TryGet<StatusEffectData>("Pull"),
                                TryGet<StatusEffectData>("Overload"),
                                TryGet<StatusEffectData>("Shroom"),
                                TryGet<StatusEffectData>("Shell"),
                                TryGet<StatusEffectData>("Scrap"),
                                TryGet<StatusEffectData>("Demonize"),
                                TryGet<StatusEffectData>("Frost"),
                                TryGet<StatusEffectData>("Lumin"),
                                TryGet<StatusEffectData>("Spice"),
                                TryGet<StatusEffectData>("MultiHit"),
                                TryGet<StatusEffectData>("Teeth"),
                                TryGet<StatusEffectData>("Haze"),
                                TryGet<StatusEffectData>("Block"),
                                TryGet<StatusEffectData>("Null"),
                                TryGet<StatusEffectData>("Temporary Summoned"),
                            };
                        }
                    )
            );
            #endregion

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
                        .ConstructWaves(3, 2, "STS", "SSN", "SRS")
                        .StartWavePoolData(3, "Wave 4: Nest In")
                        .ConstructWaves(4, 7, "RTSN", "SRSN", "STSN")
                        .StartWavePoolData(4, "Wave 5: Queen In")
                        .ConstructWaves(4, 9, "TSNQ", "RSQN", "WSQ")
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
                        .SetSprite("Nodes/FuelweaverNode.png")
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
            yield break;
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

        public ClassDataBuilder TribeCopy(string oldName, string newName) => DataCopy<ClassData, ClassDataBuilder>(oldName, newName);

        public CardData.TraitStacks TStack(string name, int amount) => new CardData.TraitStacks(TryGet<TraitData>(name), amount);

        public CardData.StatusEffectStacks SStack(string name, int amount) => new CardData.StatusEffectStacks(TryGet<StatusEffectData>(name), amount);

        private StatusEffectDataBuilder StatusCopy(string oldName, string newName)
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
            Events.OnCampaignInit += CampaignInit;

            GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            gameMode.classes = gameMode.classes.Append(TryGet<ClassData>("DST")).ToArray();
        }

        public override void Unload()
        {
            base.Unload();
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

        internal CardScript GiveUpgrade(string name = "Crown") //Give a crown
        {
            CardScriptGiveUpgrade script = ScriptableObject.CreateInstance<CardScriptGiveUpgrade>(); //This is the standard way of creating a ScriptableObject
            script.name = $"Give {name}"; //Name only appears in the Unity Inspector. It has no other relevance beyond that.
            script.upgradeData = TryGet<CardUpgradeData>(name);
            return script;
        }

        internal CardScript AddRandomHealth(int min, int max) //Boost health by a random amount
        {
            CardScriptAddRandomHealth health = ScriptableObject.CreateInstance<CardScriptAddRandomHealth>();
            health.name = "Random Health";
            health.healthRange = new Vector2Int(min, max);
            return health;
        }

        internal CardScript AddRandomDamage(int min, int max) //Boost damage by a ranom amount
        {
            CardScriptAddRandomDamage damage = ScriptableObject.CreateInstance<CardScriptAddRandomDamage>();
            damage.name = "Give Damage";
            damage.damageRange = new Vector2Int(min, max);
            return damage;
        }

        internal CardScript AddRandomCounter(int min, int max) //Increase counter by a random amount
        {
            CardScriptAddRandomCounter counter = ScriptableObject.CreateInstance<CardScriptAddRandomCounter>();
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
