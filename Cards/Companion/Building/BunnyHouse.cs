using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BunnyHouse : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("bunnyHouse", "Rabbit Hutch")
                .SetCardSprites("BunnyManHouse.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithPools("GeneralUnitPool")
                .SetStats(null, null, 12)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Instant Summon Bunnyman", 1) };
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
            StatusCopy("Instant Summon Fallow", "Instant Summon Bunnyman")
                .WithText("Summon <card=dstmod.bunnyman>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Bunnyman");
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOfOrOtherRow;
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Bunnyman")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("bunnyman");
                })
        );
    }
}
