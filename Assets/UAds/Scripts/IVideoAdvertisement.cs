﻿using System;
using System.Collections;

namespace UAds
{
	public enum VideoAdStatus
	{
		AdNotReadyOrShowing,
		Fail,
		Success,
		Cancel,
	}
	public delegate void OnFinishRewardVideo(VideoAdStatus status);
	public interface IVideoAdvertisement
	{
		void Initialize();
		/// <summary>
		/// Shows the reward video ad.
		/// </summary>
		/// <returns><c>true</c>, if reward video ad was shown, <c>false</c> otherwise.</returns>
		/// <param name="onFinish">On finish.</param>
		bool ShowRewardVideoAd(OnFinishRewardVideo onFinish);

		IEnumerator ShowRewardVideoAsync(OnFinishRewardVideo onFinish);
		bool IsReady();
	}

}