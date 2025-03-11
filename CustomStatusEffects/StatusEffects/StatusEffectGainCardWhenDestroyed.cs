using System.Collections;

public class StatusEffectGainCardWhenDestroyed : StatusEffectInstantGainCard
{
    public bool sacrificed;
    public bool consumed;
    public override void Init()
    {
        base.OnEntityDestroyed += EntityDestroyed;
    }
    public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
    {
        if (entity == target)
        {
            return CheckDeathType(deathType);
        }

        return false;
    }

    private IEnumerator EntityDestroyed(Entity entity, DeathType deathType)
    {
        if (entity.LastHitStillProcessing())
        {
            yield return entity.WaitForLastHitToFinishProcessing();
        }
        yield return Process();
    }
    public bool CheckDeathType(DeathType deathType)
    {
        if (consumed && deathType != DeathType.Consume)
        {
            return false;
        }

        if (sacrificed && !DeathSystem.KilledByOwnTeam(target))
        {
            return false;
        }

        return true;
    }
}
