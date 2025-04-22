using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Wendy : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("wendy", "Wendy")
                .SetLeaderSprites("Wendy.png", "Wendy_BG.png")
                .SetStats(8, 2, 3)
                .WithCardType("Leader")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Mourning Glory", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Abigail Destroyed Mourning Glory", 1) };
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
            StatusCopy("Summon Fallow", "Summon Abigail Enemy")
                .WithText("Summon <card=dstmod.abigail>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("abigailEnemy");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Abigail Enemy")
                .WithText("Summon <card=dstmod.abigail>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.InFrontOf;
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Abigail Enemy");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXWhenDeployed>("When Deployed Summon Abigail Enemy")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Instant Summon Abigail Enemy");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
    }

    protected override void CreateFinalSwapAsset()
    {
        var scripts = new List<CardScript>
        {
            new Scriptable<CardScriptAddPassiveEffect>(r =>
            {
                r.effect = TryGet<StatusEffectData>("When Deployed Summon Abigail Enemy");
                r.countRange = new Vector2Int(1, 1);
            }),
        };
        finalSwapAsset = (TryGet<CardData>("wendy"), scripts.ToArray());
    }
}
