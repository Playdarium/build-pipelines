using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/KeystoreAndAlias", fileName = "KeystoreAndAlias")]
	public class KeystoreAndAliasPipelineStep : APipelineStep
	{
		[SerializeField] private bool useCustomKeystore;
		[SerializeField] private string keystoreName;
		[SerializeField] private string keystorePassword;
		[SerializeField] private string aliasName;
		[SerializeField] private string aliasPassword;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			PlayerSettings.Android.useCustomKeystore = useCustomKeystore;
			PlayerSettings.Android.keystoreName = string.IsNullOrEmpty(keystoreName) ? null : keystoreName;
			PlayerSettings.Android.keystorePass = string.IsNullOrEmpty(keystorePassword) ? null : keystorePassword;
			PlayerSettings.Android.keyaliasName = string.IsNullOrEmpty(aliasName) ? null : aliasName;
			PlayerSettings.Android.keyaliasPass = string.IsNullOrEmpty(aliasPassword) ? null : aliasPassword;
			onComplete();
		}
	}
}