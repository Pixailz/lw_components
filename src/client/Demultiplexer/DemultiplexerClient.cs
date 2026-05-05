using LogicWorld.Rendering.Components;

using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Client
{
	public class DemultiplexerClient : ComponentClientCode<IDemultiplexerData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
