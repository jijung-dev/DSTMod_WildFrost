namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXWhenCertainAllyIsHit : StatusEffectApplyXWhenAllyIsHit
    {
        public CardData ally;

        public override bool RunPostHitEvent(Hit hit)
        {
            if (hit.target?.name == ally.name)
            {
                return base.RunPostHitEvent(hit);
            }

            return false;
        }
    }
}
