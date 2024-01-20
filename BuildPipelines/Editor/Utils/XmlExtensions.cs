using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Playdarium.BuildPipelines.Utils
{
	public static class XmlExtensions
	{
		public static IEnumerable<XmlElement> ToEnumerable(this XmlElement element)
		{
			var elements = new XmlElement[element.ChildNodes.Count];
			var index = 0;
			foreach (XmlElement node in element)
				elements[index++] = node;
			return elements;
		}

		public static IEnumerable<XmlAttribute> FromAttributes(this XmlElement element)
		{
			var elements = new XmlAttribute[element.Attributes.Count];
			var index = 0;
			foreach (XmlAttribute node in element.Attributes)
				elements[index++] = node;
			return elements;
		}

		public static string GetAttributeValue(this XmlElement element, string name)
			=> element.FromAttributes().First(f => f.Name == name).Value;

		public static bool ContainAttribute(this XmlElement element, string name)
			=> element.FromAttributes().Any(f => f.Name == name);
	}
}