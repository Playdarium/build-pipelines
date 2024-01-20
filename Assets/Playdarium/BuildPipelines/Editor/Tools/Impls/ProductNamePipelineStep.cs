using System;
using Playdarium.BuildPipelines.Parameters;
using UnityEditor;
using UnityEngine;

namespace Playdarium.BuildPipelines.Tools.Impls
{
	[CreateAssetMenu(menuName = "BuildPipeline/Steps/ProductName", fileName = "ProductName")]
	public class ProductNamePipelineStep : APipelineStep
	{
		[SerializeField] private string productName;

		public override void Execute(BuildParameterHolder parameterHolder, Action onComplete)
		{
			PlayerSettings.productName = productName;
			onComplete();
		}
	}
}