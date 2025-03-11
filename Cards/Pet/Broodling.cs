using Deadpan.Enums.Engine.Components.Modding;

public class Broodling : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("broodling", "Broodling")
                .IsPet("", true)
                .SetSprites("Broodling.png", "Wendy_BG.png")
                .SetStats(6, 1, 5)
                .WithCardType("Friendly")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Hit Apply Spice To AlliesInRow", 2),
                        SStack("When Hit Apply Overheat To Attacker", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("When Hit Apply Demonize To Attacker", "When Hit Apply Overheat To Attacker")
                .WithText(", <{a}><keyword=dstmod.overheat> to the attacker".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Overheat");
                })
        );
    }
}
