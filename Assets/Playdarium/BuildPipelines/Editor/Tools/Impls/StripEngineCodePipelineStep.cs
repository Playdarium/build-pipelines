using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/StripEngineCode", fileName = "StripEngineCode")]
	public class StripEngineCodePipelineStep : APipelineStep
	{
		[SerializeField] private ManagedStrippingLevel strippingLevel;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var targetGroup = parameterHolder.GetBuildTargetGroup();
			PlayerSettings.stripEngineCode = true;
			PlayerSettings.SetManagedStrippingLevel(targetGroup, strippingLevel);
			onComplete();
		}
	}
}