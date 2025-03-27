using BattleEditor;

public class DeerclopsBattle : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                2,
                new BattleDataEditor(mod, "DeerClops", 0)
                    .SetSprite("Nodes/DeerclopsNode.png")
                    .SetNameRef("DeerClops")
                    .EnemyDictionary(
                        ('E', "berryBush"),
                        ('H', "hound"),
                        ('B', "blueHound"),
                        ('S', "spider"),
                        ('N', "spiderNest"),
                        ('D', "deerclops"),
                        ('R', "stone"),
                        ('M', "smallTree"),
                        ('T', "tree")
                    )
                    .StartWavePoolData(0, "Wave 1: Deer")
                    .ConstructWaves(5, 0, "HTDRE", "STDRE") // 2 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Fuwa")
                    .ConstructWaves(4, 2, "BNMS", "NBRS") // 1 random wood rock
                    .StartWavePoolData(2, "Wave 3: Spuda or Baubau")
                    .ConstructWaves(3, 2, "RBH", "RSN") // 1 rock
                    .StartWavePoolData(3, "Wave 4: b e r r y")
                    .ConstructWaves(1, 3, "E")
                    .AddBattleToLoader()
                    .LoadBattle(2, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeAll)
            )
        );
    }
}
