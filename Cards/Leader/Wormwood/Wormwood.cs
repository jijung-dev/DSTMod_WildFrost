using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Wormwood : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("wormwood", "Wormwood")
                .SetLeaderSprites("Wormwood.png", "Wendy_BG.png")
                .WithText("<keyword=dstmod.plant>".Process())
                .SetStats(null, null, 6)
                .WithCardType("Leader")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Teeth", 3),
                        SStack("Bloomness", 1),
                        SStack("ByPassHasHealthConstraint", 1),
                        SStack("On Turn Apply Teeth To Self", 1),
                        SStack("Immune To Heal", 1),
                        SStack("When Overheat Applied To Self Gain Reduce Bloomness Instead", 1),
                        SStack("When Froze Applied To Self Gain Increase Max Counter Instead", 1),
                    };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade() };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("compostWrap", "Compost Wrap")
                .SetCardSprites("CompostWrap.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Bloomness", 3) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wormwoodOnly") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("When Shell Applied To Self Gain Spice Instead", "When Overheat Applied To Self Gain Reduce Bloomness Instead")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedTo>(data =>
                {
                    data.textKey = null;
                    data.whenAppliedTypes = new string[] { "dst.overheat" };
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Bloomness");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenYAppliedToEffect>("When Froze Applied To Self Gain Increase Max Counter Instead")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedToEffect>(data =>
                {
                    data.instead = true;
                    data.whenAppliedToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.whenAppliedEffect = new StatusEffectData[] { TryGet<StatusEffectData>("Temporary Froze") };
                    data.effectToApply = TryGet<StatusEffectData>("Increase Counter");
                    data.adjustAmount = true;
                    data.addAmount = 3;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantReduceCertainEffect>("Reduce Bloomness")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(data =>
                {
                    data.effectToReduce = TryGet<StatusEffectData>("Bloomness");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantIncreaseCounter>("Increase Counter")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantIncreaseCounter>(data =>
                {
                    data.stackable = true;
                })
        );
    }
}
