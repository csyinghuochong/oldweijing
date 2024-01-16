using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class UI_RmbStore : MonoBehaviour {
    public bool buyStatus;             //当开启支付时打开此状态
    private bool buyQueryStatus;        //党开启支付查询时打开此状态
    //private string payValue;            //支付额度
    private float rmbPayValue;          //当前支付额度
    private int rmbToZuanShiValue;      //人民币兑换钻石数量
    private string buyStatusStr;
    private string payStatusType;      //支付返回状态类型值
    private string payStatusStr;        //支付返回字符串
    private string payStr;              //支付返回值
    private string payType;             //支付类型  1:表示支付宝   2：表示微信
    private bool clickPayBtnStatus;     //点击支付按钮后开启此状态,保证当前只有一个支付调用

    public GameObject payText;
    public GameObject Obj_RoseZuanShiValue;
    public GameObject Obj_ImgZhiFuBao;
    public GameObject Obj_ImgWeiXin;
    public GameObject Obj_WeiXinChaJianHint;        //微信插件文字提示


    [DllImport("__Internal")]
    private static extern void InitIAPManager();//初始化

    [DllImport("__Internal")]
    private static extern bool IsProductAvailable();//判断是否可以购买

    [DllImport("__Internal")]
    private static extern void RequstProductInfo(string s);//获取商品信息

    [DllImport("__Internal")]
    private static extern void BuyProduct(string s);//购买商品

	// Use this for initialization
	void Start () {

#if UNITY_IPHONE
        //初始化
        InitIAPManager();
#endif

        int zuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        Obj_RoseZuanShiValue.GetComponent<Text>().text = zuanShi.ToString()+"钻";

        //获取当前支付方式
        payType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PayType", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (payType == "" || payType == "0") {
            payType = "1";
        }

#if UNITY_ANDROID
        //微信提示
        if (payType == "2")
        {
            Obj_WeiXinChaJianHint.SetActive(true);
        }
        else {
            Obj_WeiXinChaJianHint.SetActive(false);
        }
#endif

        seclectPayType(payType);




	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("rmbToZuanShiValue = " + rmbToZuanShiValue);
        buyStatus = Game_PublicClassVar.Get_game_PositionVar.PayStatus;
        if (buyStatus) {
            payStr = "支付状态开启:" + Game_PublicClassVar.Get_game_PositionVar.PayStr;
            payStatusType = Game_PublicClassVar.Get_game_PositionVar.PayStr.Split(';')[0];
            payStatusStr = Game_PublicClassVar.Get_game_PositionVar.PayStr.Split(';')[1];
            switch (payStatusType)
            { 
                //支付状态
                case "0":
                    payStr = "支付中……" + payStatusStr;
                    break;

                //支付成功
                case "1":
                    //删除充值记录
                    Game_PublicClassVar.Get_function_Rose.DeletePayID(Game_PublicClassVar.Get_game_PositionVar.PayDingDanIDNow);
                    payStr = rmbPayValue + "支付成功！" + payStatusStr;
                    //累计当前充值额度,发送指定钻石奖励
                    Game_PublicClassVar.Get_function_Rose.AddRMB(rmbToZuanShiValue);
                    Game_PublicClassVar.Get_function_Rose.AddPayValue(rmbPayValue);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint("支付成功:" + rmbPayValue + "元");
                    int zuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
                    Obj_RoseZuanShiValue.GetComponent<Text>().text = zuanShi.ToString()+"钻";
                    
                    float youmengRmbValue = rmbPayValue;
                    rmbToZuanShiValue = 0;
                    rmbPayValue = 0;
                    ClearnPayValue();       //清理支付值
                    break;

                //支付失败
                case "2":
                    payStr = "支付失败！" + payStatusStr;
                    ClearnPayValue();       //清理支付值
                    rmbToZuanShiValue = 0;
                    rmbPayValue = 0;

#if UNITY_ANDROID
                    QueryDingDanStatus();   //支付失败调用一次检查

                    //防止支付宝第一次调用错误
                    int ZhiFuBaoOpenNum = PlayerPrefs.GetInt("ZhiFuBaoOpenNum");
                    if (ZhiFuBaoOpenNum == 0) {
                        PlayerPrefs.SetInt("ZhiFuBaoOpenNum", 1);
                        if (Game_PublicClassVar.Get_game_PositionVar.PayValueNow != "") {
                            Btn_BuyZuanShi(Game_PublicClassVar.Get_game_PositionVar.PayValueNow);
                        }
                    }


                    if (payStatusStr.IndexOf("disable") != -1) {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("微信支付暂时关闭,请使用支付宝支付!!!");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("微信支付暂时关闭,请使用支付宝支付!!!");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("微信支付暂时关闭,请使用支付宝支付!!!");
                    }
#endif
                    break;

                //其他原因
                case "3":
                    payStr = "支付未知原因！" + payStatusStr;
                    ClearnPayValue();       //清理支付值
                    break;

                default :
                    payStr = "支付default" + payStatusStr;
                    ClearnPayValue();       //清理支付值
                    break;

                Debug.Log("payStr: " + payStr);
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);
            }
        }

        payText.GetComponent<Text>().text = payStr;
        /*
        buyQueryStatus = Game_PublicClassVar.Get_game_PositionVar.PayQueryStatus;
        if (buyQueryStatus) {
            payStr = "查询状态：" + Game_PublicClassVar.Get_game_PositionVar.PayStrQueryStatus;
        }
        payText.GetComponent<Text>().text = payStr;
        */

        //更新钻石显示
        if (Game_PublicClassVar.Get_game_PositionVar.PayQueryStatus) {
            Game_PublicClassVar.Get_game_PositionVar.PayQueryStatus = false;
            int zuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
            Obj_RoseZuanShiValue.GetComponent<Text>().text = zuanShi.ToString() + "钻";
        }
	}
    
    //清理支付信息
    public void ClearnPayValue() {
        Debug.Log("清理支付信息！");
        clickPayBtnStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.PayStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.PayStr = "";
        buyStatus = false;
    }

    //根据支付额度返回应获取的额度
    private int ReturnPayValue(string payValue){
        int returnZuanShiValue = 0;
        switch (payValue) {
        /*
        case "0.03":
              returnZuanShiValue = 6000;
              Debug.Log("匹配到了");
              break;
         */
          /*
          case "1.1":
              returnZuanShiValue = 5000;
              break;
          */
            //安卓付费额度
#if UNITY_ANDROID
            case "9.8":
                returnZuanShiValue = 1000;
                break;
            case "49.8":
                returnZuanShiValue = 6000;
                break;
            case "99.8":
                returnZuanShiValue = 13000;
                break;
            case "498":
                returnZuanShiValue = 75000;
                break;
            case "888":
                returnZuanShiValue = 145000;
                break;
#endif

            //ios付费额度
#if UNITY_IPHONE
            case "6":
                returnZuanShiValue = 600;
                break;
            case "50":
                returnZuanShiValue = 6000;
                break;
            case "98":
                returnZuanShiValue = 13000;
                break;
            case "298":
                returnZuanShiValue = 45000;
                break;
            case "648":
                returnZuanShiValue = 100000;
                break;
#endif
        }
        return returnZuanShiValue;
    }



    public void Btn_BuyZuanShi(string rmbValue)
    {

        Debug.Log("我点击了支付按钮:" + rmbValue);

        //判断是否可以充值
        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 2)
        {

            Game_PublicClassVar.Get_function_Rose.GamePay(rmbValue, "");
        }
        else
        {
            //弹出对应提示
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
            uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n 为落实国家相关政策,保护未成年人健康良好的成长,指引正确的消费价值观,我们不会向未实名认证以及未成年玩家提供任何充值服务,感谢你对游戏的支持!", null, null, "温馨提示", "确认", "确认");
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
        }


        return;

        if(clickPayBtnStatus){
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付开启中,请稍等……！");
            return;
        }
        clickPayBtnStatus = true;
        //判定当前是否在支付状态,如果是,则取消本次支付
        if (buyStatus)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付繁忙,请稍后支付,如长时间无反应请重启游戏应用！");
            return;
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付转接中,请不要关闭界面……");
        }

        string shopName = "感谢您的赞助";     //最多6个字
        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string GoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string RMB = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string RMBPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string shopDes = "赞助角色-" + roseName + "ID-" + zhanghaoID + " 等级-" + roseLv + " 金币-" + GoldNum + " 钻石-" + RMB + "已赞助-" + RMBPayValue + "安卓支付";

        //英语设置订单的金额存储
        Game_PublicClassVar.Get_game_PositionVar.PayValueNow = rmbValue;

        //根据选择调用不同平台的支付接口(暂时调用支付宝)
        rmbPayValue = float.Parse(rmbValue);
        Debug.Log("rmbPayValue = " + rmbPayValue);
        rmbToZuanShiValue = ReturnPayValue(rmbValue);

        //ios付费
#if UNITY_IPHONE
        BuyProduct(rmbPayValue + "R");
#endif

        //安卓付费
#if UNITY_ANDROID
        switch (payType)
        {
            //支付宝
            case "1":
                Buy_ZhiFuBao(shopName, shopDes, rmbPayValue);
                break;
            //微信
            case "2":
                Buy_WeiXin(shopName, shopDes, rmbPayValue);
                break;
        }

        //去掉订单的最前面10个
        string dingdanStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] dingdanIDList = dingdanStatus.Split(';');
        string saveDingDan = "";
        if (dingdanIDList.Length >= 10)
        {
            for (int i = 0; i <= dingdanIDList.Length - 1; i++)
            {
                if (i >= 5)
                {
                    if (saveDingDan == "")
                    {
                        saveDingDan = saveDingDan + dingdanIDList[i];
                    }
                    else
                    {
                        saveDingDan = saveDingDan + ";" + dingdanIDList[i];
                    }
                }
            }

            //Game_PublicClassVar.Get_function_UI.GameHint("清理订单缓存!");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_1", saveDingDan, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
#endif
        //ReturnPayValue("a");
        
        //Buy_WeiXin(shopName, shopDes, ddd);
        //int getPayValue = 0;    //重置支付金额
        //设置支付信息
    }

    //支付宝支付
    public void Buy_ZhiFuBao(string shopName,string shopDes, float value)
    {
#if UNITY_ANDROID
        Debug.Log("付费" + value + "元");
        payStr = "调用支付方法";
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //activity.Call("Pay_ZhiFuBao", shopName, shopDes, value);         //调用支付方法
        string payValueStr = value.ToString();
        activity.Call("Pay_ZhiFuBao", payValueStr, shopDes);                     //调用支付方法
#endif
    }


    //微信支付
    public string Buy_WeiXin(string shopName, string shopDes,float value)
    {
#if UNITY_ANDROID
        Debug.Log("Value = " + value);
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //activity.Call("Pay_WeiXin", shopName, shopDes, value);        //调用支付方法
        string payValueStr = value.ToString();
        activity.Call("Pay_WeiXin", payValueStr);                     //调用支付方法
#endif
        return buyStatusStr;

    }

    //选择
    public void BtnSeclect_ZhiFuBao() {
        payType = "1";
        seclectPayType(payType);
        //Obj_WeiXinChaJianHint.SetActive(false);
    }

    //选择
    public void BtnSeclect_WeiXin() {
        payType = "2";
        seclectPayType(payType);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("提示:微信支付需要安装安全支付插件！");
        //Obj_WeiXinChaJianHint.SetActive(true);

    }

    void seclectPayType(string payTypeStr)
    {

#if UNITY_ANDROID
        switch (payTypeStr)
        { 
            //支付宝
            case "1":
                Obj_ImgZhiFuBao.SetActive(true);
                Obj_ImgWeiXin.SetActive(false);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PayType", payTypeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            break;

            //微信
            case "2":
                Obj_ImgZhiFuBao.SetActive(false);
                Obj_ImgWeiXin.SetActive(true);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PayType", payTypeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            break;
        }
#endif
    }

    //调用查询按钮
    public void QueryDingDanStatus() {

#if UNITY_ANDROID
        string dingdanStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //string dingdanStatus = "62918c959364ba417c60313592d48444;469ee4e36115f83824674bd6885efdbe;63c01e3f9532e0e095193ac80ce0e4fe;4cf61e34c23a9d1b2bdad5c939b01038";
        string[] dingdanIDList = dingdanStatus.Split(';');
        for (int i = 0; i <= dingdanIDList.Length - 1; i++) {
            //发送查询信息
            if (dingdanIDList[i] != "0" && dingdanIDList[i] != "") {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("Query", dingdanIDList[i].Split(',')[0]);         //调用支付方法
            }
        }
#endif
    }

    /*
    public void OnPayResultReturn(string str)
    {
        Debug.Log("调用了支付返回值");
        payStr = "调用付费返回值";
        //payStr = str;
        if (str != "") {
            payStatusType = str.Split(';')[0];
            payStatusStr = str.Split(';')[1];
        }

        buyStatus = true;   //开启支付状态
    }
    */
    public void Btn_Close() {

        if (clickPayBtnStatus == true)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("提示:你已充值,请耐心等待支付返回结果,不要关闭此界面和退出游戏！");
        }
        else {
            buyStatus = false;
            payStr = "清空";
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_RmbStore();
            ClearnPayValue();
        }
    }

    public void Btn_Close1111()
    {
        //buyStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_RmbStore();
    }

}
