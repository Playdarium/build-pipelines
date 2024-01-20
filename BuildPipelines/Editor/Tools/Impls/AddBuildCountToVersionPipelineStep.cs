using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/AddBuildCountToVersion", fileName = "AddBuildCountToVersion")]
	public class AddBuildCountToVersionPipelineStep : APipelineStep
	{
		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER")?.Replace(".", "");
			if (string.IsNullOrEmpty(buildNumber))
				buildNumber = Environment.GetEnvironmentVariable("BUILD_ID")?.Replace(".", "");

			if (!string.IsNullOrEmpty(buildNumber))
				PlayerSettings.bundleVersion = PlayerSettings.bundleVersion + "." + buildNumber;
			onComplete();
		}
	}
}