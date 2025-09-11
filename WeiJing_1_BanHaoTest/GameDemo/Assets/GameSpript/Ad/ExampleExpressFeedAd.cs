using System.Collections.Generic;
using System.Threading;
using ByteDance.Union;
using UnityEngine;

/**
 * 模板信息流代码示例
 * 注：该接口不支持融合功能，仅支持穿山甲模板代码位
 */
public class ExampleExpressFeedAd
{
    public static void LoadExpressFeedAd(AdManager adManager)
    {
        if (adManager.mExpressFeedad != null)
        {
            adManager.mExpressFeedad.Dispose();
            adManager.mExpressFeedad = null;
        }
        var adSlot = new AdSlot.Builder()
            .SetCodeId(CSJMDAdPositionId.CSJ_NATIVE_EXPRESS_ID) // 必传
            ////期望模板广告view的size,单位dp，//高度设置为0,则高度会自适应
            .SetExpressViewAcceptedSize(350, 400)
            .SetAdCount(1) //请求广告数量为1条，只支持同一时间显示1条
            .Build();
        SDK.CreateAdNative().LoadNativeExpressAd(adSlot, new ExpressAdListener(adManager));
    }

    public static void ShowExpressFeedAd(AdManager adManager)
    {
        if (adManager.mExpressFeedad == null)
        {
            Debug.LogError("CSJM_Unity "+ "Example " + "请先加载广告");
            return;
        }
        adManager.mExpressFeedad.SetDislikeCallback(new ExpressAdDislikeCallback(adManager));
        adManager.mExpressFeedad.SetDownloadListener(new AppDownloadListener(adManager));
        adManager.mExpressFeedad.SetAdInteractionListener(new TTAdInteractionListener());
        adManager.mExpressFeedad.ShowExpressAd(0, 500);
    }

    // 广告加载监听器
    public sealed class ExpressAdListener : IExpressAdListener
    {
        private AdManager _adManager;

        public ExpressAdListener(AdManager adManager)
        {
            this._adManager = adManager;
            Debug.Log("ExpressFeedAdListener");
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("CSJM_Unity "+ "Example " + "onExpressFeedAdError: " + message);
        }

        public void OnExpressAdLoad(List<ExpressAd> ads)
        {
            if (ads != null && ads.Count > 0)
            {
                Debug.Log("CSJM_Unity "+ "Example " + "OnExpressFeedAdLoad, count: " + ads.Count);
                this._adManager.mExpressFeedad = ads[0];
                this._adManager.mExpressFeedad.SetExpressInteractionListener(new ExpressAdInteractionListener(_adManager));
            }
        }
    }

    // 广告展示监听器
    public sealed class ExpressAdInteractionListener : IExpressAdInteractionListener
    {
        private AdManager _adManager;

        public ExpressAdInteractionListener(AdManager adManager)
        {
            this._adManager = adManager;
        }

        public void OnAdClicked(ExpressAd ad)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express feed OnAdClicked");
            this._adManager.mExpressFeedad.UploadDislikeEvent("csjm_unity expressFeed dislike test");
        }

        public void OnAdShow(ExpressAd ad)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express feed OnAdShow");
        }

        public void OnAdViewRenderError(ExpressAd ad, int code, string message)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express feed OnAdViewRenderError");
        }

        public void OnAdViewRenderSucc(ExpressAd ad, float width, float height)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express feed OnAdViewRenderSucc");
        }

        public void OnAdClose(ExpressAd ad)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express feed OnAdClose");
        }

        public void onAdRemoved(ExpressAd ad)
        {
            Debug.Log("CSJM_Unity " + "Example " + "express feed onAdRemoved");
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
            Debug.Log("CSJM_Unity "+ "Example " + "express feed dislike OnCancel");
        }

        public void OnShow()
        {
            Debug.Log("CSJM_Unity "+ "Example " + "express feed dislike OnShow");
        }

        public void OnSelected(int var1, string var2, bool enforce)
        {
            Debug.Log("CSJM_Unity "+ "Example " + "express feed dislike OnSelected:" + var2);
            //释放广告资源
            if (this._adManager.mExpressFeedad != null)
            {
                this._adManager.mExpressFeedad.Dispose();
                this._adManager.mExpressFeedad = null;
            }
        }
    }
}