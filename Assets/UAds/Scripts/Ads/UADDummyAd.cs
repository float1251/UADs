﻿using UnityEngine;
using System.Collections;

namespace UAds
{
	public class UADDummyAd : IVideoAdvertisement
	{
		public void Initialize()
		{
		}

		public IEnumerator ShowRewardVideoAsync(OnFinishRewardVideo onFinish)
		{
			yield return new WaitForSeconds(1f);
			ShowRewardVideoAd(onFinish);
		}

		public bool IsReady()
		{
			return true;
		}

		public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
		{
#if UNITY_EDITOR
			var view = Object.FindObjectOfType<Editor.DummyAdView>();
			if (view == null) {
				var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UAds/Prefabs/Dev/RewardVideoCanvas.prefab");
				var go = GameObject.Instantiate(prefab);
				view = go.GetComponentInChildren<Editor.DummyAdView>();
			}

			view.onFinish = onFinish;
			return true;
#else
			return false;
#endif
		}
	}
}
