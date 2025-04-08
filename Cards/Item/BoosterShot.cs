using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class BoosterShot : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("boosterShot", "Booster Shot")
                .SetCardSprites("BoosterShot.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate (CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                        data.attackEffects = new CardData.StatusEffectStacks[1] { SStack("Increase Max Health", 2) };
                    }
                )
        );
    }
}
