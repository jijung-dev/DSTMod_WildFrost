using System.Linq;
using UnityEngine;

public class TargetConstraintIsNotSpecificCard : TargetConstraint
{
    [SerializeField]
    public CardData[] allowedCards;

    public override bool Check(Entity target)
    {
        return Check(target.data);
    }

    public override bool Check(CardData targetData)
    {
        if (allowedCards.Any((CardData a) => a.name == targetData.name))
        {
            return not;
        }

        return !not;
    }
}
