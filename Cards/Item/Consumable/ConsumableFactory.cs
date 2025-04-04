using System.Collections.Generic;

public class ConsumableFatory : ConsumableBase
{
    public override void CreateCard()
    {
        consumables.AddRange(
            new List<ConsumableInstance>
            {
                Create(
                    "durian",
                    "Durian",
                    "Durian.png",
                    SStack(("Teeth", 1), ("Sanity", 2)),
                    ConsumeType.Food,
                    "Extra Smelly Durian",
                    "extraSmellyDurian"
                ),
                Create("pepper", "Pepper", "Pepper.png", SStack(("Spice", 1), ("Overheat", 2)), ConsumeType.Food, "Roasted Pepper", "roastedPepper"),
                Create("potato", "Potato", "Potato.png", SStack(("Heal", 2), ("Shroom", 1)), ConsumeType.Food, "Roasted Potato", "roastedPotato"),
                Create("asparagus", "Asparagus", "Asparagus.png", SStack(("Heal", 1)), ConsumeType.Food, "Cooked Asparagus", "cookedAsparagus"),
                Create("honey", "Honey", "Honey.png", SStack(("Heal", 1)), ConsumeType.Food, "Taffy", "taffy"),
                Create("meat", "Meat", "Meat.png", SStack(("Heal", 2), ("Sanity", 2)), ConsumeType.Food, "Cooked Meat", "cookedMeat"),
                Create("frogLegs", "Frog Legs", "FrogLegs.png", SStack(("Heal", 1), ("Sanity", 2)), ConsumeType.Food, "Cooked Frog Legs", "cookedFrogLegs"),
                Create("egg", "Egg", "Egg.png", SStack(("Heal", 1)), ConsumeType.Food, "Cooked Egg", "cookedEgg"),
                Create("kelpFronds", "Kelp Fronds", "KelpFronds.png", SStack(("Heal", 1)), ConsumeType.Food, "Cooked Kelp Fronds", "cookedKelpFronds"),
                Create("berries", "Berries", "Berries.png", SStack(("Heal", 1)), ConsumeType.Food, "Roasted Berries", "roastedBerries"),
                Create(
                    "juicyBerries",
                    "Juicy Berries",
                    "JuicyBerries.png",
                    SStack(("Heal", 1)),
                    ConsumeType.Food,
                    "Roasted Juicy Berries",
                    "roastedJuicyBerries"
                ),
                Create(
                    "blueCap",
                    "Blue Cap",
                    "BlueCap.png",
                    SStack(("Heal", 1), ("Sanity", 1)),
                    ConsumeType.Food,
                    "Cooked Blue Cap",
                    "cookedBlueCap"
                ),
                Create("greenCap", "Green Cap", "GreenCap.png", SStack(("Reduce Sanity", 1)), ConsumeType.Food, "Cooked Green Cap", "cookedGreenCap"),
                Create("redCap", "Red Cap", "RedCap.png", SStack(("Heal", 2), ("Sanity", 2)), ConsumeType.Food, "Cooked Red Cap", "cookedRedCap"),
                Create(
                    "cactusFlesh",
                    "Cactus Flesh",
                    "CactusFlesh.png",
                    SStack(("Heal", 1)),
                    ConsumeType.Food,
                    "Cooked Cactus Flesh",
                    "cookedCactusFlesh"
                ),
                Create(
                    "monsterMeat",
                    "Monster Meat",
                    "MonsterMeat.png",
                    SStack(("Heal", 1), ("Sanity", 2)),
                    ConsumeType.Food,
                    "Monster Lasagna",
                    "monsterLasagna"
                ),
                Create("morsel", "Morsel", "Morsel.png", SStack(("Heal", 1),("Sanity", 1)), ConsumeType.Food, "Cooked Morsel", "cookedMorsel"),
                Create("taffy", "Taffy", "Taffy.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("cookedMeat", "Cooked Meat", "CookedMeat.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("cookedFrogLegs", "Cooked Frog Legs", "CookedFrogLegs.png", SStack(("Heal", 1)), ConsumeType.Cooked),
                Create("cookedEgg", "Cooked Egg", "CookedEgg.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("cookedKelpFronds", "Cooked Kelp Fronds", "CookedKelpFronds.png", SStack(("Heal", 2)), ConsumeType.Cooked),
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
                Create("cookedMorsel", "Cooked Morsel", "CookedMorsel.png", SStack(("Heal", 1)), ConsumeType.Cooked),
                Create("extraSmellyDurian", "Extra Smelly Durian", "ExtraSmellyDurian.png", SStack(("Teeth", 2), ("Sanity", 4)), ConsumeType.Cooked),
                Create("roastedPepper", "Roasted Pepper", "RoastedPepper.png", SStack(("Spice", 2), ("Overheat", 4)), ConsumeType.Cooked),
                Create("roastedPotato", "Roasted Potato", "RoastedPotato.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("cookedAsparagus", "Cooked Asparagus", "CookedAsparagus.png", SStack(("Heal", 2)), ConsumeType.Cooked),
                Create("lightBulb", "Light Bulb", "LightBulb.png", SStack(("Heal", 1)), ConsumeType.Consumable),
                Create("foliage", "Foliage", "Foliage.png", SStack(("Heal", 1)), ConsumeType.Consumable),
                Create("lesserGlowBerry", "Lesser Glow Berry", "LesserGlowBerry.png", SStack(("Heal", 2), ("Sanity", 1)), ConsumeType.Consumable),
            }
        );
        base.CreateCard();
    }
}
