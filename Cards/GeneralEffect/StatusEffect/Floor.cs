using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Floor : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("floor")
                .WithTitle("Floor")
                .WithShowName(true)
                .WithDescription("Place <keyword=tgestudio.wildfrost.dstmod.building> here|Can't be recalled, stealth, immune to everything i guess?")
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithNoteColour(new Color(0.88f, 0.33f, 0.96f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }
}
