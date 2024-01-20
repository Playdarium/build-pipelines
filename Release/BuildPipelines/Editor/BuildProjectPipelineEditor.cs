using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Playdarium.BuildPipelines
{
	[CustomEditor(typeof(BuildProjectPipeline))]
	public class BuildProjectPipelineEditor : UnityEditor.Editor
	{
		private ReorderableList _list;
		private SerializedProperty _pipelineNameSp;
		private SerializedProperty _buildTargetSp;

		private void OnEnable()
		{
			_buildTargetSp = serializedObject.FindProperty("buildTarget");
			var pipelinesSp = serializedObject.FindProperty("steps");
			_list = new ReorderableList(serializedObject, pipelinesSp, true, true, true, true);
			_list.drawHeaderCallback += rect => GUI.Label(rect, "Pipeline steps");
			_list.drawElementCallback += (rect, index, active, focused) =>
			{
				var element = pipelinesSp.GetArrayElementAtIndex(index);
				var obj = element.objectReferenceValue;
				EditorGUI.ObjectField(rect, element, typeof(APipelineStep),
					new GUIContent(obj == null ? "null" : obj.name));
			};
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.PropertyField(_buildTargetSp);
			_list.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}
	}
}