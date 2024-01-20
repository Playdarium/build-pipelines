using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/Define", fileName = "Define")]
	public class DefinePipelineStep : APipelineStep
	{
		[SerializeField] private string defines;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var targetGroup = parameterHolder.GetBuildTargetGroup();
			PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
			onComplete();
		}
	}
}