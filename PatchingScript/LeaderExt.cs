using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class LeaderExt
{
    public static CardScript GiveUpgrade(string name = "Crown")
    {
        CardScriptGiveUpgrade script = new Scriptable<CardScriptGiveUpgrade>();
        script.name = $"Give {name}";
        script.upgradeData = DSTMod.Instance.TryGet<CardUpgradeData>(name);
        return script;
    }

    public static CardScript AddRandomHealth(int min, int max)
    {
        CardScriptAddRandomHealth health = new Scriptable<CardScriptAddRandomHealth>();
        health.name = "Random Health";
        health.healthRange = new Vector2Int(min, max);
        return health;
    }

    public static CardScript AddRandomDamage(int min, int max)
    {
        CardScriptAddRandomDamage damage = new Scriptable<CardScriptAddRandomDamage>();
        damage.name = "Give Damage";
        damage.damageRange = new Vector2Int(min, max);
        return damage;
    }

    public static CardScript AddRandomCounter(int min, int max)
    {
        CardScriptAddRandomCounter counter = new Scriptable<CardScriptAddRandomCounter>();
        counter.name = "Give Counter";
        counter.counterRange = new Vector2Int(min, max);
        return counter;
    }

    public static CardScript AddRandomSprite(string[] sprites)
    {
        CardScriptSetSprite sprite = new Scriptable<CardScriptSetSprite>();
        sprite.name = "Give Sprite";
        sprite.sprites = sprites;
        sprite.isRandom = true;
        return sprite;
    }
}

public class CardScriptSetSprite : CardScript
{
    public string[] sprites;
    public bool isRandom;

    public override void Run(CardData target)
    {
        int ran = 0;
        if (isRandom)
        {
            ran = Random.Range(0, sprites.Length);
        }
        target.mainSprite = DSTMod.Instance.ImagePath(sprites[ran]).ToSprite();
    }
}
