using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public abstract class DataBase
{
    protected List<object> assets = new List<object>();
    public static readonly List<Type> subclasses;

    protected DSTMod mod => DSTMod.Instance;

    protected T TryGet<T>(string name)
        where T : DataFile => mod.TryGet<T>(name);

    protected CardData.TraitStacks TStack(string name, int amount) => mod.TStack(name, amount);

    protected CardData.StatusEffectStacks SStack(string name, int amount) => mod.SStack(name, amount);

    protected (string name, int amount)[] SStack(params (string name, int amount)[] effects)
    {
        return effects;
    }

    protected StatusEffectDataBuilder StatusCopy(string oldName, string newName) => mod.StatusCopy(oldName, newName);

    protected TargetConstraint TryGetConstraint(string name) => mod.TryGetConstraint(name);

    public virtual void CreateCard() { }

    protected virtual void CreateStatusEffect() { }

    protected virtual void CreateTrait() { }

    protected virtual void CreateKeyword() { }

    protected virtual void CreateIcon() { }

    protected virtual void CreateOther() { }

    public List<object> Create()
    {
        CreateStatusEffect();
        CreateTrait();
        CreateKeyword();
        CreateIcon();
        CreateCard();
        CreateOther();
        return assets;
    }

    static DataBase()
    {
        subclasses = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DataBase))).ToList();
    }
}
