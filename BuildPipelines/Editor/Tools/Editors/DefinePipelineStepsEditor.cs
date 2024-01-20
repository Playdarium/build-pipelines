using System;
using System.Collections.Generic;
using Playdarium.BuildPipelines.Tools.Impls;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Editors
{
	[CustomEditor(typeof(DefinePipelineStep))]
	public class DefinePipelineStepsEditor : UnityEditor.Editor
	{
		private ReorderableList _list;
		private DefinesHolder _definesHolder;
		private SerializedObject _defineHolderSo;

		private void OnEnable()
		{
			var definesSp = serializedObject.FindProperty("defines");
			_definesHolder = CreateInstance<DefinesHolder>();
			_definesHolder.Set(definesSp.stringValue);
			_defineHolderSo = new SerializedObject(_definesHolder);
			var definesListSp = _defineHolderSo.FindProperty("defines");
			_list = new ReorderableList(serializedObject, definesListSp, true, true, true, true);
			_list.drawHeaderCallback += rect => GUI.Label(rect, "Defines");
			_list.drawElementCallback += (rect, index, active, focused) =>
			{
				var element = definesListSp.GetArrayElementAtIndex(index);
				element.stringValue = EditorGUI.TextField(rect, element.stringValue);
			};
		}

		private void OnDisable()
		{
			DestroyImmediate(_definesHolder);
		}

		public override void OnInspectorGUI()
		{
			_list.DoLayoutList();
			_defineHolderSo.ApplyModifiedProperties();
			var definesSp = serializedObject.FindProperty("defines");
			definesSp.stringValue = _definesHolder.ToString();
			serializedObject.ApplyModifiedProperties();
		}

		[Serializable]
		private class DefinesHolder : ScriptableObject
		{
			[SerializeField] private List<string> defines;

			public void Set(string defines)
			{
				this.defines = new List<string>(defines.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
			}

			public override string ToString() => string.Join(";", defines);
		}
	}
}