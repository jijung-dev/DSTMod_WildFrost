using BattleEditor;

public class IceFishingHole : DataBase
{
    protected override void CreateBattle()
    {
        battleAssets.Add(
            (
                3,
                new BattleDataEditor(mod, "Ice Fishing Hole", 0)
                    .SetSprite("Nodes/IceFishingHoleNode.png")
                    .SetNameRef("Ice Fishing Hole")
                    .EnemyDictionary(
                        ('B', "bullKelp"),
                        ('F', "frostjaw"),
                        ('R', "rockjawDived"),
                        ('L', "glacier"),
                        ('P', "pengull"),
                        ('S', "seaStack"),
                        ('D', "driftwood"),
                        ('G', "gildedSeaStack")
                    )
                    .StartWavePoolData(0, "Wave 1: Da Sea")
                    .ConstructWaves(4, 0, "DRSB", "SRDB") // 1 wood 2 rock
                    .StartWavePoolData(1, "Wave 2: Pengu")
                    .ConstructWaves(4, 2, "GSPP", "GDPP") // 1 gold 1 random wood rock
                    .StartWavePoolData(2, "Wave 3: Ice")
                    .ConstructWaves(3, 2, "LDB") // 1 wood
                    .StartWavePoolData(3, "Wave 4: SHAAAAAAAA")
                    .ConstructWaves(3, 3, "FRP")
                    .StartWavePoolData(4, "Wave 4: k e l p")
                    .ConstructWaves(1, 4, "B")
                    .AddBattleToLoader()
                    .LoadBattle(3, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
            )
        );
    }
}
