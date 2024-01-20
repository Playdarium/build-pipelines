namespace Playdarium.BuildPipelines.Runtime
{
	public interface IBuildSetting
	{
		EBuildType BuildType { get; }

		EStoreType StoreType { get; }
	}
}