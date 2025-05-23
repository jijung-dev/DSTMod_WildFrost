using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using static CardData;

public abstract class ConsumableBase : DataBase
{
    protected List<ConsumableInstance> consumables = new List<ConsumableInstance>();

    public enum ConsumeType
    {
        None,
        Consumable,
        Food,
        Cooked,
    }

    public class ConsumableInstance
    {
        public string _name;
        public string _title;
        public string _spriteName;
        public (string name, int amount)[] _withEffects;
        public string _cookedTitle;
        public string _cookedName;
        public ConsumeType _consumeType;

        public ConsumableInstance(
            string name,
            string title,
            string spriteName,
            (string name, int amount)[] withEffects,
            ConsumeType consumeType,
            string cookedTitle,
            string cookedName
        )
        {
            _name = name;
            _title = title;
            _spriteName = spriteName;
            _withEffects = withEffects;
            _cookedTitle = cookedTitle;
            _cookedName = cookedName;
            _consumeType = consumeType;
        }
    }

    public ConsumableInstance Create(
        string name,
        string title,
        string spriteName,
        (string name, int amount)[] withEffects,
        ConsumeType consumeType,
        string cookedTitle = "",
        string cookedName = ""
    )
    {
        return new ConsumableInstance(name, title, spriteName, withEffects, consumeType, cookedTitle, cookedName);
    }

    public override void CreateCard()
    {
        foreach (ConsumableInstance item in consumables)
        {
            assets.Add(
                new CardDataBuilder(mod)
                    .CreateItem(item._name, item._title)
                    .SetCardSprites(item._spriteName, "Wendy_BG.png")
                    .WithCardType("Item")
                    .FreeModify(
                        delegate(CardData data)
                        {
                            data.canPlayOnEnemy = false;
                        }
                    )
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        if (!string.IsNullOrEmpty(item._cookedTitle) && !string.IsNullOrEmpty(item._cookedName))
                        {
                            data.startWithEffects = new StatusEffectStacks[] { SStack("When Target Crock Pot Gain " + item._cookedTitle, 1) };
                            string card = "<hiddencard=dstmod." + item._cookedName + ">";

                            StringTable collection = LocalizationHelper.GetCollection("Cards", new LocaleIdentifier(SystemLanguage.English));
                            collection.SetString(data.name + "_text", card.Process());
                            data.textKey = collection.GetString(data.name + "_text");
                        }
                        data.attackEffects = item._withEffects.Select(e => mod.SStack(e.name, e.amount)).ToArray();
                        data.traits = new List<TraitStacks>() { GetConsumeTrait(item._consumeType) };
                    })
            );
            CreateStatusEffect(item);
            if (!string.IsNullOrEmpty(item._cookedTitle))
            {
                CreateCookFoodStatusEffect(item);
            }
        }
    }

    private void CreateStatusEffect(ConsumableInstance item)
    {
        assets.Add(
            StatusCopy("Summon Junk", "Summon " + item._title)
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>(item._name);
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummon>("Instant " + item._title + " In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.canSummonMultiple = true;
                    data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon " + item._title);
                })
        );
        if (item._consumeType != ConsumeType.Cooked)
        {
            var gainText = "Drop <{a}> <card=dstmod." + item._name + ">";
            assets.Add(
                new StatusEffectDataBuilder(mod)
                    .Create<StatusEffectApplyXWhenDestroyedUnNullable>("Gain " + item._title + " When Destroyed")
                    .WithText(gainText.Process())
                    .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                    {
                        data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("drop") };
                        data.effectToApply = TryGet<StatusEffectData>("Instant " + item._title + " In Hand");
                        data.targetMustBeAlive = false;
                        data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    })
            );
        }
    }

    private void CreateCookFoodStatusEffect(ConsumableInstance item)
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenTargetCertainCard>("When Target Crock Pot Gain " + item._cookedTitle)
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenTargetCertainCard>(data =>
                {
                    data.hasAnimation = true;
                    data.constraints = new TargetConstraintIsSpecificCard[]
                    {
                        new Scriptable<TargetConstraintIsSpecificCard>(r => r.allowedCards = new CardData[] { TryGet<CardData>("crockPot") }),
                    };
                    data.effectToApply = TryGet<StatusEffectData>("Instant " + item._cookedTitle + " In Hand");
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
    }

    TraitStacks GetConsumeTrait(ConsumeType _consumeType)
    {
        switch (_consumeType)
        {
            case ConsumeType.Food:
                return TStack("Food", 1);
            case ConsumeType.Consumable:
                return TStack("Consumable", 1);
            case ConsumeType.Cooked:
                return TStack("Cooked", 1);
            default:
                throw new System.Exception($"Not found ConsumeType: {_consumeType}");
        }
    }
}
