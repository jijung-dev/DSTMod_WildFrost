using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Charms : DataBase
{
    protected override void CreateOther()
    {
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeSanityResist")
                .AddPool("GeneralCharmPool")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/Sanity_Charm.png")
                .WithTitle("Nightmare Amulet")
                .WithText("Gain <keyword=dstmod.sanityresist>".Process())
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintIsUnit>(),
                        new Scriptable<TargetConstraintHasHealth>(),
                    };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Immune To Sanity", 1) };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeOverheatResist")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/Chill_Charm.png")
                .WithTitle("Chilled Amulet")
                .WithText("Gain <keyword=dstmod.overheatresist>".Process())
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.WithPools(DSTMod.Instance.charmWithResource);
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintIsUnit>(),
                        new Scriptable<TargetConstraintHasHealth>(),
                    };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Immune To Overheat", 1) };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeFreezeResist")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/Beefalo_Charm.png")
                .WithTitle("Beefalo Hat")
                .WithText("Gain <keyword=dstmod.freezeresist>".Process())
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.WithPools(DSTMod.Instance.charmWithResource);
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Immune To Freeze", 1) };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeSnowResist")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/Thermal_Charm.png")
                .WithTitle("Thermal Stone")
                .WithText("Gain <keyword=immunetosnow>")
                .WithTier(2)
                .SetEffects(SStack("ImmuneToSnow", 1))
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.WithPools(DSTMod.Instance.charmWithResource);
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradePickAndAxe")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/PickAxe_Charm.png")
                .WithTitle("Pick/Axe")
                .WithText("Gain <keyword=dstmod.pickaxetype> and <keyword=dstmod.axetype> but reduce max <keyword=attack> by 2".Process())
                .WithTier(3)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.WithPools(DSTMod.Instance.charmWithResource);
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Reduce Attack", 2) };
                    data.giveTraits = new CardData.TraitStacks[] { TStack("PickaxeType", 1), TStack("AxeType", 1) };
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintIsUnit>(),
                        new Scriptable<TargetConstraintDoesDamage>(),
                    };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeGoldenPickaxe")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/GoldenPickaxe_Charm.png")
                .WithTitle("Golden Pickaxe")
                .WithText("Gain <keyword=dstmod.pickaxetypenorequired> but reduce max <keyword=attack> by 2".Process())
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.WithPools(DSTMod.Instance.charmWithoutResource);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Temporary Mineable", 1) };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Reduce Attack", 2) };
                    data.giveTraits = new CardData.TraitStacks[] { TStack("PickaxeTypeNoRequired", 1) };
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintIsUnit>(),
                        new Scriptable<TargetConstraintDoesDamage>(),
                    };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeGoldenAxe")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/GoldenAxe_Charm.png")
                .WithTitle("Golden Axe")
                .WithText("Gain <keyword=dstmod.axetypenorequired> but reduce max <keyword=attack> by 2".Process())
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.WithPools(DSTMod.Instance.charmWithoutResource);
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Temporary Mineable", 1) };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Reduce Attack", 2) };
                    data.giveTraits = new CardData.TraitStacks[] { TStack("AxeTypeNoRequired", 1) };
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintIsUnit>(),
                        new Scriptable<TargetConstraintDoesDamage>(),
                    };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeGoldenBlueprint")
                .AddPool("GeneralCharmPool")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/GoldenBlueprintCharm.png")
                .WithTitle("Golden Blueprint")
                .WithText("<keyword=dstmod.blueprint> lose the requirement".Process())
                .WithTier(4)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.WithPools(DSTMod.Instance.charmWithResource);
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintHasTrait>(r => r.trait = TryGet<TraitData>("Blueprint")),
                    };
                    data.scripts = new CardScript[] { new Scriptable<CardScriptRemoveRequirement>() };
                })
        );
    }
	protected override void CreateIcon()
	{
		assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "snow immune icon", statusType: "snowimmune", mod.ImagePath("Icons/Snow_Resist.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.counter)
                .WithTextboxSprite()
                // .WithApplySFX(mod.ImagePath("Heat_Attack.wav"))
                // .WithEffectDamageSFX(mod.ImagePath("Heat_Attack.wav"))
                .WithKeywords(iconKeywordOrNull: "immunetosnow")
        );
	}

}
