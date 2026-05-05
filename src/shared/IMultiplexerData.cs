using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface IMultiplexerData
	{
		int	DataWidth { get; set; }
		int SelectorWidth {get; set; }
	}

	public static class MultiplexerDataInit
	{
		public static void Initialize(this IMultiplexerData data)
		{
			data.DataWidth = CMultiplexer.DefaultDataWidth;
			data.SelectorWidth = CMultiplexer.DefaultSelectorWidth;
		}
	}
}
