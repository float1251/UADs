using UnityEngine;
using UnityEditor;

public class ExportPackage : ScriptableObject
{
	[MenuItem("Tools/UAds/Export Release Package")]
	static void DoIt()
	{
		var txt = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/UAds/VERSION.txt");
		var setting = AssetDatabase.LoadAssetAtPath<UAds.UAdsSetting>("Assets/UAds/UAdsSettings.asset");
		// データ初期化前に設定保持しておき、終了したら復元する
		var original = EditorJsonUtility.ToJson(setting);

		//  export前に初期設定にする
#if UNITY_ADS || UNITY_MONETIZATION
		setting.unityAds.androidGameId = "";
		setting.unityAds.iOSGameId = "";
#endif

#if ENABLE_ADCOLONY
		setting.adColony.androidSetting = new UAds.UAdsSetting.AdColoySetting.Setting();
		setting.adColony.iOSSetting = new UAds.UAdsSetting.AdColoySetting.Setting();
#endif

		UAds.Editor.UAdsSettingHelper.SaveUAdsSettings(setting);

		EditorUtility.DisplayProgressBar("Export Package", "Processing...", 0.1f);
		var packageName = "uads.unitypackage";
		try {

			AssetDatabase.ExportPackage("Assets/UAds", packageName, ExportPackageOptions.Recurse);
			EditorUtility.DisplayProgressBar("Export Package", "", 1f);
			Debug.Log("create " + packageName);
		}
		finally {
			// 復元処理
			EditorUtility.ClearProgressBar();
			EditorJsonUtility.FromJsonOverwrite(original, setting);
			UAds.Editor.UAdsSettingHelper.SaveUAdsSettings(setting);
		}
	}
}
