using System;
using System.IO;
using System.IO.Compression;

using LogicWorld.Server.Circuitry;

using PixLogicWorldUtils.Server;
using PixLogicWorldUtils.Shared;

using PixLogicWorldComponents.Shared.CustomData;
using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Server
{
	public class MultiReadRamServer : LogicComponent<IMultiReadRamData>
	{
		public override bool	HasPersistentValues => true;

		private ulong			sizeInByte;
		private ulong			totalSizeInByte;

		private int[]			pinsRead;

		private ulong[]		addresses;
		private byte[]			memory = null;

		private bool			isDataDirty;
		private bool			loadFromSave = true;

		private void debug_initialize()
		{
			Logger.Info("Initialised component");
			Logger.Info($"Number of read output {this.Data.ReadNumber}");
			Logger.Info($"Data Width			{this.Data.DataWidth}b");
			Logger.Info($"Address Width		 {this.Data.AddressWidth}b");
			Logger.Info($"Address UpperWidth	{this.sizeInByte / 8}o");
			Logger.Info($"Total Size			{this.totalSizeInByte}o");
		}

		private void	updateAddresses()
		{
			for (int i = 0; i < this.Data.ReadNumber; i++)
			{
				addresses[i] = Utils.InputToByte(
					Inputs,
					this.Data.AddressWidth,
					this.Data.DataWidth +
					CMultiReadRam.Pin.DataStart +
					(this.Data.AddressWidth * i) +
					this.Data.ReadNumber
				);
			}
		}

		public void	updateInputOutput()
		{
			int	newDataWidth = Outputs.Count / this.Data.ReadNumber;
			int	newAddressWidth = (
				Inputs.Count - (
					newDataWidth + CMultiReadRam.Pin.DataStart + this.Data.ReadNumber
				)
			) / this.Data.ReadNumber;

			if (this.Data.DataWidth == newDataWidth && this.Data.AddressWidth == newAddressWidth)
				return ;

			this._initialize(newDataWidth, newAddressWidth);
		}

		protected void _initialize_pins_read()
		{
			int index = this.Data.DataWidth + CMultiReadRam.Pin.DataStart;

			this.pinsRead = new int[this.Data.ReadNumber];
			for (int i = 0; i < this.Data.ReadNumber; i++)
				this.pinsRead[i] = index + i;
		}

		protected void _initialize_address()
		{
			this.addresses = new ulong[this.Data.ReadNumber];
		}

		protected void _initialize_size()
		{
			this.sizeInByte = Utils.UpperWidth(this.Data.DataWidth);
			this.totalSizeInByte = Utils.GetMask(this.Data.AddressWidth) * this.sizeInByte;
			this.totalSizeInByte += this.sizeInByte;
		}

		protected void _initialize_memory()
		{
			this.memory = new byte[this.totalSizeInByte];

		}

		protected void _initialize(int DataWidth, int AddressWidth)
		{
			this.Data.DataWidth = DataWidth;
			this.Data.AddressWidth = AddressWidth;

			this._initialize_pins_read();
			this._initialize_size();
			this._initialize_memory();
			this._initialize_address();

			// this.debug_initialize();
		}

		protected override void SetDataDefaultValues()
		{
			this.Data.ReadNumber = CodeInfoInts[0];
			this.Data.Initialize();
		}

		protected override void Initialize()
		{
			this._initialize_pins_read();
			this._initialize_address();
			this._initialize_size();
			this._initialize_memory();
		}

		private void test()
		{
			this.setData(0xfedcba9876543210, 0);

			// this.byteToOutput(0xfedcba9876543210, 0, this.dataWidth);

			// this.memory[0] = 0x01;
			// this.memory[1] = 0x23;
			// this.memory[2] = 0x45;
			// this.memory[3] = 0x67;
			// this.memory[4] = 0x89;
			// this.memory[5] = 0xab;
			// this.memory[6] = 0xcd;
			// this.memory[7] = 0xef;

			// for (int i = 0; i < 8; i++)
			// {
			// 	Logger.Info($"Memory[{i}]: 0x{this.memory[i]:X2}");
			// }

			var data = this.getData(0);

			Logger.Info($"Data read: 0x{data:X}");

			Utils.ByteToOutput(
				Outputs,
				data,
				this.Data.DataWidth,
				0
			);
		}

		private void debug()
		{
			// this.test();
			Logger.Info("---- INPUTS ----");
			Logger.Info($"Load pin:  {Converter.ToString(Inputs[CMultiReadRam.Pin.Load].On)}");
			Logger.Info($"Write pin: {Converter.ToString(Inputs[CMultiReadRam.Pin.Write].On)}");

			Logger.Info($"Data pin:  0x{Utils.InputToByte(
				Inputs,
				this.Data.DataWidth,
				CMultiReadRam.Pin.DataStart
			):X}");


			for (int r = 0; r < this.Data.ReadNumber; r++)
			{
				Logger.Info($"Read pin {r}: {Converter.ToString(
					Inputs[this.pinsRead[r]].On
				)}");
			}

			for (int r = 0; r < this.Data.ReadNumber; r++)
			{
				Logger.Info($"Address pins {r}: 0x{this.addresses[r]:X}");
			}

			Logger.Info("---- CONFIG ----");
			Logger.Info($"dataWidth: {this.Data.DataWidth}");
			Logger.Info($"addressWidth: {this.Data.AddressWidth}");
			Logger.Info($"sizeInByte: {this.sizeInByte}");
			Logger.Info($"totalSizeInByte: {this.totalSizeInByte}");
			Logger.Info("---- ====== ----");
			Logger.Info("");
		}

		protected override void DoLogicUpdate()
		{
			ulong data;

			this.updateInputOutput();
			this.updateAddresses();
			// this.debug();

			if (Inputs[CMultiReadRam.Pin.Write].On)
			{
				data = Utils.InputToByte(
					Inputs,
					this.Data.DataWidth,
					CMultiReadRam.Pin.DataStart
				);
				setData(data, addresses[0]);
			}
			for (int i = 0; i < this.Data.ReadNumber; i++)
			{
				data = 0;
				if (Inputs[pinsRead[i]].On)
				{
					data = getData(addresses[i]);
				}
				Utils.ByteToOutput(
					Outputs,
					data,
					this.Data.DataWidth,
					this.Data.DataWidth * i
				);
			}
		}

		private void	setData(ulong data, ulong addr)
		{
			ulong data_index = addr * this.sizeInByte;

			for (ulong i = 0; i < this.sizeInByte; i++)
			{
				this.memory[data_index + i] = (byte)(data & 0xFF);
				data >>= 8;
			}
			this.isDataDirty = true;
		}

		private ulong	getData(ulong addr)
		{
			ulong data = 0;
			ulong data_index = addr * this.sizeInByte;

			for (ulong i = this.sizeInByte; i > 0; i--)
			{
				byte byte_data = this.memory[data_index + i - 1];

				data = (data << 8) | byte_data;
			}
			ulong mask = Utils.GetMask(this.Data.DataWidth);

			return data & mask;
		}

		protected override void OnCustomDataUpdated()
		{
			if (Data.State == 2)
			{
				this.memory = new byte[this.totalSizeInByte];
				this.Data.State = 0;
				this.Data.ClientIncomingData = [];
				this.isDataDirty = true;
				Logger.Info("Erasing data from client");
				QueueLogicUpdate();
				return;
			}

			if (this.loadFromSave && this.Data.Memory != null || this.Data.State == 1 && this.Data.ClientIncomingData != null)
			{
				var to_load_from = this.Data.Memory;
				if (this.Data.State == 1)
				{
					Logger.Info("Loading data from client");
					to_load_from = Data.ClientIncomingData;
				}
				else
				{
					Logger.Info("Loading data from save");
				}
				MemoryStream stream = new MemoryStream(to_load_from);
				stream.Position = 0;
				if (this.memory == null)
					_initialize_memory();
				byte[] mem1 = new byte[this.memory.Length];
				try
				{
					DeflateStream decompressor = new DeflateStream(stream, CompressionMode.Decompress);
					int bytesRead;
					int nextStartIndex = 0;
					while((bytesRead = decompressor.Read(mem1, nextStartIndex, mem1.Length - nextStartIndex)) > 0){
						nextStartIndex += bytesRead;
					}

					this.memory = new byte[this.memory.Length];
					if (this.Data.State == 1)
					{
						for (int i = 0; i < nextStartIndex; i++)
						{
							// Logger.Info($"mem1[{i}] {mem1[i]:b8}");
							this.memory[i] = mem1[i];
						}
					}
					else
						Buffer.BlockCopy(mem1, 0, this.memory, 0, mem1.Length);
				}
				catch(Exception ex)
				{
					Logger.Error("[test_memory] Loading data from client failed with exception: " + ex);
				}
				this.loadFromSave = false;
				if (this.Data.State == 1)
				{
					this.Data.State = 0;
					this.Data.ClientIncomingData = [];
					this.isDataDirty = true;
				}
				QueueLogicUpdate();
			}
		}

		protected override void SavePersistentValuesToCustomData()
		{
			if (!this.isDataDirty) return;
			this.isDataDirty = false;

			MemoryStream memstream = new MemoryStream();
			memstream.Position = 0;
			DeflateStream compressor = new DeflateStream(memstream, CompressionLevel.Optimal, true);
			compressor.Write(this.memory, 0, this.memory.Length);
			compressor.Flush();
			int length = (int)memstream.Position;
			memstream.Position = 0;
			byte[] bytes = new byte[length];
			memstream.Read(bytes, 0, length);
			this.Data.Memory = bytes;
		}

		public override void Dispose()
		{
		}
	}
}
