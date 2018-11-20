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

		public static readonly string ADCOLONY_DEFINE = "ENABLE_ADCOLONY";
	}

}