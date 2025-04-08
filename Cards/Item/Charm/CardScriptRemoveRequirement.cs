using System.Linq;
using DSTMod_WildFrost;

public class CardScriptRemoveRequirement : CardScript
{
	public override void Run(CardData target)
	{
		target.startWithEffects = target.startWithEffects.Where(effect => effect.data.type != "dst.require").ToArray();
	}
}