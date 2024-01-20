using System;
using Playdarium.BuildPipelines.Runtime;
using UnityEditor;

namespace Playdarium.BuildPipelines.Parameters
{
	public static class BuildParametersHolderExtensions
	{
		private const string BUILD_TARGET = "buildTarget";
		private const string BUILD_PATH = "buildPath";
		private const string BUILD_TYPE = "buildType";
		private const string STORE_TYPE = "storeType";
		private const string BUILD_IN_SCENES = "buildInScenes";
		private const string BUILD_APP_BUNDLE = "buildAppBundle";

		public static void SetBuildTarget(this BuildParameterHolder holder, BuildTarget buildTarget)
			=> holder.SetInt(BUILD_TARGET, (int)buildTarget);

		public static BuildTarget GetBuildTarget(this BuildParameterHolder holder)
			=> (BuildTarget)holder.GetInt(BUILD_TARGET);

		public static BuildTargetGroup GetBuildTargetGroup(this BuildParameterHolder holder)
		{
			var buildTarget = holder.GetBuildTarget();
			switch (buildTarget)
			{
				case BuildTarget.iOS:
					return BuildTargetGroup.iOS;
				case BuildTarget.Android:
					return BuildTargetGroup.Android;
				case BuildTarget.WebGL:
					return BuildTargetGroup.WebGL;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static void SetBuildPath(this BuildParameterHolder holder, string buildPath)
			=> holder.SetString(BUILD_PATH, buildPath);

		public static string GetBuildPath(this BuildParameterHolder holder)
			=> holder.GetString(BUILD_PATH);

		public static void SetStoreType(this BuildParameterHolder holder, EStoreType storeType)
			=> holder.SetInt(STORE_TYPE, (int)storeType);

		public static EStoreType GetStoreType(this BuildParameterHolder holder)
			=> (EStoreType)holder.GetInt(STORE_TYPE);

		public static void SetBuildType(this BuildParameterHolder holder, EBuildType buildType)
			=> holder.SetInt(BUILD_TYPE, (int)buildType);

		public static EBuildType GetBuildType(this BuildParameterHolder holder)
			=> (EBuildType)holder.GetInt(BUILD_TYPE);

		public static string[] GetBuildInScenes(this BuildParameterHolder holder)
			=> holder.GetStringArray(BUILD_IN_SCENES);

		public static void SetBuildInScenes(this BuildParameterHolder holder, string[] scenes)
			=> holder.SetStringArray(BUILD_IN_SCENES, scenes);

		public static bool GetBuildAppBundle(this BuildParameterHolder holder)
			=> holder.GetBool(BUILD_APP_BUNDLE);

		public static void SetBuildAppBundle(this BuildParameterHolder holder, bool buildAppBundle)
			=> holder.SetBool(BUILD_APP_BUNDLE, buildAppBundle);
	}
}