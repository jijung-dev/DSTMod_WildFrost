using BattleEditor;

public class Terrarium : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "EyeOfTerror", 0)
                    .SetSprite("Nodes/TerrariumNode.png")
                    .SetNameRef("Terrarium")
                    .EnemyDictionary(
                        ('B', "berryBush"),
                        ('S', "spider"),
                        ('W', "spiderWarrior"),
                        ('N', "spiderNest"),
                        ('E', "eyeOfTerror"),
                        ('R', "stone"),
                        ('T', "smallTree")
                    )
                    .StartWavePoolData(0, "Wave 1: Spida")
                    .ConstructWaves(5, 0, "SRTWB", "WTRSB") // 1 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Oh no a nest")
                    .ConstructWaves(4, 2, "WWNR", "WWNS") // 1 random wood rock
                    .StartWavePoolData(2, "Wave 3: E Y E")
                    .ConstructWaves(3, 3, "ENR", "ENT") // 1 random wood rock
                    .StartWavePoolData(3, "Wave 4: B e r r y ?")
                    .ConstructWaves(3, 4, "SWB", "WSB")
                    .AddBattleToLoader()
                    .LoadBattle(1, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
