using Deadpan.Enums.Engine.Components.Modding;
using DSTMod_WildFrost;
using UnityEngine;
using WildfrostHopeMod.VFX;

public class Building : DataBase
{
    protected override void CreateKeyword()
    {
        assets.Add(
            new KeywordDataBuilder(mod)
                .Create("building")
                .WithTitle("Buildings")
                .WithShowName(true)
                .WithDescription(
                    "Can be place on <card=tgestudio.wildfrost.dstmod.floor>, only destroyable by <Hammers>|Can't be recalled, backline, stealth, immune to everything"
                )
                .WithTitleColour(new Color(0.65f, 0.41f, 0.34f))
                .WithNoteColour(new Color(0.88f, 0.33f, 0.96f))
                .WithBodyColour(new Color(1f, 1f, 1f))
                .WithCanStack(false)
        );
    }

    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectInstantReduceCertainEffect>("Reduce Building Health")
                .SubscribeToAfterAllBuildEvent<StatusEffectInstantReduceCertainEffect>(data =>
                {
                    data.effectToReduce = TryGet<StatusEffectData>("Building Health");
                    data.isHit = true;
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectResource>("Building Health")
                .SubscribeToAfterAllBuildEvent<StatusEffectResource>(data =>
                {
                    data.allowedCards = new TargetConstraint[] { TryGetConstraint("hammerOnly") };
                    data.preventDeath = true;
                    data.type = "dst.resource";
                })
                .Subscribe_WithStatusIcon("building icon")
        );
    }

    protected override void CreateTrait()
    {
        assets.Add(
            new TraitDataBuilder(mod)
                .Create("Building")
                .SubscribeToAfterAllBuildEvent<TraitData>(data =>
                {
                    data.keyword = TryGet<KeywordData>("building");
                    data.effects = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Low Priority Position"),
                        TryGet<StatusEffectData>("When Destroyed Summon Floor"),
                        TryGet<StatusEffectData>("Building Health"),
                        TryGet<StatusEffectData>("Immune To Everything"),
                        TryGet<StatusEffectData>("StealthSafe"),
                    };
                })
        );
    }

    protected override void CreateIcon()
    {
        assets.Add(
            new StatusIconBuilder(mod)
                .Create(name: "building icon", statusType: "dst.building", mod.ImagePath("Icons/Building.png"))
                .WithIconGroupName(StatusIconBuilder.IconGroups.health)
                .WithTextboxSprite()
        );
    }
}
