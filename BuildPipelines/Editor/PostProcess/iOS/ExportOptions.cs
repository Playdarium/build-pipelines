using Playdarium.BuildPipelines.Settings;
using UnityEditor;

namespace Playdarium.BuildPipelines.PostProcess.iOS
{
	public class ExportOptions
	{
		private const string EXPORT_ENABLED = "ExportOptions.ExportEnabled";
		private const string COMPILE_BITCODE = "ExportOptions.CompileBitcode";
		private const string STRIP_SWIFT_SYMBOLS = "ExportOptions.StripSwiftSymbols";

		public static bool ExportEnabled
		{
			get => EditorPrefs.GetBool(EXPORT_ENABLED, true);
			set => EditorPrefs.SetBool(EXPORT_ENABLED, value);
		}

		public static bool CompileBitcode
		{
			get => EditorPrefs.GetBool(COMPILE_BITCODE, true);
			set => EditorPrefs.SetBool(COMPILE_BITCODE, value);
		}

		public static bool StripSwiftSymbols
		{
			get => EditorPrefs.GetBool(STRIP_SWIFT_SYMBOLS, true);
			set => EditorPrefs.SetBool(STRIP_SWIFT_SYMBOLS, value);
		}

		public static string DevelopmentProvisionPath
		{
			get => BuildPipelineSettings.DevelopmentProvisionPath;
			set
			{
				BuildPipelineSettings.DevelopmentProvisionPath = value;
				PlayerSettings.iOS.iOSManualProvisioningProfileID =
					ProvisionXmlUtils.GetProvisionUUID(EProvisionProfileType.Development);
				PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Development;
			}
		}

		public static string DistributionProvisionPath
		{
			get => BuildPipelineSettings.DistributionProvisionPath;
			set => BuildPipelineSettings.DistributionProvisionPath = value;
		}

		public interface IApplicationIdentifierExportOptionsBuilder
		{
			ITeamIdExportOptionsBuilder SetApplicationIdentifier(string value);
		}

		public interface ITeamIdExportOptionsBuilder
		{
			IProvisionNameExportOptionsBuilder SetTeamId(string value);
		}

		public interface IProvisionNameExportOptionsBuilder
		{
			IExportOptionsBuilder SetProvisionName(string value);
		}

		public interface IExportOptionsBuilder
		{
			IExportOptionsBuilder SetCompileBitcode(bool value);

			IExportOptionsBuilder SetStripSwiftSymbols(bool value);

			string Build();
		}

		public class Builder : IApplicationIdentifierExportOptionsBuilder, ITeamIdExportOptionsBuilder,
			IProvisionNameExportOptionsBuilder, IExportOptionsBuilder
		{
			private readonly EProvisionProfileType _provisionProfileType;
			private string _applicationIdentifier;
			private string _teamId;
			private string _provisionName;
			private bool _compileBitcode = true;
			private bool _stripSwiftSymbols = true;

			public Builder(EProvisionProfileType provisionProfileType)
			{
				_provisionProfileType = provisionProfileType;
			}

			public ITeamIdExportOptionsBuilder SetApplicationIdentifier(string value)
			{
				_applicationIdentifier = value;
				return this;
			}

			IProvisionNameExportOptionsBuilder ITeamIdExportOptionsBuilder.SetTeamId(string value)
			{
				_teamId = value;
				return this;
			}

			IExportOptionsBuilder IProvisionNameExportOptionsBuilder.SetProvisionName(string value)
			{
				_provisionName = value;
				return this;
			}

			IExportOptionsBuilder IExportOptionsBuilder.SetCompileBitcode(bool value)
			{
				_compileBitcode = value;
				return this;
			}

			IExportOptionsBuilder IExportOptionsBuilder.SetStripSwiftSymbols(bool value)
			{
				_stripSwiftSymbols = value;
				return this;
			}

			string IExportOptionsBuilder.Build()
			{
				var buildStrategy = _provisionProfileType == EProvisionProfileType.Distribution
					? @"<key>destination</key>
		<string>upload</string>
		<key>method</key>
		<string>app-store</string>"
					: @"<key>destination</key>
		<string>export</string>
		<key>method</key>
		<string>development</string>";

				return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
	<dict>
		<key>compileBitcode</key>
		<{_compileBitcode.ToString().ToLower()}/>
		{buildStrategy}
		<key>provisioningProfiles</key>
		<dict>
		<key>{_applicationIdentifier}</key>
		<string>{_provisionName}</string>
		</dict>
		<key>signingCertificate</key>
		<string>Apple {_provisionProfileType}</string>
		<key>signingStyle</key>
		<string>manual</string>
		<key>stripSwiftSymbols</key>
		<{_stripSwiftSymbols.ToString().ToLower()}/>
		<key>teamID</key>
		<string>{_teamId}</string>
		<key>thinning</key>
		<string>&lt;none&gt;</string>
	</dict>
</plist>";
			}
		}
	}

	public enum EProvisionProfileType
	{
		Development,
		Distribution
	}
}