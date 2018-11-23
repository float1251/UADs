#if UNITY_MONETIZATION
using UnityEngine;
using System.Collections;
using UnityEngine.Monetization;

namespace UAds
{
	/// <summary>
	/// UAd unity monetization.
	/// cacheされるタイミングなどが不明.
	/// Editor上では何も表示されない模様.
	/// </summary>
	public class UAdUnityMonetization : IVideoAdvertisement
	{
		private string gameId;
		private string placementId;
		private bool testMode;
		private ShowAdPlacementContent _ad;
		public UAdUnityMonetization(string gameId, string placementId, bool testMode)
		{
			this.placementId = placementId;
		}

		public void Initialize()
		{
			Monetization.onPlacementContentStateChange -= Monetization_OnPlacementContentStateChange;
			Monetization.onPlacementContentReady -= Monetization_OnPlacementContentReady;
			Monetization.onPlacementContentStateChange += Monetization_OnPlacementContentStateChange;
			Monetization.onPlacementContentReady += Monetization_OnPlacementContentReady;
			Monetization.Initialize(gameId, testMode);
		}

		public bool IsReady()
		{
			return Monetization.isInitialized && Monetization.isSupported && Monetization.IsReady(this.placementId);
		}

		public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
		{
			// callback設定しているが、一応nullの際を確認しておく.
			if (_ad == null) {
				_ad = Monetization.GetPlacementContent(this.placementId) as ShowAdPlacementContent;
			}

			if (_ad != null && _ad.ready) {
				_ad.Show((finishState) =>
				{
					VideoAdStatus status = VideoAdStatus.Cancel;
					switch (finishState) {
						case ShowResult.Failed:
							status = VideoAdStatus.Fail;
							break;
						case ShowResult.Skipped:
							status = VideoAdStatus.Cancel;
							break;
						case ShowResult.Finished:
						default:
							status = VideoAdStatus.Success;
							break;
					}
					onFinish.Invoke(status);
				});
				// 一応nullを入れるが、必要あるかは不明.
				this._ad = null;
				return true;
			}

			return false;
		}

		void Monetization_OnPlacementContentReady(object sender, PlacementContentReadyEventArgs e)
		{
			_ad = e.placementContent as ShowAdPlacementContent;
		}

		void Monetization_OnPlacementContentStateChange(object sender, PlacementContentStateChangeEventArgs e)
		{
		}

	}

}

#endif