using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class ShadowAligned : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("shadow")
                .WithTitle("Shadow Aligned")
                .WithShowName(true)
                .WithDescription("Can't be recalled, stealth, immune to everything, reduce by 1 when a Boss is killed")
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
                .Create<StatusEffectStealth>("Shadow Health")
                .SubscribeToAfterAllBuildEvent<StatusEffectStealth>(data =>
                {
                    data.type = "dst.shadow";
                    data.preventDeath = true;
                })
                .Subscribe_WithStatusIcon("shadow icon")
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "shadow icon", statusType: "dst.shadow", mod.ImagePath("Icons/ShadowAligned.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(1f, 1f, 1f, 1f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                .WithKeywords(iconKeywordOrNull: "shadow")
        );
    }
}
