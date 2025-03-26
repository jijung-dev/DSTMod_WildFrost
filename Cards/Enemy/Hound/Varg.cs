using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Varg : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("varg", "Varg")
                .SetSprites("Varg.png", "Wendy_BG.png")
                .SetStats(14, 6, 5)
                .SetTraits(TStack("Wild", 1))
                .WithCardType("Miniboss")
                .WithValue(17 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 1), SStack("On Counter Turn Summon Hounds", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Hounds")
                .WithText(
                    "When <keyword=counter> reaches 0 summon <card=dstmod.hound> or <card=dstmod.redHound> or <card=dstmod.blueHound>".Process()
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Summon Hounds");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonRandom>("Summon Hounds")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                    data.randomCards = new StatusEffectSummon[]
                    {
                        TryGet<StatusEffectSummon>("Summon Hound"),
                        TryGet<StatusEffectSummon>("Summon Red Hound"),
                        TryGet<StatusEffectSummon>("Summon Blue Hound"),
                    };
                })
        );
    }
}
