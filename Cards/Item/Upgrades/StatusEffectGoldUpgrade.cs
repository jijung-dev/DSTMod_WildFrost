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
        int pick = 0;
        int axe = 0;
        foreach (var item in deck)
        {
            if (item.name == DSTMod.Instance.TryGet<CardData>("pickaxe").name)
            {
                pick++;
                deck.Remove(item);
            }
            if (item.name == DSTMod.Instance.TryGet<CardData>("axe").name)
            {
                axe++;
                deck.Remove(item);
            }
        }
        for (int i = 0; i < pick; i++)
        {
            deck.Add(DSTMod.Instance.TryGet<CardData>("goldenPickaxe"));
        }
        for (int i = 0; i < axe; i++)
        {
            deck.Add(DSTMod.Instance.TryGet<CardData>("goldenAxe"));
        }
    }
}
