using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Varglet : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("varglet", "Varglet")
                .SetSprites("Varglet.png", "Wendy_BG.png")
                .SetStats(6, 2, 5)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("On Counter Turn Summon Hounds", 1),SStack("Gain Monster Meat When Destroyed", 1) };
                    data.traits = new List<CardData.TraitStacks>() {TStack("Wild", 1), TStack("Smackback", 1) };
                })
                .WithCardType("Enemy")
                .WithValue(8 * 50)
        );
    }
}
