using UnityEngine;
using System.Collections;
#if ENABLE_ADCOLONY
using AdColony;
#endif

namespace UAds.Sample
{

	public class SampleController : MonoBehaviour
	{

		UAds.IVideoAdvertisement ads;

		[SerializeField]
		StatusText status;

		public void OnClickUnityAds()
		{
			status.UpdateStatus("UnityAds Initialzie");
			this.ads = new UAds.UADUnityAds("", "", true);
		}


		public void OnClickAdcolony()
		{
			status.UpdateStatus("Adcolony Initialzie");
#if ENABLE_ADCOLONY
			this.ads = new UAds.UADAdColony("", "", true);
#endif
		}

		public void OnClickInitialize()
		{
			status.UpdateStatus("Initialize");
			this.ads.Initialize();
		}

		public void OnClickReady()
		{
			var res = this.ads.IsReady();
			status.UpdateStatus("IsReady " + res);
		}

		public void OnClickShowAds()
		{
			status.UpdateStatus("ShowAds");
			this.ads.ShowRewardVideoAd((s) =>
			{
				status.UpdateStatus("Status: " + s);
			});
		}

	}
}
