using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class TamoShanter : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("tamoShanter", "Tam o\' Shanter")
                .SetStats(null, null, 0)
                .SetSprites("TamoShanter.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithPools("GeneralItemPool")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Sanity", 4) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1), TStack("Consume", 1) };
                })
        );
    }
}
