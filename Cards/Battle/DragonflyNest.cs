using BattleEditor;

public class DragonflyNest : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                5,
                new BattleDataEditor(mod, "DragonFly", 0)
                    .SetSprite("Nodes/DragonflyNestNode.png")
                    .SetNameRef("Dragonfly Nest")
                    .EnemyDictionary(
                        ('C', "cactus"),
                        ('H', "hound"),
                        ('R', "redHound"),
                        ('T', "buzzard"),
                        ('D', "dragonfly"),
                        ('G', "goldOre"),
                        ('B', "boulder"),
                        ('O', "stone"),
                        ('S', "spikyTree")
                    )
                    .StartWavePoolData(0, "Wave 1: F I R E")
                    .ConstructWaves(4, 0, "CBDS", "CSDB") // 1 wood 2 rock
                    .StartWavePoolData(1, "Wave 2: R E S O U R C E")
                    .ConstructWaves(5, 2, "RSHSS", "RSHSO", "HSRSS", "HSRSO") // 2 wood 1 random wood rock
                    .StartWavePoolData(2, "Wave 3: Gold?")
                    .ConstructWaves(4, 2, "OGTC", "TGOC") // 1 gold 1 rock
                    .StartWavePoolData(3, "Wave 4: Cactus yay!!")
                    .ConstructWaves(2, 3, "RC", "TC")
                    .AddBattleToLoader()
                    .LoadBattle(5, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
