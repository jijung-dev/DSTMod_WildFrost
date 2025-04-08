using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BlingsUpgrade : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("blingsUpgrade", "Upgrade")
                .WithText("Gain 100 <keyword=blings>".Process())
                .SetStats(null, null, 0)
                .SetSprites("BlingUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Blings Upgrade", 1) };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeBling>("Blings Upgrade"));
    }
}
