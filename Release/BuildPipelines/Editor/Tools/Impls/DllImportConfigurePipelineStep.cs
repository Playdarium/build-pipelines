using System;
using System.IO;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/DllImportConfigurePipelineStep",
		fileName = "DllImportConfigurePipelineStep")]
	public class DllImportConfigurePipelineStep : APipelineStep
	{
		[SerializeField] private string folderPath;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var assets = AssetDatabase.FindAssets("", new[] { folderPath });
			foreach (var asset in assets)
			{
				var path = AssetDatabase.GUIDToAssetPath(asset);
				if (Path.GetExtension(path) != ".dll")
					continue;

				var isEditor = Path.GetFileNameWithoutExtension(path).Contains("Editor");
				var pluginImporter = AssetImporter.GetAtPath(path) as PluginImporter;
				if (isEditor)
				{
					pluginImporter.SetCompatibleWithEditor(true);
				}
				else
				{
					pluginImporter.SetCompatibleWithAnyPlatform(true);
					pluginImporter.SetExcludeEditorFromAnyPlatform(false);
				}

				pluginImporter.SaveAndReimport();
			}

			AssetDatabase.ForceReserializeAssets();
			onComplete();
		}

		[ContextMenu("Execute")]
		public void Execute() => Execute(new BuildParameterHolder(), () => { });
	}
}