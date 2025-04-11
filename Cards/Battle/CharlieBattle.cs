using BattleEditor;

public class CharlieBattle : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                8,
                new BattleDataEditor(mod, "CharlieBattle", 0)
                    .SetSprite("Nodes/CharlieNode.png", 200)
                    .SetNameRef("Charlie")
                    .EnemyDictionary(('C', "charlie"))
                    .StartWavePoolData(0, "Wave 1: D A R K N E S S")
                    .ConstructWaves(1, 0, "C")
                    .AddBattleToLoader()
                    .LoadBattle(8, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
