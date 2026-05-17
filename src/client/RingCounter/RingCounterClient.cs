using LogicWorld.Rendering.Components;
using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Client
{
	public class RingCounterClient : ComponentClientCode<IRingCounterData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
