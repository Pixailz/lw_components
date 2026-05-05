using LogicWorld.Server.Circuitry;

using PixLogicWorldUtils.Server;

using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Server
{
	public class DemultiplexerServer : LogicComponent<IMultiplexerData>
	{
		public override bool	HasPersistentValues => true;

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}

		protected override void DoLogicUpdate()
		{
			ulong data = Utils.InputToByte(Inputs,
				this.Data.DataWidth,
				this.Data.SelectorWidth
			);
			int index = (int)Utils.InputToByte(Inputs,
				this.Data.SelectorWidth
			);
			Utils.ResetOutput(Outputs);
			Utils.ByteToOutput(Outputs,
				data,
				this.Data.DataWidth,
				index * this.Data.DataWidth
			);
		}
	}
}
