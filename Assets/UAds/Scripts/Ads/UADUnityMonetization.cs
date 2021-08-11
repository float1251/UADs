#if UNITY_MONETIZATION
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Advertisements;

namespace UAds {
    /// <summary>
    /// UAd unity monetization.
    /// cacheされるタイミングなどが不明.
    /// Editor上では何も表示されない模様.
    /// </summary>
    public class UAdUnityMonetization : IVideoAdvertisement, IUnityAdsListener {
        private string gameId;
        private string placementId;
        private bool testMode;
        private OnFinishRewardVideo onFinish;

        public UAdUnityMonetization(string gameId, string placementId, bool testMode) {
            this.gameId = gameId;
            this.placementId = placementId;
            this.testMode = testMode;
        }

        public void Initialize() {
            // gameIdをちゃんと設定しないとtestModeすら動かない.ログも出ない模様.
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
        }

        public IEnumerator ShowRewardVideoAsync(OnFinishRewardVideo onFinish) {
            if (Advertisement.GetPlacementState(this.placementId) == PlacementState.Waiting) {
                // 1秒くらい待ってみる.
                int count = 0;
                while (count < 10 && !Advertisement.IsReady(this.placementId)) {
                    yield return new WaitForSecondsRealtime(0.1f);
                    count++;
                }

                if (Advertisement.IsReady(this.placementId)) {
                } else {
                    onFinish.Invoke(VideoAdStatus.AdNotReadyOrShowing);
                    yield break;
                }
            }

            // 表示中の際はfalseで返す.
            if (Advertisement.isShowing) {
                onFinish.Invoke(VideoAdStatus.AdNotReadyOrShowing);
                yield break;
            }
            this.onFinish = onFinish;
            ShowAd(onFinish);
        }

        public bool IsReady() {
            return Advertisement.isInitialized && Advertisement.isSupported &&
                   Advertisement.IsReady(this.placementId) && !Advertisement.isShowing;
        }

        public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish) {
            // callback設定しているが、一応nullの際を確認しておく.
            if (Advertisement.IsReady(this.placementId)) {
                // 表示中の際はfalseで返す.
                if (Advertisement.isShowing) {
                    return false;
                }

                ShowAd(onFinish);
                return true;
            }

            this.onFinish = onFinish;

            return false;
        }

        private void ShowAd(OnFinishRewardVideo onFinish)
        {
            this.onFinish = onFinish;
            Advertisement.Show(this.placementId);
        }


        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void Log(string tag, string message) {
            UnityEngine.Debug.Log(string.Format("[{0}]: {1}", tag, message));
        }

        public void OnUnityAdsReady(string placementId) {
            Log("UAds",$"{placementId} is Ready." );
        }

        public void OnUnityAdsDidError(string message) {
        }

        public void OnUnityAdsDidStart(string placementId) {
        }

        public void OnUnityAdsDidFinish(string placementId, UnityEngine.Advertisements.ShowResult showResult) {
            VideoAdStatus status = VideoAdStatus.Cancel;
            switch (showResult) {
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
        }
    }
}


#endif