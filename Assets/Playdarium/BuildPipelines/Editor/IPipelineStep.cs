using System;
using Playdarium.BuildPipelines.Parameters;

namespace Playdarium.BuildPipelines
{
	public interface IPipelineStep
	{
		string Name { get; }

		void Execute(BuildParameterHolder parameterHolder, Action onComplete);
	}
}