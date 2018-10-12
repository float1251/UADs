using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAdsSettings : ScriptableObject
{

#if UNITY_ADS
	public string androidGameId;
	public string iOSGameId;
	public string rewardVideoZoneId = "rewardedVideo";
#endif


}
