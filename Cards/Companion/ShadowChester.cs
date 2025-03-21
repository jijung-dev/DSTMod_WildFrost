using Deadpan.Enums.Engine.Components.Modding;

public class ShadowChester : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("shadowChester", "Shadow Chester")
                .SetSprites("ShadowChester.png", "Wendy_BG.png")
                .SetStats(10, null, 0)
                .WithCardType("Friendly")
                .SetStartWithEffect(SStack("When Hit Apply Demonize To Attacker", 1))
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.WithPools(mod.unitPool);
                })
        );
    }
}
