using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class RabbitKing : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("fortuitousRabbit", "Fortuitous Rabbit")
                .SetSprites("scareRabbit.png", "Wendy_BG.png")
                .WithText("<hiddencard=dstmod.wrathfulRabbitKing><hiddencard=dstmod.benevolentRabbitKing>")
                .SetTraits(TStack("Fragile", 1))
                .SetStats(1, null, 8)
                .WithText("<keyword=dstmod.neutral>".Process())
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.unitPool);
                    data.startWithEffects = new CardData.StatusEffectStacks[2]
                    {
                        SStack("Neutral On Turn Summon BRK On Destroyed Summon WRB", 1),
                        SStack("Destroy Self After Counter Turn", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("wrathfulRabbitKing", "Wrathful Rabbit King")
                .SetSprites("evilRabbit.png", "Wendy_BG.png")
                .SetTraits(TStack("Spark", 1))
                .SetStats(5, 3, 3)
                .WithCardType("Enemy")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("While Active Frenzy To AlliesInRow", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("benevolentRabbitKing", "Benevolent Rabbit King")
                .SetSprites("friendlyRabbit.png", "Wendy_BG.png")
                .SetTraits(TStack("Spark", 1))
                .SetStats(5, 3, 3)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("While Active Frenzy To AlliesInRow", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("Neutral On Turn Summon BRK On Destroyed Summon WRB")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Benevolent Rabbit King");
                    data.effectToApplyWhenNotOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Wrathful Rabbit King");
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Benevolent Rabbit King")
                .WithText("Summon <card=dstmod.benevolentRabbitKing>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("benevolentRabbitKing");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Benevolent Rabbit King")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Benevolent Rabbit King");
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Wrathful Rabbit King")
                .WithText("Summon <card=dstmod.wrathfulRabbitKing>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("wrathfulRabbitKing");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Wrathful Rabbit King")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Wrathful Rabbit King");
                    data.summonPosition = StatusEffectInstantSummon.Position.EnemyRow;
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("neutral")
                .WithTitle("Neutral")
                .WithShowName(true)
                .WithDescription(
                    "When <keyword=counter> reaches 0 summon <card=dstmod.benevolentRabbitKing>".Process()
                        + ", if got destroyed before then summon <card=dstmod.wrathfulRabbitKing> on the enemy side".Process()
                )
                .WithTitleColour(new Color(0.96f, 0.7f, 0.1f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }
}
