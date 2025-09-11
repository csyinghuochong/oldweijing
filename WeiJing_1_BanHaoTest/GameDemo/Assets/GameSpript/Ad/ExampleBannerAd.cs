using System.Threading;
using ByteDance.Union;
using UnityEngine;

/**
 * 自渲染banner代码示例
 * 注：仅支持穿山甲代码位，不支持融合
 */
public class ExampleBannerAd
{
    public static void LoadNativeBannerAd(AdManager adManager)
    {
        if (adManager.bannerAd != null)
        {
            adManager.bannerAd.Dispose();
            adManager.bannerAd = null;
        }

        int width = UnityEngine.Screen.width;
        int height = width / 600 * 257;

        var adSlot = new AdSlot.Builder()
            .SetCodeId(CSJMDAdPositionId.CSJ_NATIVE_BANNER_ID) // 必传
            .SetImageAcceptedSize(width, height) // 单位px
            .SetNativeAdType(AdSlotType.Banner) // 仅支持banner
            .Build();
        // LoadNativeAd接口仅支持自渲染Banner
        SDK.CreateAdNative().LoadNativeAd(adSlot, new NativeBannerAdListener(adManager));
    }

    public static void ShowNativeBannerAd(AdManager adManager)
    {
        if (adManager.bannerAd == null)
        {
            Debug.LogError("CSJM_Unity "+ "Example " + "请先加载广告");
            return;
        }
        
        adManager.bannerAd.SetNativeAdInteractionListener(new NativeBannerAdInteractionListener(adManager));
        adManager.bannerAd.SetNativeAdDislikeListener(new NativeBannerAdDislikeCallback(adManager));
        adManager.bannerAd.SetDownloadListener(new AppDownloadListener(adManager));
        adManager.bannerAd.SetAdInteractionListener(new TTAdInteractionListener());
        adManager.bannerAd.ShowNativeAd(AdSlotType.Banner, 0, 500); // ShowNativeAd仅支持自渲染Banner
    }

    // 广告加载监听器
    public sealed class NativeBannerAdListener : INativeAdListener
    {
        private AdManager _adManager;
        public NativeBannerAdListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("CSJM_Unity "+ "Example " + "OnNativeBannerAdError: " + message);
        }

        public void OnNativeAdLoad(NativeAd[] ads)
        {
            if (ads == null || ads.Length <= 0)
            {
                Debug.Log("CSJM_Unity "+ "Example " + $"OnNativeBannerAdLoad ads array is null on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");

                return;
            }
            this._adManager.bannerAd = ads[0];

            Debug.Log("CSJM_Unity "+ "Example " + $"OnNativeAdLoad on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }
    }

    public sealed class NativeBannerAdInteractionListener : IInteractionAdInteractionListener
    {
        private AdManager _adManager;

        public NativeBannerAdInteractionListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnAdCreativeClick()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"NativeAd creative click on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"NativeAd show on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }

        public void OnAdClicked()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"NativeAd click  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
            this._adManager.bannerAd.UploadDislikeEvent("csjm_unity nativeBanner dislike test");
        }

        public void OnAdDismiss()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"NativeAd close  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");

            //释放广告资源
            _adManager.bannerAd?.Dispose();
        }

        public void onAdRemoved()
        {
            Debug.Log("CSJM_Unity " + "Example " + $"NativeAd onAdRemoved  on main thread: {Thread.CurrentThread.ManagedThreadId == AdManager.MainThreadId}");
        }
    }

    public class NativeBannerAdDislikeCallback : IDislikeInteractionListener
    {
        private AdManager _adManager;

        public NativeBannerAdDislikeCallback(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnCancel()
        {
            Debug.Log("CSJM_Unity " + "Example " + "native banner ad dislike OnCancel");
        }

        public void OnShow()
        {
            Debug.Log("CSJM_Unity " + "Example " + "native banner ad dislike OnShow");
        }

        public void OnSelected(int var1, string var2, bool enforce)
        {
            Debug.Log("CSJM_Unity " + "Example " + "native banner ad dislike OnSelected:" + var2);
        }
    }
}