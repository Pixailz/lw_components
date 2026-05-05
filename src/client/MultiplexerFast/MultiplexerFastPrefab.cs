using LogicAPI.Data;
using LogicWorld.SharedCode.Components;
using LogicWorld.Rendering.Dynamics;
using UnityEngine;
using PixLogicWorldComponents.Shared.Config;
using LICC;

namespace PixLogicWorldComponents.Client
{
	public class MultiplexerFastPrefab : DynamicPrefabGenerator<(
		int InputCount, int OutputCount
	)>
	{
		public override (int inputCount, int outputCount) GetDefaultPegCounts()
		{
			return ((
					(
						1 << this.currentSelector
					) * CDemultiplexer.DefaultDataWidth
				) + this.currentSelector
				+ CDemultiplexer.DefaultDataWidth,
				CMultiplexerFast.DefaultOutput
			);
		}

		protected override
			(int InputCount, int OutputCount) GetIdentifierFor(
					ComponentData componentData
				)
				=> (componentData.InputCount, componentData.OutputCount);

		private static readonly Vector3 outputRotation = new Vector3(
			90f, 0f, 0f
		);
		private static readonly Vector3 inputRotation = new Vector3(
			-90f, 0f, 0f
		);
		private static readonly Vector3 selectorRotation = new Vector3(
			90f, -90f, 0f
		);

		private int		currentDataWidth	= 0;
		private int		currentSelector		= 0;
		private float	currentHeight		= 0;
		private float	currentPad			= 0;

		public override void Setup(ComponentInfo info)
		{
			this.currentSelector = info.CodeInfoInts[0];
			this.currentHeight = 1 << this.currentSelector;
		}

		/*
			(
				(1 << this.currentData.SelectorWidth) * this.currentData.DataWidth
			) + this.currentData.SelectorWidth + this.currentData.DataWidth,

			(
				(1 << this.currentData.SelectorWidth) * this.currentData.DataWidth
			) + this.currentData.DataWidth,

			this.currentData.DataWidth + this.currentData.DataWidth,
		*/
		public static void getCurrentDataWidth(
			(int InputCount, int OutputCount, int Selector) id,
			out int dataWidth
		)
		{
			for (
				dataWidth = CMultiplexer.MinDataWidth;
				dataWidth < CMultiplexer.MaxDataWidth;
				dataWidth++
			)
			{
				if (id.InputCount ==
					(
						(1 << id.Selector) * dataWidth
					) + id.Selector + dataWidth
				)
					break ;
			}
		}

		private void getCurrentValue((int InputCount, int OutputCount) id)
		{
			getCurrentDataWidth(
				(
					InputCount: id.InputCount,
					OutputCount: id.OutputCount,
					Selector: this.currentSelector
				),
				out int tmpCurrentDataWidth
			);
			if (
				this.currentDataWidth == tmpCurrentDataWidth
			)
				return ;
			this.currentDataWidth = tmpCurrentDataWidth;

			if (this.currentDataWidth % 2 == 0)
				this.currentPad = CGlobal.Offset;
			else
				this.currentPad = 0f;
		}

		protected override Prefab GeneratePrefabFor(
			(int InputCount, int OutputCount) id
		)
		{
			ComponentInput[] inputs = new ComponentInput[id.InputCount];

			getCurrentValue(id);

			int counted_input = 0;
			float length = CMultiplexerFast.DataPinLength;

			// Selector pin
			for (int i = 0; i < this.currentSelector; i++)
			{
				counted_input++;
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						-0.5f,
						0.5f + i,
						-0.5f + (0.50f * CMultiplexerFast.BlockDepth)
					),
					Rotation = selectorRotation,
					Length = length
				};
				length += CMultiplexerFast.DataPinLengthStep;
			}

			// Data Output pin
			for (
				int i = this.currentSelector;
				i < this.currentDataWidth + this.currentSelector;
				i++
			)
			{
				counted_input++;
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						i - this.currentSelector,
						0.5f,
						-0.5f + CMultiplexerFast.BlockDepth
					),
					Rotation = outputRotation,
					Length = CMultiplexerFast.ActionPinLength
				};
			}

			float pin_y = 0.5f;
			// Data Input pin
			for (int i = this.currentSelector + this.currentDataWidth; i < id.InputCount; i++)
			{
				int ix = (i - this.currentSelector + this.currentDataWidth) % this.currentDataWidth;
				counted_input++;
				inputs[i] = new ComponentInput()
				{
					Position = new Vector3(
						ix,
						pin_y,
						-0.5f
					),
					Rotation = inputRotation,
					Length = CMultiplexerFast.ActionPinLength
				};
				if (ix == this.currentDataWidth - 1)
					pin_y += 1f;
			}

			return new Prefab()
			{
				Blocks = [
					new Block()
					{
						Scale = new Vector3(
							this.currentDataWidth,
							this.currentHeight,
							CMultiplexerFast.BlockDepth
						),
						Position = new Vector3(
							(this.currentDataWidth / 2) - this.currentPad,
							0,
							(CMultiplexerFast.BlockDepth / 2) - CGlobal.Offset
						),
						RawColor = CMultiplexerFast.BlockColor,
					}
				],
				Inputs = inputs,
				Outputs = [],
			};
		}
	}
}
