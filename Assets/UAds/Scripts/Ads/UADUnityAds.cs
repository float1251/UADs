#if UNITY_ADS
using System;
using UnityEngine.Advertisements;

namespace UAds
{

	public class UADUnityAds : IVideoAdvertisement
	{
		private string gameId;
		private bool isDebug;
		private string rewardVideoZoneId;

		public UADUnityAds(string gameId, string rewardVideoZoneId, bool isDebug)
		{
			this.gameId = gameId;
			this.isDebug = isDebug;
			this.rewardVideoZoneId = rewardVideoZoneId;
		}

		public void Initialize()
		{
			Advertisement.Initialize(gameId, isDebug);
		}

		public bool IsReady()
		{
			return Advertisement.isInitialized && Advertisement.IsReady();
		}

		public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
		{
			if (!Advertisement.isSupported) {
				return false;
			}

			if (!Advertisement.isInitialized) {
				return false;
			}

			if (!Advertisement.IsReady()) {
				return false;
			}

#if !UNITY_EDITOR
            // Editor上では表示してないのでtrueが返るような...
			if (!Advertisement.isShowing) {
				return false;
			}
#endif

			Advertisement.Show(this.rewardVideoZoneId, new ShowOptions()
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
