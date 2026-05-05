using LogicWorld.Rendering.Components;

using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Client
{
	public class MultiplexerClient : ComponentClientCode<IMultiplexerData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
