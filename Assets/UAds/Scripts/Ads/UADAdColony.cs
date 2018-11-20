using System;


#if ENABLE_ADCOLONY
namespace UAds
{
	public class UADAdColony : IVideoAdvertisement
	{
		private string appId;
		private string rewardZoneId;
		private bool isDebug;

		private event OnFinishRewardVideo onFinish;

		public UADAdColony(string appId, string rewardZoneId, bool isDebug)
		{
			var options = new AdColony.AppOptions()
			{
				DisableLogging = !isDebug,
				TestModeEnabled = isDebug,
			};
			AdColony.Ads.Configure(this.appId, options, new[] { rewardZoneId });
			AdColony.Ads.OnRequestInterstitial += Ads_OnRequestInterstitial;
			AdColony.Ads.OnExpiring += Ads_OnExpiring;
		}

		public void Initialize()
		{
		}

		public bool isReady()
		{
			return this._ad != null;
		}

		public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
		{
			if (this._ad == null) {
				// 見せれるadがない場合はrequestをする
				AdColony.Ads.RequestInterstitialAd(this.rewardZoneId, null);
				return false;
			}

			AdColony.AdOptions adOptions = new AdColony.AdOptions();
			adOptions.ShowPrePopup = false;
			adOptions.ShowPostPopup = false;

			AdColony.Ads.ShowAd(this._ad);

			return true;
		}

		#region AdColony event method

		private AdColony.InterstitialAd _ad;
		void Ads_OnRequestInterstitial(AdColony.InterstitialAd obj)
		{
			this._ad = obj;
		}

		void Ads_OnExpiring(AdColony.InterstitialAd obj)
		{
			AdColony.Ads.RequestInterstitialAd(this.rewardZoneId, null);
		}


		#endregion
	}
}

#endif