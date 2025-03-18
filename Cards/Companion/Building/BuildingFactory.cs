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
                    SStack(("On Turn Reduce Counter For Allies", 1)),
                    null,
                    new[] { (ResourceRequire.Rock, 1), (ResourceRequire.Wood, 1) },
                    new[] { 0, 0, 5 }
                ),
                Create(
                    "crockPot",
                    "Crock Pot",
                    "CrockPot.png",
                    null,
                    new[] { ("Can Cook", 1) },
                    new[] { (ResourceRequire.Rock, 1), (ResourceRequire.Wood, 1) },
                    new[] { 0, 0, 0 }
                ),
            }
        );
        base.CreateCard();
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectApplyXOnTurn>("On Turn Reduce Counter For Allies")
                .WithText("Reduce <keyword=counter> by <{a}> for allies")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnTurn>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Reduce Counter");
                    data.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                })
        );
    }
}
