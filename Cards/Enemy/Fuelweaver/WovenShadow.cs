using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class WovenShadow : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("skullWovenShadow", "Skull Woven Shadow")
                .SetCardSprites("SkullWovenShadow.png", "Wendy_BG.png")
                .WithCardType("Enemy")
                .SetStats(1, null, 4)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Counter Turn Heal Ancient Fuelweaver", 4),
                        SStack("Destroy Self After Counter Turn", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("handWovenShadow", "Hand Woven Shadow")
                .SetCardSprites("HandWovenShadow.png", "Wendy_BG.png")
                .WithCardType("Enemy")
                .SetStats(1, null, 4)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Counter Turn Heal Ancient Fuelweaver", 4),
                        SStack("Destroy Self After Counter Turn", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("unseenHand", "Unseen Hand")
                .SetCardSprites("UnseenHand.png", "Wendy_BG.png")
                .WithCardType("Enemy")
                .SetStats(1, null, 0)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("While Active Apply Shadow Shield To Fuelweaver", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Counter Turn Heal Ancient Fuelweaver")
                .WithText("Restore <{a}><keyword=health> to <card=tgestudio.wildfrost.dstmod.ancientFuelweaver> then destroy self")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Heal");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("fuelweaverOnly") };
                })
        );

        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectResource>("Shadow Shield")
                .SubscribeToAfterAllBuildEvent<StatusEffectResource>(data =>
                {
                    data.allowedCards = new TargetConstraint[] { TryGetConstraint("fuelweaverOnly") };
                })
        );
        assets.Add(
            StatusCopy("Temporary Aimless", "Temporary Shadow Shield")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("Shadow Shield");
                })
        );
        assets.Add(
            StatusCopy("While Active Increase Attack To Allies (No Desc)", "While Active Apply Shadow Shield To Fuelweaver")
                .WithText(
                    "While active, apply <keyword=tgestudio.wildfrost.dstmod.shadowshield> to <card=tgestudio.wildfrost.dstmod.ancientFuelweaver>"
                )
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Temporary Shadow Shield");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("fuelweaverOnly") };
                })
        );
    }
}
