using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Utils
{
	public static class PipelineError
	{
		public static void Throw(Exception e)
		{
			Debug.LogException(e);
			if (Environment.GetCommandLineArgs().All(f => f != "-batchmode"))
				return;

			EditorApplication.Exit(1);
		}
	}
}