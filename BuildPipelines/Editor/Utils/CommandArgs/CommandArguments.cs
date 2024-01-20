using System;

namespace Playdarium.BuildPipelines.Utils.CommandArgs
{
	public class CommandArguments
	{
		private readonly string[] _args;

		public CommandArguments()
		{
			_args = Environment.GetCommandLineArgs();
		}

		public string GetArgParameter(string key)
		{
			for (var i = 0; i < _args.Length; i++)
			{
				var arg = _args[i];
				if (arg == key)
					return _args[i + 1];
			}

			return string.Empty;
		}
	}
}