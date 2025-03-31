using Deadpan.Enums.Engine.Components.Modding;

public class Lavae : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("lavae", "Lavae")
                .SetSprites("Lavae.png", "Wendy_BG.png")
                .SetStats(5, 2, 3)
                .WithCardType("Enemy")
                .WithValue(2 * 50)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("When Destroyed Apply Overheat To Enemies", 3),
                        SStack("When Destroyed Apply Spice To DFly", 2),
                        SStack("Immune To Summoned", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("When Destroyed Apply Ink To RandomEnemy", "When Destroyed Apply Overheat To Enemies")
                .WithText("When destroyed, apply <{a}><keyword=dstmod.overheat> to enemies".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.targetMustBeAlive = false;
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Enemies;
                    data.effectToApply = TryGet<StatusEffectData>("Overheat");
                })
        );
        assets.Add(
            StatusCopy("When Destroyed Apply Ink To Allies", "When Destroyed Apply Spice To DFly")
                .WithText("then apply <{a}><keyword=spice> to <card=dstmod.dragonfly>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDestroyed>(data =>
                {
                    data.applyConstraints = new TargetConstraint[] { TryGetConstraint("dflyOnly") };
                    data.effectToApply = TryGet<StatusEffectData>("Spice");
                })
        );
        assets.Add(
            StatusCopy("Instant Summon Fallow", "Instant Summon Lavae")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantSummon>(data =>
                {
                    data.targetSummon = TryGet<StatusEffectSummon>("Summon Lavae");
                })
        );
        assets.Add(
            StatusCopy("Summon Fallow", "Summon Lavae")
                .SubscribeToAfterAllBuildEvent<StatusEffectSummon>(data =>
                {
                    data.setCardType = TryGet<CardType>("Enemy");
                    data.summonCard = TryGet<CardData>("lavae");
                })
        );
    }
}
