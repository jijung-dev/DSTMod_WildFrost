public class TargetConstraintNotHasTrait : TargetConstraint
{
    public TraitData trait;

    public bool ignoreSilenced;

    public override bool Check(Entity target)
    {
        if (ignoreSilenced && target.silenced)
        {
            return not;
        }

        bool flag = false;
        foreach (Entity.TraitStacks trait in target.traits)
        {
            if (trait.data.name == this.trait.name)
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            return !not;
        }

        return not;
    }

    public override bool Check(CardData targetData)
    {
        bool flag = false;
        foreach (CardData.TraitStacks trait in targetData.traits)
        {
            if (trait.data.name == this.trait.name)
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            return !not;
        }

        return not;
    }
}