using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;

public class BuildingFactory : BuildingBase
{
    public override void CreateCard()
    {
        buildings.AddRange(
            new List<BuildingInstance>
            {
                Create(
                    "scienceMachine",
                    "Science Machine",
                    "ScienceMachine.png",
                    SStack(("On Turn Reduce Counter Allies", 1)),
                    null,
                    new[] { (ResourceRequire.Rock, 1), (ResourceRequire.Wood, 1) },
                    new[] { 0, 0, 5 },
                    false
                ),
                Create(
                    "crockPot",
                    "Crock Pot",
                    "CrockPot.png",
                    null,
                    new[] { ("Can Cook", 1) },
                    new[] { (ResourceRequire.Rock, 2), (ResourceRequire.Wood, 1) },
                    new[] { 0, 0, 0 }
                ),
                Create(
                    "firePit",
                    "Fire Pit",
                    "FirePit.png",
                    SStack(("While Active Freeze Immune To Allies", 1)),
                    null,
                    new[] { (ResourceRequire.Rock, 1), (ResourceRequire.Wood, 1) },
                    new[] { 0, 0, 0 },
                    false
                ),
                Create(
                    "tent",
                    "Tent",
                    "Tent.png",
                    SStack(("On Turn Reduce Sanity Allies", 2), ("On Turn Heal Allies", 1)),
                    null,
                    new[] { (ResourceRequire.Rock, 1), (ResourceRequire.Wood, 2) },
                    new[] { 0, 0, 4 }
                ),
                Create(
                    "trap",
                    "Trap",
                    "Trap.png",
                    SStack(("On Turn Summon Rabbit In Hand", 1)),
                    null,
                    new[] { (ResourceRequire.Rock, 1), (ResourceRequire.Wood, 1) },
                    new[] { 0, 0, 5 },
                    false
                ),
                Create(
                    "iceFlingomatic",
                    "Ice Flingomatic",
                    "IceFlingomatic.png",
                    SStack(("While Active Overheat Immune To Allies", 1), ("On Turn Snow Random Enemies", 3)),
                    null,
                    new[] { (ResourceRequire.Rock, 2), (ResourceRequire.Wood, 1), (ResourceRequire.Gold, 1) },
                    new[] { 0, 0, 5 }
                ),
                Create(
                    "prestihatitator",
                    "Prestihatitator",
                    "Prestihatitator.png",
                    SStack(("While Active Increase Attack To Allies", 4), ("On Turn Apply Sanity Allies", 2)),
                    null,
                    new[] { (ResourceRequire.Rock, 1), (ResourceRequire.Wood, 2), (ResourceRequire.Gold, 1), (ResourceRequire.Rabbit, 1) },
                    new[] { 0, 0, 3 }
                ),
                Create(
                    "potterWheel",
                    "Potter's Wheel",
                    "PotterWheel.png",
                    SStack(("On Turn Summon Statue In Hand", 1), ("Require Rock", 1)),
                    null,
                    new[] { (ResourceRequire.Rock, 2), (ResourceRequire.Wood, 1) },
                    new[] { 0, 0, 8 }
                ),
            }
        );
        base.CreateCard();
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Reduce Counter Allies")
                .WithText("Reduce <keyword=counter> by <{a}> for allies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Apply Sanity Allies")
                .WithText("Apply <keyword=dstmod.sanity> by <{a}> to allies".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Sanity");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                })
        );
        assets.Add(
            StatusCopy("While Active Snow Immune To Allies", "While Active Freeze Immune To Allies")
                .WithText("While active, allies gain immune to <keyword=dstmod.freeze>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Immune To Freeze");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                })
        );
        assets.Add(
            StatusCopy("While Active Snow Immune To Allies", "While Active Overheat Immune To Allies")
                .WithText("While active, allies gain immune to <keyword=dstmod.overheat>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectWhileActiveX>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Immune To Overheat");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Reduce Sanity Allies")
                .WithText("Reduce allies <keyword=dstmod.sanity> by <{a}>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Sanity");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Summon Rabbit In Hand")
                .WithText("Gain <card=dstmod.rabbit>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Instant Gain Rabbit In Hand");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Summon Statue In Hand")
                .WithText("Gain <card=dstmod.statue>".Process())
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Instant Gain Statue In Hand");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Snow Random Enemies")
                .WithText("Apply <{a}><keyword=snow> to a random enemy")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Snow");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.RandomEnemy;
                })
        );
    }
}
