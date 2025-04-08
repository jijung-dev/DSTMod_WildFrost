using Deadpan.Enums.Engine.Components.Modding;

public class Torch : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("torch", "Torch")
                .SetCardSprites("Torch.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SetTraits(TStack("Noomlin", 1))
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Overheat", 1) };
                })
        );
    }
}
