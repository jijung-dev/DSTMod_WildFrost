
public class StatusEffectUpgradeSlow : StatusEffectData, IUpgrade
{
	public void Run()
	{
		References.LeaderData.counter -= 2;
		References.LeaderData.attackEffects = Ext.RemoveAttackEffect("Instant Gain Handy Remote");
		References.LeaderData.attackEffects = Ext.AddAttackEffect("Instant Gain Handy Remote Slow", 1);
	}
}