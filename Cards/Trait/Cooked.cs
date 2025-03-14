using Deadpan.Enums.Engine.Components.Modding;
using Steamworks.Ugc;
using UnityEngine;

public class Cooked : DataBase
{
	protected override void CreateKeyword()
	{
		assets.Add(
			new KeywordDataBuilder(mod)
				.Create("cooked")
				.WithTitle("Cooked")
				.WithShowName(true)
				.WithDescription("Consume, Free Action")
				.WithTitleColour(new Color(1f, 0.57f, 0.21f))
				.WithNoteColour(new Color(0.65f, 0.65f, 0.65f))
				.WithBodyColour(new Color(1f, 1f, 1f))
				.WithCanStack(false)
		);
	}

	protected override void CreateTrait()
	{
		assets.Add(
			new TraitDataBuilder(mod)
				.Create("Cooked")
				.SubscribeToAfterAllBuildEvent<TraitData>(data =>
				{
					data.keyword = TryGet<KeywordData>("cooked");
					data.effects = new StatusEffectData[]
					{
						TryGet<StatusEffectData>("Free Action"),
						TryGet<StatusEffectData>("Destroy After Use"),
					};
					data.overrides = new TraitData[] { TryGet<TraitData>("Consumable"), TryGet<TraitData>("Food") };
				})
		);

	}
}
