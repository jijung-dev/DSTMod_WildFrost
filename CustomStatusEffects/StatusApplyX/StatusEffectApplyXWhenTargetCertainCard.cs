using System.Linq;
using DSTMod_WildFrost;
using WildfrostHopeMod.VFX;

public class StatusEffectApplyXWhenTargetCertainCard : StatusEffectApplyXOnCardPlayed
{
	public TargetConstraint[] constraints;
	public bool hasAnimation;

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (targets.Any(target => constraints.Any(constraint => constraint.Check(target))) || constraints == null)
		{
			if (hasAnimation) VFXMod.instance.SFX.TryPlaySoundFromPath(DSTMod.Instance.ImagePath("Heat_Apply.wav"));

			return base.RunCardPlayedEvent(entity, targets);
		}
		return false;
	}

}