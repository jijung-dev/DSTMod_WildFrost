using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class Other : DataBase
{
    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectInstantReduceMaxHealthSafe>("Reduce Max Health Safe"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectDestroySelfAfterCounterTurn>("Destroy Self After Counter Turn"));
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectStealth>("StealthSafe"));
        assets.Add(
            StatusCopy("Hit All Enemies", "Hit All Units")
                .WithText("Hit All Units On Board")
                .SubscribeToAfterAllBuildEvent<StatusEffectChangeTargetMode>(data =>
                {
                    data.targetMode = ScriptableObject.CreateInstance<TargetModeAllUnit>();
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod).Create<StatusEffectResistX>("ByPassConstraint").WithType("dstmod.ByPass")
        );
    }
}
