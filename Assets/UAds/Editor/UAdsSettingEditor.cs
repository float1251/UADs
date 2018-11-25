using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UAds.Editor
{
	public class UAdsSettingEditor : EditorWindow
	{

		private UAdsSetting setting;
		private bool enableAdColony;
		private ScriptDefineSymbol adcolonySymbol;
		private ScriptDefineSymbol unityMonetizationSymbol;
		private BuildTargetGroup[] groups = new[] { BuildTargetGroup.Android, BuildTargetGroup.iOS };
		private bool enableUnityMonetization;

		[MenuItem("Tools/UAds/Show SettingWindow")]
		public static void ShowWindow()
		{

			var window = GetWindow<UAdsSettingEditor>();
			window.Show();
		}

		// constructorと同じ役割
		private void OnEnable()
		{
			this.setting = UAdsSettingHelper.LoadOrCreateUAdsSettings();
			this.adcolonySymbol = new ScriptDefineSymbol(groups, UAdsSettingHelper.ADCOLONY_DEFINE);
			this.unityMonetizationSymbol = new ScriptDefineSymbol(groups, UAdsSettingHelper.UNITY_MONETIZATION);
			this.enableAdColony = this.adcolonySymbol.HasDefine();
			this.enableUnityMonetization = this.unityMonetizationSymbol.HasDefine();
		}

		public void OnGUI()
		{
			// compile中は表示する.
			if (EditorApplication.isCompiling)
				EditorGUILayout.LabelField("compiling...");

			using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
				EditorGUILayout.LabelField("UnityAds");
				var tmpEnableMonetization = EditorGUILayout.ToggleLeft("Enable UnityMonetization", this.enableUnityMonetization);
				if (tmpEnableMonetization != enableUnityMonetization) {
					this.enableUnityMonetization = tmpEnableMonetization;
					setting.enableUnityMonetization = enableUnityMonetization;
					if (enableUnityMonetization) {
						this.unityMonetizationSymbol.SetDefine();
					} else {
						unityMonetizationSymbol.RemoveDefine();
					}
					// defineを変更したのでconmpileさせる
					AssetDatabase.Refresh();
				}

#if UNITY_ADS || UNITY_MONETIZATION
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
						setting.unityAds.rewardVideoPlacementId = EditorGUILayout.TextField(setting.unityAds.rewardVideoPlacementId);
					}
				}
#endif
			}

			#region AdColony

			EditorGUILayout.Space();
			DisplayAdcolonyLayout(this.setting);
			#endregion

			using (new EditorGUILayout.HorizontalScope()) {
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Export")) {
					var path = EditorUtility.SaveFilePanel("Export Json", Application.dataPath, "export_setting.json", "json");
					UAdsSettingHelper.Export(path, JsonUtility.ToJson(setting));
				}
				if (GUILayout.Button("Import")) {
					var path = EditorUtility.OpenFilePanel("Import Json", Application.dataPath, "json");
					UAdsSettingHelper.Import(path, ref this.setting);
					UAdsSettingHelper.SaveUAdsSettings(this.setting);
				}
				if (GUILayout.Button("Save")) {
					UAdsSettingHelper.SaveUAdsSettings(setting);
				}
			}
		}

		private void DisplayAdcolonyLayout(UAdsSetting setting)
		{
			using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
				EditorGUILayout.LabelField("AdColony");
				using (new EditorGUI.IndentLevelScope()) {
					var tmp = EditorGUILayout.ToggleLeft("Enable AdColony", enableAdColony);
					if (tmp != enableAdColony) {
						enableAdColony = tmp;
						setting.enableAdcolony = enableAdColony;
						if (enableAdColony) {
							adcolonySymbol.SetDefine();
						} else {
							adcolonySymbol.RemoveDefine();
						}
						// defineを変更したのでconmpileさせる
						AssetDatabase.Refresh();
					}

#if ENABLE_ADCOLONY
					using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
						EditorGUILayout.LabelField("Android");
						DisplayAdcolonyIndividualLayout(setting.adColony.androidSetting, "Android");
					}

					using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
						EditorGUILayout.LabelField("iOS");
						DisplayAdcolonyIndividualLayout(setting.adColony.iOSSetting, "iOS");
					}
#endif
				}
			}
		}

		private void DisplayAdcolonyIndividualLayout(UAdsSetting.AdColoySetting.Setting setting, string os)
		{
			using (new EditorGUILayout.HorizontalScope()) {
				EditorGUILayout.PrefixLabel(string.Format("appId", os));
				setting.appId = EditorGUILayout.TextField(setting.appId);
			}
			// using (new EditorGUILayout.HorizontalScope()) {
			// 	EditorGUILayout.PrefixLabel(string.Format("RewardZoneName", os));
			// 	setting.rewardZoneName = EditorGUILayout.TextField(setting.rewardZoneName);
			// }
			using (new EditorGUILayout.HorizontalScope()) {
				EditorGUILayout.PrefixLabel(string.Format("RewardZoneID", os));
				setting.rewardZoneId = EditorGUILayout.TextField(setting.rewardZoneId);
			}
		}

	}
}