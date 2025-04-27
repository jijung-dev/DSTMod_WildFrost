using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class SpiderNurse : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spiderNurse", "Spider Nurse")
                .SetCardSprites("SpiderNurse.png", "Wendy_BG.png")
                .SetStats(5, 1, 4)
                .WithCardType("Enemy")
                .WithValue(3 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("Gain Monster Meat When Destroyed", 1),
                        SStack("On Turn Heal Allies", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Spider Nurse")
                .WithText("Summon <card=dstmod.spiderNurse>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("spiderNurse");
                })
        );
    }
}
