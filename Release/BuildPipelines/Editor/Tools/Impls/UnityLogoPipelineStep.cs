using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/UnityLogo", fileName = "UnityLogo")]
	public class UnityLogoPipelineStep : APipelineStep
	{
		[SerializeField] private bool showUnityLogo;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			PlayerSettings.SplashScreen.showUnityLogo = showUnityLogo;
			onComplete();
		}
	}
}