using System;
using System.Linq;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/SceneToBuild", fileName = "SceneToBuild")]
	public class SceneToBuildPipelineStep : APipelineStep
	{
		[SerializeField] private SceneAsset[] scenes;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var scenePaths = scenes.Select(AssetDatabase.GetAssetPath).ToArray();
			parameterHolder.SetBuildInScenes(scenePaths);
			onComplete();
		}
	}
}