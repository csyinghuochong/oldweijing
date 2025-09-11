using System;
using System.Collections.Generic;
using System.Threading;
using ByteDance.Union;
using ByteDance.Union.Mediation;
using UnityEngine;

/**
 * 激励视频代码示例。
 * 注：该接口支持融合功能
 */
public class ExampleRewardAd
{

    // 加载广告
    public static void LoadReward(AdManager adManager, bool isM)
    {
        // 释放上一次广告
        if (adManager.rewardAd != null)
        {
            adManager.rewardAd.Dispose();
            adManager.rewardAd = null;
        }

        // 竖屏
        var codeId = isM ? CSJMDAdPositionId.M_REWARD_VIDEO_V_ID : CSJMDAdPositionId.CSJ_REWARD_V_ID;
        // 创造广告参数对象
        var adSlot = new AdSlot.Builder()
            .SetCodeId(codeId) // 必传
            .SetUserID("user123") // 用户id,必传参数
            .SetOrientation(AdOrientation.Horizontal) // 必填参数，期望视频的播放方向
            .SetRewardName("银币") // 可选
            .SetRewardAmount(777) // 可选
            .SetMediaExtra("media_extra") //⚠️设置透传信息(穿山甲广告 或 聚合维度iOS广告时)，需可序列化
            .SetMediationAdSlot(
                new MediationAdSlot.Builder()
#if UNITY_ANDROID  //⚠️设置透传信息(当加载聚合维度Android广告时)
                    .SetExtraObject(AdConst.KEY_GROMORE_EXTRA, "gromore-server-reward-extra-unity") // 可选，设置gromore服务端验证的透传参数
                    .SetExtraObject("pangle", "pangleCustomData") // 可选，不是gromore服务端验证时，用于各个adn的参数透传
#endif
                    .SetScenarioId("reward-m-scenarioId") // 可选
                    .SetBidNotify(true) // 可选
                    .SetUseSurfaceView(false) // 可选
                    .Build()
                    )
            
            .Build();
        // 加载广告
        SDK.CreateAdNative().LoadRewardVideoAd(adSlot, new RewardVideoAdListener(adManager));
    }

    // 展示广告
    public static void ShowReward(AdManager adManager)
    {
        if (adManager.rewardAd == null)
        {
            Debug.LogError("CSJM_Unity " + "Example " + "请先加载广告");
        }
        else
        {
            // 设置展示阶段的监听器
            adManager.rewardAd.SetRewardAdInteractionListener(new RewardAdInteractionListener(adManager));
            adManager.rewardAd.SetAgainRewardAdInteractionListener(new RewardAgainAdInteractionListener(adManager));
            adManager.rewardAd.SetDownloadListener(new AppDownloadListener(adManager));
            adManager.rewardAd.SetAdInteractionListener(new TTAdInteractionListener());
#if UNITY_ANDROID
            adManager.rewardAd.SetRewardPlayAgainController(new RewardAdPlayAgainController());
#endif
            adManager.rewardAd.ShowRewardVideoAd();
        }
    }

    /**
     * 广告加载监听器
     */
    public sealed class RewardVideoAdListener : IRewardVideoAdListener
    {
        private AdManager _adManager;
        public RewardVideoAdListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("CSJM_Unity " + "Example " + $"OnRewardError:{message} on main thread:{Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnRewardVideoAdLoad(RewardVideoAd ad)
        {
            Debug.Log("CSJM_Unity " + "Example " + $"OnRewardVideoAdLoad on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
            
            this._adManager.rewardAd = ad;
            
            // 加载完成后显示
            ExampleRewardAd.ShowReward(this._adManager);
        }

        public void OnRewardVideoCached()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"OnRewardVideoCached on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnRewardVideoCached(RewardVideoAd ad)
        {
            Debug.Log("CSJM_Unity " + "Example " + $"OnRewardVideoCached RewardVideoAd ad on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }
    }

    // 广告展示监听器
    public sealed class RewardAdInteractionListener : IRewardAdInteractionListener
    {
        private AdManager _adManager;

        public RewardAdInteractionListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"rewardVideoAd show on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");

            LogMediationInfo(_adManager);
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"rewardVideoAd bar click on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnAdClose()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"rewardVideoAd close on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");

            if (this._adManager.rewardAd != null)
            {
                this._adManager.rewardAd.Dispose();
                this._adManager.rewardAd = null;
            }
        }

        public void OnVideoSkip()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"rewardVideoAd skip on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnVideoComplete()
        {
            Debug.Log("CSJM_Unity " + "Example " + "Example " + $"rewardVideoAd complete on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnVideoError()
        {
            Debug.LogError("CSJM_Unity " + "Example " + $"rewardVideoAd error on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnRewardArrived(bool isRewardValid, int rewardType, IRewardBundleModel extraInfo)
        {
            var logString = "OnRewardArrived verify:" + isRewardValid + " rewardType:" + rewardType + " extraInfo: " + extraInfo.ToString() +
                            $" on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}";
            Debug.Log("CSJM_Unity " + "Example " + logString);
        }
    }

    // 广告再看一个监听器
    public sealed class RewardAdPlayAgainController : IRewardAdPlayAgainController
    {
        public void GetPlayAgainCondition(int nextPlayAgainCount, Action<PlayAgainCallbackBean> callback)
        {
            Debug.Log("CSJM_Unity " + "Example " + "Reward GetPlayAgainCondition");
            AdManager.MNextPlayAgainCount = nextPlayAgainCount;
            var bean = new PlayAgainCallbackBean(true, "金币", nextPlayAgainCount + "个");
            callback?.Invoke(bean);
        }
    }

    // 广告再看一个监听器
    public sealed class RewardAgainAdInteractionListener : IRewardAdInteractionListener
    {
        private AdManager _adManager;

        public RewardAgainAdInteractionListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"again rewardVideoAd show on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
            string msg = "Callback --> 第 " + AdManager.MNowPlayAgainCount + " 次再看 rewardPlayAgain show";
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"again rewardVideoAd bar click on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnAdClose()
        {
            Debug.Log("CSJM_Unity " + "Example " + "OnAdClose");
        }

        public void OnVideoSkip()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"again rewardVideoAd skip on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnVideoComplete()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"again rewardVideoAd complete on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnVideoError()
        {
            Debug.LogError("CSJM_Unity " + "Example " + $"again rewardVideoAd error on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnRewardArrived(bool isRewardValid, int rewardType, IRewardBundleModel extraInfo)
        {
            var logString = "again OnRewardArrived verify:" + isRewardValid + " rewardType:" + rewardType + " extraInfo:" + extraInfo +
                            $" on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}";
            Debug.Log("CSJM_Unity " + "Example " + logString);
        }
    }

    // 打印广告相关信息
    private static void LogMediationInfo(AdManager adManager)
    {
        MediationAdEcpmInfo showEcpm = adManager.rewardAd.GetMediationManager().GetShowEcpm();
        if (showEcpm != null)
        {
            LogUtils.LogMediationAdEcpmInfo(showEcpm, "GetShowEcpm");
        }

        MediationAdEcpmInfo bestEcpm = adManager.rewardAd.GetMediationManager().GetBestEcpm();
        if (bestEcpm != null)
        {
            LogUtils.LogMediationAdEcpmInfo(bestEcpm, "GetBestEcpm");
        }

        List<MediationAdEcpmInfo> multiBiddingEcpmList = adManager.rewardAd.GetMediationManager().GetMultiBiddingEcpm();
        foreach (MediationAdEcpmInfo item in multiBiddingEcpmList)
        {
            LogUtils.LogMediationAdEcpmInfo(item, "GetMultiBiddingEcpm");
        }

        List<MediationAdEcpmInfo> cacheList = adManager.rewardAd.GetMediationManager().GetCacheList();
        foreach (MediationAdEcpmInfo item in cacheList)
        {
            LogUtils.LogMediationAdEcpmInfo(item, "GetCacheList");
        }

        List<MediationAdLoadInfo> adLoadInfoList = adManager.rewardAd.GetMediationManager().GetAdLoadInfo();
        foreach (MediationAdLoadInfo item in adLoadInfoList)
        {
            LogUtils.LogAdLoadInfo(item);
        }
    }
}
