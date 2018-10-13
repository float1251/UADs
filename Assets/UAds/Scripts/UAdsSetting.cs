using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAdsSetting : ScriptableObject
{

#if UNITY_ADS
	[System.Serializable]
	public class UnityAdsSetting
	{
		public string androidGameId;
		public string iOSGameId;
		public string rewardVideoZoneId = "rewardedVideo";
	}

	public UnityAdsSetting unityAds = new UnityAdsSetting();

#endif


}
