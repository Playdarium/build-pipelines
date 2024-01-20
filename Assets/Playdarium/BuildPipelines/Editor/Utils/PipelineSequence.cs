using System;
using System.Collections.Generic;
using Playdarium.BuildPipelines.Parameters;
using Playdarium.BuildPipelines.Settings;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Utils
{
	public class PipelineSequence
	{
		private IReadOnlyList<string> _stepAssets;
		private int _currentStepIndex;

		public PipelineSequence SetStepAssets(IReadOnlyList<string> stepAssets)
		{
			_stepAssets = stepAssets;
			return this;
		}

		public void Execute(BuildParameterHolder parameterHolder, Action<Exception> onComplete)
		{
			_currentStepIndex = PipelineSequenceUtils.StepIndex;
			Debug.Log($"[{nameof(PipelineSequence)}] Current step index: {_currentStepIndex}");
			if (_stepAssets.Count == _currentStepIndex)
			{
				onComplete(null);
				return;
			}

			var stepAsset = _stepAssets[_currentStepIndex++];
			PipelineSequenceUtils.StepIndex = _currentStepIndex;

			try
			{
				var pipelineStep = AssetDatabase.LoadAssetAtPath<APipelineStep>(stepAsset);
				Debug.Log($"[{nameof(PipelineSequence)}] Build step: {pipelineStep.Name}");
				pipelineStep.Execute(parameterHolder, () => Execute(parameterHolder, onComplete));
			}
			catch (Exception e)
			{
				onComplete(e);
			}
		}
	}
}