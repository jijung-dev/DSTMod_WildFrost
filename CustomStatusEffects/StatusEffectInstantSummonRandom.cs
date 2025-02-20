using System.Collections;

namespace DSTMod_WildFrost
{
    public class StatusEffectInstantSummonRandom : StatusEffectInstantSummon
    {
        public StatusEffectSummon[] randomCards;

        public override IEnumerator Process()
        {
            targetSummon = GetRandomCard();
            return base.Process();
        }

        private StatusEffectSummon GetRandomCard()
        {
            int ranNum = UnityEngine.Random.Range(0, randomCards.Length);
            return randomCards[ranNum];
        }
    }
}
