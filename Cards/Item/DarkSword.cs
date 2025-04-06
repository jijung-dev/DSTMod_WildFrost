using Deadpan.Enums.Engine.Components.Modding;

public class DarkSword : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("darkSword", "Dark Sword")
                .SetStats(null, 7, 0)
                .SetSprites("DarkSword.png", "Wendy_BG.png")
                .WithPools("GeneralItemPool")
                .WithCardType("Item")
                .WithValue(50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.canPlayOnFriendly = false;
                    data.canPlayOnHand = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Played Apply Sanity To Allies In Row", 4) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCardPlayed>("When Played Apply Sanity To Allies In Row")
                .WithText("Apply <{a}> <keyword=dstmod.sanity> to allies in row".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Sanity");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.AlliesInRow;
                    data.targetMustBeAlive = false;
                })
        );
    }
}
