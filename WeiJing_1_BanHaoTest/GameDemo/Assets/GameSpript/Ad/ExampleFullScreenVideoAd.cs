using System.Collections.Generic;
using System.Threading;
using ByteDance.Union;
using ByteDance.Union.Mediation;
using UnityEngine;

public class ExampleFullScreenVideoAd
{
    public static void LoadFullScreenVideoAd(AdManager adManager, bool isM)
    {
        if (adManager.fullScreenVideoAd != null)
        {
            adManager.fullScreenVideoAd.Dispose();
            adManager.fullScreenVideoAd = null;
        }
        var adSlot = new AdSlot.Builder()
            .SetCodeId(isM == false ? CSJMDAdPositionId.CSJ_ExpressFullScreen_V_ID :
            CSJMDAdPositionId.M_INTERSTITAL_FULL_SCREEN_ID) // 必传
            .SetOrientation(AdOrientation.Vertical)
            .SetMediationAdSlot(new MediationAdSlot.Builder()
                .SetScenarioId("ScenarioId") // 可选
                .SetUseSurfaceView(false) // 可选
                .SetBidNotify(true) // 可选
                .Build())
            .Build();
        SDK.CreateAdNative().LoadFullScreenVideoAd(adSlot, new FullScreenVideoAdListener(adManager));
    }

    public static void ShowFullScreenVideoAd(AdManager adManager)
    {
        if (adManager.fullScreenVideoAd == null)
        {
            Debug.LogError("CSJM_Unity "+ "Example " + "请先加载广告");
            return;
        }

        adManager.fullScreenVideoAd.SetFullScreenVideoAdInteractionListener(new FullScreenAdInteractionListener(adManager));
        adManager.fullScreenVideoAd.SetDownloadListener(new AppDownloadListener(adManager));
        adManager.fullScreenVideoAd.SetAdInteractionListener(new TTAdInteractionListener());

        adManager.fullScreenVideoAd.ShowFullScreenVideoAd();
    }

    // 广告加载监听器
    public sealed class FullScreenVideoAdListener : IFullScreenVideoAdListener
    {
        private AdManager _adManager;

        public FullScreenVideoAdListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("CSJM_Unity "+ "Example " + $"OnFullScreenError: {message}  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnFullScreenVideoAdLoad(FullScreenVideoAd ad)
        {
            Debug.Log("CSJM_Unity "+ "Example " + $"OnFullScreenAdLoad  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");

            this._adManager.fullScreenVideoAd = ad;
        }

        public void OnFullScreenVideoCached()
        {
            Debug.Log("CSJM_Unity "+ "Example " + $"OnFullScreenVideoCached  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnFullScreenVideoCached(FullScreenVideoAd ad)
        {
        }
    }

    // 广告展示监听器
    public sealed class FullScreenAdInteractionListener : IFullScreenVideoAdInteractionListener
    {
        private AdManager _adManager;

        public FullScreenAdInteractionListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"fullScreenVideoAd show  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");

            // log
            LogMediationInfo(_adManager);
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"fullScreenVideoAd bar click  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnAdClose()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"fullScreenVideoAd close  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");

            if (this._adManager.fullScreenVideoAd != null)
            {
                this._adManager.fullScreenVideoAd.Dispose();
                this._adManager.fullScreenVideoAd = null;
            }
        }

        public void OnVideoComplete()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"fullScreenVideoAd complete  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnVideoError()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"fullScreenVideoAd OnVideoError  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnSkippedVideo()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"fullScreenVideoAd OnSkippedVideo  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }
    }

    // 打印广告相关信息
    private static void LogMediationInfo(AdManager adManager)
    {
        MediationAdEcpmInfo showEcpm = adManager.fullScreenVideoAd.GetMediationManager().GetShowEcpm();
        if (showEcpm != null)
        {
            LogUtils.LogMediationAdEcpmInfo(showEcpm, "GetShowEcpm");
        }

        MediationAdEcpmInfo bestEcpm = adManager.fullScreenVideoAd.GetMediationManager().GetBestEcpm();
        if (bestEcpm != null)
        {
            LogUtils.LogMediationAdEcpmInfo(bestEcpm, "GetBestEcpm");
        }

        List<MediationAdEcpmInfo> multiBiddingEcpmList = adManager.fullScreenVideoAd.GetMediationManager().GetMultiBiddingEcpm();
        foreach (MediationAdEcpmInfo item in multiBiddingEcpmList)
        {
            LogUtils.LogMediationAdEcpmInfo(item, "GetMultiBiddingEcpm");
        }

        List<MediationAdEcpmInfo> cacheList = adManager.fullScreenVideoAd.GetMediationManager().GetCacheList();
        foreach (MediationAdEcpmInfo item in cacheList)
        {
            LogUtils.LogMediationAdEcpmInfo(item, "GetCacheList");
        }

        List<MediationAdLoadInfo> adLoadInfoList = adManager.fullScreenVideoAd.GetMediationManager().GetAdLoadInfo();
        foreach (MediationAdLoadInfo item in adLoadInfoList)
        {
            LogUtils.LogAdLoadInfo(item);
        }
    }

}
