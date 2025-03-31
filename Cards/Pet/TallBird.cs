using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class TallBird : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("tallbirdEgg", "Tallbird Egg")
                .SetSprites("TallBirdEgg.png", "Wendy_BG.png")
                .WithText("<hiddencard=dstmod.smallbird>".Process())
                .IsPet("", true)
                .SetStats(1, null, 6)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[2]
                    {
                        SStack("On Counter Turn Summon Smallbird", 1),
                        SStack("Destroy Self After Counter Turn", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("smallbird", "Smallbird")
                .WithText("<hiddencard=dstmod.smallishTallbird>".Process())
                .SetSprites("SmallBird.png", "Wendy_BG.png")
                .SetStats(3, 2, 6)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[3]
                        {
                            SStack("On Counter Turn Summon Smallish Tallbird", 1),
                            SStack("Destroy Self After Counter Turn", 1),
                            SStack("Trigger When Ally Is Hit", 1),
                        };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("smallishTallbird", "Smallish Tallbird")
                .WithText("<hiddencard=dstmod.tallbird>".Process())
                .SetSprites("SmallishBird.png", "Wendy_BG.png")
                .SetStats(5, 4, 6)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[3]
                        {
                            SStack("On Counter Turn Summon Tallbird", 1),
                            SStack("Destroy Self After Counter Turn", 1),
                            SStack("Trigger When Ally Is Hit", 1),
                        };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("tallbird", "Tallbird")
                .SetSprites("TallBird.png", "Wendy_BG.png")
                .SetStats(8, 3, 4)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 1), SStack("Trigger When Ally Is Hit", 1) };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Smallbird")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("smallbird");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Smallbird")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Smallbird");
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Smallish Tallbird")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("smallishTallbird");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Smallish Tallbird")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Smallish Tallbird");
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Tallbird")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("tallbird");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Tallbird")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Tallbird");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Counter Turn Summon Smallbird")
                .WithText("<keyword=dstmod.growth>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Smallbird");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Counter Turn Summon Smallish Tallbird")
                .WithText("<keyword=dstmod.growth1>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Smallish Tallbird");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedWithCounterTurn>("On Counter Turn Summon Tallbird")
                .WithText("<keyword=dstmod.growth2>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedWithCounterTurn>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApplyWhenOnCounterTurn = TryGet<StatusEffectData>("Instant Summon Tallbird");
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("growth")
                .WithTitle("Growth")
                .WithShowName(true)
                .WithDescription("When <keyword=counter> reaches 0 summon <card=dstmod.smallbird> then destroy self".Process())
                .WithTitleColour(new Color(0.57f, 0.40f, 1.00f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("growth1")
                .WithTitle("Growth I")
                .WithShowName(true)
                .WithDescription("When <keyword=counter> reaches 0 summon <card=dstmod.smallishTallbird> then destroy self".Process())
                .WithTitleColour(new Color(0.57f, 0.40f, 1.00f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("growth2")
                .WithTitle("Growth II")
                .WithShowName(true)
                .WithDescription("When <keyword=counter> reaches 0 summon <card=dstmod.tallbird> then destroy self".Process())
                .WithTitleColour(new Color(0.57f, 0.40f, 1.00f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }
}
