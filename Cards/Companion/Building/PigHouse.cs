using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class PigHouse : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("pigHouse", "Pig House")
                .SetCardSprites("PigHouse.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .SetStats(null, null, 8)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Instant Summon Pig", 1) };
                    data.WithPools(DSTMod.Instance.unitWithResource);
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Require Wood", 2),
                        SStack("Require Rock", 2),
                        SStack("Scrap", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Pig")
                .WithText("Summon <card=dstmod.pigman>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Pig");
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Pig")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("pigman");
                })
        );
    }
}
