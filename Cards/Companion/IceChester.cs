using Deadpan.Enums.Engine.Components.Modding;

public class IceChester : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("iceChester", "Ice Chester")
                .SetSprites("IceChester.png", "Wendy_BG.png")
                .SetStats(12, null, 0)
                .WithCardType("Friendly")
                .SetStartWithEffect(SStack("When Hit Apply Snow To Attacker", 2))
        );
    }
}
