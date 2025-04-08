using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Froze : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("froze")
                .WithTitle("Froze")
                .WithShowName(true)
                .WithDescription("Can't attack. Can be remove by <keyword=tgestudio.wildfrost.dstmod.overheat>")
                .WithTitleColour(new Color(0.60f, 0.81f, 0.98f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Froze")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("froze");
                    data.effects = new StatusEffectData[] { TryGet<StatusEffectData>("Froze") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectTemporaryTrait>("Temporary Froze")
                .SubscribeToAfterAllBuildEvent<StatusEffectTemporaryTrait>(data =>
                {
                    data.removeOnDiscard = true;
                    data.trait = TryGet<TraitData>("Froze");
                    data.affectedBySnow = false;
                    data.stackable = false;
                    data.type = "dst.froze";
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                })
        );
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectFroze>("Froze").Subscribe_WithStatusIcon("froze icon"));
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "froze icon", statusType: "dst.froze", mod.ImagePath("Icons/Freeze.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.crown)
                .WithApplySFX(mod.ImagePath("Sounds/Froze_Apply.wav"))
        );
    }
}
