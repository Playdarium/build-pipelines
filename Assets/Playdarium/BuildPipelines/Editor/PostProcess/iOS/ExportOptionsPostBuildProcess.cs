#if UNITY_IOS

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace BuildPipelines.PostProcess.iOS
{
	public class ExportOptionsPostBuildProcess : IPostprocessBuildWithReport
	{
		public int callbackOrder => 1;

		public void OnPostprocessBuild(BuildReport report)
		{
			if (!ExportOptions.ExportEnabled || report.summary.platform != BuildTarget.iOS)
				return;

			if (PlayerSettings.iOS.appleEnableAutomaticSigning)
				throw new Exception(
					$"[{nameof(ExportOptionsPostBuildProcess)}] For work ExportOptions use manual signing");

			var outputPath = report.summary.outputPath;
			CreateExportOptions(outputPath, EProvisionProfileType.Development);
			CreateExportOptions(outputPath, EProvisionProfileType.Distribution);
		}

		private static void CreateExportOptions(string directory, EProvisionProfileType profileType)
		{
			var provisionName = ProvisionXmlUtils.GetProvisionName(profileType);
			var exportOptionsPlistText = new ExportOptions.Builder(profileType)
				.SetApplicationIdentifier(PlayerSettings.applicationIdentifier)
				.SetTeamId(PlayerSettings.iOS.appleDeveloperTeamID)
				.SetProvisionName(provisionName)
				.SetCompileBitcode(ExportOptions.CompileBitcode)
				.SetStripSwiftSymbols(ExportOptions.StripSwiftSymbols)
				.Build();

			var exportOptionsPlistPath = Path.Combine(directory, $"ExportOptions{profileType}.plist");
			File.WriteAllText(exportOptionsPlistPath, exportOptionsPlistText);
		}
	}
}
#endif