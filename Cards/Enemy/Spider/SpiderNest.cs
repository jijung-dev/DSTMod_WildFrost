using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class SpiderNest : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spiderNest", "Spider Nest")
                .SetCardSprites("SpiderNest.png", "Wendy_BG.png")
                .SetStats(6, null, 4)
                .SetTraits(TStack("Backline", 1), TStack("Unmovable", 1))
                .WithCardType("Enemy")
                .WithValue(4 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.canShoveToOtherRow = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Counter Turn Summon Spiders", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Spiders")
                .WithText("Summon <card=dstmod.spider> or <card=dstmod.spiderWarrior>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Summon Spiders");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonRandom>("Summon Spiders")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                    data.randomCards = new StatusEffectSummon[2]
                    {
                        TryGet<StatusEffectSummon>("Summon Spider"),
                        TryGet<StatusEffectSummon>("Summon Spider Warrior"),
                    };
                })
        );
    }
}
