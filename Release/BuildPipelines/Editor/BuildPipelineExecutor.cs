using System;
using System.Diagnostics;
using System.Linq;
using Playdarium.BuildPipelines.Parameters;
using Playdarium.BuildPipelines.Settings;
using Playdarium.BuildPipelines.Utils;
using Playdarium.BuildPipelines.Utils.CommandArgs;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Playdarium.BuildPipelines
{
	[InitializeOnLoad]
	public static class BuildPipelineExecutor
	{
		static BuildPipelineExecutor()
		{
			Debug.Log($"[{nameof(BuildPipelineExecutor)}] Initialize");
			AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
		}

		private static void OnAfterAssemblyReload()
		{
			Debug.Log($"[{nameof(BuildPipelineExecutor)}] OnAfterAssemblyReload");
			if (Process.GetCurrentProcess().Id != PipelineSequenceUtils.ProcessId)
			{
				PipelineSequenceUtils.Clear();
				return;
			}

			var pipelineName = PipelineSequenceUtils.PipelineName;
			RestorePipeline(pipelineName);
		}

		public static void Build()
		{
			PipelineSequenceUtils.Clear();
			PipelineSequenceUtils.ProcessId = Process.GetCurrentProcess().Id;
			var commandArguments = new CommandArguments();
			var pipelineName = commandArguments.GetPipelineName();
			PipelineSequenceUtils.PipelineName = pipelineName;
			Debug.Log($"[{nameof(BuildPipelineExecutor)}] Execute pipeline: {pipelineName}");
			var buildPath = commandArguments.GetBuildPath();
			ExecutePipeline(pipelineName, buildPath);
		}

		public static void ExecutePipeline(string pipelineName, string buildPath)
		{
			var pipeline = FindPipeline(pipelineName);
			var buildParameterHolder = new BuildParameterHolder();
			buildParameterHolder.SetBuildPath(buildPath);
			ExecutePipeline(buildParameterHolder, pipeline);
		}

		private static void RestorePipeline(string pipelineName)
		{
			Debug.Log($"[{nameof(BuildPipelineExecutor)}] Restore pipeline: {pipelineName}");
			var pipeline = FindPipeline(pipelineName);
			var buildParameterHolder = new BuildParameterHolder(PipelineSequenceUtils.ParametersHolderBytes);
			ExecutePipeline(buildParameterHolder, pipeline);
		}

		private static BuildProjectPipeline FindPipeline(string pipelineName)
		{
			var pipelines = PipelineUtils.FindAll();
			var pipeline = pipelines.SingleOrDefault(p => p.PipelineName == pipelineName);

			if (pipeline == null)
				throw new Exception($"[{nameof(BuildPipelineExecutor)}] No pipeline with name: {pipelineName}");

			return pipeline;
		}

		private static void ExecutePipeline(BuildParameterHolder parameterHolder, BuildProjectPipeline pipeline)
		{
			void OnCompleted(Exception ex)
			{
				PipelineSequenceUtils.Clear();

				if (ex == null)
				{
					Debug.Log($"[{nameof(BuildPipelineExecutor)}] Completed success, exit from editor.");
					return;
				}

				PipelineError.Throw(ex);
			}

			pipeline.Execute(parameterHolder, OnCompleted);
		}
	}
}