using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UAds
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
	{
		protected static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null) {
					instance = (T)FindObjectOfType(typeof(T));

					if (instance == null) {
						Debug.LogWarning(typeof(T) + "is nothing");
					}
				}

				return instance;
			}
		}

		virtual protected void Awake()
		{
			CheckInstance();
		}

		protected bool CheckInstance()
		{
			if (instance == null) {
				instance = (T)this;
				return true;
			} else if (Instance == this) {
				return true;
			}

			Destroy(this);
			return false;
		}
	}

	public class UAdsManager : SingletonMonoBehaviour<UAdsManager>
	{
		[SerializeField]
		UAdsSetting setting;

		[SerializeField]
		bool isDebug;

		private List<IVideoAdvertisement> _ads = new List<IVideoAdvertisement>();

		private void Start()
		{
			// 有効になっている動画広告の設定とInitializeを行う.
			_ads.Add(new UADUnityAds(setting.unityAds.GameId, setting.unityAds.rewardVideoZoneId, isDebug));

			if (setting.enableAdcolony) {
				var adcolony = setting.adColony.GetSetting;
#if ENABLE_ADCOLONY
				_ads.Add(new UADAdColony(adcolony.appId, adcolony.rewardZoneId, this.isDebug));
#endif
			}
		}
	}
}