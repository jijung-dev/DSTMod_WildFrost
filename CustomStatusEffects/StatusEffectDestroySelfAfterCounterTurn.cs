namespace DSTMod_WildFrost
{
    public class StatusEffectDestroySelfAfterCounterTurn : StatusEffectData
    {
        public bool cardPlayed;

        public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
        {
            if (!cardPlayed && entity.counter.current <= 0 && entity == target && !target.silenced)
            {
                ActionQueue.Add(new ActionKill(entity));
                cardPlayed = true;
            }

            return false;
        }
    }
}
