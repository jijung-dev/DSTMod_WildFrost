using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Darkness : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("darkness")
                .WithTitle("Darkness")
                .WithShowName(true)
                .WithDescription("On turn kill everything on board, destroy all enemies or bosses to reset <keyword=counter>")
                .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                .WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectTemporaryTrait>("Temporary Darkness")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.trait = TryGet<TraitData>("Darkness");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectDarkness>("Darkness")
                .SubscribeToAfterAllBuildEvent<StatusEffectDarkness>(data =>
                {
                    data.bossFillEffects = new List<StatusEffectData>()
                    {
                        TryGet<StatusEffectData>("Fill Slot Crystal Deerclops"),
                        TryGet<StatusEffectData>("Fill Slot Possesed Varg"),
                        TryGet<StatusEffectData>("Fill Slot Winter Klaus"),
                    };
                    data.resourceFillEffects = TryGet<StatusEffectData>("Fill Board With Resource And Enemies");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillBoardExt>("Fill Board With Resource And Enemies")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillBoardExt>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[]
                    {
                        TryGet<CardData>("damagedRook"),
                        TryGet<CardData>("damagedBishop"),
                        TryGet<CardData>("damagedKnight"),
                        TryGet<CardData>("stalagmite"),
                        TryGet<CardData>("tallStalagmite"),
                        TryGet<CardData>("blueMushtree"),
                        TryGet<CardData>("greenMushtree"),
                        TryGet<CardData>("redMushtree"),
                    };
                    data.spawnBoard = StatusEffectInstantFillBoardExt.Board.Enemy;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Kill All")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("noBuilding"), TryGetConstraint("noChestHealth") };
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies | StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.effectToApply = TryGet<StatusEffectData>("Kill");
                })
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Darkness")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("darkness");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Darkness"), TryGet<StatusEffectData>("On Turn Kill All") };
                })
        );
    }
}
