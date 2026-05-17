using System.Collections.Generic;
using LogicWorld.UI;
using EccsGuiBuilder.Client.Wrappers;
using EccsGuiBuilder.Client.Wrappers.AutoAssign;
using EccsGuiBuilder.Client.Layouts.Helper;
using LogicUI.MenuParts;
using PixLogicWorldComponents.Shared.Config;
using LogicWorld.BuildingManagement;
using LogicAPI.Data.BuildingRequests;
using PixLogicWorldComponents.Shared.CustomData;

namespace PixLogicWorldComponents.Client.Menus
{
	public class RingCounterMenu : EditComponentMenu, IAssignMyFields
	{
		public static void init()
		{
			WS.window("PixLogicWorldComponents-RingCounter")
				.setLocalizedTitle("PixLogicWorldComponents - RingCounter")
				.setYPosition(150)
				.configureContent(content => content
					.layoutVertical()
					.addContainer("FixedContainer", container => container
						.layoutGrowGapHorizontalInnerCentered()
						.add(WS.textLine.setLocalizationKey("PixLogicWorldComponents.Size"))
						.add(WS.slider
							.injectionKey(nameof(sizePegSlider))
							.fixedSize(500, 45)
							.setInterval(CRingCounter.StepSize)
							.setMin(CRingCounter.MinSize)
							.setMax(CRingCounter.MaxSize)
						)
					)
				)
				.add<RingCounterMenu>()
				.build();
		}

		[AssignMe]
		private InputSlider sizePegSlider;

		public override void Initialize()
		{
			base.Initialize();
			sizePegSlider.OnValueChangedInt += ringCounterSizeChanged;
		}

		protected override void OnStartEditing()
		{
			sizePegSlider.SetValueWithoutNotify(
				FirstComponentBeingEdited.Component.Data.OutputCount
			);
		}

		private void ringCounterSizeChanged(int newSize)
		{
			BuildRequestManager.SendBuildRequest(
				new BuildRequest_ChangeDynamicComponentPegCounts(
					FirstComponentBeingEdited.Address,
					CRingCounter.DefaultInput,
					newSize
				)
			);

			(FirstComponentBeingEdited.ClientCode as RingCounterClient)
				.Data.Size = newSize;
		}

		protected override IEnumerable<string> GetTextIDsOfComponentTypesThatCanBeEdited()
		{
			return [
				"PixLogicWorldComponents.RingCounter",
			];
		}
	}
}
