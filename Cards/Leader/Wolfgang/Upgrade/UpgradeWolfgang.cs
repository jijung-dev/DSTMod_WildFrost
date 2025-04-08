using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class UpgradeWolfgang : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("gymUpgrade", "Mighty Gym Upgrade")
                .WithText("Increase <keyword=dstmod.mightiness> cap to 15 stacks".Process())
                .SetStats(null, null, 0)
                .SetCardSprites("GymUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Gym", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("gembellUpgrade", "Gembells Upgrade")
                .WithText("Gain <card=dstmod.firebell> and <card=dstmod.icebell> and <card=dstmod.gembell>".Process())
                .SetStats(null, null, 0)
                .SetCardSprites("GembellUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Gembell", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("whistleUpgrade", "Coaching Whistle Upgrade")
                .WithText(
                    "<card=dstmod.wolfgang> now increase max <keyword=attack> to all allies instead but increase <card=dstmod.wolfgang> max <keyword=counter> by 3".Process()
                )
                .SetStats(null, null, 0)
                .SetCardSprites("WhistleUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Whistle", 1) };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeGym>("Upgrade Gym"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeGembell>("Upgrade Gembell"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeWhistle>("Upgrade Whistle"));
    }
}
