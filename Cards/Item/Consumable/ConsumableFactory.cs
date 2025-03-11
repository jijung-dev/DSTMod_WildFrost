using System.Collections.Generic;

public class ConsumableFatory : ConsumableBase
{
    public override void CreateCard()
    {
        consumables.AddRange(
            new List<ConsumableInstance>
            {
                Create("honey", "Honey", "Honey.png", SStack(("Heal", 1)), ConsumeType.Food),
                Create("berries", "Berries", "Berries.png", SStack(("Heal", 1)), ConsumeType.Food),
                Create("juicyBerries", "Juicy Berries", "JuicyBerries.png", SStack(("Heal", 1)), ConsumeType.Food),
                Create("honey", "Honey", "Honey.png", SStack(("Heal", 1)), ConsumeType.Food),
                Create("blueCap", "Blue Cap", "BlueCap.png", SStack(("Heal", 1)), ConsumeType.Food),
                Create("greenCap", "Green Cap", "GreenCap.png", SStack(("Reduce Sanity", 1)), ConsumeType.Food),
                Create("redCap", "Red Cap", "RedCap.png", SStack(("Heal", 2), ("Sanity", 2)), ConsumeType.Food),
                Create("cactusFlesh", "Cactus Flesh", "CactusFlesh.png", SStack(("Heal", 2), ("Reduce Sanity", 2)), ConsumeType.Food),
                Create("monsterMeat", "Monster Meat", "MonsterMeat.png", SStack(("Heal", 1), ("Sanity", 2)), ConsumeType.Food),
                Create("lightBulb", "Light Bulb", "LightBulb.png", SStack(("Heal", 1)), ConsumeType.Consumable),
                Create("foliage", "Foliage", "Foliage.png", SStack(("Heal", 1)), ConsumeType.Consumable),
                Create("lesserGlowBerry", "Lesser Glow Berry", "LesserGlowBerry.png", SStack(("Heal", 2), ("Sanity", 1)), ConsumeType.Consumable),
            }
        );
        base.CreateCard();
    }
}
