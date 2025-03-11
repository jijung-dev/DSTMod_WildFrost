using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Hammer : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("hammer", "Hammer")
                .SetStats(null, null, 0)
                .SetSprites("Hammer.png", "Wendy_BG.png")
                .WithCardType("Item")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1), TStack("HammerType", 1) };
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Building Health", 1) };

                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("buildingOnly") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Junk", "Summon Hammer")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("hammer");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Junk In Hand", "Instant Summon Hammer In Hand")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Hammer");
                })
        );
    }
}
