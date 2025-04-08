using Deadpan.Enums.Engine.Components.Modding;

public class FireStaff : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("fireStaff", "Fire Staff")
                .SetCardSprites("FireStaff.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SetTraits(TStack("Consume", 1), TStack("Aimless", 1))
                .WithPools("GeneralItemPool")
                .WithValue(50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("MultiHit", 4) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Overheat", 2) };
                })
        );
    }
}
