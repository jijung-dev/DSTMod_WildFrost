using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Varg : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("varg", "Varg")
                .SetBossSprites("Varg.png", "Wendy_BG.png")
                .SetStats(15, 3, 4)
                .SetTraits(TStack("Wild", 1))
                .WithCardType("Miniboss")
                .WithText("<keyword=dstmod.alphahound><hiddencard=dstmod.hound><hiddencard=dstmod.redHound><hiddencard=dstmod.blueHound>".Process())
                .WithValue(15 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 1), SStack("On Counter Turn Summon Hounds", 1) };
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("alphahound")
                .WithTitle("Alpha Hound")
                .WithShowName(true)
                .WithDescription(
                    "When <keyword=counter> reaches 0 summon <card=dstmod.hound> or <card=dstmod.redHound> or <card=dstmod.blueHound>".Process()
                )
                .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Summon Hounds")
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
