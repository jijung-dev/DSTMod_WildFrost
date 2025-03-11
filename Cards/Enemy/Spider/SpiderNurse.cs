using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class SpiderNurse : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spiderNurse", "Spider Nurse")
                .SetSprites("SpiderNurse.png", "Wendy_BG.png")
                .SetStats(5, 1, 3)
                .SetStartWithEffect(SStack("On Turn Heal Allies", 1))
                .WithCardType("Enemy")
                .WithValue(4 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 2) };
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
