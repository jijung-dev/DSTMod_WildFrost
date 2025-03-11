using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class HamBat : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("hamBat", "Ham Bat")
                .SetStats(null, 4, 0)
                .SetSprites("HamBat.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Card Played Reduce Attack", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCardPlayed>("On Card Played Reduce Attack")
                .WithText("Reduce <keyword=attack> by <{a}> when played")
                .FreeModify(
                    delegate(StatusEffectData data)
                    {
                        data.canBeBoosted = false;
                    }
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Attack");
                })
        );
    }
}
