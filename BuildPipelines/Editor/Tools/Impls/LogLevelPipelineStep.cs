using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/LogLevel", fileName = "LogLevel")]
	public class LogLevelPipelineStep : APipelineStep
	{
		[SerializeField] private StackTraceLogType logLevel;
		[SerializeField] private StackTraceLogType warningLevel;
		[SerializeField] private StackTraceLogType errorLevel;
		[SerializeField] private StackTraceLogType assertLevel;
		[SerializeField] private StackTraceLogType exceptionLevel;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			PlayerSettings.SetStackTraceLogType(LogType.Log, logLevel);
			PlayerSettings.SetStackTraceLogType(LogType.Warning, warningLevel);
			PlayerSettings.SetStackTraceLogType(LogType.Error, errorLevel);
			PlayerSettings.SetStackTraceLogType(LogType.Assert, assertLevel);
			PlayerSettings.SetStackTraceLogType(LogType.Exception, exceptionLevel);

			onComplete();
		}
	}
}