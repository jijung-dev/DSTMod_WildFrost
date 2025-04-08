using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class LuxuryFan : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("luxuryFan", "Luxury Fan")
                .SetStats(null, null, 0)
                .SetCardSprites("LuxuryFan.png", "Wendy_BG.png")
                .WithCardType("Item")
                .WithPools("GeneralItemPool")
                .WithValue(60)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Overheat", 3) };
                    data.traits = new System.Collections.Generic.List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantReduceCertainEffect>("Reduce Overheat")
                .WithText("Reduce <keyword=dstmod.overheat> by <{a}>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(data =>
                {
                    data.effectToReduce = TryGet<StatusEffectData>("Overheat");
                })
        );
    }
}
