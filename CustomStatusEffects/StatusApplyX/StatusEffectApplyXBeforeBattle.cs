using System.Collections;
using UnityEngine;

namespace DSTMod_WildFrost
{
    public class StatusEffectApplyXBeforeBattle : StatusEffectApplyX
    {
        private bool isApplied;
        public bool checkBeforeSpawn;

        public override void Init()
        {
            base.OnEnable += Activate;
            base.OnCardMove += CheckCardMove;
        }

        public override bool RunEnableEvent(Entity entity)
        {
            if (checkBeforeSpawn && Battle.IsOnBoard(entity))
                return false;

            if (!isApplied && entity == target)
            {
                isApplied = true;
                return true;
            }

            return false;
        }

        public override bool RunCardMoveEvent(Entity entity)
        {
            if (target.enabled && entity == target)
            {
                return target.InHand();
            }

            return false;
        }

        public IEnumerator CheckCardMove(Entity entity)
        {
            return Run(GetTargets());
        }

        public IEnumerator Activate(Entity entity)
        {
            yield return Run(GetTargets(null));
        }

        public override bool RunDisableEvent(Entity entity)
        {
            isApplied = false;
            return base.RunDisableEvent(entity);
        }
    }
}
