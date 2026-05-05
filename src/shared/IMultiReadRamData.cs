namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface IMultiReadRamData
	{
		int	DataWidth { get; set; }
		int	AddressWidth { get; set; }
		int		ReadNumber { get; set; }

		byte[]	Memory { get; set; }
		byte	State { get; set; }
		byte[]	ClientIncomingData { get; set; }
	}

	public static class MultiReadRamDataInit
	{
		public static void Initialize(this IMultiReadRamData Data)
		{
			Data.State = 0;
			Data.ClientIncomingData = [];
			Data.Memory = [];
		}
	}
}
