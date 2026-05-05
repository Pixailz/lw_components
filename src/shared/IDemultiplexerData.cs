using PixLogicWorldUtils.Shared;
using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface IDemultiplexerData
	{
		int	DataWidth { get; set; }
		int SelectorWidth {get; set; }
	}

	public static class DemultiplexerDataInit
	{
		public static void Initialize(this IDemultiplexerData data)
		{
			data.DataWidth = CDemultiplexer.DefaultDataWidth;
			data.SelectorWidth = CDemultiplexer.DefaultSelectorWidth;
		}
	}
}
