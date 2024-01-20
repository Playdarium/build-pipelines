using System;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Settings
{
	public static class PipelineSequenceUtils
	{
		private static string BuildProcessIdKey => $"{PlayerSettings.productGUID}.Process.Id";
		private static string PipelineNameKey => $"{PlayerSettings.productGUID}.Build.Pipeline.Name";
		private static string PipelineStepIndexKey => $"{PlayerSettings.productGUID}.Build.Pipeline.Step";
		private static string ParametersHolderKey => $"{PlayerSettings.productGUID}.Parameter.Holder";

		public static int ProcessId
		{
			get => EditorPrefs.GetInt(BuildProcessIdKey, int.MinValue);
			set => EditorPrefs.SetInt(BuildProcessIdKey, value);
		}

		public static string PipelineName
		{
			get => EditorPrefs.GetString(PipelineNameKey, string.Empty);
			set => EditorPrefs.SetString(PipelineNameKey, value);
		}

		public static int StepIndex
		{
			get => EditorPrefs.GetInt(PipelineStepIndexKey, 0);
			set => EditorPrefs.SetInt(PipelineStepIndexKey, value);
		}

		public static byte[] ParametersHolderBytes
		{
			get => EditorPrefs.HasKey(ParametersHolderKey)
				? JsonUtility.FromJson<BinaryJson>(EditorPrefs.GetString(ParametersHolderKey)).data
				: null;
			set
			{
				var binaryJson = new BinaryJson(value);
				var json = JsonUtility.ToJson(binaryJson);
				EditorPrefs.SetString(ParametersHolderKey, json);
			}
		}

		public static void Clear()
		{
			EditorPrefs.DeleteKey(BuildProcessIdKey);
			EditorPrefs.DeleteKey(PipelineNameKey);
			EditorPrefs.DeleteKey(PipelineStepIndexKey);
			EditorPrefs.DeleteKey(ParametersHolderKey);
		}


		[Serializable]
		public class BinaryJson
		{
			public byte[] data;

			public BinaryJson()
			{
			}

			public BinaryJson(byte[] data)
			{
				this.data = data;
			}
		}
	}
}