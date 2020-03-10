using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InspectorDictionary
{
	public string key;
	public string value;

	public static InspectorDictionary [] MakeDict(Dictionary<string,object> dict) {


		List <InspectorDictionary> list = new List <InspectorDictionary>();

		try {
			foreach (string key in dict.Keys) {
				InspectorDictionary keyValue = new InspectorDictionary ();
				keyValue.key = key;
				if (dict[key] != null) {

					var value = dict [key];
//					if (value.GetType() is Dictionary<string,object>) {
//						//TODO
//					} else if (value.GetType () is List<object>) {
//						//TODO
//					} else {
						keyValue.value = dict[key].ToString();
//					}
				}
				list.Add (keyValue);
			}
		} catch {
			Debug.LogWarning ("[INSPECTOR DICTIONARY] - Error parsing dictionary: "+dict.GetType());
		}

		return list.ToArray();
	}

}
