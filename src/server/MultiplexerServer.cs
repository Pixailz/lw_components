using LogicWorld.Server.Circuitry;

using PixLogicWorldUtils.Server;

using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Server
{
	public class MultiplexerServer : LogicComponent<IMultiplexerData>
	{
		public override bool	HasPersistentValues => true;

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}

		protected override void DoLogicUpdate()
		{
			int selector_id = (int)Utils.InputToByte(Inputs,
				this.Data.SelectorWidth
			);
			ulong selected_data = Utils.InputToByte(Inputs,
				this.Data.DataWidth,
				(this.Data.DataWidth * selector_id) + this.Data.SelectorWidth
			);
			Utils.ByteToOutput(Outputs,
				selected_data,
				this.Data.DataWidth
			);
		}
	}
}
