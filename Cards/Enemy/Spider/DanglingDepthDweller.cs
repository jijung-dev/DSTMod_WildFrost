using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class DanglingDepthDweller : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("danglingDepthDweller", "Dangling Depth Dweller")
                .SetCardSprites("DanglingDepthDweller.png", "Wendy_BG.png")
                .SetStats(6, 2, 4)
                .WithCardType("Enemy")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("MultiHit", 1),
                        SStack("Gain Monster Meat When Destroyed", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Dangling Depth Dweller")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Dangling Depth Dweller");
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Dangling Depth Dweller")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("danglingDepthDweller");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenHit>("When Hit Summon Dangling Depth Dweller")
                .WithText("When hit, summon <card=dstmod.danglingDepthDweller>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Dangling Depth Dweller");
                })
        );
    }
}
