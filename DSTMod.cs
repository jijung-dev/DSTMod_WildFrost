using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using UnityEngine.XR;

namespace DSTMod_WildFrost
{
    public class DSTMod : WildfrostMod
    {
        public static DSTMod Instance;

        public static List<object> assets = new List<object>();
        private bool preLoaded = false;

        public DSTMod(string modDir)
            : base(modDir) { }

        public override string GUID => "tgestudio.wildfrost.dstmod";

        public override string[] Depends => new string[] { };

        public override string Title => "Don't Frostbite";

        public override string Description => "A mod that get idea from don't starve";

        private void CreateModAssets()
        {
            //Code for Status Effects

            //Code for Card
            #region Leader

            #region Wendy Card
            //Wendy Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("wendy", "Wendy")
                    .WithCardType("Leader")
                    .SetSprites("Wendy.png", "Wendy_BG.png")
                    .SetStats(6, 2, 2)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("When Deployed Summon Abigail", 1),
                            };
                            data.createScripts = new CardScript[] //These scripts run when right before Events.OnCardDataCreated
                            {
                                GiveUpgrade(), //By our definition, no argument will give a crown
                                AddRandomHealth(-1, 2),
                                AddRandomDamage(-1, 1),
                                AddRandomCounter(0, 1),
                            };
                        }
                    )
            );
            //Wendy Effect
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Abigail")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>("abigail");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Abigail")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectInstantSummon)data).targetSummon =
                                TryGet<StatusEffectData>("Summon Abigail") as StatusEffectSummon;
                        }
                    )
            );
            assets.Add(
                StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Abigail")
                    .WithText("When deployed, summon {0}")
                    .WithTextInsert("<card=tgestudio.wildfrost.dstmod.abigail>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXWhenDeployed)data).effectToApply =
                                TryGet<StatusEffectData>("Instant Summon Abigail");
                        }
                    )
            );
            #endregion

            #region Wortox Card
            //Wortox Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("wortox", "Wortox")
                    .WithCardType("Leader")
                    .SetSprites("Wortox.png", "Wendy_BG.png")
                    .SetStats(6, 2, 2)
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("When Enemy Is Killed Gain Card", 1),
                            };
                            data.createScripts = new CardScript[]
                            {
                                GiveUpgrade(),
                                AddRandomHealth(-1, 1),
                                AddRandomDamage(-1, 2),
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
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXWhenCardDestroyed)data).effectToApply =
                                TryGet<StatusEffectData>("Instant Summon Soul In Hand");
                        }
                    )
            );

            //Wortox Effect
            assets.Add(
                StatusCopy("Summon Junk", "Summon Soul")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>("soul");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Souls")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>("souls");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectInstantSummonRandomSoul>("Instant Summon Soul In Hand")
                    .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(
                        delegate(StatusEffectInstantSummon data)
                        {
                            data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                            ((StatusEffectInstantSummonRandomSoul)data).SoulCard =
                                TryGet<StatusEffectSummon>("Summon Soul");
                            ((StatusEffectInstantSummonRandomSoul)data).SoulsCard =
                                TryGet<StatusEffectSummon>("Summon Souls");
                        }
                    )
            );

            //Soul Effect
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Heal To Allies")
                    .WithText("Restore {0} to all allies")
                    .WithTextInsert("<{a}><keyword=health>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXOnCardPlayed)data).applyToFlags = StatusEffectApplyX
                                .ApplyToFlags
                                .Allies;
                            ((StatusEffectApplyXOnCardPlayed)data).effectToApply =
                                TryGet<StatusEffectData>("Heal");
                        }
                    )
            );

            //Soul Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateItem("souls", "Souls")
                    .SetSprites("Soul.png", "Wendy_BG.png")
                    .SetTraits(TStack("Zoomlin", 1), TStack("Consume", 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
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
                    .SetStats(4, 3, 0)
                    .WithCardType("Summoned")
                    .SetTraits(new CardData.TraitStacks(Get<TraitData>("Barrage"), 1))
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate (CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Trigger When Wendy Attacks", 1)
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
                        delegate(StatusEffectData data)
                        {
                            data.isReaction = true;
                            data.stackable = false;
                        }
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectTriggerWhenCertainAllyAttacks)data).ally =
                                TryGet<CardData>("wendy");
                        }
                    )
            );

            #endregion

            #region Chester Card
            //Chester Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("chester", "Chester")
                    .SetSprites("Chester.png", "Wendy_BG.png")
                    .SetStats(20, null, 0)
                    .WithCardType("Summoned")
                    .WithFlavour("A mobile chest")
            );
            //Chester Effect
            assets.Add(
                StatusCopy("Summon Plep", "Summon Chester")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.chester>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>("chester");
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
                        delegate(CardData data)
                        {
                            data.canPlayOnBoard = true;
                            data.canPlayOnFriendly = true;
                            data.canShoveToOtherRow = true;
                            data.needsTarget = true;
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
            //Chester Card
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
            //Chester Card
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
            //Gloomer Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("glommer", "Glommer")
                    .SetSprites("Glommer.png", "Wendy_BG.png")
                    .SetStats(8, null, 4)
                    .WithCardType("Friendly")
                    .SetStartWithEffect(SStack("On Turn Add Attack To Allies", 2))
            );
            #endregion

            #region Tallbird Card

            #region Tallbird Egg
            //TallbirdEgg Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("tallbirdEgg", "Tallbird Egg")
                    .SetSprites("TallBirdEgg.png", "Wendy_BG.png")
                    .SetStats(1, null, 5)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("On Turn Summon Smallbird", 1),
                                SStack("Destroy Self After Turn", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnTurn>("On Turn Summon Smallbird")
                    .WithText("Summon <card=tgestudio.wildfrost.dstmod.smallbird>")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXOnTurn)data).applyToFlags = StatusEffectApplyX
                                .ApplyToFlags
                                .Self;
                            ((StatusEffectApplyXOnTurn)data).effectToApply =
                                TryGet<StatusEffectData>("Instant Summon Smallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Smallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>("smallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Smallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectInstantSummon)data).targetSummon =
                                TryGet<StatusEffectData>("Summon Smallbird") as StatusEffectSummon;
                        }
                    )
            );
            #endregion

            #region BabyTallBird
            //BabyTallBirdCard
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("smallbird", "Smallbird")
                    .SetSprites("SmallBird.png", "Wendy_BG.png")
                    .WithText(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.smallishTallbird> then destroy self"
                    )
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
            //BabyTallBird Effect
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>("On Turn Summon Smallish Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXOnCounterTurn)data).applyToFlags =
                                StatusEffectApplyX.ApplyToFlags.Self;
                            ((StatusEffectApplyXOnCounterTurn)data).effectToApply =
                                TryGet<StatusEffectData>("Instant Summon Smallish Tallbird");
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
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>(
                                "smallishTallbird"
                            );
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Smallish Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectInstantSummon)data).targetSummon =
                                TryGet<StatusEffectData>("Summon Smallish Tallbird")
                                as StatusEffectSummon;
                        }
                    )
            );
            #endregion

            #region SmallishTallBird
            //SmallishTallBird Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("smallishTallbird", "Smallish Tallbird")
                    .WithText(
                        "When <keyword=counter> reaches 0 summon <card=tgestudio.wildfrost.dstmod.tallbird> then destroy self"
                    )
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
            //JuvenileTallBird Effect
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCounterTurn>("On Turn Summon Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXOnCounterTurn)data).applyToFlags =
                                StatusEffectApplyX.ApplyToFlags.Self;
                            ((StatusEffectApplyXOnCounterTurn)data).effectToApply =
                                TryGet<StatusEffectData>("Instant Summon Tallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Summon Fallow", "Summon Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>("tallbird");
                        }
                    )
            );
            assets.Add(
                StatusCopy("Instant Summon Fallow", "Instant Summon Tallbird")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectInstantSummon)data).targetSummon =
                                TryGet<StatusEffectData>("Summon Tallbird") as StatusEffectSummon;
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
                    .SetStats(2, null, 4)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
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
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXOnTurn)data).applyToFlags = StatusEffectApplyX
                                .ApplyToFlags
                                .Allies;
                            ((StatusEffectApplyXOnTurn)data).effectToApply =
                                TryGet<StatusEffectData>("Increase Effects");
                        }
                    )
            );
            #endregion

            #region RabbitMan Card
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("rabbitMan", "Rabbit Man")
                    .SetSprites("Dummy.png", "Wendy_BG.png")
                    .SetStats(8, 6, 3)
                    .WithCardType("Friendly")
                    .SubscribeToAfterAllBuildEvent<CardData>(
                        delegate(CardData data)
                        {
                            data.startWithEffects = new CardData.StatusEffectStacks[2]
                            {
                                SStack("After Hit Escape", 1),
                                SStack("After Hit Summon Rabbit Man Injured In Hand", 1),
                            };
                        }
                    )
            );
            assets.Add(
                new CardDataBuilder(this)
                    .CreateUnit("rabbitManInjured", "Rabbit Man Injured")
                    .SetSprites("Dummy.png", "Wendy_BG.png")
                    .SetStats(8, 6, 3)
                    .WithCardType("Friendly")
                    //.Traits(TStack("Injured", 1))
                    //.SubscribeToAfterAllBuildEvent<CardData>(
                    //    delegate (CardData data)
                    //    {
                    //        data.traits = new List<CardData.TraitStacks>()
                    //        {
                    //            TStack("Injured", 1)
                    //        };
                    //        //data.attackEffects = new CardData.StatusEffectStacks[1]
                    //        //{
                    //        //    SStack("Injury", 1),
                    //        //};
                    //    }
                    //)
            );
            assets.Add(
                StatusCopy("Summon Junk", "Summon Rabbit Man Injured")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectSummon)data).summonCard = TryGet<CardData>(
                                "rabbitManInjured"
                            );
                        }
                    )
            );
            assets.Add(
                StatusCopy(
                        "Instant Summon Junk In Hand",
                        "Instant Summon Rabbit Man Injured In Hand"
                    )
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectInstantSummon)data).targetSummon =
                                TryGet<StatusEffectSummon>("Summon Rabbit Man Injured");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenHit>("After Hit Escape")
                    .WithText("After hit escape from the battle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXWhenHit)data).applyToFlags = StatusEffectApplyX
                                .ApplyToFlags
                                .Self;
                            ((StatusEffectApplyXWhenHit)data).effectToApply =
                                TryGet<StatusEffectData>("Escape");
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXWhenHit>("After Hit Summon Rabbit Man Injured In Hand")
                    .WithText("Lmao After hit escape from the battle")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXWhenHit)data).applyToFlags = StatusEffectApplyX
                                .ApplyToFlags
                                .Self;
                            ((StatusEffectApplyXWhenHit)data).effectToApply =
                                TryGet<StatusEffectData>("Instant Summon Rabbit Man Injured In Hand");
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
                    .SetStats(null, 2, 0)
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
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate(StatusEffectData data)
                        {
                            ((StatusEffectApplyXOnCardPlayed)data).applyToFlags = StatusEffectApplyX
                                .ApplyToFlags
                                .Self;
                            ((StatusEffectApplyXOnCardPlayed)data).effectToApply =
                                TryGet<StatusEffectData>("Reduce Attack");
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
                                SStack("Snow", 1)
                            };
                            data.startWithEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("On Card Played Increase Snow", 1)
                            };
                        }
                    )
            );
            assets.Add(
                new StatusEffectDataBuilder(this)
                    .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Increase Snow")
                    .WithText("Increase <keyword=snow> by 1 when played")
                    .SubscribeToAfterAllBuildEvent<StatusEffectData>(
                        delegate (StatusEffectData data)
                        {
                            ((StatusEffectApplyXOnCardPlayed)data).applyToFlags = StatusEffectApplyX
                                .ApplyToFlags
                                .Self;
                            ((StatusEffectApplyXOnCardPlayed)data).effectToApply =
                                TryGet<StatusEffectData>("Increase Effects");
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
                                SStack("Reduce Counter", 3)
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
                                SStack("Shell", 2)
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
                            data.traits = new List<CardData.TraitStacks>()
                            {
                                TStack("Consume", 1)
                            };
                            data.attackEffects = new CardData.StatusEffectStacks[1]
                            {
                                SStack("Increase Max Health", 2)
                            };
                        }
                    )
            );
            #endregion

            #endregion

            #region Tribe
            //Add Tribe
            assets.Add(
                TribeCopy("Magic", "DST")
                    .WithFlag("Images/DSTFlag.png")
                    .WithSelectSfxEvent(
                        FMODUnity.RuntimeManager.PathToEventReference("event:/sfx/card/draw_multi")
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
                                    "tallbirdEgg",
                                    "spear",
                                    "spear",
                                    "spear",
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
                                    "TheBaker",
                                    "Pimento",
                                    "Pootie",
                                    "Tusk",
                                    "Ditto",
                                    "Flash",
                                    "TinyTyko"
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

            preLoaded = true;
        }

        public override List<T> AddAssets<T, Y>()
        {
            if (assets.OfType<T>().Any())
                Debug.LogWarning(
                    $"[{Title}] adding {typeof(Y).Name}s: {assets.OfType<T>().Select(a => a._data.name).Join()}"
                );
            return assets.OfType<T>().ToList();
        }

        private T[] DataList<T>(params string[] names)
            where T : DataFile => names.Select((s) => TryGet<T>(s)).ToArray();

        private T TryGet<T>(string name)
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

        private CardDataBuilder CardCopy(string oldName, string newName) =>
            DataCopy<CardData, CardDataBuilder>(oldName, newName);

        private ClassDataBuilder TribeCopy(string oldName, string newName) =>
            DataCopy<ClassData, ClassDataBuilder>(oldName, newName);

        public CardData.TraitStacks TStack(string name, int amount) =>
            new CardData.TraitStacks(TryGet<TraitData>(name), amount);

        private CardData.StatusEffectStacks SStack(string name, int amount) =>
            new CardData.StatusEffectStacks(TryGet<StatusEffectData>(name), amount);

        private T DataCopy<Y, T>(string oldName, string newName)
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
                CreateModAssets();
            } //The if statement is a flourish really. It makes the 2nd load of Load-Unload-Load faster.

            base.Load();
            CreateLocalizedStrings();
            Events.OnEntityCreated += FixImage;

            GameMode gameMode = TryGet<GameMode>("GameModeNormal"); //GameModeNormal is the standard game mode.
            gameMode.classes = gameMode.classes.Append(TryGet<ClassData>("DST")).ToArray();
        }

        public override void Unload()
        {
            base.Unload();
            Events.OnEntityCreated -= FixImage;

            GameMode gameMode = TryGet<GameMode>("GameModeNormal");
            gameMode.classes = RemoveNulls(gameMode.classes); //Without this, a non-restarted game would crash on tribe selection
            UnloadFromClasses();
        }

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

        #region Leader Stuffs

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

        #endregion

        #region Tribe Stuffs
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

        #region Patching Stuffs
        [HarmonyPatch(typeof(References), nameof(References.Classes), MethodType.Getter)]
        static class FixClassesGetter
        {
            static void Postfix(ref ClassData[] __result) =>
                __result = AddressableLoader.GetGroup<ClassData>("ClassData").ToArray();
        }

        [HarmonyPatch(typeof(TribeHutSequence), "SetupFlags")]
        class PatchTribeHut
        {
            static string TribeName = "DST";

            static void Postfix(TribeHutSequence __instance)
            {
                GameObject gameObject = GameObject.Instantiate(__instance.flags[0].gameObject);
                gameObject.transform.SetParent(
                    __instance.flags[0].gameObject.transform.parent,
                    false
                );
                TribeFlagDisplay flagDisplay = gameObject.GetComponent<TribeFlagDisplay>();
                ClassData tribe = DSTMod.Instance.TryGet<ClassData>(TribeName);
                flagDisplay.flagSprite = tribe.flag;
                __instance.flags = __instance.flags.Append(flagDisplay).ToArray();
                flagDisplay.SetAvailable();
                flagDisplay.SetUnlocked();

                TribeDisplaySequence sequence2 = GameObject.FindObjectOfType<TribeDisplaySequence>(
                    true
                );
                GameObject gameObject2 = GameObject.Instantiate(sequence2.displays[1].gameObject);
                gameObject2.transform.SetParent(
                    sequence2.displays[2].gameObject.transform.parent,
                    false
                );
                sequence2.tribeNames = sequence2.tribeNames.Append(TribeName).ToArray();
                sequence2.displays = sequence2.displays.Append(gameObject2).ToArray();

                Button button = flagDisplay.GetComponentInChildren<Button>();
                button.onClick.SetPersistentListenerState(
                    0,
                    UnityEngine.Events.UnityEventCallState.Off
                );
                button.onClick.AddListener(() =>
                {
                    sequence2.Run(TribeName);
                });

                //(SfxOneShot)
                gameObject2.GetComponent<SfxOneshot>().eventRef =
                    FMODUnity.RuntimeManager.PathToEventReference("event:/sfx/card/draw_multi");

                //0: Flag (ImageSprite)
                gameObject2
                    .transform.GetChild(0)
                    .GetComponent<ImageSprite>()
                    .SetSprite(tribe.flag);

                //1: Left (ImageSprite)
                Sprite wortox = DSTMod.Instance.TryGet<CardData>("wortox").mainSprite;
                gameObject2.transform.GetChild(1).GetComponent<ImageSprite>().SetSprite(wortox);

                //2: Right (ImageSprite)
                Sprite wendy = DSTMod.Instance.TryGet<CardData>("wendy").mainSprite;
                gameObject2.transform.GetChild(2).GetComponent<ImageSprite>().SetSprite(wendy);
                gameObject2.transform.GetChild(2).localScale *= 1.2f;

                //3: Textbox (Image)
                gameObject2.transform.GetChild(3).GetComponent<Image>().color = new Color(
                    0.12f,
                    0.47f,
                    0.57f
                );

                //3-0: Text (LocalizedString)
                StringTable collection = LocalizationHelper.GetCollection(
                    "UI Text",
                    SystemLanguage.English
                );
                gameObject2
                    .transform.GetChild(3)
                    .GetChild(0)
                    .GetComponent<LocalizeStringEvent>()
                    .StringReference = collection.GetString(DSTMod.Instance.TribeDescKey);

                //4:Title Ribbon (Image)
                //4-0: Text (LocalizedString)
                gameObject2
                    .transform.GetChild(4)
                    .GetChild(0)
                    .GetComponent<LocalizeStringEvent>()
                    .StringReference = collection.GetString(DSTMod.Instance.TribeTitleKey);
            }
        }
        #endregion
    }
    //Status Effect Class
    public class StatusEffectTriggerWhenCertainAllyAttacks : StatusEffectTriggerWhenAllyAttacks
    {
        //Cannot change allyInRow or againstTarget without some publicizing. Abigail is sad :(
        public bool useName = true;
        public CardData ally;

        public CardType allyCardType;

        public override bool RunHitEvent(Hit hit)
        {
            //Debug.Log(hit.attacker?.name);
            if (useName)
            {
                if (hit.attacker?.name == ally.name)
                {
                    return base.RunHitEvent(hit);
                }
            }
            else
            {
                if (hit.attacker?.data.cardType == allyCardType)
                {
                    return base.RunHitEvent(hit);
                }
            }

            return false;
        }
    }

    public class StatusEffectInstantSummonRandomSoul : StatusEffectInstantSummon
    {
        public StatusEffectSummon SoulCard;
        public StatusEffectSummon SoulsCard;

        public override IEnumerator Process()
        {
            targetSummon = GetRandomCard();
            return base.Process();
        }

        private StatusEffectSummon GetRandomCard()
        {
            int ranNum = UnityEngine.Random.Range(0, 2);
            if (ranNum == 0)
            {
                return SoulCard;
            }
            else
            {
                return SoulsCard;
            }
        }
    }

    public class StatusEffectApplyXOnCounterTurn : StatusEffectApplyX
    {
        [SerializeField]
        public bool trueOnTurn;

        public bool turnTaken;

        public override void Init()
        {
            base.OnCardPlayed += CheckCardPlay;
            base.OnTurn += CheckTurn;
        }

        public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
        {
            if (
                entity.counter.current <= 0
                && !turnTaken
                && target.enabled
                && entity == target
                && Battle.IsOnBoard(target)
            )
            {
                if (trueOnTurn)
                {
                    turnTaken = true;
                    return false;
                }
                return true;
            }

            return false;
        }

        public override bool RunPostApplyStatusEvent(StatusEffectApply apply)
        {
            return base.RunPostApplyStatusEvent(apply);
        }

        public IEnumerator CheckCardPlay(Entity entity, Entity[] targets)
        {
            return Run(GetTargets());
        }

        public override bool RunTurnEvent(Entity entity)
        {
            if (entity.counter.current <= 0 && trueOnTurn && turnTaken && entity == target)
            {
                return Battle.IsOnBoard(target);
            }
            return false;
        }

        public IEnumerator CheckTurn(Entity entity)
        {
            yield return Run(GetTargets());
            turnTaken = false;
        }
    }

    public class StatusEffectDestroySelfAfterCounterTurn : StatusEffectData
    {
        public bool cardPlayed;

        public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
        {
            if (!cardPlayed && entity.counter.current <= 0 && entity == target && !target.silenced)
            {
                ActionQueue.Add(new ActionKill(entity));
                cardPlayed = true;
            }

            return false;
        }
    }
}
