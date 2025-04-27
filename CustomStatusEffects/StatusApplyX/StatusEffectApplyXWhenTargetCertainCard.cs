using System.Linq;
using DSTMod_WildFrost;
using WildfrostHopeMod.VFX;

public class StatusEffectApplyXWhenTargetCertainCard : StatusEffectApplyXOnCardPlayed
{
    public TargetConstraint[] constraints;
    public bool hasAnimation;

    public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
    {
        if (targets?.Any(r => r != null && constraints.Any(c => c.Check(r))) ?? false)
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
