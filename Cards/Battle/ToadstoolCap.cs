using BattleEditor;

public class ToadstoolCap : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "ToadStool", 0)
                    .SetSprite("Nodes/ToadstoolNode.png")
                    .SetNameRef("Toadstool Cap")
                    .EnemyDictionary(
                        ('C', "caveSpider"),
                        ('B', "batilisk"),
                        ('T', "toadstool"),
                        ('M', "blueMushtree"),
                        ('R', "redMushtree"),
                        ('G', "greenMushtree"),
                        ('S', "stalagmite"),
                        ('A', "tallStalagmite")
                    )
                    .StartWavePoolData(0, "Wave 1: CAVING")
                    .ConstructWaves(4, 0, "MCSC", "RCSC", "GCSC") // 1 wood 1 rock 1 gold
                    .StartWavePoolData(1, "Wave 2: Tall guy")
                    .ConstructWaves(4, 2, "CABB") // 2 rock 1 gold
                    .StartWavePoolData(2, "Wave 3: Mushtree biome!")
                    .ConstructWaves(3, 2, "MBM", "RBR", "GBG") // 2 wood
                    .StartWavePoolData(3, "Wave 4: T O A D")
                    .ConstructWaves(3, 3, "TCS", "TCM", "TCR", "TCR") // 1 random rock wood
                    .StartWavePoolData(4, "Wave 5: Yes?")
                    .ConstructWaves(1, 4, "S", "A") // 1 random rock wood
                    .AddBattleToLoader()
                    .LoadBattle(6, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
