using System;

namespace Playdarium.BuildPipelines.Utils
{
	public interface ISequence
	{
		void Do(Action onComplete);
	}
}