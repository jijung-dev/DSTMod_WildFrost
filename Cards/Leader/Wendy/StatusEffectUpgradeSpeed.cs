public class StatusEffectUpgradeSpeed : StatusEffectData, IUpgrade
{
	public void Run()
	{
		if (References.LeaderData.counter > 1)
			References.LeaderData.counter -= 1;
	}
}