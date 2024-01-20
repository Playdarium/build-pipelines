using System;
using Playdarium.BuildPipelines.Parameters;
using Playdarium.BuildPipelines.PostProcess.iOS;
using Playdarium.BuildPipelines.Runtime;
using Playdarium.BuildPipelines.Utils;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/iOS Build", fileName = "iOSBuild")]
	public class iOSBuildPipelineStep : APipelineStep
	{
		[SerializeField] private string projectPrefix;
		[SerializeField] private BuildOptions buildOptions;
		[SerializeField] private XcodeBuildConfig buildType;

		[Tooltip("Add ExportOptions.plist to output ios project for build agent")]
		[FormerlySerializedAs("exportEnabled")]
		[SerializeField]
		private bool exportOptionsEnabled = true;

		[Tooltip("Work if ExportOptionsEnabled = true")] [SerializeField]
		private bool compileBitcode = true;

		[Tooltip("Work if ExportOptionsEnabled = true")] [SerializeField]
		private bool stripSwiftSymbols = true;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			ExportOptions.ExportEnabled = exportOptionsEnabled;
			ExportOptions.CompileBitcode = compileBitcode;
			ExportOptions.StripSwiftSymbols = stripSwiftSymbols;

			EditorUserBuildSettings.iOSXcodeBuildConfig = buildType;
			var scenes = parameterHolder.GetBuildInScenes();
			var buildPath = parameterHolder.GetBuildPath();
			var fileName = GetFileName(parameterHolder);
			var buildReport =
				BuildPipeline.BuildPlayer(scenes, $"{buildPath}/{fileName}", BuildTarget.iOS, buildOptions);
			var buildSummary = buildReport.summary;
			var message =
				$"[{GetType().Name}] {DateTime.Now:g} Build for {buildSummary.platform}: {buildSummary.outputPath} Size:{buildSummary.totalSize} Result: {buildSummary.result}";

			if (buildSummary.result != BuildResult.Succeeded)
				throw new Exception(message);

			Debug.Log(message);
			onComplete();
		}

		private string GetFileName(BuildParameterHolder parameterHolder)
		{
			var fileName = projectPrefix;
			fileName += $"-{parameterHolder.GetStoreType().ToStorePrefix()}";
			fileName += parameterHolder.GetBuildType() == EBuildType.Release ? string.Empty : "-dev";
			fileName += $"-{PlayerSettings.bundleVersion}";
			return fileName;
		}
	}
}