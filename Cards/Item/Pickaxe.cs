using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Pickaxe : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("pickaxe", "Pickaxe")
                .SetStats(null, 2, 0)
                .SetCardSprites("Pickaxe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.WithPools(DSTMod.Instance.itemWithResource);
                        data.traits = new List<CardData.TraitStacks>() { TStack("PickaxeType", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("pickaxenorequired", "Pickaxe")
                .SetStats(null, 2, 0)
                .SetCardSprites("Pickaxe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Temporary Mineable", 1) };
                        data.traits = new List<CardData.TraitStacks>() { TStack("PickaxeTypeNoRequired", 1) };
                    }
                )
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("goldenPickaxe", "Golden Pickaxe")
                .SetStats(null, 2, 0)
                .SetCardSprites("GoldenPickaxe.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(
                    delegate(CardData data)
                    {
                        data.traits = new List<CardData.TraitStacks>() { TStack("PickaxeType", 1), TStack("Barrage", 1) };
                    }
                )
        );
    }
}
