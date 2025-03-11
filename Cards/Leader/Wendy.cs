using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Wendy : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("wendy", "Wendy")
                .SetSprites("Wendy.png", "Wendy_BG.png")
                .SetStats(8, 2, 3)
                .WithCardType("Leader")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Mourning Glory", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Deployed Summon Abigail", 1),
                        SStack("When Abigail Destroyed Mourning Glory", 1),
                    };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade(), LeaderExt.AddRandomHealth(0, 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Abigail")
                .WithText("Summon <card=dstmod.abigail>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("abigail");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Abigail")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Abigail");
                })
        );
        assets.Add(
            StatusCopy("When Deployed Summon Wowee", "When Deployed Summon Abigail")
                .WithText("When deployed, summon <card=dstmod.abigail>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Abigail");
                })
        );
    }
}
