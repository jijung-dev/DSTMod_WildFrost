using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class BlueHound : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("blueHound", "Blue Hound")
                .SetSprites("HoundIce.png", "Wendy_BG.png")
                .SetStats(5, 1, 3)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Destroyed Freezing All Enemies", 3),
                        SStack("Gain Monster Meat When Destroyed", 1),
                        SStack("MultiHit", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Wild", 1) };
                })
                .WithCardType("Enemy")
                .WithValue(3 * 36)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Blue Hound")
                .WithText("Summon <card=dstmod.blueHound>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("blueHound");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyed>("When Destroyed Freezing All Enemies")
                .WithText("When destroyed apply <{a}> <keyword=dstmod.freeze> to enemies".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.effectToApply = TryGet<StatusEffectData>("Freezing");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                })
        );
    }
}
