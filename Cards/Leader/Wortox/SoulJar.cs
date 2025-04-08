using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class SoulJar : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("souls2", "Souls")
                .SetCardSprites("Souls.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Cooked", 1) };
                    data.needsTarget = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[1] { SStack("On Card Played Heal To Allies", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("soul2", "Soul")
                .SetCardSprites("Soul.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Cooked", 1) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 3) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenCardDestroyed>("When Enemy Is Killed Gain Random Soul 2")
                .WithText("When an Enemy is killed, Add <card=dstmod.soul2> or <card=dstmod.souls2> to hand".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(data =>
                {
                    data.descColorHex = "F99C61";
                    data.canBeAlly = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.constraints = new TargetConstraint[] { TryGetConstraint("noChopable"), TryGetConstraint("noMineable") };
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Random Soul In Hand 2");
                })
        );
        assets.Add(
            StatusCopy("Summon Junk", "Summon Soul 2")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("soul2");
                })
        );
        assets.Add(
            StatusCopy("Summon Junk", "Summon Souls 2")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("souls2");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonRandom>("Instant Summon Random Soul In Hand 2")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.Hand;
                    data.randomCards = new StatusEffectSummon[]
                    {
                        TryGet<StatusEffectSummon>("Summon Soul 2"),
                        TryGet<StatusEffectSummon>("Summon Souls 2"),
                    };
                })
        );
    }
}
