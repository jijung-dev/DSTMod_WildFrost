using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Sanity : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("crawlingHorror", "Crawling Horror")
                .SetSprites("CrawlingHorror.png", "Wendy_BG.png")
                .SetStats(8, 2, 4)
                .WithCardType("Enemy")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Shadow Align", 1) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 2) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("terrorbeak", "Terrorbeak")
                .SetSprites("TerrorBeak.png", "Wendy_BG.png")
                .SetStats(5, 4, 3)
                .WithCardType("Enemy")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Shadow Align", 1) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Sanity", 3) };
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("sanity")
                .WithTitle("Sanity")
                .WithShowName(false)
                .WithDescription("When more than or equal to health summon <Shadow Creature> at the enemy side")
                .WithTitleColour(new Color(0.34f, 0f, 0.63f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(true)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectSanity>("Sanity")
                .SubscribeToAfterAllBuildEvent<StatusEffectSanity>(data =>
                {
                    data.type = "dst.sanity";
                    data.summonRan = TryGet<StatusEffectData>("Instant Summon Random Shadow Creature");
                    data.shadowEnemy = new StatusEffectSummon[2]
                    {
                        TryGet<StatusEffectSummon>("Summon Crawling Horror"),
                        TryGet<StatusEffectSummon>("Summon Terrorbeak"),
                    };
                })
                .Subscribe_WithStatusIcon("sanity icon")
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantSummonRandom>("Instant Summon Random Shadow Creature")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummonRandom>(data =>
                {
                    data.summonPosition = StatusEffectInstantSummon.Position.EnemyRow;
                    data.randomCards = new StatusEffectSummon[]
                    {
                        TryGet<StatusEffectSummon>("Summon Crawling Horror"),
                        TryGet<StatusEffectSummon>("Summon Terrorbeak"),
                    };
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Crawling Horror")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("crawlingHorror");
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Terrorbeak")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("terrorbeak");
                })
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "sanity icon", statusType: "dst.sanity", mod.ImagePath("Icons/Sanity.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithEffectDamageVFX(mod.ImagePath("Icons/Heat_Apply.gif"))
                .WithEffectDamageSFX(mod.ImagePath("Sanity_Apply.wav"), 0.1f)
                .WithApplyVFX(mod.ImagePath("Icons/Sanity_Apply.gif"))
                .WithApplySFX(mod.ImagePath("Sanity_Attack.wav"), 0.1f)
                //.WithApplySFX(ImagePath("Sanity_Attack.wav"), 0.1f)
                .WithKeywords(iconKeywordOrNull: "sanity")
        );
    }
}
