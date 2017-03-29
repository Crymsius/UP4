using System;
using System.Collections.Generic;
using UnityEngine;

//The forech return item
public struct DictionaryItem<TKey, TValue>{
	public readonly TKey Key;
	public readonly TValue Value;
	public DictionaryItem(TKey Key, TValue Value){
		this.Key = Key;
		this.Value = Value;
	}
}
//The Base Class
[System.Serializable]
public class MyDictionary<TKey, TValue> : System.Collections.Generic.IEnumerable<DictionaryItem<TKey, TValue>> {
	[SerializeField]
	protected List<TKey> _keys = new List<TKey> ();
	[SerializeField]
	protected List<TValue> _values = new List<TValue> ();

	public int Count{get{return _keys.Count;}}

	public System.Collections.Generic.IEnumerator<DictionaryItem<TKey,TValue>> GetEnumerator(){
		for (int i = 0; i < Count; i++) {
			yield return new DictionaryItem<TKey, TValue> (_keys [i], _values [i]);
		}
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator(){
		return GetEnumerator ();
	}

	public bool ContainsKey(TKey key){
		return _keys.Contains (key);
	}

	public bool ContainsValue(TValue value){
		return _values.Contains (value);
	}

	public void Add(TKey key, TValue value){
		if (!ContainsKey (key)) {
			_keys.Add (key);
			_values.Add (value);
			return;
		}
		throw new System.Exception ("The Key Already Exists In Our Dictionary.");
	}

	public bool Remove(TKey key){
		if (ContainsKey (key)) {
			int i = _keys.IndexOf (key);
			_keys.RemoveAt (i);
			_values.RemoveAt (i);
			return true;
		}
		return false;
	}

	public void Clear(){
		_keys = new List<TKey> ();
		_values = new List<TValue> ();
	}

	public TValue this[TKey key]{
		get{
			if (ContainsKey (key)) {
				return _values [_keys.IndexOf (key)];
			}
			throw new System.Exception ("The Key Does Not Exist In Our Dictionary.");
		}
		set{
			if (ContainsKey (key)) {
				_values [_keys.IndexOf (key)] = value;
				return;
			}
			Add (key, value);
		}
	}

	public TKey[] Keys{ get { return _keys.ToArray (); } }
	public TValue[] Values{ get { return _values.ToArray (); } }
}

////Inherit my dictionary<Some Key type, Some Value Type>
//[System.Serializable]
//public class OurDictionaryStringString : MyDictionary<string, string>{}
//
////Declair a pre defined dictionary (this will serialize)
//public OurDictionaryStringString myDict = new OurDictionaryStringString ();