using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAds
{

	[System.Serializable]
	public class UAdsSetting : ScriptableObject
	{
		public bool enableUnityMonetization = false;

#if UNITY_ADS || UNITY_MONETIZATION
		[System.Serializable]
		public class UnityAdsSetting
		{
			public string androidGameId;
			public string iOSGameId;
			public string rewardVideoPlacementId = "rewardedVideo";

			public string GameId
			{
				get
				{
#if UNITY_ANDROID
					return androidGameId;
#elif UNITY_IOS
		return iOSGameId;
#else
		return "OTHER_PLATFORM";
#endif
				}
			}
		}

		public UnityAdsSetting unityAds = new UnityAdsSetting();

#endif

		public bool enableAdcolony;

		[System.Serializable]
		public class AdColoySetting
		{
			public Setting androidSetting = new Setting();
			public Setting iOSSetting = new Setting();

			[System.Serializable]
			public class Setting
			{
				public string appId;
				public string rewardZoneId;
			}
			public Setting GetSetting
			{
				get
				{
#if UNITY_ANDROID
					return androidSetting;
#elif UNITY_IOS
        return iOSSetting;
#else
		return new Setting();
#endif
				}
			}
		}
		public AdColoySetting adColony = new AdColoySetting();

	}

}