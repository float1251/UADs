using System;
using UnityEngine;

namespace UAds
{

	[Serializable]
	public class UAdsSetting : ScriptableObject
	{
		public bool enableUnityMonetization = false;

		[Serializable]
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


		public bool enableAdcolony;

		[Serializable]
		public class AdColoySetting
		{
			public Setting androidSetting = new Setting();
			public Setting iOSSetting = new Setting();

			[Serializable]
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