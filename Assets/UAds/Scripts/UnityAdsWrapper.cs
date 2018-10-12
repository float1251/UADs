#if UNITY_ADS
using System;
using UnityEngine.Advertisements;

public class UnityAdsWrapper : IVideoAdvertisement
{
	private string gameId;
	private bool isDebug;

	public UnityAdsWrapper(string gameId, bool isDebug)
	{
		this.gameId = gameId;
		this.isDebug = isDebug;
	}


	public void Initialize()
	{
		Advertisement.Initialize(gameId, isDebug);
	}

	public bool isReady()
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

		if (!Advertisement.isShowing) {
			return false;
		}

		Advertisement.Show("", new ShowOptions()
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
#endif
