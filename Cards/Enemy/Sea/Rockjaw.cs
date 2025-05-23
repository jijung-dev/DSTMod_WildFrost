using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Rockjaw : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("rockjaw", "Rockjaw")
                .SetCardSprites("Rockjaw.png", "Wendy_BG.png")
                .WithText("Dive down <hiddencard=dstmod.rockjawDived>".Process())
                .SetStats(8, 2, 5)
                .WithCardType("Enemy")
                .WithValue(4 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1), TStack("Spark", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Destroyed Gain Rock To Chest", 1),
                        SStack("On Turn Apply Dive Down Rockjaw To Self", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("rockjawDived", "Rockjaw Dived")
                .SetCardSprites("RockjawDived.png", "Wendy_BG.png")
                .WithText("Dive up <hiddencard=dstmod.rockjaw>".Process())
                .SetStats(null, null, 2)
                .WithCardType("Enemy")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Chest Health", 1),
                        SStack("Dive Up Rockjaw", 1),
                        SStack("Building Immune To Everything", 1),
                        SStack("On Turn Apply Reduce Chest Health To Self", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Dive Down Rockjaw")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("divedown") };
                    data.killSelfWhenApplied = true;
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("rockjawDived");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectNextPhaseExt>("Dive Up Rockjaw")
                .SubscribeToAfterAllBuildEvent<StatusEffectNextPhaseExt>(data =>
                {
                    data.preventDeath = true;
                    data.nextPhase = TryGet<CardData>("rockjaw");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Turn Apply Reduce Chest Health To Self")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.doPing = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Chest Health");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCounterTurn>("On Turn Apply Dive Down Rockjaw To Self")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCounterTurn>(data =>
                {
                    data.hiddenKeywords = new KeywordData[] { TryGet<KeywordData>("divedown") };
                    data.doPing = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                    data.effectToApply = TryGet<StatusEffectData>("Dive Down Rockjaw");
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("divedown")
                .WithDescription("\"Dive down\" restore full <keyword=health> and became untargetable")
                .WithTitleColour(new Color(0.2588235f, 0.06666667f, 0.1294118f, 1f))
                .WithBodyColour(new Color(0.4313726f, 0.2f, 0.1921569f, 1f))
                .WithPanelColour(new Color(0.9058824f, 0.8274511f, 0.6784314f, 0.9411765f))
                .WithNoteColour(new Color(0.4313726f, 0.2f, 0.1921569f, 1f))
                .WithIconTint(new Color(1f, 0.792156862745098f, 0.3411764705882353f))
                .WithCanStack(false)
                .WithShowIcon(false)
                .WithShow(false)
                .SubscribeToAfterAllBuildEvent(data => data.panelSprite = TryGet<KeywordData>("Active").panelSprite)
        );
    }
}
