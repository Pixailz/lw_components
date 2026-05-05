using LogicWorld.Server.Circuitry;

using PixLogicWorldUtils.Server;

using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Server
{
	public class MultiplexerFastServer : LogicComponent<IMultiplexerData>
	{
		public override bool	HasPersistentValues => true;

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
			this.Data.SelectorWidth = CodeInfoInts[0];
		}

		private int	previousSelectorId = -1;
		private int previousDataWidth = 0;

		protected override void DoLogicUpdate()
		{
			int selector_id = (int)Utils.InputToByte(Inputs,
				this.Data.SelectorWidth
			);
			if (this.Data.DataWidth  != this.previousDataWidth)
				this.previousSelectorId = -1;
			else if (selector_id == this.previousSelectorId)
				return ;

			this.reLink(selector_id);

			this.previousSelectorId = selector_id;
			this.previousDataWidth = this.Data.DataWidth;
		}

		private void reLink(int selector_id)
		{
			// Logger.Info($"Selector Width {this.Data.SelectorWidth} Data Width {this.Data.DataWidth}, len Inputs {Inputs.Count}");

			int indexOut = this.Data.SelectorWidth;
			int indexInNew = indexOut + this.Data.DataWidth + (this.Data.DataWidth * selector_id);
			int indexInOld = indexOut + this.Data.DataWidth + (this.Data.DataWidth * this.previousSelectorId);

			// Logger.Info($"Index Out {indexOut}, Index In New {indexInNew}, Index In Old {indexInOld}");

			for (int i = 0; i < this.Data.DataWidth; i++)
			{
				int iinn = indexInNew + i;
				int iino = indexInOld + i;
				int iout = indexOut + i;
				if (this.previousSelectorId != -1)
				{
					// Logger.Info($"Remove {iino} -> {iout}");
					this.Inputs[iino].RemoveOneWayPhasicLinkTo(
						this.Inputs[iout]
					);
				}
				this.Inputs[iinn].AddOneWayPhasicLinkTo(
					this.Inputs[iout]
				);
				// Logger.Info($"Adding {iinn} -> {iout}");
			}
		}
	}
}
