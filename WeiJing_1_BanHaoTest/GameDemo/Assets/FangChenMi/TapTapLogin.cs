using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TapSDK.Login;
using UnityEngine.UI;
using TapSDK.Compliance;
using System.Collections.Generic;

/// <summary>
/// 登录场景
/// </summary>
public class TapTapLogin : MonoBehaviour
{

    /// <summary>
    /// 初始化 SDK 并判断本地是否已登录，已登录时开始合规认证检查，否则显示登录按钮
    /// </summary>
    async void Start()
    {
        Debug.Log("GameSDKManager.Instance.InitSDK");


        // 初始化 SDK
        GameSDKManager.Instance.InitSDK();

        // 注册合规认证限制事件回调，显示对应提示弹窗
        GameSDKManager.Instance.RegisterListener(HandleAntiAddictionError);

        TapTapAccount account = null;
        try
        {
            // 检查本地是否已存在 account 信息
            account = await TapSDK.Login.TapTapLogin.Instance.GetCurrentTapAccount();
        }
        catch (Exception e)
        {
            Debug.Log("本地无用户信息");
        }

        // 根据本地是否存在用户信息显示不同 UI 
        SwitchLoginState(account != null);

        // 本地存在用户信息且未通过合规认证时进行合规认证检查
        if (account != null && !GameSDKManager.Instance.hasCheckedCompliance)
        {
            //Debug.Log("本地有用户信息");
            //StartCheckCompliance();
        }
    }

    /// <summary>
    /// 登录按钮点击后执行 Tap 登录
    /// </summary>
    public async void OnTapLoginButtonClick()
    {
        Debug.Log("OnTapLoginButtonClick");

        TapTapAccount account = null;
        //try
        //{
        //    // 检查本地是否已存在 account 信息
        //    account = await TapSDK.Login.TapTapLogin.Instance.GetCurrentTapAccount();
        //}
        //catch (Exception e)
        //{
        //    Debug.Log("本地无用户信息");
        //}

        //// 本地存在用户信息且未通过合规认证时进行合规认证检查
        //if (account != null && !GameSDKManager.Instance.hasCheckedCompliance)
        //{
        //    //Debug.Log("本地有用户信息");
        //    StartCheckCompliance();
        //    return;
        //}


        try
        {
            List<string> scopes = new List<string>
            {
                TapSDK.Login.TapTapLogin.TAP_LOGIN_SCOPE_PUBLIC_PROFILE
            };
            // 发起 Tap 登录并获取用户信息
            account = await TapSDK.Login.TapTapLogin.Instance.LoginWithScopes(scopes.ToArray());

            // 切换 UI 显示状态
            SwitchLoginState(true);

            // 开始合规认证检查
            StartCheckCompliance();
        }
        catch (Exception e)
        {
            Debug.Log("用户登录取消或错误 :" + e.Message);
        }
    }

    /// <summary>
    /// 根据登录状态切换 UI 
    /// </summary>
    /// <param name="logged">是否已登录</param>
    private async void SwitchLoginState(bool logged = true)
    {
        if (logged)
        {
            var account = await TapSDK.Login.TapTapLogin.Instance.GetCurrentTapAccount();
            string tv_userTiptext = account.name + " 欢迎回来 ！";
            Debug.Log($"SwitchLoginState:  {tv_userTiptext}");
        }
        else
        {

        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void OnEnterGame()
    {
        
    }

    /// <summary>
    /// 切换账号
    /// </summary>
    public void OnSwitchAccount()
    {
        TapSDK.Login.TapTapLogin.Instance.Logout();
        TapTapCompliance.Exit();
        SwitchLoginState(false);
    }

    /// <summary>
    /// 因触发限制切换账号
    /// </summary>
    public void OnSwitchAccountForRestrict()
    {
        OnSwitchAccount();
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void OnExit()
    {
        Application.Quit();
    }


    /// <summary>
    /// 重新出发防沉迷
    /// </summary>
    public void OnRetryAntiaddiction()
    {
        StartCheckCompliance();
    }

    /// <summary>
    /// 开始合规认证检查
    /// </summary>
    public async void StartCheckCompliance()
    {
        Debug.Log("StartCheckCompliance");

        // 获取当前已登录用户的 account 信息
        TapTapAccount account = null;
        try
        {
            account = await TapSDK.Login.TapTapLogin.Instance.GetCurrentTapAccount();
        }
        catch (Exception exception)
        {
            Debug.Log($"获取 account 信息出现异常：{exception}");
        }
        if (account == null)
        {
            // 无法获取 Profile 时，登出并显示登录按钮
            TapSDK.Login.TapTapLogin.Instance.Logout();
            SwitchLoginState(false);
            return;
        }

        // 使用当前 Tap 用户的 unionid 作为用户标识进行合规认证检查
        string userIdentifier = account.unionId;
        GameSDKManager.Instance.StartCheckCompliance(userIdentifier);
    }

    /// <summary>
    /// 处理合规认证限制异常回调，显示对应异常 UI
    /// </summary>
    /// <param name="type">异常类型</param>
    public async void HandleAntiAddictionError(int type)
    {
        Debug.Log($"HandleAntiAddictionError:  {type}");
        switch (type)   
        {
            case GameSDKManager.EVENT_TYPE_AGE_RESTRICT:
                Debug.Log($"适龄限制") ;   //未成年啥的
                break;
            case GameSDKManager.EVENT_TYPE_NETWORK_ERROR:
                Debug.Log($"网络异常");   //没网络或者后台配置不对
                break;
            default:
                Debug.Log($"可以游戏");
                break;
        }

        //授权成功
        int age = await GameSDKManager.Instance.GetAgeRange();
        int remaintime = await GameSDKManager.Instance.GetRemainingTime();
        Debug.Log($"HandleAntiAddictionError: age: {age}  remaintime: {remaintime}");
        this.gameObject.GetComponent<WWWSet>().AgeRange = age;
        this.gameObject.GetComponent<WWWSet>().RemainingTime = remaintime;
        if (type == 0)
        {
            //可以游戏  todo
        }
        else
        {
            //根据年龄和剩余时间做处理
        }
    }

    void OnDestroy()
    {
        // 移除对应监听
        GameSDKManager.Instance.UnRegisterListener(HandleAntiAddictionError);
    }

}
