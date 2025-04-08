using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class PickAxeUpgrade : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("pickAxeUpgrade", "Upgrade")
                .WithText("Gain 1 <card=dstmod.pickaxe> and 1 <card=dstmod.axe>".Process())
                .SetStats(null, null, 0)
                .SetCardSprites("PickAxeUpgrade.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("PickAxe Upgrade", 1) };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradePickAxe>("PickAxe Upgrade"));
    }
}
