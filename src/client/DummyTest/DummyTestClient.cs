using LogicWorld.Rendering.Components;
using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Client
{
	public class DummyTestClient : ComponentClientCode<IDummyData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
