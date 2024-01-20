using System;
using System.Linq;
using Playdarium.BuildPipelines.Parameters;
using Playdarium.BuildPipelines.Runtime;
using Playdarium.BuildPipelines.Runtime.Impls;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/BuildAndStore", fileName = "BuildAndStore")]
	public class BuildAndStorePipelineStep : APipelineStep
	{
		[SerializeField] private EBuildType buildType;
		[SerializeField] private EStoreType storeType;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var guids = AssetDatabase.FindAssets("t:ScriptableObject");
			var collection = guids.Select(guid =>
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				return AssetDatabase.LoadAssetAtPath<BuildSetting>(path);
			}).Where(f => f != null);
			var buildSettings = collection.FirstOrDefault();
			if (buildSettings == null)
				throw new System.Exception($"[BuildTools] No build settings");

			var buildSettingsSo = new SerializedObject(buildSettings);
			var buildTypeSp = buildSettingsSo.FindProperty("buildType");
			SetEnum(buildTypeSp, buildType);
			parameterHolder.SetBuildType(buildType);

			var storeTypeSp = buildSettingsSo.FindProperty("storeType");
			SetEnum(storeTypeSp, storeType);
			buildSettingsSo.ApplyModifiedProperties();
			parameterHolder.SetStoreType(storeType);
			onComplete();
		}

		private static void SetEnum<T>(SerializedProperty property, T value)
			where T : Enum
		{
			var stringValue = value.ToString();
			var names = Enum.GetNames(typeof(T));
			var index = -1;
			for (var i = 0; i < names.Length; i++)
			{
				if (names[i] != stringValue)
					continue;

				index = i;
				break;
			}

			property.enumValueIndex = index;
		}
	}
}