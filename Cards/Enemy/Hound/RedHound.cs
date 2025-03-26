using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class RedHound : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("redHound", "Red Hound")
                .SetSprites("HoundFire.png", "Wendy_BG.png")
                .SetStats(5, 1, 3)
                .SetStartWithEffect(SStack("MultiHit", 1))
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Monster Meat When Destroyed", 1),
                        SStack("When Destroyed Overheat All Enemies", 3),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Wild", 1) };
                })
                .WithCardType("Enemy")
                .WithValue(4 * 36)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Red Hound")
                .WithText("Summon <card=dstmod.redHound>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("redHound");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyed>("When Destroyed Overheat All Enemies")
                .WithText("When destroyed apply <{a}> <keyword=dstmod.overheat> to enemies".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.effectToApply = TryGet<StatusEffectData>("Overheat");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                })
        );
    }
}
