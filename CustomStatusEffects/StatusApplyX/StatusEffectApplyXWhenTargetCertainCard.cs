using System.Linq;
using DSTMod_WildFrost;
using WildfrostHopeMod.VFX;

public class StatusEffectApplyXWhenTargetCertainCard : StatusEffectApplyXOnCardPlayed
{
    public TargetConstraint[] constraints;
    public bool hasAnimation;

    public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
    {
        if (constraints == null || targets.Any(r => constraints.Any(c => c.Check(r))))
        {
            if (hasAnimation && entity == target)
            {
                VFXHelper.SFX.TryPlaySound("Heat_Apply");
                hasAnimation = false;
            }

            return base.RunCardPlayedEvent(entity, targets);
        }
        return false;
    }
}
