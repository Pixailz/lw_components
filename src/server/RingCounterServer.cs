using LICC;
using LogicWorld.Server.Circuitry;
using PixLogicWorldComponents.Shared.Config;
using PixLogicWorldComponents.Shared.CustomData;
using PixLogicWorldUtils.Server;

namespace PixLogicWorldComponents.Server
{
	public class RingCounterServer : LogicComponent<IRingCounterData>
	{
		protected override void Initialize()
		{
			setNextStage();
		}

		private void setNextStage()
		{
			Utils.ResetOutput(Outputs);
			Outputs[this.Data.NextStage].On = true;
		}

		protected override void DoLogicUpdate()
		{
			if (Inputs[CRingCounter.Pin.Reset].On)
			{
				this.Data.NextStage = 0;
			}
			else if (Inputs[CRingCounter.Pin.Enable].On)
			{
				this.Data.NextStage++;
				if (this.Data.NextStage == this.Data.Size)
					this.Data.NextStage = 0;
			}
			setNextStage();
			QueueLogicUpdate();
		}

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
