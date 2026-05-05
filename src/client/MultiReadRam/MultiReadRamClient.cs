using LogicWorld.Rendering.Components;
using System.IO;
using System.IO.Compression;

using PixLogicWorldComponents.Shared.CustomData;
using LICC;
using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Client
{
	public class MultiReadRamClient : ComponentClientCode<IMultiReadRamData>, FileLoadable
	{
		public void Load(byte[] filedata, LineWriter writer, bool force)
		{
			if (force || GetInputState(CMultiReadRam.Pin.Load))
			{
				this.Data.ClientIncomingData = Compress(filedata);
				this.Data.State = 1;
				writer.WriteLine($"✓ Loaded {filedata.Length} bytes into RAM");
			}
		}

		public void Erase(LineWriter writer)
		{
			this.Data.ClientIncomingData = null;
			this.Data.State = 2;
			writer.WriteLine($"⚠ Erasing RAM");
		}

		static byte[] Compress(byte[] data)
		{
			MemoryStream output = new MemoryStream();
			using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
			{
				dstream.Write(data, 0, data.Length);
			}
			return output.ToArray();
		}

		protected override void SetDataDefaultValues()
		{
			this.Data.Initialize();
		}
	}
}
