using BattleEditor;

public class AntlionBattle : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                3,
                new BattleDataEditor(mod, "Antlion", 0)
                    .SetSprite("Nodes/AntlionNode.png")
                    .SetNameRef("Desert")
                    .EnemyDictionary(
                        ('C', "cactus"),
                        ('H', "hound"),
                        ('B', "buzzard"),
                        ('A', "antlion"),
                        ('R', "boulder"),
                        ('T', "stone"),
                        ('S', "spikyTree"),
                        ('G', "goldOre")
                    )
                    .StartWavePoolData(0, "Wave 1: Sand")
                    .ConstructWaves(4, 0, "RCSB", "RCSH") // 1 wood 2 rock
                    .StartWavePoolData(1, "Wave 2: Lots of breaking")
                    .ConstructWaves(4, 2, "HHGT", "HHGS", "BBGS", "BBGT") // 1 gold 1 random wood rock
                    .StartWavePoolData(2, "Wave 3: C A C T I")
                    .ConstructWaves(4, 2, "CBSC", "CHSC") // 1 wood
                    .StartWavePoolData(3, "Wave 4: Ant")
                    .ConstructWaves(3, 3, "HBA", "BHA")
                    .StartWavePoolData(4, "Wave 4: One more cactus")
                    .ConstructWaves(1, 4, "C")
                    .AddBattleToLoader()
                    .LoadBattle(3, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeAll)
            )
        );
    }
}
