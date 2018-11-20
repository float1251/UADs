using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAds
{

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

#if ENABLE_ADCOLONY
		[System.Serializable]
		public class AdColoySetting
		{
			public Setting androidSetting = new Setting();
			public Setting iOSSetting = new Setting();

			[System.Serializable]
			public class Setting
			{
				public string appId;
				public string rewardZoneName;
				public string rewardZoneId;
			}
		}



		public AdColoySetting adColony = new AdColoySetting();
#endif

	}

}