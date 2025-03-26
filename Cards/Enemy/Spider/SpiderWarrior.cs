using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class SpiderWarrior : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("spiderWarrior", "Spider Warrior")
                .SetSprites("SpiderWarrior.png", "Wendy_BG.png")
                .SetStats(6, 3, 3)
                .WithCardType("Enemy")
                .WithValue(4 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Gain Monster Meat When Destroyed", 1) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Frontline", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Spider Warrior")
                .WithText("Summon <card=dstmod.spiderWarrior>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("spiderWarrior");
                })
        );
    }
}
