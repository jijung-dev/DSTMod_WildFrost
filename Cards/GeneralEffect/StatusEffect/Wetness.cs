using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Wetness : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("wetness")
                .WithTitle("Wetness")
                .WithShowName(false)
                .WithDescription("Each stack adds 10% chance for card to not play on use")
                .WithTitleColour(new Color(0.00f, 0.37f, 0.68f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(true)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectWetness>("Wetness")
                .SubscribeToAfterAllBuildEvent<StatusEffectWetness>(data =>
                {
                    data.type = "dst.wetness";
                })
                .Subscribe_WithStatusIcon("wetness icon")
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "wetness icon", statusType: "dst.wetness", mod.ImagePath("Icons/Wet.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextColour(new Color(0.02f, 0.02f, 0.10f))
                .WithTextShadow(new Color(0f, 0f, 0f, 0.75f))
                .WithTextboxSprite()
                //.WithEffectDamageVFX(mod.ImagePath("Icons/Heat_Apply.gif"))
                //.WithEffectDamageSFX(mod.ImagePath("Sanity_Apply.wav"), 0.1f)
                //.WithApplyVFX(mod.ImagePath("Icons/Sanity_Apply.gif"))
                //.WithApplySFX(mod.ImagePath("Sanity_Attack.wav"), 0.1f)
                //.WithApplySFX(ImagePath("Sanity_Attack.wav"), 0.1f)
                .WithKeywords(iconKeywordOrNull: "wetness")
        );
    }
}
