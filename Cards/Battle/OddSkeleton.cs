using BattleEditor;

public class OddSkeleton : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "Fuelweaver", 0)
                    .SetSprite("Nodes/FuelweaverNode.png", 200)
                    .SetNameRef("Odd Skeleton")
                    .EnemyDictionary(
                        ('S', "smallTree"),
                        ('T', "tree"),
                        ('R', "stone"),
                        ('G', "goldOre"),
                        ('A', "stalagmite"),
                        ('L', "tallStalagmite"),
                        ('B', "blueMushtree"),
                        ('M', "redMushtree"),
                        ('E', "greenMushtree"),
                        ('F', "forestFuelweaver")
                    )
                    .StartWavePoolData(0, "Wave 1: Forest")
                    .ConstructWaves(3, 0, "SRF", "RSF") // 1 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Cave stuffs?")
                    .ConstructWaves(2, 2, "AB", "AM", "AE") // 1 rock 1 wood 1 gold
                    .StartWavePoolData(2, "Wave 3: B I O M E S")
                    .ConstructWaves(3, 3, "LM", "MAM", "LB", "BAB", "LE", "EAE") // 1 wood 1 rock 1 gold 1 random wood rock
                    .StartWavePoolData(3, "Wave 4: Little more")
                    .ConstructWaves(1, 4, "S", "R")
                    .AddBattleToLoader()
                    .LoadBattle(7, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
