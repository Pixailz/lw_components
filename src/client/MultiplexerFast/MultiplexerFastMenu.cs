using System.Collections.Generic;
using UnityEngine;
using LogicWorld.UI;
using LogicAPI.Data.BuildingRequests;
using LogicUI.MenuParts;
using EccsGuiBuilder.Client.Layouts.Helper;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using LogicWorld.BuildingManagement;

using PixLogicWorldComponents.Shared.Config;
using PixLogicWorldComponents.Shared.CustomData;
using LogicAPI.Data;

namespace PixLogicWorldComponents.Client.Menus
{
	public class MultiplexerFastMenu : EditComponentMenu, IAssignMyFields
	{
		public static void init()
		{
			WS.window("PixLogicWorldComponents-MultiplexerFast")
				.setLocalizedTitle("PixLogicWorldComponents - Multiplexer Fast")
				.setYPosition(150)
				.configureContent(content => content
					.layoutVertical()
					.addContainer("BottomBox", bottomBox => bottomBox
						.injectionKey(nameof(bottomSection))
						.layoutVerticalInnerCentered()
						.addContainer("BottomInnerBox", innerBox => innerBox
							.layoutGrowGapVerticalInner()
							.addContainer("BottomBox1", container => container
								.layoutGrowGapHorizontalInnerCentered()
								.add(WS.textLine.setLocalizationKey("PixLogicWorldComponents.MultiplexerWidth"))
								.add(WS.slider
									.injectionKey(nameof(dataWidthSlider))
									.fixedSize(500, 45)
									.setInterval(CMultiplexer.StepDataWidth)
									.setMax(CMultiplexer.MaxDataWidth)
									.setMin(CMultiplexer.MinDataWidth)
								)
							)
						)
					)
				)
				.add<MultiplexerFastMenu>()
				.build();
		}

		[AssignMe]
		public InputSlider dataWidthSlider;
		[AssignMe]
		public GameObject bottomSection;

		private IMultiplexerData currentData;

		protected override void OnStartEditing()
		{
			this.currentData =
				FirstComponentBeingEdited.ClientCode.CustomDataObject as IMultiplexerData;

			dataWidthSlider.SetValueWithoutNotify(this.currentData.DataWidth);
			bottomSection.SetActive(true);
		}

		public override void Initialize()
		{
			base.Initialize();
			dataWidthSlider.OnValueChangedInt += OnDataWidthChanged;
		}

		private void SendBuildRequest(
			ComponentAddress addr,
			int newDataWidth
		)
		{
			this.currentData.DataWidth = newDataWidth;
			BuildRequestManager.SendBuildRequest(
				new BuildRequest_ChangeDynamicComponentPegCounts(addr,
					(
						(1 << this.currentData.SelectorWidth) * this.currentData.DataWidth
					) + this.currentData.SelectorWidth + this.currentData.DataWidth,
					0
				)
			);
		}

		private void OnDataWidthChanged(int newDataWidth)
		{
			SendBuildRequest(
				FirstComponentBeingEdited.Address,
				newDataWidth
			);
		}

		protected override
		IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
		{
			return [
				"PixLogicWorldComponents.MultiplexerFast1",
				"PixLogicWorldComponents.MultiplexerFast2",
				"PixLogicWorldComponents.MultiplexerFast3",
				"PixLogicWorldComponents.MultiplexerFast4",
			];
		}
	}
}
