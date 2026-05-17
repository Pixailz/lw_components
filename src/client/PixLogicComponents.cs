using LogicAPI.Client;
using System.Collections.Generic;
using PixLogicWorldComponents.Client;
using PixLogicWorldComponents.Client.Menus;
using System;
using EccsLogicWorldAPI.Client.Hooks;
using LogicWorld;

namespace PixLogicWorldComponents
{
	public class PixLogicWorldComponentsClient : ClientMod
	{
		public static List<FileLoadable> fileLoadables = [];

		protected override void Initialize()
		{
			LoadMenus();
			Logger.Info("[✔️] Client: loaded PixLogicWorldComponents");
		}

		public void LoadMenus()
		{
			WorldHook.worldLoading += () => {
				try
				{
					RamMenu.init();
					DecoderMenu.init();
					RegisterMenu.init();
					HexDisplayMenu.init();
					ScreenMenu.init();
					DummyTestMenu.init();
					DoubleDabbleMenu.init();
					MultiplexerMenu.init();
					MultiplexerFastMenu.init();
					DemultiplexerMenu.init();
					RingCounterMenu.init();
				}
				catch(Exception e)
				{
					Logger.Error("❌ Failed to initialize PixLogicWorldComponents Menus");
					SceneAndNetworkManager.TriggerErrorScreen(e);
				}
			};
		}
	}
}
