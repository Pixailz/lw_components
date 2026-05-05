using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface IRegData
	{
		ulong	Value { get; set; }
		int	DataWidth { get; set; }
		bool	LoadFromSave { get; set; }
	}

	public static class RegDataInit
	{
		public static void Initialize(this IRegData data)
		{
			data.Value = 0;
			data.DataWidth = CRegister.DefaultDataWidth;
			data.LoadFromSave = true;
		}
	}
}
