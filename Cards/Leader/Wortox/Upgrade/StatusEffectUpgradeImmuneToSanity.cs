
public class StatusEffectUpgradeImmuneToSanity : StatusEffectData, IUpgrade
{
	public void Run()
	{
		References.LeaderData.startWithEffects = Ext.AddStartEffect("Immune To Sanity", 1);
	}
}