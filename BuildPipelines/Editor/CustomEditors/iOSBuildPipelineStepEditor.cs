using System;
using System.IO;
using Playdarium.BuildPipelines.PostProcess.iOS;
using Playdarium.BuildPipelines.Tools.Impls;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Playdarium.BuildPipelines.CustomEditors
{
	[CustomEditor(typeof(iOSBuildPipelineStep))]
	public class iOSBuildPipelineStepEditor : UnityEditor.Editor
	{
		private const string EXTENSION = "mobileprovision";

		private GUIStyle _style;

		public override VisualElement CreateInspectorGUI()
		{
			_style = new GUIStyle
			{
				richText = true,
				normal =
				{
					textColor = Color.white
				}
			};
			return base.CreateInspectorGUI();
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			DrawProvisionChoosePath(
				"Development Provision:",
				() => ExportOptions.DevelopmentProvisionPath,
				s => ExportOptions.DevelopmentProvisionPath = s
			);
			DrawProvisionChoosePath(
				"Distribution Provision:",
				() => ExportOptions.DistributionProvisionPath,
				s => ExportOptions.DistributionProvisionPath = s
			);
		}

		private void DrawProvisionChoosePath(string label, Func<string> getterPath, Action<string> setterPath)
		{
			GUILayout.Space(5);
			GUILayout.Label($"<b>{label}</b>", _style);
			using (new GUILayout.HorizontalScope())
			{
				var assetProvisionPath = getterPath();
				assetProvisionPath = !string.IsNullOrEmpty(assetProvisionPath) ? assetProvisionPath : string.Empty;
				GUILayout.Label(assetProvisionPath);
				if (!GUILayout.Button("Browse", GUILayout.Width(100)))
					return;

				var provisionPath = Path.Combine(GetProjectDir(), assetProvisionPath);
				provisionPath = EditorUtility.OpenFilePanel("Choose development provision", provisionPath,
					EXTENSION);
				setterPath(provisionPath.Replace(GetProjectDir(), string.Empty));
			}
		}

		private static string GetProjectDir()
		{
			var length = "Assets".Length;
			return Application.dataPath.Remove(Application.dataPath.Length - length, length);
		}
	}
}