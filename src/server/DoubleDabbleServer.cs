using LogicAPI.Server.Components;

using PixLogicWorldUtils.Server;

namespace PixLogicWorldComponents.Server
{
	public class DoubleDabbleServer : LogicComponent
	{
		private void DoDoubleDabble(ulong n)
		{
			Utils.ResetOutput(Outputs);
			for (int i = 1; n > 0; i++, n /= 10)
			{
				Utils.ByteToOutput(Outputs, n % 10, 4, Outputs.Count - (4 * i));
			}
		}

		protected override void DoLogicUpdate()
		{
			ulong n = Utils.InputToByte(Inputs, Inputs.Count);

			DoDoubleDabble(n);
		}
	}
}
