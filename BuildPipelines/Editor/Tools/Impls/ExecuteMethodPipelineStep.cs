using System;
using System.Linq;
using System.Reflection;
using Playdarium.BuildPipelines.Parameters;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/ExecuteMethodPipelineStep",
		fileName = "ExecuteMethodPipelineStep")]
	public class ExecuteMethodPipelineStep : APipelineStep
	{
		[SerializeField] private string methodName;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			var args = methodName.Split(".", StringSplitOptions.RemoveEmptyEntries);
			var typeFullName = string.Join(".", args.Take(args.Length - 1));
			
			try
			{
				var type = AppDomain.CurrentDomain.GetAssemblies()
					.Select(s => s.GetTypes())
					.Aggregate((a, b) => a.Concat(b).ToArray())
					.Single(f => f.FullName == typeFullName);
				var method = type.GetMethod(args[^1], BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				method.Invoke(null, Array.Empty<object>());
			}
			catch (Exception e)
			{
				throw new Exception($"[{nameof(ExecuteMethodPipelineStep)}] Cannot execute method '{methodName}'");
			}
		}

		[ContextMenu("Execute")]
		public void Execute() => Execute(new BuildParameterHolder(), () => { });
	}
}