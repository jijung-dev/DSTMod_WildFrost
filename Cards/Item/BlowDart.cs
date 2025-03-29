using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BlowDart : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("blowDart", "BlowDart")
                .SetStats(null, null, 0)
                .SetSprites("BlowDart.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(70)
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Random Snow Fire Shroom", 4) };
                        data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                    }
                )
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantRandom>("Random Snow Fire Shroom")
                .WithText("Apply <{a}> <keyword=snow> or <{a}> <keyword=dstmod.overheat> or <{a}> <keyword=shroom>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantRandom>(data =>
                    data.effects = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Snow"),
                        TryGet<StatusEffectData>("Shroom"),
                        TryGet<StatusEffectData>("Overheat"),
                    }
                )
        );
    }
}
