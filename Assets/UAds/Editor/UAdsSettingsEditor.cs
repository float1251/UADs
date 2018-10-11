using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UAdsSettingsEditor {

    [MenuItem("UAds/CreateSettings")]
    public static void CreateAsset() 
    {
       var asset = ScriptableObject.CreateInstance<UAdsSettings>();

       AssetDatabase.CreateAsset(asset, "Assets/UAds/Resources/UAdsSettings.asset");
       AssetDatabase.Refresh();
    }
}
