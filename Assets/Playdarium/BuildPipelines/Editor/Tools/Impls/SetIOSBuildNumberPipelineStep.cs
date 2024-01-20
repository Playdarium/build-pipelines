using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/SetIOSBuildNumber", fileName = "SetIOSBuildNumber")]
	public class SetIOSBuildNumberPipelineStep : APipelineStep
	{
		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER")?.Replace(".", "");
			if (string.IsNullOrEmpty(buildNumber))
				buildNumber = Environment.GetEnvironmentVariable("BUILD_ID")?.Replace(".", "");

			if (!string.IsNullOrEmpty(buildNumber))
				PlayerSettings.iOS.buildNumber = buildNumber;

			onComplete();
		}
	}
}