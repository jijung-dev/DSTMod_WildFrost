using BattleEditor;

public class MooseNest : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "MooseNest", 0)
                    .SetSprite("Nodes/MooseNestNode.png")
                    .SetNameRef("Moose Nest")
                    .EnemyDictionary(
                        ('B', "berryBush"),
                        ('F', "frog"),
                        ('A', "marotter"),
                        ('M', "moose"),
                        ('E', "mooseEgg"),
                        ('L', "mosling"),
                        ('R', "stone"),
                        ('S', "smallTree"),
                        ('T', "tree")
                    )
                    .StartWavePoolData(0, "Wave 1: The Nest")
                    .ConstructWaves(5, 0, "FTMER") // 2 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Otter kirai")
                    .ConstructWaves(5, 2, "RABLL") // 1 rock
                    .StartWavePoolData(2, "Wave 3: Some bushes some wetness")
                    .ConstructWaves(4, 3, "FBFR", "FBFS", "ABS", "ABR") // 1 random wood rock
                    .AddBattleToLoader()
                    .LoadBattle(2, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
