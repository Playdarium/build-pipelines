using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Playdarium.BuildPipelines.Utils
{
	public static class Binary
	{
		private static readonly BinaryFormatter _bf = new();

		public static byte[] Serialize(object o)
		{
			using var ms = new MemoryStream();
			_bf.Serialize(ms, o);
			return ms.ToArray();
		}

		public static T Deserialize<T>(byte[] bytes)
		{
			using var ms = new MemoryStream(bytes);
			var obj = _bf.Deserialize(ms);
			return (T)obj;
		}
	}
}