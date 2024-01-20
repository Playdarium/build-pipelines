using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Playdarium.BuildPipelines.Utils
{
	public static class GitHelper
	{
		private static string GitDir => Application.dataPath.Replace("Assets", "") + ".git";

		public static string GetDomain()
		{
			var lines = File.ReadAllLines(Path.Combine(GitDir, "config"));
			var uriStr = lines.SkipWhile(l => !l.Contains("[remote \"origin\"]"))
				.First(l => l.Contains("url = "))
				.Split(new[] { '=' }, StringSplitOptions.None)[1]
				.Replace(" ", string.Empty);

			if (uriStr.StartsWith("git@"))
			{
				var hostAndPath = uriStr.Split(':', StringSplitOptions.RemoveEmptyEntries);
				var host = hostAndPath[0].Split('@', StringSplitOptions.RemoveEmptyEntries)[1];
				uriStr = $"https://{host}/{hostAndPath[1]}";
			}

			var uri = new Uri(uriStr);
			return $"{uri.Scheme}://{uri.Host}";
		}

		public static GitBranchInfo[] GetRemoteBranches()
		{
			var output = ExecuteCommandSync($"git --git-dir={GitDir} branch -r ");
			if (string.IsNullOrEmpty(output))
				return Array.Empty<GitBranchInfo>();

			return output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Skip(1)
				.Select(v => v.Replace(" ", ""))
				.Select(ToBranchInfo)
				.ToArray();
		}

		private static GitBranchInfo ToBranchInfo(string branch)
		{
			var firstSlash = branch.IndexOf('/');
			var branchName = branch.Remove(0, firstSlash + 1);
			int secondSlash;
			var group = (secondSlash = branchName.IndexOf('/')) == -1 ? "origin" : branchName[..secondSlash];
			return new GitBranchInfo(group, branchName);
		}

		private static string ExecuteCommandSync(string command)
		{
			var procStartInfo = GetProcessStart(command);
			procStartInfo.RedirectStandardOutput = true;
			procStartInfo.UseShellExecute = false;
			procStartInfo.CreateNoWindow = true;
			var proc = new Process();
			proc.StartInfo = procStartInfo;
			proc.Start();
			return proc.StandardOutput.ReadToEnd();
		}

		private static ProcessStartInfo GetProcessStart(string command)
		{
			switch (Application.platform)
			{
				case RuntimePlatform.LinuxEditor:
				case RuntimePlatform.OSXEditor:
					return new ProcessStartInfo
					{
						FileName = "/bin/bash",
						Arguments = $"-c \"{command}\"",
					};
				case RuntimePlatform.WindowsEditor:
					return new ProcessStartInfo
					{
						FileName = "cmd.exe",
						Arguments = $"/C \"{command}\"",
					};
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public class GitBranchInfo
	{
		public readonly string Group;
		public readonly string Name;

		public GitBranchInfo(string group, string name)
		{
			Group = group;
			Name = name;
		}
	}
}