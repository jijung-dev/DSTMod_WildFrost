using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class CampaignNodeTypeBlueprint : CampaignNodeTypeEvent
{
    public override IEnumerator SetUp(CampaignNode node)
    {
        node.data = new Dictionary<string, object>();
        yield return null;

        List<CardData> list = new List<CardData>()
        {
            DSTMod.Instance.TryGet<CardData>("crockPotBlueprint"),
            DSTMod.Instance.TryGet<CardData>("scienceMachineBlueprint"),
            DSTMod.Instance.TryGet<CardData>("crockPotBlueprint"),
        };
        List<CardData> list2 = new List<CardData>();

        node.data = new Dictionary<string, object>
        {
            { "cards", ToSaveCollectionOfNames(list) },
            { "curses", ToSaveCollectionOfNames(list2) },
            { "analyticsEventSent", false },
        };
    }

    // public override IEnumerator Run(CampaignNode node)
    // {
    // 	//More code here later
    // 	node.SetCleared();       //Closes the node
    // 	References.Map.Continue(); //Allows player to move on to adjacent nodes
    // 	return base.Run(node);   //Placeholder. Does nothing.
    // }

    //Called on the continue run screen. Returning true makes the run unable to continue.
    public override bool HasMissingData(CampaignNode node)
    {
        string[] saveCollection = node.data.GetSaveCollection<string>("cards");
        if (!MissingCardSystem.HasMissingData(saveCollection))
        {
            return MissingCardSystem.HasMissingData(saveCollection.Where((string a) => a != null));
        }

        return true;
    }

    public override IEnumerator Populate(CampaignNode node)
    {
        EventRoutineCurseItems eventRoutineCurseItems = Object.FindObjectOfType<EventRoutineCurseItems>();
        eventRoutineCurseItems.node = node;
        yield return eventRoutineCurseItems.Populate();
    }

    public static SaveCollection<string> ToSaveCollectionOfNames(IEnumerable<Object> list)
    {
        string[] collection = list.Select((Object a) => (!a) ? null : a.name).ToArray();
        return new SaveCollection<string>(collection);
    }
}
