using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost;

public class StatusEffectDarkness : StatusEffectData
{
	public CardAnimation buildupAnimation = new Scriptable<CardAnimationShake>();
	public bool isBossWave = false;
	bool isSpawned = true;

	public List<StatusEffectData> bossFillEffects;
	public StatusEffectData resourceFillEffects;

	public override void Init()
	{
		base.OnEntityDestroyed += EntityDestroyed;
		base.OnTurnEnd += TurnEnd;
	}

	private IEnumerator TurnEnd(Entity entity)
	{
		if (!isSpawned)
		{
			isSpawned = true;
			var effect = target.FindStatus(resourceFillEffects);
			if ((bool)effect)
			{
				yield return effect.Remove();
			}
			Routine.Clump clump = new Routine.Clump();
			Hit hit = new Hit(target, target, 0) { damageType = "dst.resourcefill" };
			hit.AddStatusEffect(resourceFillEffects, 1);
			hit.AddStatusEffect(DSTMod.Instance.TryGet<StatusEffectData>("Set Counter"), 15);

			clump.Add(hit.Process());
			yield return clump.WaitForEnd();
		}
		else
		{
			if (IsCleared())
			{
				var effect = bossFillEffects[UnityEngine.Random.Range(0, bossFillEffects.Count)];
				bossFillEffects.Remove(effect);

				Routine.Clump clump = new Routine.Clump();
				Hit hit = new Hit(target, target, 0) { damageType = "dst.bossfill" };
				hit.AddStatusEffect(effect, 1);
				hit.AddStatusEffect(DSTMod.Instance.TryGet<StatusEffectData>("Set Counter"), 999);

				clump.Add(hit.Process());
				yield return clump.WaitForEnd();
				isSpawned = false;
			}
		}
	}

	public bool IsCleared()
	{
		var rows = References.Battle.GetRows(Battle.instance.enemy);
		float freeSlots = 0;
		foreach (CardContainer item in rows)
		{
			if (item is CardSlotLane cardSlotLane)
			{
				freeSlots += cardSlotLane.slots.Count((CardSlot slot) => slot.Empty);
			}
		}
		if (freeSlots >= 6)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public override bool RunTurnEndEvent(Entity entity)
	{
		return entity == target && !isBossWave;
	}

	private IEnumerator EntityDestroyed(Entity entity, DeathType deathType)
	{
		VFXHelper.SFX.TryPlaySound("Sanity_Apply");
		VFXHelper.VFX.TryPlayEffect("Darkness_Apply", target.transform.position);
		if ((bool)buildupAnimation)
		{
			yield return buildupAnimation.Routine(target);
		}

		var effect = target.FindStatus("dst.shadow");
		yield return effect?.RemoveStacks(1, false);
		isBossWave = false;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (entity.data.cardType == DSTMod.Instance.TryGet<CardType>("BossSmall"))
		{
			return true;
		}
		return false;
	}
}
