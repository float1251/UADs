using System;
using UnityEngine;


#if ENABLE_ADCOLONY
namespace UAds
{
	/// <summary>
	/// Adcolony.
	/// Errorケースが怪しい。
	/// あと動画が閉じられた際に呼ばれるものはなにかがわからない.
	/// おそらく以下の可能性が高い
	///     動画が見られたとき: OnRewardGranted
	///     ユーザが動画をキャンセルした: OnClosed? OnRewardGranted?
	/// </summary>
	public class UADAdColony : IVideoAdvertisement
	{
		private string appId;
		private string rewardZoneId;
		private bool isDebug;

		private AdColony.InterstitialAd _ad;

		private event OnFinishRewardVideo onFinish;

		public UADAdColony(string appId, string rewardZoneId, bool isDebug)
		{
			var options = new AdColony.AppOptions()
			{
				DisableLogging = !isDebug,
				TestModeEnabled = isDebug,
			};
			this.appId = appId;
			this.rewardZoneId = rewardZoneId;
			this.isDebug = isDebug;
			AdColony.Ads.Configure(this.appId, options, new[] { rewardZoneId });
			AdColony.Ads.OnRequestInterstitial += Ads_OnRequestInterstitial;
			AdColony.Ads.OnExpiring += Ads_OnExpiring;
			AdColony.Ads.OnRewardGranted += Ads_OnRewardGranted;
			AdColony.Ads.OnClosed += Ads_OnClosed;
		}

		public void Initialize()
		{
			AdColony.Ads.RequestInterstitialAd(this.rewardZoneId, new AdColony.AdOptions()
			{
				ShowPostPopup = false,
				ShowPrePopup = false,
			});
		}

		public bool IsReady()
		{
			if (this._ad == null || this._ad.Expired) {
				this._ad = null;
				RequestRewardAds();
				return false;
			}
			return this._ad != null;
		}

		public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
		{
			if (this._ad == null || this._ad.Expired) {
				this._ad = null;
				// 見せれるadがない場合はrequestをする
				RequestRewardAds();
				return false;
			}
			this.onFinish = onFinish;

			AdColony.Ads.ShowAd(this._ad);
			return true;
		}

		#region AdColony event method

		void Ads_OnRequestInterstitial(AdColony.InterstitialAd obj)
		{
			PrintDebug(string.Format("OnRequestInterstitial: " + obj.ZoneId));
			this._ad = obj;
		}

		void Ads_OnExpiring(AdColony.InterstitialAd obj)
		{
			PrintDebug(string.Format("OnExpired: " + obj.ZoneId));
			this._ad = null;
			RequestRewardAds();
		}

		void Ads_OnRewardGranted(string zoneId, bool success, string rewardType, int quantity)
		{
			PrintDebug(string.Format("OnRewardGranted: {0}, {1}, {2}, {3}", zoneId, success, rewardType, quantity));
			if (this.onFinish != null)
				this.onFinish.Invoke(success ? VideoAdStatus.Success : VideoAdStatus.Fail);
		}

		void Ads_OnClosed(AdColony.InterstitialAd obj)
		{
			PrintDebug(string.Format("OnClosed: " + obj.ZoneId));
		}

		#endregion

		private void RequestRewardAds()
		{
			AdColony.Ads.RequestInterstitialAd(this.rewardZoneId, new AdColony.AdOptions()
			{
				ShowPostPopup = false,
				ShowPrePopup = false
			});
		}

		private void PrintDebug(string message)
		{
			if (this.isDebug)
				Debug.Log(message);
		}
	}
}

#endif