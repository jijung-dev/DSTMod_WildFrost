using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using static CardData;

public abstract class ConsumableBase : DataBase
{
    protected List<ConsumableInstance> consumables = new List<ConsumableInstance>();

    public enum ConsumeType
    {
        Consumable,
        Food,
    }

    public class ConsumableInstance
    {
        public string _name;
        public string _title;
        public string _spriteName;
        public (string name, int amount)[] _withEffects;
        public ConsumeType _consumeType;

        public ConsumableInstance(string name, string title, string spriteName, (string name, int amount)[] withEffects, ConsumeType consumeType)
        {
            _name = name;
            _title = title;
            _spriteName = spriteName;
            _withEffects = withEffects;
            _consumeType = consumeType;
        }
    }

    public ConsumableInstance Create(string name, string title, string spriteName, (string name, int amount)[] withEffects, ConsumeType consumeType)
    {
        return new ConsumableInstance(name, title, spriteName, withEffects, consumeType);
    }

    public override void CreateCard()
    {
        foreach (ConsumableInstance item in consumables)
        {
            assets.Add(
                new CardDataBuilder(mod)
                    .CreateItem(item._name, item._title)
                    .SetSprites(item._spriteName, "Wendy_BG.png")
                    .WithCardType("Item")
                    .FreeModify(
                        delegate(CardData data)
                        {
                            data.canPlayOnEnemy = false;
                            data.canPlayOnHand = false;
                        }
                    )
                    .SubscribeToAfterAllBuildEvent<CardData>(data =>
                    {
                        data.attackEffects = item._withEffects.Select(e => mod.SStack(e.name, e.amount)).ToArray();
                        data.traits = new List<TraitStacks>() { GetConsumeTrait(item._consumeType) };
                    })
            );
            CreateStatusEffect(item);
        }
    }

    private void CreateStatusEffect(ConsumableInstance item)
    {
        string effectTitle = "Gain " + item._title + " When Destroyed";
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectGainCardWhenDestroyed>(effectTitle)
                .SubscribeToAfterAllBuildEvent<StatusEffectGainCardWhenDestroyed>(data =>
                {
                    data.cardGain = TryGet<CardData>(item._name);
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
            default:
                return TStack("Consumable", 1);
        }
    }
}
