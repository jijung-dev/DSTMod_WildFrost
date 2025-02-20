namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXOnCounterTurn : StatusEffectApplyXOnTurn
    {
        public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
        {
            if (entity.counter.current <= 0)
            {
                return base.RunCardPlayedEvent(entity, targets);
            }

            return false;
        }

        public override bool RunTurnEvent(Entity entity)
        {
            if (entity.counter.current <= 0)
            {
                return base.RunTurnEvent(entity);
            }
            return false;
        }
    }
}
