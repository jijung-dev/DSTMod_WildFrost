using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using HarmonyLib;
using UnityEngine;
using static CardData;

public abstract class BuildingBase : DataBase
{
    protected List<BuildingInstance> buildings = new List<BuildingInstance>();

    public enum ResourceRequire
    {
        Gold,
        Rock,
        Wood,
        Rabbit,
    }

    public class BuildingInstance
    {
        public string _name;
        public string _title;
        public string _spriteName;
        public int[] _stats;
        public (string name, int amount)[] _withEffects;
        public (string name, int amount)[] _withTraits;
        public (ResourceRequire name, int amount)[] _resourceRequired;
        public bool _isPool;

        public BuildingInstance(
            string name,
            string title,
            string spriteName,
            (string name, int amount)[] withEffects,
            (string name, int amount)[] withTraits,
            (ResourceRequire name, int amount)[] resourceRequired,
            int[] stats,
            bool isPool
        )
        {
            _name = name;
            _title = title;
            _spriteName = spriteName;
            _stats = stats;
            _withEffects = withEffects;
            _withTraits = withTraits;
            _resourceRequired = resourceRequired;
            _isPool = isPool;
        }
    }

    public BuildingInstance Create(
        string name,
        string title,
        string spriteName,
        (string name, int amount)[] withEffects,
        (string name, int amount)[] withTraits,
        (ResourceRequire name, int amount)[] resourceRequired,
        int[] stats,
        bool isPool = true
    )
    {
        return new BuildingInstance(name, title, spriteName, withEffects, withTraits, resourceRequired, stats, isPool);
    }

    public override void CreateCard()
    {
        foreach (BuildingInstance item in buildings)
        {
            assets.Add(
                new CardDataBuilder(mod)
                    .CreateUnit(item._name, item._title)
                    .SetHealth(item._stats[0] == 0 ? (int?)null : item._stats[0])
                    .SetDamage(item._stats[1] == 0 ? (int?)null : item._stats[1])
                    .SetCounter(item._stats[2])
                    .SetCardSprites(item._spriteName, "Wendy_BG.png")
                    .WithCardType("Clunker")
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.startWithEffects = GetBuildingStatusEffect(item);
                        data.traits = GetBuildingTrait(item);
                    })
            );
            if (!item._isPool)
            {
                assets.Add(
                    new CardDataBuilder(mod)
                        .CreateUnit(item._name + "norequired", item._title)
                        .SetHealth(item._stats[0] == 0 ? (int?)null : item._stats[0])
                        .SetDamage(item._stats[1] == 0 ? (int?)null : item._stats[1])
                        .SetCounter(item._stats[2])
                        .SetCardSprites(item._spriteName, "Wendy_BG.png")
                        .WithCardType("Clunker")
                        .WithValue(GetResourcePrice(item))
                        .SubscribeToAfterAllBuildEvent<CardData>(data =>
                        {
                            data.WithPools(DSTMod.Instance.unitWithoutResource);
                            data.startWithEffects = GetBuildingStatusEffectNoRequired(item);
                        })
                );
            }
            assets.Add(
                new CardDataBuilder(mod)
                    .CreateItem(item._name + "Blueprint", item._title + " Blueprint")
                    .WithText($"Place <card=dstmod.{item._name}><hiddencard=dstmod.floor>".Process())
                    .SetCardSprites("Blueprint.png", "Wendy_BG.png")
                    .WithCardType("Item")
                    .WithValue(GetResourcePrice(item))
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        Ext.blueprints.Add(data);
                        data.targetConstraints = new TargetConstraint[] { mod.TryGetConstraint("floorOnly") };
                        data.WithPools(DSTMod.Instance.itemWithResource);

                        data.traits = new List<CardData.TraitStacks>() { TStack("Blueprint", 1), TStack("Consume", 1), TStack("Zoomlin", 1) };
                        data.attackEffects = new CardData.StatusEffectStacks[]
                        {
                            SStack("Build " + item._title, 1),
                            SStack("Instant Summon Hammer In Hand", 1),
                            SStack("Reduce Chest Health", 1),
                        };
                        data.startWithEffects = GetRequireStatusEffect(item);
                    })
            );
            CreateStatusEffect(item);
        }
    }

    private void CreateStatusEffect(BuildingInstance item)
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Build " + item._title)
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.type = "dst.build";
                    data.killSelfWhenApplied = true;
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>(item._name);
                    data.animation = TryGet<StatusEffectNextPhase>("SoulboundBossPhase2").animation;
                })
        );
    }

    int GetResourcePrice(BuildingInstance item)
    {
        int price = 0;
        foreach (var resource in item._resourceRequired)
        {
            switch (resource.name)
            {
                case ResourceRequire.Wood:
                case ResourceRequire.Rock:
                    price += 30;
                    break;
                case ResourceRequire.Rabbit:
                case ResourceRequire.Gold:
                    price += 40;
                    break;
                default:
                    break;
            }
        }
        return price;
    }

    StatusEffectStacks[] GetBuildingStatusEffect(BuildingInstance item)
    {
        List<StatusEffectStacks> statusEffects = new List<StatusEffectStacks>();
        if (item._withEffects != null)
            statusEffects.AddRange(item._withEffects?.Select(e => mod.SStack(e.name, e.amount)));

        statusEffects.AddRange(
            item._resourceRequired.Where(e => e.name != ResourceRequire.Rabbit)
                .Select(e => mod.SStack("When Destroyed Gain " + e.name.ToString() + " To Chest", e.amount))
        );

        statusEffects.Add(SStack("Cannot Recall", 1));
        statusEffects.Add(SStack("Chest Health", 1));
        return statusEffects.ToArray();
    }

    StatusEffectStacks[] GetBuildingStatusEffectNoRequired(BuildingInstance item)
    {
        List<StatusEffectStacks> statusEffects = new List<StatusEffectStacks>();
        if (item._withEffects != null)
            statusEffects.AddRange(item._withEffects?.Select(e => mod.SStack(e.name, e.amount)));

        statusEffects.Add(SStack("Scrap", 5 - item._resourceRequired.Length));
        return statusEffects.ToArray();
    }

    List<TraitStacks> GetBuildingTrait(BuildingInstance item)
    {
        List<TraitStacks> statusEffects = new List<TraitStacks>();
        if (item._withTraits != null)
            statusEffects.AddRange(item._withTraits.Select(e => mod.TStack(e.name, e.amount)));
        statusEffects.AddRange(new[] { TStack("Building", 1), TStack("Super Unmovable", 1) });
        return statusEffects;
    }

    StatusEffectStacks[] GetRequireStatusEffect(BuildingInstance item)
    {
        List<StatusEffectStacks> statusEffects = new List<StatusEffectStacks>();
        statusEffects.AddRange(item._resourceRequired.Select(e => mod.SStack("Require " + e.name.ToString(), e.amount)));
        return statusEffects.ToArray();
    }
}
