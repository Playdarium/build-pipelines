using System;
using System.IO;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/ServerBuildPipelineStep", fileName = "ServerBuildPipelineStep")]
	public class ServerBuildPipelineStep : APipelineStep
	{
		[SerializeField] private string executableFileName;
		[SerializeField] private BuildOptions buildOptions;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var scenes = parameterHolder.GetBuildInScenes();
			var buildPath = parameterHolder.GetBuildPath();
			var options = new BuildPlayerOptions()
			{
				locationPathName = Path.Combine(buildPath, executableFileName),
				target = BuildTarget.StandaloneLinux64,
				targetGroup = BuildTargetGroup.Standalone,
				subtarget = (int)StandaloneBuildSubtarget.Server,
				scenes = scenes,
				options = buildOptions
			};
			var buildReport = BuildPipeline.BuildPlayer(options);
			var buildSummary = buildReport.summary;
			var message =
				$"[{GetType().Name}] {DateTime.Now:g} Build for {buildSummary.platform}: {buildSummary.outputPath} Size:{buildSummary.totalSize} Result: {buildSummary.result}";

			if (buildSummary.result != BuildResult.Succeeded)
				throw new Exception(message);

			Debug.Log(message);
			onComplete();
		}
	}
}