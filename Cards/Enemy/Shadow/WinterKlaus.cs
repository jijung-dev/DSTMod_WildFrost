using System.Collections.Generic;
using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;

public class WinterKlaus : DataBase
{
    public override void CreateCard()
    {
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("winterKlaus", "Winter Klaus")
                .SetBossSprites("WinterKlaus.png", "Wendy_BG.png")
                .SetStats(40, 5, 4)
                .WithCardType("BossSmall")
                .WithValue(20 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.attackEffects = new CardData.StatusEffectStacks[] { SStack("Freezing", 2) };
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("ImmuneToSnow", 1),
                        SStack("Immune To Freeze", 1),
                        SStack("When Destroyed Kill All Allies", 1),
                        SStack("When Deployed Fill Slot With Red Gem Deer", 1),
                        SStack("When Deployed Fill Slot With Blue Gem Deer", 1),
                    };
                    data.traits = new List<CardData.TraitStacks>() { TStack("Barrage", 1) };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("winterRedGemDeer", "Winter Red Gem Deer")
                .SetCardSprites("WinterRedGemDeer.png", "Wendy_BG.png")
                .SetStats(15, 3, 3)
                .WithCardType("Enemy")
                .WithValue(6 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Turn Apply Spice To Klaus", 4),
                        SStack("When Destroyed Double Klaus Health And Attack", 1),
                        SStack("When Destroyed Red Gem", 1),
                    };
                })
        );
        assets.Add(
            new CardDataBuilder(mod)
                .CreateUnit("winterBlueGemDeer", "Winter Blue Gem Deer")
                .SetCardSprites("WinterBlueGemDeer.png", "Wendy_BG.png")
                .SetStats(15, 3, 3)
                .WithCardType("Enemy")
                .WithValue(6 * 36)
                .SubscribeToAfterAllBuildEvent<CardData>(data =>
                {
                    data.startWithEffects = new CardData.StatusEffectStacks[]
                    {
                        SStack("On Turn Apply Snow To RandomEnemy", 3),
                        SStack("When Destroyed Double Klaus Health And Attack", 1),
                        SStack("When Destroyed Blue Gem", 1),
                    };
                })
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Slot With Red Gem Deer")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Fill Slot Red Gem Deer");
                })
        );
        assets.Add(
            StatusCopy("When Deployed Fill Board (Final Boss)", "When Deployed Fill Slot With Blue Gem Deer")
                .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(data =>
                {
                    data.effectToApply = TryGet<StatusEffectData>("Fill Slot Blue Gem Deer");
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillSlots>("Fill Slot Red Gem Deer")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("winterRedGemDeer") };
                    data.slotID = 12;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillSlots>("Fill Slot Blue Gem Deer")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("winterBlueGemDeer") };
                    data.slotID = 8;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantFillSlots>("Fill Slot Winter Klaus")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantFillSlots>(data =>
                {
                    data.isEnemy = true;
                    data.withCards = new CardData[] { TryGet<CardData>("winterKlaus") };
                    int[] ran = new int[] { 9, 10, 13, 14 };
                    data.slotID = ran[UnityEngine.Random.Range(0, ran.Length)];
                })
        );
    }
}
