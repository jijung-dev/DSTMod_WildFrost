using BattleEditor;

public class HoundMound : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                new BattleDataEditor(mod, "Dog Spawner", 0)
                    .SetSprite("Nodes/HoundNode.png")
                    .SetNameRef("Hound Mound")
                    .EnemyDictionary(
                        ('J', "juicyBerryBush"),
                        ('H', "hound"),
                        ('R', "redHound"),
                        ('B', "blueHound"),
                        ('L', "varglet"),
                        ('V', "varg"),
                        ('S', "stone"),
                        ('T', "smallTree")
                    )
                    .StartWavePoolData(0, "Wave 1: Bau bau")
                    .ConstructWaves(4, 0, "THJS", "SHJT") // 1 wood 1 rock
                    .StartWavePoolData(1, "Wave 2: Fuwa or Moco")
                    .ConstructWaves(3, 2, "TRR", "SBB", "SRR", "TBB") //1 random wood rock
                    .StartWavePoolData(3, "Wave 3: BAU BAU")
                    .ConstructWaves(3, 3, "HLS", "HLT") //1 random wood rock
                    .StartWavePoolData(4, "Wave 4: BAU BAU BAU")
                    .ConstructWaves(4, 4, "RVR", "BVB", "HVHH")
                    .StartWavePoolData(4, "Wave 4: b e r r i e s")
                    .ConstructWaves(3, 5, "HHJ", "RJ", "BJ")
                    .AddBattleToLoader()
                    .LoadBattle(1, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
