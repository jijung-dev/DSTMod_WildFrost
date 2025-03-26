using Deadpan.Enums.Engine.Components.Modding;

public class SewingKit : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("sewingKit", "Sewing Kit")
                .SetSprites("SewingKit.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithValue(30)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.itemPool);
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("clunkerOnly") };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Instant Add Scrap", 1) };
                })
        );
    }
}
