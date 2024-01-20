using System;
using System.Collections.Generic;
using Playdarium.BuildPipelines.Settings;
using Playdarium.BuildPipelines.Utils;

namespace Playdarium.BuildPipelines.Parameters
{
	public class BuildParameterHolder
	{
		private readonly Dictionary<string, dynamic> _parameters = new();

		public BuildParameterHolder()
		{
			SaveState();
		}

		public BuildParameterHolder(byte[] bytes)
		{
			var state = Binary.Deserialize<State>(bytes);

			for (var i = 0; i < state.Keys.Length; i++)
			{
				var key = state.Keys[i];
				var value = state.Values[i];
				_parameters.Add(key, value);
			}
		}

		public bool HasKey(string key) => _parameters.ContainsKey(key);

		public string GetString(string key, string defaultValue = default) => GetInternal(key, defaultValue);

		public void SetString(string key, string value) => SetInternal(key, value);

		public string[] GetStringArray(string key, string[] defaultValue = default) => GetInternal(key, defaultValue);

		public void SetStringArray(string key, string[] value) => SetInternal(key, value);

		public long GetLong(string key, long defaultValue = default) => GetInternal(key, defaultValue);

		public void SetLong(string key, long value) => SetInternal(key, value);

		public int GetInt(string key, int defaultValue = default) => GetInternal(key, defaultValue);

		public void SetInt(string key, int value) => SetInternal(key, value);

		public float GetFloat(string key, float defaultValue = default) => GetInternal(key, defaultValue);

		public void SetFloat(string key, float value) => SetInternal(key, value);

		public bool GetBool(string key, bool defaultValue = default) => GetInternal(key, defaultValue);

		public void SetBool(string key, bool value) => SetInternal(key, value);

		private dynamic GetInternal(string key, dynamic defaultValue)
			=> _parameters.TryGetValue(key, out var value) ? value : defaultValue;

		private void SetInternal(string key, dynamic value)
		{
			_parameters[key] = value;
			SaveState();
		}

		private void SaveState()
		{
			var state = new State(_parameters.Count);

			var index = 0;
			foreach (var kvp in _parameters)
			{
				state.Keys[index] = kvp.Key;
				state.Values[index++] = kvp.Value;
			}

			var bytes = Binary.Serialize(state);
			PipelineSequenceUtils.ParametersHolderBytes = bytes;
		}

		[Serializable]
		public class State
		{
			public string[] Keys { get; set; }
			public dynamic[] Values { get; set; }

			public State()
			{
			}

			public State(int length)
			{
				Keys = new string[length];
				Values = new dynamic[length];
			}
		}
	}
}