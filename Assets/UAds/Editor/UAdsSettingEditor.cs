using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UAdsSettingEditor : EditorWindow
{

	private UAdsSetting setting;
	private bool enableAdColony;
	private ScriptDefineSymbol adcolonySymbol;

	private BuildTargetGroup[] groups = new[] { BuildTargetGroup.Android, BuildTargetGroup.iOS };

	[MenuItem("UAds/Show SettingWindow")]
	public static void ShowWindow()
	{

		var window = GetWindow<UAdsSettingEditor>();
		window.Show();
	}

	// constructorと同じ役割
	private void OnEnable()
	{
		this.setting = UAdsSettingHelper.LoadOrCreateUAdsSettings();
		this.adcolonySymbol = new ScriptDefineSymbol(groups, "ENABLE_ADCOLONY");
		this.enableAdColony = this.adcolonySymbol.HasDefine();
	}

	public void OnGUI()
	{
		// compile中は表示する.
		if (EditorApplication.isCompiling)
			EditorGUILayout.LabelField("compiling...");


#if UNITY_ADS
		using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
			EditorGUILayout.LabelField("UnityAds");
			using (new EditorGUI.IndentLevelScope()) {
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.PrefixLabel("androidGameId");
					setting.unityAds.androidGameId = EditorGUILayout.TextField(setting.unityAds.androidGameId);
				}
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.PrefixLabel("iOSGameId");
					setting.unityAds.iOSGameId = EditorGUILayout.TextField(setting.unityAds.iOSGameId);
				}
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.PrefixLabel("rewardVideoZoneId");
					setting.unityAds.rewardVideoZoneId = EditorGUILayout.TextField(setting.unityAds.rewardVideoZoneId);
				}
			}
		}
#endif

		#region AdColony

		EditorGUILayout.Space();
		using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
			EditorGUILayout.LabelField("AdColony");
			using (new EditorGUI.IndentLevelScope()) {
				var tmp = EditorGUILayout.ToggleLeft("Enable AdColony", enableAdColony);
				if (tmp != enableAdColony) {
					enableAdColony = tmp;
					if (enableAdColony) {
						adcolonySymbol.SetDefine();
					} else {
						adcolonySymbol.RemoveDefine();
					}
					// defineを変更したのでconmpileさせる
					AssetDatabase.Refresh();
				}

#if ENABLE_ADCOLONY
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.PrefixLabel("androidGameId");
					setting.unityAds.androidGameId = EditorGUILayout.TextField(setting.unityAds.androidGameId);
				}
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.PrefixLabel("iOSGameId");
					setting.unityAds.iOSGameId = EditorGUILayout.TextField(setting.unityAds.iOSGameId);
				}
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.PrefixLabel("rewardVideoZoneId");
					setting.unityAds.rewardVideoZoneId = EditorGUILayout.TextField(setting.unityAds.rewardVideoZoneId);
				}
#endif
			}
		}

		#endregion

	}
}
