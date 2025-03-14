using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class Hound : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("hound", "Hound")
                .SetSprites("Hound.png", "Wendy_BG.png")
                .SetStats(4, 1, 3)
                .SetStartWithEffect(SStack("MultiHit", 1))
                .WithCardType("Enemy")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Monster", 1), TStack("Wild", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Hound")
                .WithText("Summon <card=dstmod.hound>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("hound");
                })
        );
    }
}
