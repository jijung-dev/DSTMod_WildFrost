using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class UpgradeWinona : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("slowUpgrade", "Upgrade")
                .WithText("<card=dstmod.winona> reduce max <keyword=counter> by 2 but <card=dstmod.handyRemote> lose <keyword=noomlin>".Process())
                .SetStats(null, null, 0)
                .SetCardSprites("SlowUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Slow", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("2tapeUpgrade", "Upgrade")
                .WithText("Gain 2 <card=dstmod.tape><hiddencard=dstmod.catapult>".Process())
                .SetStats(null, null, 0)
                .SetCardSprites("2TapeUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Tape", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("generator", "Generator")
                .SetCardSprites("Generator.png", "Wendy_BG.png")
                .SetStats(null, null, 8)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 3),
                        SStack("On Turn Reduce Scrap To Self", 1),
                        SStack("On Turn Add Scrap To Catapult", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spotlight", "Spotlight")
                .SetCardSprites("Spotlight.png", "Wendy_BG.png")
                .SetStats(null, null, 5)
                .WithCardType("Clunker")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Scrap", 2),
                        SStack("On Turn Reduce Sanity To Allies In Row", 3),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeSlow>("Upgrade Slow"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeTape>("Upgrade Tape"));
        assets.Add(
            StatusCopy("On Turn Add Attack To Allies", "On Turn Add Scrap To Catapult")
                .WithText("Add <{a}><keyword=scrap> to all <card=dstmod.catapult>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("catapultOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Scrap");
                })
        );
        assets.Add(
            StatusCopy("On Turn Add Attack To Allies", "On Turn Reduce Scrap To Self")
                .WithText("Lose <{a}><keyword=scrap>")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Lose Scrap");
                })
        );
        assets.Add(
            StatusCopy("On Turn Add Attack To Allies", "On Turn Reduce Sanity To Allies In Row")
                .WithText("Reduce <keyword=dstmod.sanity> by <{a}> to allies in row".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AlliesInRow;
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Sanity");
                })
        );
    }
}
