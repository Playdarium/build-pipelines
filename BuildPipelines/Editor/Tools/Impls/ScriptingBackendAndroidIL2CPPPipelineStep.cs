using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/ScriptingBackendAndroidIL2CPP",
		fileName = "ScriptingBackendAndroidIL2CPP")]
	public class ScriptingBackendAndroidIL2CPPPipelineStep : APipelineStep
	{
		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
			PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
			onComplete();
		}
	}
}