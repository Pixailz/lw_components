using PixLogicWorldComponents.Shared.Config;
using PixLogicWorldUtils.Shared.CustomData;

namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface IHexDisplayData : IDisplayConfigurationData
	{
		int Size { get; set; }
	}

	public static class HexDisplayDataInit
	{
		public static void Initialize(this IHexDisplayData data)
		{
			data.BitsPerPixel = 1;
			data.ConfigurationIndex = 0;

			data.Size = CHexDisplay.DefaultSize;
		}
	}
}