using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Spider : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spider", "Spider")
                .SetSprites("Spider.png", "Wendy_BG.png")
                .SetStats(4, 1, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Monster Meat When Destroyed", 1),
                        SStack("MultiHit", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Aimless", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Spider")
                .WithText($"Summon <card=dstmod.spider>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("spider");
                })
        );
    }
}
