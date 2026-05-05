using LICC;

namespace PixLogicWorldComponents.Client
{
	public interface FileLoadable
	{
		void Load(byte[] filedata, LineWriter writer, bool force);
		public void Erase(LineWriter writer);
	}
}
