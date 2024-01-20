namespace Playdarium.BuildPipelines.Utils.CommandArgs
{
	public static class CommandArgumentsExtensions
	{
		private const string PIPELINE_NAME = "-pipelineName";
		private const string BUILD_PATH = "-buildPath";

		public static string GetPipelineName(this CommandArguments arguments)
			=> arguments.GetArgParameter(PIPELINE_NAME);

		public static string GetBuildPath(this CommandArguments arguments)
			=> arguments.GetArgParameter(BUILD_PATH);
	}
}