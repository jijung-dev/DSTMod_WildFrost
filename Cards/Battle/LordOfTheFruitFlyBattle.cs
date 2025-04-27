using BattleEditor;

public class LordOfTheFruitFlyBattle : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "LordFruitFly", 0)
                    .SetSprite("Nodes/FruitFlyFruitNode.png")
                    .SetNameRef("Lord of the Fruit Flies")
                    .EnemyDictionary(
                        ('S', "smallTree"),
                        ('R', "stone"),
                        ('P', "rottenPotato"),
                        ('E', "rottenPepper"),
                        ('D', "rottenDurian"),
                        ('A', "rottenAsparagus"),
                        ('L', "lordFruitFly"),
                        ('F', "fruitFly")
                    )
                    .StartWavePoolData(0, "Wave 1: Rotten")
                    .ConstructWaves(4, 0, "SPR", "SERE", "SDR") // 1 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Double Rotten")
                    .ConstructWaves(4, 2, "EAD", "DAP", "PAE")
                    .StartWavePoolData(2, "Wave 3: F L I E S")
                    .ConstructWaves(5, 3, "SFLFF", "RFLFF") // 1 random wood rock
                    .StartWavePoolData(3, "Wave 4: Lost one?")
                    .ConstructWaves(2, 3, "FA", "FD", "FE", "FP")
                    .AddBattleToLoader()
                    .LoadBattle(0, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
