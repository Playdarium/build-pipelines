﻿using System;
using Playdarium.BuildPipelines.Parameters;
using Playdarium.BuildPipelines.Runtime;
using Playdarium.BuildPipelines.Utils;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/AndroidBuild", fileName = "AndroidBuild")]
	public class AndroidBuildPipelineStep : APipelineStep
	{
		[SerializeField] private string projectPrefix;
		[SerializeField] private BuildOptions buildOptions;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var scenes = parameterHolder.GetBuildInScenes();
			var buildPath = parameterHolder.GetBuildPath();
			var fileName = GetFileName(parameterHolder);
			var buildReport =
				BuildPipeline.BuildPlayer(scenes, $"{buildPath}/{fileName}", BuildTarget.Android, buildOptions);
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
			fileName += parameterHolder.GetBuildAppBundle()
				? $"-aab-{PlayerSettings.bundleVersion}.aab"
				: $"-apk-{PlayerSettings.bundleVersion}.apk";
			return fileName;
		}
	}
}