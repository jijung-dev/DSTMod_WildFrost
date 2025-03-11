using System.Collections.Generic;
using System.Linq;

public class TargetModeAllUnit : TargetMode
{
    public TargetConstraint[] constraints;

    public override bool NeedsTarget => false;

    public override Entity[] GetPotentialTargets(Entity entity, Entity target, CardContainer targetContainer)
    {
        HashSet<Entity> hashSet = new HashSet<Entity>();
        hashSet.AddRange(from e in Battle.GetAllUnits() where (bool)e && e.enabled && e.alive && e.canBeHit && CheckConstraints(e) select e);
        if (hashSet.Count <= 0)
        {
            return null;
        }

        return hashSet.ToArray();
    }

    public override Entity[] GetSubsequentTargets(Entity entity, Entity target, CardContainer targetContainer)
    {
        HashSet<Entity> hashSet = new HashSet<Entity>();
        hashSet.AddRange(Battle.GetAllUnits());
        hashSet.Remove(entity);
        if (hashSet.Count <= 0)
        {
            return null;
        }

        return hashSet.ToArray();
    }

    public bool CheckConstraints(Entity target)
    {
        TargetConstraint[] array = constraints;
        if (array != null && array.Length > 0)
        {
            return constraints.All((TargetConstraint c) => c.Check(target));
        }

        return true;
    }
}
