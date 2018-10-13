using System;
using System.Linq;
using UnityEditor;

public class ScriptDefineSymbol
{

	BuildTargetGroup[] targets;
	string define;

	private static readonly char separator = ';';

	public ScriptDefineSymbol(BuildTargetGroup[] groups, string define)
	{
		this.targets = groups;
		this.define = define;
	}

	public bool HasDefine()
	{
		foreach (var p in this.targets) {
			var group = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Split(separator);
			var res = group.Any(v => v == define);
			if (!res)
				return false;
		}
		return true;

	}

	public void SetDefine()
	{
		foreach (var v in this.targets) {
			var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(v).Split(separator);
			ArrayUtility.Add(ref symbols, define);
			PlayerSettings.SetScriptingDefineSymbolsForGroup(v, string.Join(separator.ToString(), symbols.Distinct().ToArray()));
		}
	}

	public void RemoveDefine()
	{
		foreach (var v in this.targets) {
			var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(v).Split(separator);
			ArrayUtility.Remove(ref symbols, define);
			PlayerSettings.SetScriptingDefineSymbolsForGroup(v, string.Join(separator.ToString(), symbols.Distinct().ToArray()));
		}

	}
}
