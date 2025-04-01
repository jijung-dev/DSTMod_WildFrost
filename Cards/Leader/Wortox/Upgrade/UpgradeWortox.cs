using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class UpgradeWortox : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("soulJar", "Soul Jar Upgrade")
                .WithText("<card=dstmod.soul> and <card=dstmod.souls> is now <keyword=dstmod.cooked>".Process())
                .SetStats(null, null, 0)
                .SetSprites("SoulJar.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Soul Jar", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("immuneSanityUpgarde", "Upgrade")
                .WithText("<card=dstmod.wortox> gain <keyword=dstmod.sanityresist>".Process())
                .SetStats(null, null, 0)
                .SetSprites("ImmuneSanityUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Immune To Sanity", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("knabsack", "Knabsack Upgrade")
                .WithText("<card=dstmod.wortox> gain <Hit All Enemies> but reduce <keyword=attack> by 2".Process())
                .SetStats(null, null, 0)
                .SetSprites("Knabsack.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Knabsack", 1) };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeSoulJar>("Upgrade Soul Jar"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeImmuneToSanity>("Upgrade Immune To Sanity"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeKnabsack>("Upgrade Knabsack"));
    }
}
