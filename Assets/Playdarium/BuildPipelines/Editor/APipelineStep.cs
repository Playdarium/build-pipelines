using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEngine;

namespace Playdarium.BuildPipelines
{
	public abstract class APipelineStep : ScriptableObject, IPipelineStep
	{
		public virtual string Name => name;

		public abstract void Execute(BuildParameterHolder parameterHolder, Action onComplete);
	}
}