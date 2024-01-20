using System;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Playdarium.BuildPipelines.PostProcess.iOS
{
	public static class ProvisionXmlUtils
	{
		public static string GetProvisionUUID(EProvisionProfileType profileType)
		{
			var provisionXml = GetProvisionXml(profileType);
			var dict = ReadDict(provisionXml);
			return GetProvisionKeyValue("UUID", dict).InnerText;
		}

		public static string GetProvisionName(EProvisionProfileType profileType)
		{
			var provisionXml = GetProvisionXml(profileType);
			var dict = ReadDict(provisionXml);
			return GetProvisionKeyValue("Name", dict).InnerText;
		}

		private static XmlDocument GetProvisionXml(EProvisionProfileType profileType)
		{
			var provisionAssetPath = profileType == EProvisionProfileType.Development
				? ExportOptions.DevelopmentProvisionPath
				: ExportOptions.DistributionProvisionPath;

			if (string.IsNullOrEmpty(provisionAssetPath))
				throw new Exception(
					$"[{nameof(ProvisionXmlUtils)}] Path to provision profile with type '{profileType}' is empty.");

			var assetsDir = new DirectoryInfo(Application.dataPath);
			var projectDir = assetsDir.Parent;
			var filePath = Path.Combine(projectDir.FullName, provisionAssetPath);
			if (!File.Exists(filePath))
				throw new Exception(
					$"[{nameof(ProvisionXmlUtils)}] Cannot find provision files '{filePath}'");

			return GetProvisionXml(filePath);
		}

		private static XmlDocument GetProvisionXml(string provisionPath)
		{
			const string startXmlText = "<?xml";
			const string endXmlText = "</plist>";

			var text = File.ReadAllText(provisionPath);
			var startIndex = text.IndexOf(startXmlText, StringComparison.Ordinal);
			var endIndex = text.IndexOf(endXmlText, startIndex, StringComparison.Ordinal);
			var provisionXml = text.Substring(startIndex, endIndex - startIndex + endXmlText.Length);

			var xDoc = new XmlDocument();
			xDoc.LoadXml(provisionXml);
			return xDoc;
		}

		private static XmlNodeList ReadDict(XmlDocument xDoc)
		{
			var document = xDoc.DocumentElement;
			var dict = document.GetElementsByTagName("dict").Item(0);
			return dict.ChildNodes;
		}

		private static XmlNode GetProvisionKeyValue(string key, XmlNodeList list)
		{
			for (var i = 0; i < list.Count; i++)
			{
				var node = list.Item(i);
				if (node != null && node.Name == "key" && node.InnerText == key)
					return list.Item(i + 1);
			}

			return null;
		}

		private static string GetApsEnvironmentName(EProvisionProfileType profileType)
		{
			switch (profileType)
			{
				case EProvisionProfileType.Development:
					return "development";
				case EProvisionProfileType.Distribution:
					return "production";
				default:
					throw new ArgumentOutOfRangeException(nameof(profileType), profileType, null);
			}
		}
	}
}