using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Statue : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("statue", "Statue")
                .SetSprites("Statue.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .SetStats(null, null, 0)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Scrap", 2), SStack("When Destroyed Gain Rock To Chest", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDestroyedUnNullable>("When Destroyed Gain Rock To Chest")
                .WithText("Drop <{a}><keyword=dstmod.rock>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyedUnNullable>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("drop2") };
                    data.canBeBoosted = false;
                    data.targetMustBeAlive = false;
                    data.effectToApply = TryGet<StatusEffectData>("Rock");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("chestOnly") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantGainCard>("Instant Gain Statue In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantGainCard>(data =>
                {
                    data.cardGain = TryGet<CardData>("statue");
                })
        );
    }
}
