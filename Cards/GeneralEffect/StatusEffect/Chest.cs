using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Chest : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("chest")
                .WithTitle("Chest")
                .WithShowName(true)
                .WithDescription("Can't be recalled, stealth, immune to everything i guess?")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithNoteColour(new Color(0.88f, 0.33f, 0.96f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantReduceCertainEffect>("Reduce Chest Health")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(data =>
                {
                    data.effectToReduce = TryGet<StatusEffectData>("Chest Health");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectStealth>("Chest Health")
                .SubscribeToAfterAllBuildEvent<StatusEffectStealth>(data =>
                {
                    data.preventDeath = true;
                })
                .Subscribe_WithStatusIcon("chest icon")
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "chest icon", statusType: "dst.chest", mod.ImagePath("Icons/Chest.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.counter)
                .WithTextboxSprite()
                .WithKeywords(iconKeywordOrNull: "chest")
        );
    }
}
