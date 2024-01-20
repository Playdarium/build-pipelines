using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/ScriptingBackendAndroidMono", fileName = "ScriptingBackendAndroidMono")]
	public class ScriptingBackendAndroidMonoPipelineStep : APipelineStep
	{
		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
			PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
			onComplete();
		}
	}
}