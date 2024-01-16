//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using cn.sharesdk.unity3d;

//public class UI_GameFenXiang : MonoBehaviour {
    
//    private string FenXiang_Type;           //1：微信朋友圈   2：QQ空间  3：新浪微博  4：微信好友  5:QQ好友
//    private string FenXiang_Title;          //分享标题
//    private string FenXiang_Text;           //分享文本
//    private string FenXiang_ImageUrl;       //分享图片路径
//    private string FenXiang_ClickUrl;       //分享点击链接

//    public ShareSDK ssdk;
//    private string showText;                //分享结果展示文本
//    public GameObject Rmb_WeiXin;
//    public GameObject YiJingQu_WeiXin;
//    public GameObject Rmb_QQ;
//    public GameObject YiJingQu_QQ;
//    public GameObject Rmb_Sina;
//    public GameObject YiJingQu_Sina;
//    private string FenXiangTypeStr;
    

//	// Use this for initialization
//	void Awake () {

//        FenXiang_Title = "一把木剑、野外地图、随机属性、隐藏任务，和我一起开启一个新的冒险！";
//        FenXiang_Text = "我发现了一款超好玩的暗黑单机ARPG手游《危境》！";
//        FenXiang_ImageUrl = "https://img.tapimg.com/market/lcs/db4bb6c16ab0863791852f5619e4530c_360.png?imageView2/1/w/180/q/40/interlace/1/ignore-error/1";
//        //TapTap
//        FenXiang_ClickUrl = "http://l.taptap.com/Hys7qS2P";         //点击链接填写安卓链接,标题写苹果用户直接搜索危境下载
//        //豌豆荚
//        //FenXiang_ClickUrl = "https://www.wandoujia.com/apps/com.example.weijinggame?utm_source=yunying&utm_medium=cpc&utm_term=危境:崛起下载安装&utm_content=(广泛)危境:崛起top词&utm_campaign=004";         //点击链接填写安卓链接,标题写苹果用户直接搜索危境下载



//        //FenXiang_ClickUrl = "https://www.taptap.com/app/43609";         //点击链接填写安卓链接,标题写苹果用户直接搜索危境下载
    
//	    //分享回调
//        /*
//        ssdk = gameObject.GetComponent<ShareSDK>();
//        ssdk.authHandler = OnAuthResultHandler;
//        ssdk.shareHandler = OnShareResultHandler;
//        ssdk.showUserHandler = OnGetUserInfoResultHandler;
//        ssdk.getFriendsHandler = OnGetFriendsResultHandler;
//        ssdk.followFriendHandler = OnFollowFriendResultHandler;
//        */

//        ssdk = gameObject.GetComponent<ShareSDK>();
//        ssdk.authHandler = OnAuthResultHandler;
//        ssdk.shareHandler = OnShareResultHandler;
//        ssdk.showUserHandler = OnGetUserInfoResultHandler;
//        ssdk.getFriendsHandler = OnGetFriendsResultHandler;
//        ssdk.followFriendHandler = OnFollowFriendResultHandler;

//        updataFenXiangShow();
         
//	}
	
//	// Update is called once per frame
//	void Update () {
	
//	}

//    void OnGUI()
//    {

//    }
    
//    void updataFenXiangShow() {
//        //初始化分享显示
//        //微信显示
//        string fengxiang_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//        string fengxiang_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//        string fengxiang_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//        //微信
//        if (fengxiang_1 == "0")
//        {
//            //未分享
//            Rmb_WeiXin.SetActive(true);
//            YiJingQu_WeiXin.SetActive(false);
//        }
//        else
//        {
//            //已分享
//            Rmb_WeiXin.SetActive(false);
//            YiJingQu_WeiXin.SetActive(true);
//        }
//        //QQ
//        if (fengxiang_2 == "0")
//        {
//            //未分享
//            Rmb_QQ.SetActive(true);
//            YiJingQu_QQ.SetActive(false);
//        }
//        else
//        {
//            //已分享
//            Rmb_QQ.SetActive(false);
//            YiJingQu_QQ.SetActive(true);
//        }
//        //新浪
//        if (fengxiang_3 == "0")
//        {
//            //未分享
//            Rmb_Sina.SetActive(true);
//            YiJingQu_Sina.SetActive(false);
//        }
//        else
//        {
//            //已分享
//            Rmb_Sina.SetActive(false);
//            YiJingQu_Sina.SetActive(true);
//        }
//    }

//    public void FenXiang(string fenxiangtype)
//    {
//        FenXiangTypeStr = "";
//        showText = "点击分享按钮";
//        //设置分享
//        ShareContent content = new ShareContent();
//        //title标题，印象笔记、邮箱、信息、微信、人人网、QQ和QQ空间使用
//        content.SetTitle(FenXiang_Title);
//        //分享文本
//        content.SetText(FenXiang_Text);
//        //分享网络图片，新浪微博分享网络图片需要通过审核后申请高级写入接口，否则请注释掉测试新浪微博
//        content.SetImageUrl(FenXiang_ImageUrl);
//        // titleUrl是标题的网络链接，仅在Linked-in,QQ和QQ空间使用
//        content.SetTitleUrl(FenXiang_ClickUrl);
//        // imagePath是图片的本地路径，Linked-In以外的平台都支持此参数
//        //content.SetImagePath("/sdcard/test.jpg");//确保SDcard下面存在此张图片
//        // url仅在微信（包括好友和朋友圈）中使用
//        content.SetUrl(FenXiang_ClickUrl);
//        // site是分享此内容的网站名称，仅在QQ空间使用
//        content.SetSite(FenXiang_Title);
//        // siteUrl是分享此内容的网站地址，仅在QQ空间使用
//        content.SetSiteUrl(FenXiang_ClickUrl);
//        //设置分享类型
//        content.SetShareType(ContentType.Webpage);
//        //content.SetShareType(ContentType.Text);
//        FenXiangTypeStr = fenxiangtype;

//        switch (fenxiangtype)
//        {
//            //微信朋友圈
//            case "1":
//                //弹出菜单
//                ssdk.ShareContent(PlatformType.WeChatMoments, content);     
//                break;
//            //QQ空间
//            case "2":
//                //弹出菜单
//                ssdk.ShareContent(PlatformType.QZone, content);
//                break;
//            //新浪微博
//            case "3":
//                //弹出菜单
//                ssdk.ShareContent(PlatformType.SinaWeibo, content);
//                break;
//            //微信好友
//            case "4":
//                //弹出菜单
//                ssdk.ShareContent(PlatformType.WeChat, content);
//                break;
//            //QQ好友
//            case "5":
//                //弹出菜单
//                ssdk.ShareContent(PlatformType.QQ, content);
//                break;
//        }

//    }

//    public void Btn_CloseUI() {
//        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_FenXiang();
//        //Destroy(this.gameObject);
//    }


//    public void Btn_PingLun()
//    {
//        Application.OpenURL("http://l.taptap.com/Hys7qS2P");
//    }


//    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        showText = "进入授权回调！";
//        if (state == ResponseState.Success)
//        {
//            showText = "授权用户分享成功！";
//            print("authorize success !" + "Platform :" + type);
//        }
//        else if (state == ResponseState.Fail)
//        {
//            showText = "授权用户分享失败！";
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//            showText = "进入授权取消！";
//        }
//    }

//    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        showText = "进入指定用户回调！";
//        if (state == ResponseState.Success)
//        {
//            showText = "指定用户分享成功！";
//            print("get user info result :");
//            print(MiniJSON.jsonEncode(result));
//            print("Get userInfo success !Platform :" + type);
//        }
//        else if (state == ResponseState.Fail)
//        {
//            showText = "指定用户分享失败！";
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            showText = "进入指定用户回调取消！";
//            print("cancel !");
//        }
//    }

//    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        showText = "进入微信用户回调！";
//        if (state == ResponseState.Success)
//        {
//            showText = "微信分享成功！";

//            //判断当前次数是否已经分享
//            //微信显示
//            string fengxiang_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//            string fengxiang_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//            string fengxiang_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

//            //写入分享数据
//            switch (FenXiangTypeStr)
//            {
//                case "1":
//                    if (fengxiang_1 == "0") {
//                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_1", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
//                        Game_PublicClassVar.Get_function_Rose.SendReward("2", "200");
//                    }
//                    break;
//                case "2":
//                    if (fengxiang_2 == "0") {
//                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_2", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
//                        Game_PublicClassVar.Get_function_Rose.SendReward("2", "200");
//                    }
//                    break;
//                case "3":
//                    if (fengxiang_3 == "0") {
//                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_3", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
//                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
//                        Game_PublicClassVar.Get_function_Rose.SendReward("2", "200");
//                    }
//                    break;
//            }
//            updataFenXiangShow();//更新显示
//            print("share successfully - share result :");
//            print(MiniJSON.jsonEncode(result));
//        }
//        else if (state == ResponseState.Fail)
//        {
//            showText = "微信分享失败！";
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            showText = "进入指定用户回调取消！";
//            print("cancel !");
//        }
//    }

//    void OnGetFriendsResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        showText = "进入回调AAA—111111111！";
//        if (state == ResponseState.Success)
//        {
//            showText = "进入回调AAA—222222222！";
//            print("get friend list result :");
//            print(MiniJSON.jsonEncode(result));
//        }
//        else if (state == ResponseState.Fail)
//        {
//            showText = "进入回调AAA—333333333！";
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            showText = "进入回调AAA—44444444！";
//            print("cancel !");
//        }
//    }

//    void OnFollowFriendResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
//    {
//        showText = "进入回调BBBB—111111111！";
//        if (state == ResponseState.Success)
//        {
//            showText = "进入回调BBBB—22222222！";
//            print("Follow friend successfully !");
//        }
//        else if (state == ResponseState.Fail)
//        {
//            showText = "进入回调BBBB—333333333！";
//#if UNITY_ANDROID
//            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
//#elif UNITY_IPHONE
//			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
//#endif
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            showText = "进入回调BBBB—444444444！";
//            print("cancel !");
//        }
//    }
    
//}
