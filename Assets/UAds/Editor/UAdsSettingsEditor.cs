using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UAdsSettingsEditor : EditorWindow
{

	[MenuItem("UAds/CreateSettings")]
	public static void CreateAsset()
	{
		var asset = ScriptableObject.CreateInstance<UAdsSettings>();

		AssetDatabase.CreateAsset(asset, "Assets/UAds/Resources/UAdsSettings.asset");
		AssetDatabase.Refresh();
	}

	[MenuItem("UAds/Show SettingWindow")]
	public static void ShowWindow()
	{
		GetWindow<UAdsSettingsEditor>().Show();
	}

	public void OnGUI()
	{
#if UNITY_ADS
		EditorGUILayout.LabelField("UnityAds");
		using (new EditorGUI.IndentLevelScope()) {
			using (new EditorGUILayout.HorizontalScope()) {
				EditorGUILayout.PrefixLabel("androidGameId");
				EditorGUILayout.TextField("test");
			}
			using (new EditorGUILayout.HorizontalScope()) {
				EditorGUILayout.PrefixLabel("iOSGameId");
				EditorGUILayout.TextField("test");
			}
			using (new EditorGUILayout.HorizontalScope()) {
				EditorGUILayout.PrefixLabel("rewardVideoZoneId");
				EditorGUILayout.TextField("test");
			}
		}
#endif



		GUILayout.Button("Save");
	}
}
