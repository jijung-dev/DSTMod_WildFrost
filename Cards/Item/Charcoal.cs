using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Charcoal : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("charcoal", "Charcoal")
                .SetCardSprites("Charcoal.png", "Wendy_BG.png")
                .SetStats(null, null, 0)
                .WithCardType("Item")
                .WithValue(60)
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Double Overheat", 1) };
                        data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1), TStack("Consume", 1) };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Double Spice", "Double Overheat")
                .WithText("Double the target's <keyword=dstmod.overheat>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantDoubleX>(data =>
                {
                    data.statusToDouble = TryGet<StatusEffectData>("Overheat");
                })
        );
    }
}
