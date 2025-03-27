using BattleEditor;

public class GiantBeeHive : DataBase
{
	protected override void CreateBattle()
	{
		battleAssets.Add(
				(
					4,
					new BattleDataEditor(mod, "BeeQueen", 0)
						.SetSprite("Nodes/BeeQueenHiveNode.png")
						.SetNameRef("Giant Beehive")
						.EnemyDictionary(
							('B', "bee"),
							('K', "killerBee"),
							('N', "beehive"),
							('Q', "beeQueen"),
							('G', "grumbleBee"),

							('S', "smallTree"),
							('R', "stone"),
							('T', "tree"),
							('O', "goldOre")
						)
						.StartWavePoolData(0, "Wave 1: Bzz")
						.ConstructWaves(4, 0, "RBBS", "SBBR") // 1 wood 1 rock
						.StartWavePoolData(1, "Wave 2: Bzzzz")
						.ConstructWaves(5, 2, "BTRN", "BRNRS", "BRSSN") // 1 wood 1 rock 1 random wood rock
						.StartWavePoolData(2, "Wave 3: BZZzz")
						.ConstructWaves(4, 2, "KOKN") // 1 gold
						.StartWavePoolData(3, "Wave 4: Q U E E N")
						.ConstructWaves(4, 3, "GGQG")
						.StartWavePoolData(4, "Wave 5: BzzZz")
						.ConstructWaves(2, 4, "SG", "RG", "RN", "SN") // 1 random wood rock
						.AddBattleToLoader()
						.LoadBattle(4, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeAll)
				)
			);
	}
}
