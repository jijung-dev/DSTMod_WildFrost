using System.Collections.Generic;
using System.Linq;
using DSTMod_WildFrost;
using UnityEngine;

public class StatusEffectGoldUpgrade : StatusEffectData, IUpgrade
{
    List<CardData> deck;

    public void Run()
    {
        deck = References.Player.data.inventory.deck.list;
        deck.AddRange(DSTMod.Instance.DataList<CardData>("goldenPickaxe", "goldenPickaxe", "goldenAxe", "goldenAxe").Select(c => c.Clone()));

        deck.RemoveAllWhere(r =>
            r.name == DSTMod.Instance.TryGet<CardData>("pickaxe").name || r.name == DSTMod.Instance.TryGet<CardData>("axe").name
        );
    }
}
