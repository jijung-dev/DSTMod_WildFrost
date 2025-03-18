using System.Collections.Generic;

public class ConsumableFatory : ConsumableBase
{
    public override void CreateCard()
    {
        consumables.AddRange(
            new List<ConsumableInstance>
            {
                Create("honey", "Honey", "Honey.png", SStack(("Heal", 1)), ConsumeType.Food, "Taffy"),
                Create("berries", "Berries", "Berries.png", SStack(("Heal", 1)), ConsumeType.Food, "Roasted Berries"),
                Create("juicyBerries", "Juicy Berries", "JuicyBerries.png", SStack(("Heal", 1)), ConsumeType.Food, "Roasted Juicy Berries"),
                Create("blueCap", "Blue Cap", "BlueCap.png", SStack(("Heal", 1), ("Sanity", 1)), ConsumeType.Food, "Cooked Blue Cap"),
                Create("greenCap", "Green Cap", "GreenCap.png", SStack(("Reduce Sanity", 1)), ConsumeType.Food, "Cooked Green Cap"),
                Create("redCap", "Red Cap", "RedCap.png", SStack(("Heal", 2), ("Sanity", 2)), ConsumeType.Food, "Cooked Red Cap"),
                Create("cactusFlesh", "Cactus Flesh", "CactusFlesh.png", SStack(("Heal", 1)), ConsumeType.Food, "Cooked Cactus Flesh"),
                Create("monsterMeat", "Monster Meat", "MonsterMeat.png", SStack(("Heal", 1), ("Sanity", 2)), ConsumeType.Food, "Monster Lasagna"),
                Create("taffy", "Taffy", "Taffy.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("roastedBerries", "Roasted Berries", "RoastedBerries.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("roastedJuicyBerries", "Roasted Juicy Berries", "RoastedJuicyBerries.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("cookedBlueCap", "Cooked Blue Cap", "CookedBlueCap.png", SStack(("Heal", 1), ("Reduce Sanity", 2)), ConsumeType.Cooked),
                Create("cookedGreenCap", "Cooked Green Cap", "CookedGreenCap.png", SStack(("Reduce Sanity", 3)), ConsumeType.Cooked),
                Create("cookedRedCap", "Cooked Red Cap", "CookedRedCap.png", SStack(("Heal", 2), ("Sanity", 1)), ConsumeType.Cooked),
                Create(
                    "cookedCactusFlesh",
                    "Cooked Cactus Flesh",
                    "CookedCactusFlesh.png",
                    SStack(("Heal", 1), ("Reduce Sanity", 2)),
                    ConsumeType.Cooked
                ),
                Create("monsterLasagna", "Monster Lasagna", "MonsterLasagna.png", SStack(("Heal", 2), ("Sanity", 2)), ConsumeType.Cooked),
                Create("lightBulb", "Light Bulb", "LightBulb.png", SStack(("Heal", 1)), ConsumeType.Consumable),
                Create("foliage", "Foliage", "Foliage.png", SStack(("Heal", 1)), ConsumeType.Consumable),
                Create("lesserGlowBerry", "Lesser Glow Berry", "LesserGlowBerry.png", SStack(("Heal", 2), ("Sanity", 1)), ConsumeType.Consumable),
            }
        );
        base.CreateCard();
    }
}
