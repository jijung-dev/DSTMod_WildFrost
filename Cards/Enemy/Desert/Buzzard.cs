using Deadpan.Enums.Engine.Components.Modding;

public class Buzzard : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("buzzard", "Buzzard")
                .SetSprites("Buzzard.png", "Wendy_BG.png")
                .SetTraits(TStack("Aimless", 1))
                .SetStats(5, 2, 4)
                .SetStartWithEffect(SStack("MultiHit", 1))
                .WithCardType("Enemy")
                .WithValue(2 * 50)
        );
    }
}
