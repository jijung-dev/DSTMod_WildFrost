using BattleEditor;

public class LootStash : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                5,
                new BattleDataEditor(mod, "Klaus", 0)
                    .SetSprite("Nodes/LootStashNode.png")
                    .SetNameRef("Loot Stash")
                    .EnemyDictionary(
                        ('K', "klaus"),
                        ('P', "krampus"),
                        ('R', "redGemDeer"),
                        ('B', "blueGemDeer"),
                        ('J', "juicyBerryBush"),
                        ('G', "goldOre"),
                        ('S', "smallTree"),
                        ('O', "stone"),
                        ('T', "tree")
                    )
                    .StartWavePoolData(0, "Wave 1: Naughty Santa")
                    .ConstructWaves(5, 0, "TORBK", "OTBRK") // 2 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: R E S O U R C E")
                    .ConstructWaves(5, 2, "SOSJP", "SOOJP") // 1 wood 1 random wood rock
                    .StartWavePoolData(2, "Wave 3: goop XD")
                    .ConstructWaves(4, 2, "GOOP") // 1 gold 1 rock
                    .StartWavePoolData(3, "Wave 4: Japan XD")
                    .ConstructWaves(2, 3, "JP")
                    .AddBattleToLoader()
                    .LoadBattle(5, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
