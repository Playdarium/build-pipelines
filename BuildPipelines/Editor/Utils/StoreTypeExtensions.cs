using System;
using Playdarium.BuildPipelines.Runtime;

namespace Playdarium.BuildPipelines.Utils
{
	public static class StoreTypeExtensions
	{
		public static string ToStorePrefix(this EStoreType storeType)
		{
			switch (storeType)
			{
				case EStoreType.Editor:
					return "ed";
				case EStoreType.GooglePlay:
					return "gp";
				case EStoreType.AppStore:
					return "ios";
				case EStoreType.Amazon:
					return "am";
				case EStoreType.Huawei:
					return "hu";
				case EStoreType.Samsung:
					return "sm";
				default:
					throw new ArgumentOutOfRangeException(nameof(storeType), storeType, null);
			}
		}
	}
}