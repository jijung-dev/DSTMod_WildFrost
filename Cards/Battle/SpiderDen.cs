using BattleEditor;

public class SpiderDen : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "Spider Den", 0)
                    .SetSprite("Nodes/SpiderNode.png")
                    .SetNameRef("Spider Den")
                    .EnemyDictionary(
                        ('B', "berryBush"),
                        ('S', "spider"),
                        ('W', "spiderWarrior"),
                        ('N', "spiderNest"),
                        ('Q', "spiderQueen"),
                        ('R', "stone"),
                        ('T', "smallTree")
                    )
                    .StartWavePoolData(0, "Wave 1: His")
                    .ConstructWaves(5, 0, "RTSB", "BTRS") // 1 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Hiss")
                    .ConstructWaves(2, 2, "WB", "NB")
                    .StartWavePoolData(3, "Wave 3: Hisss")
                    .ConstructWaves(3, 2, "TN", "RW", "TW", "RN") // 1 random wood or rock
                    .StartWavePoolData(4, "Wave 4: QUEEN ")
                    .ConstructWaves(3, 3, "NQ", "QN", "WQ")
                    .StartWavePoolData(5, "Wave 5: After Queen")
                    .ConstructWaves(3, 4, "WB", "SSB", "NB")
                    .AddBattleToLoader()
                    .LoadBattle(0, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
