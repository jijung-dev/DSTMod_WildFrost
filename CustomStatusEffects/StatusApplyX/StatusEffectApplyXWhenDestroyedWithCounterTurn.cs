namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXWhenDestroyedWithCounterTurn : StatusEffectApplyXWhenDestroyedUnNullable
    {
        public StatusEffectData effectToApplyWhenNotOnCounterTurn;
        public StatusEffectData effectToApplyWhenOnCounterTurn;

        public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
        {
            if (entity.counter.current <= 0)
            {
                effectToApply = effectToApplyWhenOnCounterTurn;
            }
            else
            {
                if (effectToApplyWhenNotOnCounterTurn == null)
                    return false;
                effectToApply = effectToApplyWhenNotOnCounterTurn;
            }
            return base.RunEntityDestroyedEvent(entity, deathType);
        }
    }
}
