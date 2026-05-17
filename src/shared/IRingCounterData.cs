using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface IRingCounterData
	{
		int	Size { get; set; }
		int NextStage {get; set; }
	}

	public static class RingCounterDataInit
	{
		public static void Initialize(this IRingCounterData data)
		{
			data.Size = CRingCounter.DefaultSize;
			data.NextStage = 0;
		}
	}
}
