using System;
using System.IO;
using Playdarium.BuildPipelines.Utils;
using UnityEngine;

namespace Playdarium.BuildPipelines.Settings
{
	public class BuildPipelineSettings
	{
		private const string SETTINGS_FILE_NAME = "BuildPipelines.json";

		public static string ProjectId
		{
			get => ReadSettings().ProjectId;
			set => ChangeSettings(settings => settings.ProjectId = value);
		}

		public static string PipelineTriggerToken
		{
			get => ReadSettings().PipelineTriggerToken;
			set => ChangeSettings(settings => settings.PipelineTriggerToken = value);
		}

		public static string RemotePipelineName
		{
			get => ReadSettings().RemotePipelineName;
			set => ChangeSettings(settings => settings.RemotePipelineName = value);
		}

		public static string DevelopmentProvisionPath
		{
			get => ReadSettings().DevelopmentProvisionPath;
			set => ChangeSettings(settings => settings.DevelopmentProvisionPath = value);
		}

		public static string DistributionProvisionPath
		{
			get => ReadSettings().DistributionProvisionPath;
			set => ChangeSettings(settings => settings.DistributionProvisionPath = value);
		}

		private static Settings ReadSettings()
		{
			var fileInfo = GetSettingsFileInfo();
			if (!fileInfo.Exists)
				return new Settings();

			var text = File.ReadAllText(fileInfo.FullName);
			return JsonUtility.FromJson<Settings>(text);
		}

		private static void ChangeSettings(Action<Settings> action)
		{
			var settings = ReadSettings();
			action(settings);
			var fileInfo = GetSettingsFileInfo();
			var directory = fileInfo.Directory.FullName;
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			var json = JsonUtility.ToJson(settings);
			File.WriteAllText(fileInfo.FullName, json);
		}

		private static FileInfo GetSettingsFileInfo()
		{
			var assetsDir = new DirectoryInfo(Application.dataPath);
			var projectDir = assetsDir.Parent;
			return new FileInfo(Path.Combine(projectDir.FullName, "ProjectSettings", SETTINGS_FILE_NAME));
		}

		[Serializable]
		private class Settings
		{
			[SerializeField] private string projectId;
			[SerializeField] private string pipelineTriggerToken;
			[SerializeField] private string remotePipelineName = PipelineUtils.EMPTY;
			[SerializeField] private string developmentProvisionPath;
			[SerializeField] private string distributionProvisionPath;

			public string ProjectId
			{
				get => projectId;
				set => projectId = value;
			}

			public string PipelineTriggerToken
			{
				get => pipelineTriggerToken;
				set => pipelineTriggerToken = value;
			}

			public string RemotePipelineName
			{
				get => remotePipelineName;
				set => remotePipelineName = value;
			}

			public string DevelopmentProvisionPath
			{
				get => developmentProvisionPath;
				set => developmentProvisionPath = value;
			}

			public string DistributionProvisionPath
			{
				get => distributionProvisionPath;
				set => distributionProvisionPath = value;
			}
		}
	}
}