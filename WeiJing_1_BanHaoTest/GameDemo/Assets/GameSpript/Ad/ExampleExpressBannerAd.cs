using System.Collections.Generic;
using System.Threading;
using ByteDance.Union;
using ByteDance.Union.Mediation;
using UnityEngine;

/**
 * 模板banner代码示例
 * 注：该接口支持融合功能。并且支持将信息流混出到banner中。
 */
public class ExampleExpressBannerAd
{
    public static void LoadExpressBannerAd(AdManager adManager, bool isM)
    {
        if (adManager.mExpressBannerAd != null)
        {
            adManager.mExpressBannerAd.Dispose();
            adManager.mExpressBannerAd = null;
        }

        int width = (int) (UnityEngine.Screen.width / UnityEngine.Screen.dpi * 160);
        int height = (int)((float)width / 250 * 150); 
        Debug.Log("CSJM_Unity " + "Example " + "express banner w: " + width + ", h: " + height + ", dpi: " + (UnityEngine.Screen.dpi/160));

        string adsRit = isM ? CSJMDAdPositionId.M_BANNER_ID : CSJMDAdPositionId.CSJ_BANNER_ID;
        
        var adSlot = new AdSlot.Builder()
            .SetCodeId(adsRit) // 必传
            .SetSlideIntervalTime(30) // 单位秒。仅当单独使用csj是生效，启用融合时使用的是Gromore线上配置。
            //期望模板广告view的size,单位dp
            .SetExpressViewAcceptedSize(width, height)
            .SetMediationAdSlot(
                new MediationAdSlot.Builder()
                    .SetBidNotify(true) // 可选
                    .SetScenarioId("unity-SetScenarioId") // 可选
                    .SetWxAppId("unity-wxAppId") // 可选
                    .SetAllowShowCloseBtn(true) // 可选
                    .SetMuted(true)
                    .SetVolume(0.7f)
                    .Build())
            .Build();

        SDK.CreateAdNative().LoadExpressBannerAd(adSlot, new ExpressBannerAdListener(adManager));
    }

    public static void ShowExpressBannerAd(AdManager adManager)
    {
        if (adManager.mExpressBannerAd == null)
        {
            Debug.LogError("CSJM_Unity "+ "Example " + "请先加载广告");
            return;
        }

#if UNITY_ANDROID
        adManager.mExpressBannerAd.SetSlideIntervalTime(30 * 1000);
#endif
        adManager.mExpressBannerAd.SetExpressInteractionListener(new ExpressBannerInteractionListener(adManager));
        adManager.mExpressBannerAd.SetDislikeCallback(new ExpressAdDislikeCallback(adManager));
        adManager.mExpressBannerAd.SetDownloadListener(new AppDownloadListener(adManager));
        adManager.mExpressBannerAd.SetAdInteractionListener(new TTAdInteractionListener());
        adManager.mExpressBannerAd.ShowExpressAd(0, 500);
    }

    // 广告加载监听器
    public sealed class ExpressBannerAdListener : IExpressBannerAdListener
    {
        private AdManager _adManager;

        public ExpressBannerAdListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnError(int code, string message)
        {
            Debug.Log("CSJM_Unity "+ "Example " + "onExpressAdError: " + message);
        }

        public void OnBannerAdLoad(ExpressBannerAd ad)
        {
            Debug.Log("CSJM_Unity "+ "Example " + "OnExpressBannerAdLoad");
            this._adManager.mExpressBannerAd = ad;
        }
    }

    // 广告展示监听器
    public sealed class ExpressBannerInteractionListener : IExpressBannerInteractionListener
    {
        private AdManager _adManager;

        public ExpressBannerInteractionListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnAdClicked()
        {
            Debug.Log("CSJM_Unity " + "Example " + "OnAdClicked");
            this._adManager.mExpressBannerAd.UploadDislikeEvent("csjm_unity expressBanner dislike test");
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + "Example " + "OnAdShow");

            LogMediationInfo(_adManager);
        }

        public void OnAdViewRenderError(int code, string message)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express banner OnAdViewRenderError code: " + code + ", msg: " + message);
        }

        public void OnAdViewRenderSucc(float width, float height)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express banner OnAdViewRenderSucc");
        }

        public void OnAdClose()
        {
            Debug.Log("CSJM_Unity " + "Example " + "OnAdClose");
        }

        public void onAdRemoved()
        {
            Debug.Log("CSJM_Unity " + "Example " + "onAdRemoved");
        }
    }

    // dislike监听器
    public sealed class ExpressAdDislikeCallback : IDislikeInteractionListener
    {
        private AdManager _adManager;

        public ExpressAdDislikeCallback(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnCancel()
        {
            Debug.Log("CSJM_Unity "+ "Example " + "express banner dislike OnCancel");
        }

        public void OnShow()
        {
            Debug.Log("CSJM_Unity "+ "Example " + "express banner dislike OnShow");
        }

        public void OnSelected(int var1, string var2, bool enforce)
        {
            Debug.Log("CSJM_Unity "+ "Example " + "express banner dislike OnSelected:" + var2);

            //释放广告资源
            if (this._adManager.mExpressBannerAd != null)
            {
                this._adManager.mExpressBannerAd.Dispose();
                this._adManager.mExpressBannerAd = null;
            }
        }
    }

    // 打印广告相关信息
    private static void LogMediationInfo(AdManager adManager)
    {
        MediationAdEcpmInfo showEcpm = adManager.mExpressBannerAd.GetMediationManager().GetShowEcpm();
        if (showEcpm != null)
        {
            LogUtils.LogMediationAdEcpmInfo(showEcpm, "GetShowEcpm");
        }

        MediationAdEcpmInfo bestEcpm = adManager.mExpressBannerAd.GetMediationManager().GetBestEcpm();
        if (bestEcpm != null)
        {
            LogUtils.LogMediationAdEcpmInfo(bestEcpm, "GetBestEcpm");
        }

        List<MediationAdEcpmInfo> multiBiddingEcpmList = adManager.mExpressBannerAd.GetMediationManager().GetMultiBiddingEcpm();
        foreach (MediationAdEcpmInfo item in multiBiddingEcpmList)
        {
            LogUtils.LogMediationAdEcpmInfo(item, "GetMultiBiddingEcpm");
        }

        List<MediationAdEcpmInfo> cacheList = adManager.mExpressBannerAd.GetMediationManager().GetCacheList();
        foreach (MediationAdEcpmInfo item in cacheList)
        {
            LogUtils.LogMediationAdEcpmInfo(item, "GetCacheList");
        }

        List<MediationAdLoadInfo> adLoadInfoList = adManager.mExpressBannerAd.GetMediationManager().GetAdLoadInfo();
        foreach (MediationAdLoadInfo item in adLoadInfoList)
        {
            LogUtils.LogAdLoadInfo(item);
        }
    }
}
