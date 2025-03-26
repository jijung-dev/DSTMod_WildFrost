using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;

public class BlueprintSelectMapNode : DataBase
{
    protected override void CreateOther()
    {
        assets.Add(
            mod.NodeCopy("CampaignNodeCurseItems", "CampaignNodeBlueprintSelect")
                .WithZoneName("Blueprint") //The name of the CampaignNode associated to the map node. Used for special event replacement.
                .WithCanEnter(true) //Needs to be true to be interactable
                .WithInteractable(true) //Needs to be true to be interactable
                .WithMustClear(true) //If you want this node unskippable, replace this line with .WithMustClear(true)
                .WithCanLink(true) //See below.
                .WithLetter("p") //See below.
                .SubscribeToAfterAllBuildEvent(
                    (CampaignNodeTypeCurseItems data) =>
                    {
                        //MapNode stuff
                        MapNode mapNode = TryGet<CampaignNodeType>("CampaignNodeCurseItems").mapNodePrefab.InstantiateKeepName(); //There's a lot of things in one of these prefabs
                        mapNode.name = mod.GUID + ".BlueprintSelect";
                        data.mapNodePrefab = mapNode;

                        data.curseCards = 0;
                        //name + Blueprint
                        data.force = new List<CardData>()
                        {
                            DSTMod.Instance.TryGet<CardData>("trapBlueprint"),
                            DSTMod.Instance.TryGet<CardData>("scienceMachineBlueprint"),
                            DSTMod.Instance.TryGet<CardData>("firePitBlueprint"),
                        };

                        // Campaign campaign = Object.FindObjectOfType<Campaign>();
                        // CharacterRewards characterRewards = campaign.GetComponent<CharacterRewards>();
                        // data.illegalCards = characterRewards.poolLookup["Items"].current.Cast<CardData>().ToArray();

                        StringTable uiText = LocalizationHelper.GetCollection("UI Text", SystemLanguage.English);
                        string key = mapNode.name + "Ribbon";
                        uiText.SetString(key, "Wagstaff's Factory");
                        mapNode.label.GetComponentInChildren<LocalizeStringEvent>().StringReference = uiText.GetString(key);

                        mapNode.spriteOptions = new Sprite[] { mod.ScaledSprite("Icons/BlueprintSelect.png", 150) };
                        mapNode.clearedSpriteOptions = new Sprite[] { mod.ScaledSprite("Icons/BlueprintSelect.png", 150) };

                        GameObject nodeObject = mapNode.gameObject;
                        nodeObject.transform.SetParent(DSTMod.prefabHolder.transform);
                    }
                )
        );
    }
}
