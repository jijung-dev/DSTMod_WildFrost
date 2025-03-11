using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Resource : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("gold")
                .WithTitle("Gold")
                .WithShowName(false)
                .WithDescription("Use to summon <keyword=tgestudio.wildfrost.dstmod.building>")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );

        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("rock")
                .WithTitle("Rock")
                .WithShowName(false)
                .WithDescription("Use to summon <keyword=tgestudio.wildfrost.dstmod.building>")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );

        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("wood")
                .WithTitle("Wood")
                .WithShowName(false)
                .WithDescription("Use to summon <keyword=tgestudio.wildfrost.dstmod.building>")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectResistX>("Gold").Subscribe_WithStatusIcon("gold icon"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectResistX>("Rock").Subscribe_WithStatusIcon("rock icon"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectResistX>("Wood").Subscribe_WithStatusIcon("wood icon"));
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "resource icon", statusType: "dst.resource", mod.ImagePath("Icons/Resource.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithWhenHitSFX(mod.ImagePath("Rock_Apply.wav"))
        );
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "gold icon", statusType: "dst.gold", mod.ImagePath("Icons/Gold.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithWhenHitSFX(mod.ImagePath("Rock_Apply.wav"))
                .WithKeywords(iconKeywordOrNull: "gold")
        );
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "rock icon", statusType: "dst.rock", mod.ImagePath("Icons/Rock.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithWhenHitSFX(mod.ImagePath("Rock_Apply.wav"))
                .WithKeywords(iconKeywordOrNull: "rock")
        );
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "wood icon", statusType: "dst.wood", mod.ImagePath("Icons/Wood.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0f, 0f, 0f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithWhenHitSFX(mod.ImagePath("Rock_Apply.wav"))
                .WithKeywords(iconKeywordOrNull: "wood")
        );
    }
}
