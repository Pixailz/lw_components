using LogicWorld.Rendering.Dynamics;
using LogicWorld.SharedCode.Components;
using JimmysUnityUtilities;
using LogicAPI.Data;
using UnityEngine;
using PixLogicWorldComponents.Shared.Config;

namespace PixLogicWorldComponents.Client
{
	public class DummyTestPrefab : DynamicPrefabGenerator<int>
	{
		protected override int GetIdentifierFor(ComponentData componentData)
			=> 0; // No variants

		public override (int inputCount, int outputCount) GetDefaultPegCounts()
			=> (0, 0);

		protected override Prefab GeneratePrefabFor(int identifier)
		{
			// Create a simple 2x2 block
			return new Prefab
			{
				Blocks = [
					new Block
					{
						RawColor = CDummyTest.BlockColor,
						Position = new Vector3(1f, 1f, 0f),
						Scale = new Vector3(2f, 2f, CDummyTest.BlockDepth)
					}
				],
				Inputs = [],
				Outputs = []
			};
		}
	}
}
