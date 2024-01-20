using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/PackageName", fileName = "PackageName")]
	public class PackageNamePipelineStep : APipelineStep
	{
		[SerializeField] private string packageName;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var targetGroup = parameterHolder.GetBuildTargetGroup();
			PlayerSettings.SetApplicationIdentifier(targetGroup, packageName);
			onComplete();
		}
	}
}