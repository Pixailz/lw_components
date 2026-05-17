using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using JimmysUnityUtilities;
using LogicAPI.Data;
using UnityEngine;
using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Client
{
	public class RingCounterPrefab : DynamicPrefabGenerator<(int InputCount, int OutputCount)>
	{
		protected override (int InputCount, int OutputCount) GetIdentifierFor(ComponentData componentData)
			=> (componentData.InputCount, componentData.OutputCount);

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (CRingCounter.DefaultInput, CRingCounter.DefaultOutput);

		protected override Prefab GeneratePrefabFor((int InputCount, int OutputCount) id)
		{
			float	offset_height = 0.5f * CDecoder.BlockHeight;
			float	offset_depth = 0.5f * CDecoder.BlockDepth;
			float	current_pad = 0f;
			if (id.OutputCount % 2 == 0)
				current_pad = CGlobal.Offset;

			ComponentInput[] inputs = new ComponentInput[id.InputCount];

			inputs[CRingCounter.Pin.Enable] = new ComponentInput()
			{
				Position = new Vector3(
					-CGlobal.LSBDir * CGlobal.Offset,
					0.50f * CRingCounter.BlockHeight,
					-CGlobal.Offset + 0.25f * CRingCounter.BlockDepth
				),
				Rotation = new Vector3(90f, CGlobal.LSBDir * -90f, 0f),
				Length = CRingCounter.ActionPinLength,
			};

			inputs[CRingCounter.Pin.Reset] = new ComponentInput()
			{
				Position = new Vector3(
					-CGlobal.LSBDir * CGlobal.Offset,
					0.50f * CRingCounter.BlockHeight,
					-CGlobal.Offset + 0.75f * CRingCounter.BlockDepth
				),
				Rotation = new Vector3(90f, CGlobal.LSBDir * -90f, 0f),
				Length = CRingCounter.ActionPinLength,
			};


			ComponentOutput[] outputs = new ComponentOutput[id.OutputCount];

			for (int i = 0; i < id.OutputCount; i++)
			{
				outputs[i] = new ComponentOutput()
				{
					Position = new Vector3(
						CGlobal.LSBDir * i,
						offset_height,
						offset_depth
					),
					Rotation = new Vector3(90f, 0f, 0f),
				};
			};

			return new Prefab
			{
				Blocks = [
					new Block
					{
						RawColor = CRingCounter.BlockColor,
						Scale = new Vector3(
							id.OutputCount,
							CRingCounter.BlockHeight,
							CRingCounter.BlockDepth
						),
						Position = new Vector3(
							CGlobal.LSBDir * ((id.OutputCount / 2) - current_pad),
							0f,
							0f
						),
					}
				],
				Inputs = inputs,
				Outputs = outputs
			};
		}
	}
}
