using UnityEngine;

namespace Playdarium.BuildPipelines.Runtime.Impls
{
	[CreateAssetMenu(menuName = "Settings/BuildSettings", fileName = "BuildSettings")]
	public class BuildSetting : ScriptableObject, IBuildSetting
	{
		[SerializeField] private EBuildType buildType;
		[SerializeField] private EStoreType storeType;

		public EBuildType BuildType => buildType;

		public EStoreType StoreType => storeType;
	}
}
