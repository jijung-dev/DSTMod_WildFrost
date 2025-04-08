using Deadpan.Enums.Engine.Components.Modding;

public class FriendFly : DataBase
{
    public override void CreateCard()
    {
        assets.Add(new CardDataBuilder(mod).CreateUnit("chester", "Chester").SetCardSprites("Chester.png", "Wendy_BG.png").SetStats(15, null, 0));
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("eyeBone", "Eye Bone")
                .IsPet("", true)
                .WithCardType("Item")
                .SetCardSprites("EyeBone.png", "Wendy_BG.png")
                .SetTraits(TStack("Consume", 1))
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.playOnSlot = true;
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Summon Chester", 1) };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("Summon Plep", "Summon Chester")
                .WithText("Summon <card=dstmod.chester>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.summonCard = TryGet<CardData>("chester");
                })
        );
    }
}
