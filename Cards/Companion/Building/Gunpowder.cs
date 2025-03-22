using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Gunpowder : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("gunpowder", "Gunpowder")
                .SetSprites("Gunpowder.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .SetStats(null, null, 0)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    //data.WithPools(mod.unitPool);
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 2),
                        SStack("When Overheat Applied To Self Gain Explode Instead", 1),
                        SStack("When Overheat Applied To Self Kill Self", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("When Shell Applied To Self Gain Spice Instead", "When Overheat Applied To Self Gain Explode Instead")
                .WithText("When <keyword=dstmod.overheat>'d gain <keyword=explode> <5> then kill self".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(data =>
                {
                    data.whenAppliedTypes = new string[] { "dst.overheat" };
                    data.effectToApply = TryGet<StatusEffectData>("Temporary Explode");
                    data.adjustAmount = true;
                    data.addAmount = 4;
                })
        );
        assets.Add(
            StatusCopy("When Shell Applied To Self Gain Spice Instead", "When Overheat Applied To Self Kill Self")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(data =>
                {
                    data.textKey = null;
                    data.whenAppliedTypes = new string[] { "dst.overheat" };
                    data.effectToApply = TryGet<StatusEffectData>("Lose Scrap");
                    data.adjustAmount = true;
                    data.addAmount = 99;
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Explode")
                .WithType("temp.explode")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("Explode");
                })
        );
    }
}
