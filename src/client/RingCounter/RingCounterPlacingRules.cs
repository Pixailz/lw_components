using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using LogicAPI.Data;
using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Client
{
	public class RingCounterPlacingRules : DynamicPlacingRulesGenerator<int>
	{
		protected override int GetIdentifierFor(ComponentData componentData)
			=> 0;

		protected override PlacingRules GeneratePlacingRulesFor(int identifier)
		{
			return new PlacingRules
			{
				AllowFineRotation = true,

				FlippingPointHeight = 0.50f * CRingCounter.BlockHeight,
			};
		}
	}
}
