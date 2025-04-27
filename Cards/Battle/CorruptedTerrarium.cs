using BattleEditor;

public class CorruptedTerrarium : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "TwinsOfTerror", 0)
                    .SetSprite("Nodes/CorruptedTerrariumNode.png")
                    .SetNameRef("Corrupted Terrarium")
                    .EnemyDictionary(
                        ('B', "blueMushtree"),
                        ('W', "webbedBlueMushtree"),
                        ('G', "greenMushtree"),
                        ('R', "redMushtree"),
                        ('C', "caveSpider"),
                        ('S', "spitter"),
                        ('E', "retinazor"),
                        ('T', "spazmatism"),
                        ('L', "stalagmite"),
                        ('O', "stone")
                    )
                    .StartWavePoolData(0, "Wave 1: Webbed Guy")
                    .ConstructWaves(4, 0, "GWOS", "RWOS") // 2 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Spuda lots")
                    .ConstructWaves(5, 2, "LCOS", "LCBS", "LCGS", "LCRS") // 1 gold 1 rock 1 random wood rock
                    .StartWavePoolData(3, "Wave 4: THE TWINS")
                    .ConstructWaves(2, 3, "ET", "TE")
                    .StartWavePoolData(4, "Wave 5: More Mushtree?")
                    .ConstructWaves(2, 4, "W", "BC", "GC", "RC", "OC") // 1 random wood rock
                    .AddBattleToLoader()
                    .LoadBattle(4, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
