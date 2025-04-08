using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Campfire : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("campfire", "Campfire")
                .SetCardSprites("Campfire.png", "Wendy_BG.png")
                .WithCardType("Clunker")
                .WithPools("GeneralUnitPool")
                .SetStats(null, null, 0)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Require Wood", 1),
                        SStack("Scrap", 1),
                        SStack("While Active Freeze Immune To FrontAlly", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("While Active Snow Immune To Allies", "While Active Freeze Immune To FrontAlly")
                .WithText("While active, ally in front gain immune to <keyword=dstmod.freeze>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Immune To Freeze");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AllyInFrontOf;
                })
        );
    }
}
