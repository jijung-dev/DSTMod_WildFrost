using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BattleEditor;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public abstract class DataBase
{
    protected List<object> assets = new List<object>();
    protected List<BattleDataEditor> battleAssets = new List<BattleDataEditor>();
    protected (CardData, CardScript[]) finalSwapAsset;

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

    protected virtual void CreateBattle() { }

    protected virtual void CreateFinalSwapAsset() { }

    public List<object> CreateAssest()
    {
        CreateStatusEffect();
        CreateTrait();
        CreateKeyword();
        CreateIcon();
        CreateCard();
        CreateOther();
        return assets;
    }

    public FinalBossCardModifier CreateFinalSwap()
    {
        CreateFinalSwapAsset();
        if (finalSwapAsset.Item1 == null)
            return null;

        FinalBossCardModifier cardModifier = new Scriptable<FinalBossCardModifier>(r =>
            r.card = finalSwapAsset.Item1 ?? throw new Exception("WHAT Where card")
        );
        var scripts = finalSwapAsset.Item2;
        cardModifier.runAll = scripts ?? throw new Exception("WHAT NO SCRIPT???");
        return cardModifier;
    }

    public List<BattleDataEditor> CreateBattleAsset()
    {
        CreateBattle();
        return battleAssets;
    }

    static DataBase()
    {
        subclasses = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(DataBase))).ToList();
    }
}
