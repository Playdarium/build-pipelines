using System;
using System.Collections.Generic;
using System.Linq;
using Playdarium.BuildPipelines.Parameters;
using Playdarium.BuildPipelines.Utils;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines
{
	[CreateAssetMenu(menuName = "BuildPipeline/BuildProjectPipeline", fileName = "BuildProjectPipeline")]
	public class BuildProjectPipeline : ScriptableObject
	{
		[SerializeField] private BuildTarget buildTarget;
		[SerializeField] private List<APipelineStep> steps;

		private PipelineSequence _pipelineSequence;

		public string PipelineName => name;

		public BuildTarget BuildTarget => buildTarget;

		public void Execute(BuildParameterHolder parameterHolder, Action<Exception> onComplete)
		{
			var stepAssets = steps.Select(AssetDatabase.GetAssetPath).ToArray();
			parameterHolder.SetBuildTarget(buildTarget);
			_pipelineSequence = new PipelineSequence();
			_pipelineSequence.SetStepAssets(stepAssets).Execute(parameterHolder, onComplete);
		}
	}
}