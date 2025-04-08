using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;

public class UpgradeWendy : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("unyieldingDraught", "Unyielding Draught")
                .WithText("<keyword=dstmod.ghostlyelixirs><hiddencard=dstmod.abigail>".Process())
                .SetCardSprites("UnyieldingDraught.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Block", 2) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("abigailOnly") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("distilledVengeance", "Distilled Vengeance")
                .WithText("<keyword=dstmod.ghostlyelixirs><hiddencard=dstmod.abigail>".Process())
                .SetCardSprites("DistilledVengeance.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Teeth", 3) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("abigailOnly") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("nightshadeNostrum", "Nightshade Nostrum")
                .WithText("<keyword=dstmod.ghostlyelixirs><hiddencard=dstmod.abigail>".Process())
                .SetCardSprites("NightshadeNostrum.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Spice", 3) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("abigailOnly") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("vigorMortis", "Vigor Mortis Upgrade")
                .WithText("Reduce <card=dstmod.wendy> max <keyword=counter> by 1".Process())
                .SetCardSprites("VigorMortis.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[] { SStack("Upgrade Vigor Mortis", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("revenantRestorative", "Revenant Restorative")
                .WithText("<keyword=dstmod.ghostlyelixirs><hiddencard=dstmod.abigail>".Process())
                .SetCardSprites("RevenantRestorative.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Heal", 99) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("abigailOnly") };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateItem("ghastlyExperience", "Ghastly Experience")
                .WithText("Reset <keyword=dstmod.mourningglory> to 1".Process())
                .SetCardSprites("GhastlyExperience.png", "Wendy_BG.png")
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Reduce Mourning Glory", 99) };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Consume", 1) };
                    data.targetConstraints = new TargetConstraint[] { TryGetConstraint("wendyOnly") };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(new StatusEffectDataBuilder(mod).Create<StatusEffectUpgradeSpeed>("Upgrade Vigor Mortis"));
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantReduceCertainEffect>("Reduce Mourning Glory")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(data =>
                {
                    data.stackable = true;
                    data.effectToReduce = TryGet<StatusEffectData>("Temporary Mourning Glory");
                })
        );
    }

    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("ghostlyelixirs")
                .WithTitle("Ghostly Elixirs")
                .WithShowName(true)
                .WithDescription("Can only be use on <card=dstmod.abigail>".Process())
                .WithTitleColour(new Color(0.83f, 0.83f, 0.83f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }
}
