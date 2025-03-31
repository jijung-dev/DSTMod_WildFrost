using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;

public class UpgradeSelectMapNode : DataBase
{
    protected override void CreateOther()
    {
        assets.Add(
            mod.NodeCopy("CampaignNodeCurseItems", "CampaignNodeUpgradeSelect")
                .WithZoneName("Upgrade")
                .WithCanEnter(true)
                .WithInteractable(true)
                .WithMustClear(true)
                .WithCanLink(true)
                .WithLetter("^")
                .SubscribeToAfterAllBuildEvent(
                    (CampaignNodeTypeCurseItems data) =>
                    {
                        //MapNode stuff
                        MapNode mapNode = TryGet<CampaignNodeType>("CampaignNodeCurseItems").mapNodePrefab.InstantiateKeepName(); //There's a lot of things in one of these prefabs
                        mapNode.name = mod.GUID + ".UpgradeSelect";
                        data.mapNodePrefab = mapNode;

                        data.curseCards = 0;

                        StringTable uiText = LocalizationHelper.GetCollection("UI Text", SystemLanguage.English);
                        string key = mapNode.name + "Ribbon";
                        uiText.SetString(key, "Wagstaff's Science Machine");
                        mapNode.label.GetComponentInChildren<LocalizeStringEvent>().StringReference = uiText.GetString(key);

                        mapNode.spriteOptions = new Sprite[] { mod.ScaledSprite("Nodes/ScienceMachineNode.png", 175) };
                        mapNode.clearedSpriteOptions = new Sprite[] { mod.ScaledSprite("Nodes/ScienceMachineNode.png", 175) };

                        GameObject nodeObject = mapNode.gameObject;
                        nodeObject.transform.SetParent(DSTMod.prefabHolder.transform);
                    }
                )
        );
    }
}
