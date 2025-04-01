public class StatusEffectUpgradeHusk : StatusEffectData, IUpgrade
{
    public void Run()
    {
        References.LeaderData.startWithEffects = Ext.AddStartEffect("On Turn Apply Teeth To Self", 1);
    }
}
