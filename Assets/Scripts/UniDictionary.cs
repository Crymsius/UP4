using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniTools
{
	/// <summary>
	/// Dictionary that is Unity serializable.
	/// Simply two lists. Missing some common methods.
	/// </summary>
	/// <typeparam name="Key"></typeparam>
	/// <typeparam name="Value"></typeparam>
	[Serializable]
	public class UniDictionary<Key, Value>
	{
		[SerializeField]
		private List<Key> keys = new List<Key>();

		[SerializeField]
		private List<Value> values = new List<Value>();

		public void Add(Key key, Value value)
		{
			if (keys.Contains(key))
				return;

			keys.Add(key);
			values.Add(value);
		}

		public void Remove(Key key)
		{
			if (!keys.Contains(key))
				return;

			int index = keys.IndexOf(key);

			keys.RemoveAt(index);
			values.RemoveAt(index);
		}

		public bool TryGetValue(Key key, out Value value)
		{
			if (keys.Count != values.Count)
			{
				keys.Clear();
				values.Clear();
				value = default(Value);
				return false;
			}

			if (!keys.Contains(key))
			{
				value = default(Value);
				return false;
			}

			int index = keys.IndexOf(key);
			value = values[index];

			return true;
		}
//
//		// save the dictionary to lists
//		public void OnBeforeSerialize()
//		{
//			keys.Clear();
//			values.Clear();
//			foreach(KeyValuePair<Key, Value> pair in this)
//			{
//				keys.Add(pair.Key);
//				values.Add(pair.Value);
//			}
//		}

		public void ChangeValue(Key key, Value value)
		{
			if (!keys.Contains(key))
				return;

			int index = keys.IndexOf(key);

			values[index] = value;
		}
	}
}