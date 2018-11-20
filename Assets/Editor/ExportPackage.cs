using UnityEngine;
using UnityEditor;

public class ExportPackage : ScriptableObject
{
	[MenuItem("Tools/UAds/Export Release Package")]
	static void DoIt()
	{
		// TODO: export前に初期設定にする
		EditorUtility.DisplayProgressBar("Export Package", "Processing...", 0.1f);
		try {
			AssetDatabase.ExportPackage("Assets/UAds", "release.unitypackage", ExportPackageOptions.Recurse | ExportPackageOptions.IncludeLibraryAssets);
			EditorUtility.DisplayProgressBar("Export Package", "", 1f);
			Debug.Log("create unitypacakge");
		}
		finally {
			EditorUtility.ClearProgressBar();
		}
	}
}
