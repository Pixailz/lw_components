using PixLogicWorldUtils.Shared;

namespace PixLogicWorldComponents.Shared.CustomData
{
	public interface IDummyData
	{
		ulong	Value { get; set; }
	}

	public static class DummyDataInit
	{
		public static void Initialize(this IDummyData data)
		{
			data.Value = 0;
		}
	}
}
