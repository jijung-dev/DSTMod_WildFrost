public class StatusEffectUpgradeKnabsack : StatusEffectData, IUpgrade
{
    public void Run()
    {
        References.LeaderData.startWithEffects = Ext.AddStartEffect("Hit All Enemies", 1);
        References.LeaderData.damage -= 2;
    }
}
