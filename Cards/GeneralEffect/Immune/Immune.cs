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
                .Create<StatusEffectImmune>("Immune To Shroom")
                .SubscribeToAfterAllBuildEvent<StatusEffectImmune>(data =>
                {
                    data.immuneTo = new StatusEffectData[] { TryGet<StatusEffectData>("Shroom") };
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
                        TryGet<StatusEffectData>("Cleanse"),
                        TryGet<StatusEffectData>("Budge"),
                        TryGet<StatusEffectData>("Boost Effects"),
                        TryGet<StatusEffectData>("Reduce Effects"),
                        TryGet<StatusEffectData>("Increase Effects"),
                        TryGet<StatusEffectData>("Increase Attack"),
                        TryGet<StatusEffectData>("Increase Max Health"),
                        TryGet<StatusEffectData>("Increase Max Counter"),
                        TryGet<StatusEffectData>("Reduce Counter"),
                        TryGet<StatusEffectData>("Reduce Max Counter"),
                        TryGet<StatusEffectData>("Temporary Summoned"),
                        TryGet<StatusEffectData>("Immune To Sanity"),
                        TryGet<StatusEffectData>("Immune To Overheat"),
                        TryGet<StatusEffectData>("Immune To Freeze"),
                    };
                })
        );
    }
}
