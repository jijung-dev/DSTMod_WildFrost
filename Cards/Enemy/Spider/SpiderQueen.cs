using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class SpiderQueen : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spiderQueen", "Spider Queen")
                .SetBossSprites("SpiderQueen.png", "Wendy_BG.png")
                .SetStats(10, 2, 4)
                .SetTraits(TStack("Smackback", 1))
                .WithCardType("Miniboss")
                .WithValue(13 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Counter Turn Summon Spiders With Nurse", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Spiders With Nurse")
                .WithText("When <keyword=counter> reaches 0 summon <card=dstmod.spiderWarrior> or <card=dstmod.spiderNurse>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Summon Spiders With Nurse");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonRandom>("Summon Spiders With Nurse")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                    data.randomCards = new StatusEffectSummon[2]
                    {
                        TryGet<StatusEffectSummon>("Summon Spider Nurse"),
                        TryGet<StatusEffectSummon>("Summon Spider Warrior"),
                    };
                })
        );
    }
}
