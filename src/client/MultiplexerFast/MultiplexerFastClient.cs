using LogicWorld.Rendering.Components;

using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Client
{
	public class MultiplexerFastClient : ComponentClientCode<IMultiplexerData>
	{
		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
			this.Data.SelectorWidth = CodeInfoInts[0];
		}
	}
}
