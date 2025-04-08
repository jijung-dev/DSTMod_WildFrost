using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class Wendy : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("wendy", "Wendy")
                .SetLeaderSprites("Wendy.png", "Wendy_BG.png")
                .SetStats(8, 2, 3)
                .WithCardType("Leader")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.traits = new List<CardData.TraitStacks>() { TStack("Mourning Glory", 1) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Abigail Destroyed Mourning Glory", 1),
                    };
                    data.createScripts = new CardScript[] { LeaderExt.GiveUpgrade(), LeaderExt.AddRandomHealth(0, 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Abigail")
                .WithText("Summon <card=dstmod.abigail>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("abigail");
                })
        );
    }
}
