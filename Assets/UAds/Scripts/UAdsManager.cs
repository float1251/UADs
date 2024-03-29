﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UAds.Utils;
using UnityEngine;

namespace UAds
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T) FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        Debug.LogWarning(typeof(T) + "is nothing");
                    }
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {
            CheckInstance();
        }

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = (T) this;
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }

            Destroy(this);
            return false;
        }
    }

    public class UAdsManager : SingletonMonoBehaviour<UAdsManager>
    {
        [SerializeField]
        UAdsSetting setting;

        public bool isDebug;

        [SerializeField]
        bool initializeManually;

        private List<IVideoAdvertisement> _ads = new List<IVideoAdvertisement>();

        public bool randomize = false;

        protected override void Awake()
        {
            var res = CheckInstance();
            if (res)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Start()
        {
            if (!initializeManually)
                Initialize();
        }

        public void Initialize()
        {
            if (_ads.Count() == 0)
            {
                if (Application.isEditor)
                {
                    // Editor用の画面を表示する
                    _ads.Add(new UADDummyAd());
                }
                else
                {
                    // 有効になっている動画広告の設定とInitializeを行う.
#if UNITY_ADS
					_ads.Add(new UADUnityAdsV2(setting.unityAds.GameId, setting.unityAds.rewardVideoPlacementId, isDebug));
#elif UNITY_MONETIZATION
                    _ads.Add(new UAdUnityMonetization(setting.unityAds.GameId, setting.unityAds.rewardVideoPlacementId,
                        isDebug));
#endif

                    if (setting.enableAdcolony)
                    {
                        var adcolony = setting.adColony.GetSetting;
#if ENABLE_ADCOLONY
                        _ads.Add(new UADAdColony(adcolony.appId, adcolony.rewardZoneId, this.isDebug));
#endif
                    }
                }

                this._ads.ForEach(v => v.Initialize());
            }
        }

        public bool IsReady()
        {
            return this._ads.Any(v => v.IsReady());
        }

        public bool ShowRewardVideoAd(OnFinishRewardVideo onFinish)
        {
            var adList = new List<IVideoAdvertisement>();
            adList.AddRange(this._ads);
            if (randomize)
                adList.Shuffle();

            foreach (var v in adList)
            {
                if (v.IsReady())
                {
                    var res = v.ShowRewardVideoAd(onFinish);
                    if (res)
                        return true;
                }
            }

            // 現在表示できるネットワークがない. cacheにのっていない可能性もあるので、しばらくしてから再度行うように案内するのが無難.
            return false;
        }

        public void ShowRewardVideoAdAsync(OnFinishRewardVideo onFinish)
        {
            StartCoroutine(_ShowRewardVideoAdAsync(onFinish));
        }

        private IEnumerator _ShowRewardVideoAdAsync(OnFinishRewardVideo onFinish)
        {
            var adList = new List<IVideoAdvertisement>();
            adList.AddRange(this._ads);
            if (randomize)
                adList.Shuffle();

            bool finish;
            VideoAdStatus status = VideoAdStatus.AdNotReadyOrShowing;
            foreach (var v in adList)
            {
                finish = false;
                StartCoroutine(v.ShowRewardVideoAsync((s) =>
                {
                    finish = true;
                    status = s;
                }));
                yield return new WaitUntil(() => finish);
                if (status != VideoAdStatus.AdNotReadyOrShowing)
                {
                    onFinish.Invoke(status);
                    yield break;
                }
            }

            onFinish.Invoke(VideoAdStatus.Fail);
        }
    }
}