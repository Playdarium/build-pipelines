using UnityEditor;

namespace Playdarium.BuildPipelines.Utils
{
	public class ChooseBuildPath
	{
		private static string _oldPath;
		private static string _key;

		public static string Choose(string name)
		{
			_key = $"BuildTool.{name}.LastPath";
			var path = ChoosePath();
			if (string.IsNullOrEmpty(path))
			{
				UnityEngine.Debug.Log("[BuildTools] Build canceled");
				return null;
			}

			SaveBuildPath();
			return path;
		}

		private static string ChoosePath()
		{
			_oldPath = EditorPrefs.HasKey(_key) ? EditorPrefs.GetString(_key) : "";
			_oldPath = EditorUtility.SaveFolderPanel("Choose Location of Built", _oldPath, "");
			return _oldPath;
		}

		private static void SaveBuildPath() => EditorPrefs.SetString(_key, _oldPath);
	}
}