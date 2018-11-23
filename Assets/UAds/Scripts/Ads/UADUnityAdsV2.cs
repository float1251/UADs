#if UNITY_ADS
using System;
using UnityEngine.Advertisements;

namespace UAds
{

	public class UADUnityAdsV2 : IVideoAdvertisement
	{
		private string gameId;
		private bool isDebug;
		private string placementId;

		public UADUnityAdsV2(string gameId, string placementId, bool isDebug)
		{
			this.gameId = gameId;
			this.isDebug = isDebug;
			this.placementId = placementId;
		}

		public void Initialize()
		{
			Advertisement.Initialize(gameId, isDebug);
		}

		public bool IsReady()
		{
			return Advertisement.isInitialized && Advertisement.IsReady(this.placementId);
		}

		public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
		{
			if (!Advertisement.isSupported) {
				return false;
			}

			if (!Advertisement.isInitialized) {
				return false;
			}

			if (!Advertisement.IsReady(this.placementId)) {
				return false;
			}

#if !UNITY_EDITOR
            // Editor上では表示してないのにtrueが返るような...
			if (!Advertisement.isShowing) {
				return false;
			}
#endif
			Advertisement.Show(this.placementId, new ShowOptions()
			{
				resultCallback = (v) =>
				{
					VideoAdStatus status = VideoAdStatus.Fail;
					switch (v) {
						case ShowResult.Failed:
							status = VideoAdStatus.Fail;
							break;
						case ShowResult.Skipped:
							status = VideoAdStatus.Cancel;
							break;
						case ShowResult.Finished:
							status = VideoAdStatus.Success;
							break;
					}
					onFinish.Invoke(status);
				}
			});

			return true;

		}
	}
}
#endif
