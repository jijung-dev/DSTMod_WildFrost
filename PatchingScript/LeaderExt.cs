using DSTMod_WildFrost;
using UnityEngine;

public class LeaderExt
{
    public static CardScript GiveUpgrade(string name = "Crown")
    {
        CardScriptGiveUpgrade script = ScriptableObject.CreateInstance<CardScriptGiveUpgrade>();
        script.name = $"Give {name}";
        script.upgradeData = DSTMod.Instance.TryGet<CardUpgradeData>(name);
        return script;
    }

    public static CardScript AddRandomHealth(int min, int max)
    {
        CardScriptAddRandomHealth health = ScriptableObject.CreateInstance<CardScriptAddRandomHealth>();
        health.name = "Random Health";
        health.healthRange = new Vector2Int(min, max);
        return health;
    }

    public static CardScript AddRandomDamage(int min, int max)
    {
        CardScriptAddRandomDamage damage = ScriptableObject.CreateInstance<CardScriptAddRandomDamage>();
        damage.name = "Give Damage";
        damage.damageRange = new Vector2Int(min, max);
        return damage;
    }

    public static CardScript AddRandomCounter(int min, int max)
    {
        CardScriptAddRandomCounter counter = ScriptableObject.CreateInstance<CardScriptAddRandomCounter>();
        counter.name = "Give Counter";
        counter.counterRange = new Vector2Int(min, max);
        return counter;
    }
}
