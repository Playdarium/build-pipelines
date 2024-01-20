using System;

namespace Playdarium.BuildPipelines.Runtime
{
	[Flags]
	public enum EStoreType
	{
		Editor = 1,
		GooglePlay = 2,
		AppStore = 4,
		Amazon = 8,
		Huawei = 16,
		Samsung = 32
	}
}