using BattleEditor;

public class GlowBerry : DataBase
{
	protected override void CreateBattle()
	{
		battleAssets.Add(
			(
				6,
				new BattleDataEditor(mod, "DepthWorm", 0)
					.SetSprite("Nodes/GlowBerryNode.png")
					.SetNameRef("Glow Berry?")
					.EnemyDictionary(
						('H', "greatDepthsWormHead"),
						('B', "greatDepthsWormBody"),
						('T', "greatDepthsWormTail"),
						('W', "depthsWorm"),
						('S', "slurper"),
						('M', "blueMushtree"),
						('R', "redMushtree"),
						('G', "greenMushtree"),
						('L', "stalagmite"),
						('A', "tallStalagmite")
					)
					.StartWavePoolData(0, "Wave 1: B I G  A F  W O R M")
					.ConstructWaves(6, 0, "MLHBTB", "RLHBTB", "GLHBTB") // 1 wood 1 rock 1 gold
					.StartWavePoolData(1, "Wave 2: Smol worm")
					.ConstructWaves(2, 2, "AW") // 2 rock 1 gold
					.StartWavePoolData(2, "Wave 3: Mushtree biome!")
					.ConstructWaves(4, 2, "SMMS", "SRRS", "SGGS") // 2 wood
					.StartWavePoolData(3, "Wave 4: More smol worm")
					.ConstructWaves(2, 2, "MW", "LW", "RW", "GW") // 1 random rock wood
					.StartWavePoolData(4, "Wave 5: Just to fit lol")
					.ConstructWaves(1, 3, "M", "R", "G", "L") // 1 random rock wood
					.AddBattleToLoader()
					.LoadBattle(6, resetAllOnClear: true, "GameModeNormal", BattleStack.Exclusivity.removeUnmodded)
			)
		);
	}
}
