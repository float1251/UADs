using UnityEngine;
using UnityEditor;

public class ExportPackage : ScriptableObject
{
	[MenuItem("Tools/UAds/Export Release Package")]
	static void DoIt()
	{
		var txt = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/UAds/VERSION.txt");
		// TODO: export前に初期設定にする
		EditorUtility.DisplayProgressBar("Export Package", "Processing...", 0.1f);
		var packageName = string.Format("uads_{0}.unitypackage", txt.text.Trim());
		try {

			AssetDatabase.ExportPackage("Assets/UAds", packageName, ExportPackageOptions.Recurse);
			EditorUtility.DisplayProgressBar("Export Package", "", 1f);
			Debug.Log("create " + packageName);
		}
		finally {
			EditorUtility.ClearProgressBar();
		}
	}
}
