#if UNITY_MONETIZATION
using System.Collections;
using System.Diagnostics;
using UnityEngine;
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
            this.gameId = gameId;
            this.placementId = placementId;
            this.testMode = testMode;
        }

        public void Initialize()
        {
            // Monetization.onPlacementContentStateChange -= Monetization_OnPlacementContentStateChange;
            // Monetization.onPlacementContentReady -= Monetization_OnPlacementContentReady;
            // Monetization.onPlacementContentStateChange += Monetization_OnPlacementContentStateChange;
            // Monetization.onPlacementContentReady += Monetization_OnPlacementContentReady;
            // gameIdをちゃんと設定しないとtestModeすら動かない.ログも出ない模様.
            Monetization.Initialize(gameId, testMode);
        }

        public IEnumerator ShowRewardVideoAsync(OnFinishRewardVideo onFinish)
        {
            if (_ad == null || !_ad.ready || _ad.state == PlacementContentState.Disabled ||
                _ad.state == PlacementContentState.NotAvailable)
            {
                // 1秒くらい待ってみる.
                int count = 10;
                while (count < 10 && !Monetization.IsReady(this.placementId))
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                    count++;
                }

                if (Monetization.IsReady(this.placementId))
                {
                    _ad = Monetization.GetPlacementContent(this.placementId) as ShowAdPlacementContent;
                }
                else
                {
                    onFinish.Invoke(VideoAdStatus.AdNotReadyOrShowing);
                    yield break;
                }
            }

            // 表示中の際はfalseで返す.
            if (_ad.showing)
            {
                onFinish.Invoke(VideoAdStatus.AdNotReadyOrShowing);
                yield break;
            }

            ShowAd(onFinish);
            // 一応nullを入れるが、必要あるかは不明.
            this._ad = null;
        }

        public bool IsReady()
        {
            return Monetization.isInitialized && Monetization.isSupported && Monetization.IsReady(this.placementId);
        }


        public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
        {
            // callback設定しているが、一応nullの際を確認しておく.
            if (_ad == null)
            {
                _ad = Monetization.GetPlacementContent(this.placementId) as ShowAdPlacementContent;
            }

            if (_ad != null && _ad.ready)
            {
                // 表示中の際はfalseで返す.
                if (_ad.showing)
                {
                    return false;
                }

                ShowAd(onFinish);
                return true;
            }

            return false;
        }

        private void ShowAd(OnFinishRewardVideo onFinish)
        {
            _ad.Show((finishState) =>
            {
                VideoAdStatus status = VideoAdStatus.Cancel;
                switch (finishState)
                {
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
        }

        void Monetization_OnPlacementContentReady(object sender, PlacementContentReadyEventArgs e)
        {
            _ad = e.placementContent as ShowAdPlacementContent;
        }

        void Monetization_OnPlacementContentStateChange(object sender, PlacementContentStateChangeEventArgs e)
        {
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void Log(string tag, string message)
        {
            UnityEngine.Debug.Log(string.Format("[{0}]: {1}", tag, message));
        }
    }
}

#endif