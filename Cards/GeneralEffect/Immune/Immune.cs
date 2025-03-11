using Deadpan.Enums.Engine.Components.Modding;

public class Immune : DataBase
{
    protected override void CreateStatusEffect()
    {
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmune>("Immune To Summoned")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Temporary Summoned") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmune>("Immune To Overheat")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Overheat") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmune>("Immune To Shroom")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Shroom") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmune>("Immune To Sanity")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Sanity") };
                })
        );
        assets.Add(
            new StatusEffectDataBuilder(mod)
                .Create<StatusEffectImmune>("Immune To Everything")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.immuneTo = new StatusEffectData[]
                    {
                        TryGet<StatusEffectData>("Overheat"),
                        TryGet<StatusEffectData>("Freezing"),
                        TryGet<StatusEffectData>("Froze"),
                        TryGet<StatusEffectData>("Sanity"),
                        TryGet<StatusEffectData>("Weakness"),
                        TryGet<StatusEffectData>("Snow"),
                        TryGet<StatusEffectData>("Pull"),
                        TryGet<StatusEffectData>("Overload"),
                        TryGet<StatusEffectData>("Shroom"),
                        TryGet<StatusEffectData>("Shell"),
                        TryGet<StatusEffectData>("Scrap"),
                        TryGet<StatusEffectData>("Demonize"),
                        TryGet<StatusEffectData>("Frost"),
                        TryGet<StatusEffectData>("Lumin"),
                        TryGet<StatusEffectData>("Spice"),
                        TryGet<StatusEffectData>("MultiHit"),
                        TryGet<StatusEffectData>("Teeth"),
                        TryGet<StatusEffectData>("Haze"),
                        TryGet<StatusEffectData>("Block"),
                        TryGet<StatusEffectData>("Null"),
                        TryGet<StatusEffectData>("Temporary Summoned"),
                    };
                })
        );
    }
}
