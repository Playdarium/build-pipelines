using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/BuildApk", fileName = "BuildApk")]
	public class BuildAppBundlePipelineStep : APipelineStep
	{
		[SerializeField] private bool buildAppBundle;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			EditorUserBuildSettings.buildAppBundle = buildAppBundle;
			parameterHolder.SetBuildAppBundle(buildAppBundle);
			onComplete();
		}
	}
}