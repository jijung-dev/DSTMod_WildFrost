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
                    data.targetMode = new Scriptable<TargetModeAllUnit>();
                })
        );
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectResistX>("ByPassHasHealthConstraint").WithType("dstmod.ByPassHasHealth"));
    }

    protected override void CreateOther()
    {
        //assets.Add(mod.CardTypeCopy("Friendly", "Friendly").WithCanRecall(false));
    }
}
