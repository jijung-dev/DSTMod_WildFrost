using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetModeWide : TargetMode
{
	public TargetConstraint[] constraints;

	public override bool NeedsTarget => true;

	public override Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
	{
		HashSet<Entity> hashSet = new HashSet<Entity>();
		if ((bool)target)
		{
			hashSet.Add(target);
			var rows = Battle.instance.GetRows(target.owner).Cast<CardSlotLane>().ToArray();
			foreach (var item in rows)
			{
				int index = item.slots.FindIndex(slot => slot.GetTop() == target);
				if (index >= 0)
					hashSet.Add(item.slots[index].GetTop());
			}
		}
		// else
		// {
		// 	int[] rowIndices = Battle.instance.GetRowIndices(entity);
		// 	if (rowIndices.Length != 0)
		// 	{
		// 		int[] array = rowIndices;
		// 		foreach (int rowIndex in array)
		// 		{
		// 			AddTargets(entity, hashSet, rowIndex);
		// 		}

		// 		if (hashSet.Count == 0)
		// 		{
		// 			int rowCount = Battle.instance.rowCount;
		// 			for (int j = 0; j < rowCount; j++)
		// 			{
		// 				if (!rowIndices.Contains(j))
		// 				{
		// 					AddTargets(entity, hashSet, j);
		// 				}
		// 			}
		// 		}
		// 	}
		// }

		if (hashSet.Count <= 0)
		{
			return null;
		}
		return hashSet.ToArray();
	}

	// public override Entity[] GetSubsequentTargets(Entity entity, Entity target, CardContainer targetContainer)
	// {
	// 	HashSet<Entity> hashSet = new HashSet<Entity>();
	// 	hashSet.AddRange(Battle.GetCardsOnBoard(target.owner));
	// 	hashSet.Remove(entity);
	// 	if (hashSet.Count <= 0)
	// 	{
	// 		return null;
	// 	}

	// 	return hashSet.ToArray();
	// }

	public bool CheckConstraints(Entity target)
	{
		TargetConstraint[] array = constraints;
		if (array != null && array.Length > 0)
		{
			return constraints.All((TargetConstraint c) => c.Check(target));
		}

		return true;
	}
	public void AddTargets(Entity entity, HashSet<Entity> targets, int rowIndex)
	{
		List<Entity> enemiesInRow = entity.GetEnemiesInRow(rowIndex);
		Entity entity2 = null;
		foreach (Entity item in enemiesInRow)
		{
			if ((bool)item && item.enabled && item.alive && item.canBeHit)
			{
				entity2 = item;
				break;
			}
		}

		if ((bool)entity2)
		{
			targets.Add(entity2);
			return;
		}

		entity2 = GetEnemyCharacter(entity);
		if ((bool)entity2)
		{
			targets.Add(entity2);
		}
	}

}
