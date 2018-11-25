using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace UAds.Editor
{
	public static class UAdsSettingHelper
	{
		public static UAdsSetting LoadOrCreateUAdsSettings()
		{
			var asset = AssetDatabase.LoadAssetAtPath<UAdsSetting>("Assets/UAds/UAdsSettings.asset");
			if (asset != null) {
				return asset;
			}

			asset = ScriptableObject.CreateInstance<UAdsSetting>();

			AssetDatabase.CreateAsset(asset, "Assets/UAds/UAdsSettings.asset");
			AssetDatabase.Refresh();
			return asset;
		}

		public static void SaveUAdsSettings(UAdsSetting setting)
		{
			EditorUtility.SetDirty(setting);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static readonly string ADCOLONY_DEFINE = "ENABLE_ADCOLONY";
		public static readonly string UNITY_MONETIZATION = "UNITY_MONETIZATION";

		public static void Export(string path, string json)
		{
			System.IO.File.WriteAllText(path, json);
		}
		public static void Import(string path, ref UAdsSetting setting)
		{
			var json = System.IO.File.ReadAllText(path);
			if (json.Length == 0) {
				return;
			}
			// https://docs.unity3d.com/Manual/JSONSerialization.html
			// MonobehaviorやScriptableObjectはFromJsonOverwriteを使う必要がある模様.
			JsonUtility.FromJsonOverwrite(json, setting);
		}
	}


}