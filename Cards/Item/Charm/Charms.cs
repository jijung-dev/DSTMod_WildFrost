using Deadpan.Enums.Engine.Components.Modding;

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
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Immune To Sanity", 1) };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeOverheatResist")
                .AddPool("GeneralCharmPool")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/Chill_Charm.png")
                .WithTitle("Chilled Amulet")
                .WithText("Gain <keyword=dstmod.overheatresist>".Process())
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Immune To Overheat", 1) };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeFreezeResist")
                .AddPool("GeneralCharmPool")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/Beefalo_Charm.png")
                .WithTitle("Chilled Amulet")
                .WithText("Gain <keyword=dstmod.freezeresist>".Process())
                .WithTier(2)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Immune To Freeze", 1) };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradeSnowResist")
                //.AddPool("GeneralCharmPool")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/Thermal_Charm.png")
                .WithTitle("Thermal Stone")
                .WithText("Gain <keyword=immunetosnow>")
                .WithTier(2)
                .SetEffects(SStack("ImmuneToSnow", 1))
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.targetConstraints = new TargetConstraint[] { new Scriptable<TargetConstraintIsUnit>() };
                })
        );
        assets.Add(
            new CardUpgradeDataBuilder(mod)
                .Create("CardUpgradePickAndAxe")
                .AddPool("GeneralCharmPool")
                .WithType(CardUpgradeData.Type.Charm)
                .WithImage("Icons/PickAxe_Charm.png")
                .WithTitle("Pick/Axe")
                .WithText("Gain <keyword=dstmod.pickaxetype> and <keyword=dstmod.axetype> but reduce max <keyword=attack> by 2".Process())
                .WithTier(4)
                .SubscribeToAfterAllBuildEvent(data =>
                {
                    data.effects = new CardData.StatusEffectStacks[] { SStack("Reduce Attack", 2) };
                    data.giveTraits = new CardData.TraitStacks[] { TStack("PickaxeType", 1), TStack("AxeType", 1) };
                    data.targetConstraints = new TargetConstraint[]
                    {
                        new Scriptable<TargetConstraintIsUnit>(),
                        new Scriptable<TargetConstraintDoesDamage>(),
                    };
                })
        );
    }
}
