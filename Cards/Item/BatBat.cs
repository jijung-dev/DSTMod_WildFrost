using Deadpan.Enums.Engine.Components.Modding;

public class BatBat : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("batBat", "BatBat")
                .SetStats(null, 4, 0)
                .SetSprites("BatBat.png", "Wendy_BG.png")
                .WithPools("GeneralItemPool")
                .WithCardType("Item")
                .WithValue(40)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.canPlayOnFriendly = false;
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("When Played Apply Heal To Random Ally", 3) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnCardPlayed>("When Played Apply Heal To Random Ally")
                .WithText("Restore <{a}> <keyword=health> to random ally")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Heal");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomAlly;
                    data.targetMustBeAlive = false;
                })
        );
    }
}
