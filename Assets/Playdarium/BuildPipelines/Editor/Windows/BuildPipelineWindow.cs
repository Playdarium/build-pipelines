using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Playdarium.BuildPipelines.Settings;
using Playdarium.BuildPipelines.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Playdarium.BuildPipelines.Windows
{
	public class BuildPipelineWindow : EditorWindow
	{
		private const string SELECTED_BUILD_TARGET_KEY = "BuildPipeline.BuildTarget.Index";

		private static readonly string[] BuildTargets =
		{
			"android",
			"ios"
		};

		private GitBranchInfo[] _branches;
		private int _buildTargetSelected;

		private GUIStyle _mainHeaderStyle;
		private BuildProjectPipeline[] _pipelines;
		private GUIStyle _titleStyle;
		private Vector2 _scrollWindow;

		[MenuItem("Tools/Build/Draw Git Domain")]
		public static void DrawGitDomain()
		{
			var domain = GitHelper.GetDomain();
			Debug.Log($"[{nameof(BuildPipelineWindow)}] {domain}");
		}

		[MenuItem("Tools/Build/Pipelines")]
		public static void Open()
		{
			var window = GetWindow<BuildPipelineWindow>();
			window.Show();
		}

		private void OnEnable()
		{
			_mainHeaderStyle = new GUIStyle
			{
				normal =
				{
					textColor = Color.white * 0.95f
				},
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold
			};

			_titleStyle = new GUIStyle
			{
				normal =
				{
					textColor = Color.white * 0.95f
				},
				fontStyle = FontStyle.Bold
			};

			_pipelines = PipelineUtils.FindAll();
			_branches = GitHelper.GetRemoteBranches();
			_buildTargetSelected = EditorPrefs.GetInt(SELECTED_BUILD_TARGET_KEY, 0);
		}

		private void OnGUI()
		{
			GUILayout.Space(5);
			DrawPipelines();
			GUILayout.Space(15);
			DrawRemoteBuild();
			GUILayout.Space(15);
			using var scrollPosition = new GUILayout.ScrollViewScope(_scrollWindow);
			_scrollWindow = scrollPosition.scrollPosition;
			DrawGitBranches();
		}

		private void DrawPipelines()
		{
			GUILayout.Label("PIPELINES", _mainHeaderStyle);
			var groupBy = _pipelines.GroupBy(p => p.BuildTarget);

			foreach (var group in groupBy)
			{
				GUILayout.Label(" " + group.Key, _titleStyle);
				foreach (var pipeline in group)
					if (GUILayout.Button(pipeline.PipelineName))
						Build(group.Key, pipeline.PipelineName);

				GUILayout.Space(5);
			}
		}

		private Rect _buttonRect;

		private void DrawRemoteBuild()
		{
			GUILayout.Label("REMOTE BUIlD", _mainHeaderStyle);

			using (var change = new EditorGUI.ChangeCheckScope())
			{
				var projectId = EditorGUILayout.TextField("Project Id", BuildPipelineSettings.ProjectId);
				if (change.changed)
					BuildPipelineSettings.ProjectId = projectId;
			}

			using (var change = new EditorGUI.ChangeCheckScope())
			{
				var token = EditorGUILayout.TextField("Trigger Token", BuildPipelineSettings.PipelineTriggerToken);
				if (change.changed)
					BuildPipelineSettings.PipelineTriggerToken = token;
			}

			var selectedBuildTarget = EditorGUILayout.Popup("Build Target", _buildTargetSelected, BuildTargets);
			if (_buildTargetSelected != selectedBuildTarget)
			{
				_buildTargetSelected = selectedBuildTarget;
				EditorPrefs.SetInt(SELECTED_BUILD_TARGET_KEY, _buildTargetSelected);
			}

			using (var change = new EditorGUI.ChangeCheckScope())
			{
				using (new GUILayout.HorizontalScope())
				{
					EditorGUILayout.LabelField("Pipeline Name", GUILayout.Width(100));
					if (GUILayout.Button(BuildPipelineSettings.RemotePipelineName))
					{
						PopupWindow.Show(_buttonRect, new ChooseRemotePipelinePopupContext());
					}

					if (Event.current.type == EventType.Repaint)
						_buttonRect = GUILayoutUtility.GetLastRect();
				}
			}
		}

		private void DrawGitBranches()
		{
			GUILayout.Label("GIT BRANCHES", _mainHeaderStyle);

			if (GUILayout.Button("Fetch")) _branches = GitHelper.GetRemoteBranches();

			if (_branches.Length == 0)
				GUILayout.Label("Git not connected or no branches");
			else
				foreach (var group in _branches.GroupBy(info => info.Group))
				{
					GUILayout.Label(" " + group.Key, _titleStyle);

					foreach (var info in group)
					{
						if (!GUILayout.Button(info.Name))
							continue;

						var buildTarget = BuildTargets[_buildTargetSelected];
						if (EditorUtility.DisplayDialog($"Build {buildTarget}",
							    $"Launch remote build for branch '{info.Name}'", "Build", "Cancel"))
							BuildRequest(buildTarget, info.Name);
					}
				}
		}

		private static void Build(BuildTarget buildTarget, string pipelineName)
		{
			var buildPath = ChooseBuildPath.Choose($"{buildTarget}.{pipelineName}");
			if (string.IsNullOrEmpty(buildPath))
			{
				Debug.LogError(
					$"[{nameof(BuildPipelineWindow)}] Build pipeline '{pipelineName}' start canceled.");
				return;
			}

			BuildPipelineExecutor.ExecutePipeline(pipelineName, buildPath);
		}

		private static void BuildRequest(string buildTarget, string branchName)
		{
			var token = BuildPipelineSettings.PipelineTriggerToken;
			var projectId = BuildPipelineSettings.ProjectId;
			var pipelineName = BuildPipelineSettings.RemotePipelineName;
			var domain = GitHelper.GetDomain();
			var requestUrl =
				$"{domain}/api/v4/projects/{projectId}/trigger/pipeline?token={token}&ref={branchName}&variables[PIPELINE_NAME]={pipelineName}&variables[TARGET_PLATFORM]={buildTarget}";
			Debug.Log($"[{nameof(BuildPipelineWindow)}] Send request");
			StartCoroutine(ExecuteWebRequest(requestUrl, branchName));
		}

		private static IEnumerator ExecuteWebRequest(string requestUrl, string branchName)
		{
			const int timeout = 10;
			const string title = "BuildPipeline";
			const string info = "Wait response...";
			EditorUtility.DisplayProgressBar(title, info, 0);
			var webRequest = new WebRequestHandler(requestUrl, branchName, timeout);
			var startup = Time.realtimeSinceStartup;
			var completeTimeS = startup + timeout;

			webRequest.Execute();
			while (!webRequest.IsCompleted)
			{
				var elapsedTimeS = completeTimeS - startup;
				var progress = elapsedTimeS / timeout;
				EditorUtility.DisplayProgressBar(title, info, progress);
				yield return null;
			}

			EditorUtility.ClearProgressBar();

			if (webRequest.WebException != null)
				throw new Exception($"[{nameof(BuildPipelineWindow)}] {webRequest.WebException}");

			Debug.Log($"[{nameof(BuildPipelineWindow)}] <color=green>Build execute successful</color>");
		}

		private static void StartCoroutine(IEnumerator enumerator)
		{
			void Coroutine()
			{
				if (enumerator.MoveNext())
				{
					if (enumerator.Current != null)
						enumerator = (IEnumerator)enumerator.Current;
				}
				else
				{
					EditorApplication.update -= Coroutine;
				}
			}

			EditorApplication.update += Coroutine;
		}

		private class WebRequestHandler
		{
			private readonly string _branchName;
			private readonly int _timeoutS;
			private readonly string _url;

			public WebException WebException;

			public WebRequestHandler(string url, string branchName, int timeoutS)
			{
				_url = url;
				_branchName = branchName;
				_timeoutS = timeoutS;
			}

			public bool IsCompleted { get; private set; }

			public void Execute()
			{
				var data = new Dictionary<string, string>();
				var request = UnityWebRequest.Post(_url, data);
				var operation = request.SendWebRequest();

				void ResponseCallback(AsyncOperation asyncOperation)
				{
					if (request.result != UnityWebRequest.Result.Success)
						WebException = new WebException(request.error);
					IsCompleted = true;
				}

				operation.completed += ResponseCallback;
			}
		}
	}
}