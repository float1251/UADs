using UnityEngine;
using System.Collections;
#if ENABLE_ADCOLONY
using AdColony;
#endif

namespace UAds.Sample
{
    public class SampleController : MonoBehaviour
    {
        UAds.IVideoAdvertisement ads;

        [SerializeField]
        StatusText status;

        public void OnClickUnityAds()
        {
            status.UpdateStatus("UnityAds Initialzie");
#if UNITY_ADS
			this.ads = new UAds.UADUnityAdsV2("", "", true);
#elif UNITY_MONETIZATION
            // Errorが出て動作しないです。 GameId等を設定する必要があります。
            // Error while initializing Unity Services: empty game ID, halting Unity Ads init
            this.ads = new UAds.UAdUnityMonetization("", "", true);
#endif
        }


        public void OnClickAdcolony()
        {
            status.UpdateStatus("Adcolony Initialzie");
#if ENABLE_ADCOLONY
            this.ads = new UAds.UADAdColony("", "", true);
#endif
        }

        public void OnClickInitialize()
        {
            status.UpdateStatus("Initialize");
            this.ads.Initialize();
        }

        public void OnClickReady()
        {
            var res = this.ads.IsReady();
            status.UpdateStatus("IsReady " + res);
        }

        public void OnClickShowAds()
        {
            status.UpdateStatus("ShowAds");
            this.ads.ShowRewardVideoAd((s) => { status.UpdateStatus("Status: " + s); });
        }


        public void OnClickManagerInitialize()
        {
            status.UpdateStatus("Initialize");
            UAdsManager.Instance.Initialize();
        }

        public void OnClickManagerReady()
        {
            var res = UAdsManager.Instance.IsReady();
            status.UpdateStatus("IsReady " + res);
        }

        public void OnClickManagerShowAds()
        {
            status.UpdateStatus("ShowAds");

            UAdsManager.Instance.ShowRewardVideoAd((s) => { status.UpdateStatus("Status: " + s); });
        }

        public void OnClickShowAdAsync()
        {
            status.UpdateStatus("ShowAdsAsync");
            UAdsManager.Instance.ShowRewardVideoAdAsync((s) => { status.UpdateStatus("Status: " + s); });
        }
    }
}