public class StatusEffectUpgradeSoulJar : StatusEffectData, IUpgrade
{
    public void Run()
    {
        References.LeaderData.startWithEffects = Ext.RemoveStartEffect("When Enemy Is Killed Gain Random Soul");
        References.LeaderData.startWithEffects = Ext.AddStartEffect("When Enemy Is Killed Gain Random Soul 2", 1);
    }
}
