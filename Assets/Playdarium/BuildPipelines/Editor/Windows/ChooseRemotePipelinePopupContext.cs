using System.Collections.Generic;
using System.Linq;
using Playdarium.BuildPipelines.Settings;
using Playdarium.BuildPipelines.Utils;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Windows
{
	public class ChooseRemotePipelinePopupContext : PopupWindowContent
	{
		private List<string> _projectPipelines;

		public override void OnOpen()
		{
			_projectPipelines = new List<string> { PipelineUtils.EMPTY };
			_projectPipelines.AddRange(PipelineUtils.FindAll().Select(s => s.PipelineName));
		}

		public override void OnGUI(Rect rect)
		{
			var remotePipelineName = BuildPipelineSettings.RemotePipelineName;
			foreach (var pipeline in _projectPipelines)
			{
				using var change = new EditorGUI.ChangeCheckScope();
				var toggle = EditorGUILayout.Toggle(pipeline, remotePipelineName == pipeline);
				if (!change.changed)
					continue;

				if (pipeline != PipelineUtils.EMPTY)
					BuildPipelineSettings.RemotePipelineName = toggle ? pipeline : PipelineUtils.EMPTY;
				else
					BuildPipelineSettings.RemotePipelineName = PipelineUtils.EMPTY;
			}
		}
	}
}