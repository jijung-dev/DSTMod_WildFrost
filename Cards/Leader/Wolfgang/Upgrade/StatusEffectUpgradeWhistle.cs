
public class StatusEffectUpgradeWhistle : StatusEffectData, IUpgrade
{
	public void Run()
	{
		References.LeaderData.counter += 3;
		References.LeaderData.startWithEffects = Ext.RemoveStartEffect("On Counter Turn Increase Max Damage");
		References.LeaderData.startWithEffects = Ext.AddStartEffect("On Counter Turn Increase Max Damage All Allies", 1);
	}
}