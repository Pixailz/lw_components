using JimmysUnityUtilities;
using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface ISevenSegmentData
	{
		Color24 Color { get; set; }
		int Size { get; set; }
	}

	public static class SevenSegmentDataInit
	{
		public static void Initialize(this ISevenSegmentData data)
		{
			data.Color = Color24.AcidGreen;
			data.Size = CSevenSegment.DefaultSize;
		}
	}
}