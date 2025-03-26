using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class Drop : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("drop")
                .WithDescription("\"Drop\" when destroyed summoned card to your hand")
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
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("drop2")
                .WithDescription("\"Drop\" When destroyed gain resources in Chest")
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
