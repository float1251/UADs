using System;


public enum VideoAdStatus
{
	Fail,
	Success,
	Cancel,
}
public delegate void OnFinishRewardVideo(VideoAdStatus status);
public interface IVideoAdvertisement
{
	void Initialize();
	bool ShowRewardVideoAd(OnFinishRewardVideo onFinish);
	bool isReady();
}
