using System.Linq;
using UnityEditor;

namespace Playdarium.BuildPipelines.Utils
{
	public static class PipelineUtils
	{
		public const string EMPTY = "None";

		public static BuildProjectPipeline[] FindAll()
			=> AssetDatabase.FindAssets($"t:{nameof(BuildProjectPipeline)}")
				.Select(AssetDatabase.GUIDToAssetPath)
				.Select(AssetDatabase.LoadAssetAtPath<BuildProjectPipeline>)
				.ToArray();
	}
}