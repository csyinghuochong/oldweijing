using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_HuoDongDaTing : MonoBehaviour {

    public GameObject Obj_YueKaSet;
    public GameObject Obj_XueLieHaoSet;
    public GameObject Obj_DengLuSet;
    public GameObject Obj_ZhiChiZuoZheSet;
    public GameObject Obj_MeiRiRewardSet;

    public GameObject Obj_XuLieHaoID;       //序列号Obj
    public GameObject Obj_XuLieHaoQQ;
    //月卡相关
    public GameObject Obj_YueKa_DayNum;
    public GameObject Obj_YueKa_BuyText;
    public GameObject Obj_YueKa_Buy;
    public GameObject Obj_YueKa_Get;
    public GameObject Obj_YueKaRewardItemObjSet;
    public GameObject Obj_YueKaRewardItemObj;

    //支持作者
    public GameObject Obj_ZhiChiZuoZhe_BuyText;
    public GameObject Obj_ZhiChiZuoZheItemObjSet;
    public GameObject Obj_ZhiChiZuoZheItemObj;

    //登陆
    public GameObject Obj_DengLuRewardDayObj;
    public GameObject Obj_DengLuRewardDayObjSet;

    //每日奖励
    public GameObject Obj_MeiRiPayValue;

    private string ZuiHouLiBaoID;
    private int yincangnNum;
    public GameObject xuliehaoobj;
    
	// Use this for initialization
	void Start () {

        ZuiHouLiBaoID = "10014";
	    //默认显示月卡
        Btn_YueKaSet();

#if UNITY_IPHONE	
        xuliehaoobj.SetActive(false);
#endif

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //显示月卡
    public void Btn_YueKaSet() {
        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_YueKaRewardItemObjSet);
        Obj_YueKaSet.SetActive(true);
        Obj_XueLieHaoSet.SetActive(false);
        Obj_DengLuSet.SetActive(false);
        Obj_ZhiChiZuoZheSet.SetActive(false);
        Obj_MeiRiRewardSet.SetActive(false);

        //显示月卡内容
        string yuekaValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKa", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Debug.Log("yuekaValue = " + yuekaValue) ;
        int yuekaValueInt = int.Parse(yuekaValue) - 1;
        //如果月卡状态为0,则置为月卡天数为0
        if (yuekaValueInt < 0) {
            yuekaValueInt = 0;
        }
        Obj_YueKa_DayNum.GetComponent<Text>().text = "周卡剩余领取次数：" + yuekaValueInt.ToString() + "/7";
        if (yuekaValue == "0")
        {
            int buyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaBuyValue", "GameMainValue"));
            Obj_YueKa_BuyText.SetActive(true);
            Obj_YueKa_BuyText.GetComponent<Text>().text = "开启周卡需消耗：" + buyValue + "钻石(最多购买1次)";
            Obj_YueKa_Buy.SetActive(true);
            Obj_YueKa_Get.SetActive(false);
        }
        else {
            Obj_YueKa_Buy.SetActive(false);
            Obj_YueKa_Get.SetActive(true);
            Obj_YueKa_BuyText.SetActive(false);
        }

        
        string[] rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaReward", "GameMainValue").Split(';');
        //显示奖励
        for (int i = 0; i <= rewardStr.Length - 1; i++)
        {
            string[] rewardYueKaStr = rewardStr[i].Split(',');
            GameObject itemObj = (GameObject)Instantiate(Obj_YueKaRewardItemObj);
            itemObj.transform.SetParent(Obj_YueKaRewardItemObjSet.transform);
            itemObj.transform.localPosition = new Vector3(100 * i-50, 0, 0);
            itemObj.transform.localScale = new Vector3(1, 1, 1);
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemID = rewardYueKaStr[0];
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = rewardYueKaStr[1];
        }
    }
    //显示序列号
    public void Btn_XueLieHaoSet()
    {
        Obj_YueKaSet.SetActive(false);
        Obj_XueLieHaoSet.SetActive(true);
        Obj_DengLuSet.SetActive(false);
        Obj_ZhiChiZuoZheSet.SetActive(false);
        Obj_MeiRiRewardSet.SetActive(false);

        //获取加群序列号是否领取
        if (Game_PublicClassVar.Get_function_Rose.ifGetXuLieHao("weijing666"))
        {
            Obj_XuLieHaoQQ.SetActive(false);
        }
        else {
            Obj_XuLieHaoQQ.SetActive(true);
        }
    }
    //显示登陆
    public void Btn_DengLuSet()
    {
        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_DengLuRewardDayObjSet);
        Obj_YueKaSet.SetActive(false);
        Obj_XueLieHaoSet.SetActive(false);
        Obj_DengLuSet.SetActive(true);
        Obj_ZhiChiZuoZheSet.SetActive(false);
        Obj_MeiRiRewardSet.SetActive(false);

        for (int i = 1; i <= 7; i++) { 
            string dengLuRewardValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DengLu_" + i, "GameMainValue");
            //实例化控件
            GameObject obj = (GameObject)Instantiate(Obj_DengLuRewardDayObj);
            obj.transform.SetParent(Obj_DengLuRewardDayObjSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_HuoDongDaTingDengLu>().DengLuRewardStr = dengLuRewardValue;
            obj.GetComponent<UI_HuoDongDaTingDengLu>().DengLuRewardDay = i.ToString();
        }
    }
    //显示支持作者
    public void Btn_ZhiChiZuoZheSet()
    {

        yincangnNum = yincangnNum + 1;
        if (yincangnNum >= 4)
        {
            xuliehaoobj.SetActive(true);
        }

        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ZhiChiZuoZheItemObjSet);
        Obj_YueKaSet.SetActive(false);
        Obj_XueLieHaoSet.SetActive(false);
        Obj_DengLuSet.SetActive(false);
        Obj_ZhiChiZuoZheSet.SetActive(true);
        Obj_MeiRiRewardSet.SetActive(false);

        //读取当前奖励
        string zhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (zhiChiZuoZheID == ZuiHouLiBaoID)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("感谢你为作者做出的赞助,已经够多了,作者在此感谢你！");
            Obj_ZhiChiZuoZheItemObjSet.SetActive(false);
            Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text = "感谢你为作者做出的赞助,已经够多了,作者在此感谢你！";
            return;
        }
        string[] zhiChiZuoZheValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ZhiChiZuoZhe_" + zhiChiZuoZheID, "GameMainValue").Split(';');
        float nowPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

        //显示奖励
        for (int i = 1; i <= zhiChiZuoZheValue.Length - 1; i++)
        {
            string[] rewardStr = zhiChiZuoZheValue[i].Split(',');
            GameObject itemObj = (GameObject)Instantiate(Obj_ZhiChiZuoZheItemObj);
            itemObj.transform.SetParent(Obj_ZhiChiZuoZheItemObjSet.transform);
            itemObj.transform.localPosition = new Vector3(100 * i, 0, 0);
            itemObj.transform.localScale = new Vector3(1, 1, 1);
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemID = rewardStr[0];
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = rewardStr[1];
        }
    
        //显示赞助额度
        float needValue = float.Parse(zhiChiZuoZheValue[0]) - nowPayValue;
        string needValueStr = "";
        if (needValue <= 0)
        {
            needValue = 0;
            needValueStr = needValue.ToString();
            Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text = "点击领取作者回馈礼包";
        }
        else {
            //向下取整显示
            //needValueStr = needValue.ToString("0.0");   //保留小数点后一位
            needValueStr = needValue.ToString();
            Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text = "再次赞助" + needValueStr + "元开启礼包";
        }
    }

    //显示每日奖励
    public void Btn_MeiRiRewardSet()
    {
        //清空列表
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_DengLuRewardDayObjSet);
        Obj_YueKaSet.SetActive(false);
        Obj_XueLieHaoSet.SetActive(false);
        Obj_DengLuSet.SetActive(false);
        Obj_ZhiChiZuoZheSet.SetActive(false);
        Obj_MeiRiRewardSet.SetActive(true);

        string payValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_8", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Obj_MeiRiPayValue.GetComponent<Text>().text = "你今日已赞助" + payValue + "元，此额度每日0点清空";

    }


    //月卡购买
    public void Btn_YueKaBuy() { 
    

        //获取钻石当前值
        int buyValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaBuyValue", "GameMainValue"));
        //获取当前月卡状态
        string yuekaValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKa", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (yuekaValue == "0") { 
            bool openStatus = Game_PublicClassVar.Get_function_Rose.CostRMB(buyValue);
            if(openStatus){
                //开启月卡状态
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKa", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                Game_PublicClassVar.Get_function_UI.GameHint("开启周卡成功!");
                //更新UI显示状态
                Btn_YueKaSet();
            }else{
                Game_PublicClassVar.Get_function_UI.GameHint("钻石不足!");
            }
        }
        
    }


    //月卡领取
    public void Btn_YueKaGet() {

        //查看月卡进入是否领取
        string yueKaDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKaDayStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (yueKaDayStatus == "1") {
            Game_PublicClassVar.Get_function_UI.GameHint("今日周卡已经领取");
            return;
        }

        //获取当前月卡状态
        string yuekaValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKa", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (yuekaValue != "0")
        {
            //开启月卡状态
            int yuekaNum = int.Parse(yuekaValue) + 1;
            //如果月卡>=7天则重置其状态
            if (yuekaNum > 7) {
                yuekaNum = 0;
                Debug.Log("yuekaNum1111 = " + yuekaNum);
            }

            //发送月卡奖励
            string[] rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "YueKaReward", "GameMainValue").Split(';');
            //检测背包格子
            if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(rewardStr.Length))
            {
                Debug.Log("yuekaNum = " + yuekaNum);
                //写入月卡每日领取数据
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKaDayStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKa", yuekaNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                //发送奖励
                for (int i = 0; i <= rewardStr.Length - 1; i++)
                {
                    string[] rewardYueKaStr = rewardStr[i].Split(',');
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardYueKaStr[0], int.Parse(rewardYueKaStr[1]));
                }

                //更新UI显示状态
                Btn_YueKaSet();
            }
            else {
                Game_PublicClassVar.Get_function_UI.GameHint("背包请预留至少" + rewardStr.Length +"个位置!");
            }
        }
    }

    //序列号按钮
    public void Btn_XuelieHao() {
        string xulieHaoID = Obj_XuLieHaoID.GetComponent<InputField>().text;
        //Debug.Log("输入值:" + xulieHaoID);
        if (xulieHaoID == "") {
            Game_PublicClassVar.Get_function_UI.GameHint("请输入序列号!");
            return;
        }
        if (Game_PublicClassVar.Get_function_Rose.ifGetXuLieHao(xulieHaoID))
        {
            Game_PublicClassVar.Get_function_UI.GameHint("序列号已被领取!");
            return;
        }

        switch (xulieHaoID) { 

            case "zhongji":

                //升级
                Game_PublicClassVar.Get_function_Rose.SetRoseLv(30);
                Game_PublicClassVar.Get_function_Rose.AddExp(100);
                //主城等级
                Game_PublicClassVar.Get_function_Country.SetGuoJiang(15);
                //关卡
                Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter("3;5");
                break;

            case "gaoji":
                //升级
                Game_PublicClassVar.Get_function_Rose.SetRoseLv(60);
                Game_PublicClassVar.Get_function_Rose.AddExp(100);
                //主城等级
                Game_PublicClassVar.Get_function_Country.SetGuoJiang(25);
                //关卡
                Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter("5;5");
                break;
            
            //未成年
            case "weichengnian":

                Game_PublicClassVar.Get_wwwSet.FangChengMi_Type = 0;

                PlayerPrefs.SetInt("FangChenMi_Type", 0);
                PlayerPrefs.SetString("FangChenMi_Name", "");
                PlayerPrefs.SetString("FangChenMi_ID", "");

                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("激活未成年模式成功!");

                Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng();

                break;

            //成年
            case "chengnian":

                PlayerPrefs.SetInt("FangChenMi_Type", 1);
                PlayerPrefs.SetString("FangChenMi_Name", "王姿瑞");
                PlayerPrefs.SetString("FangChenMi_ID", "430121199607127349");
                PlayerPrefs.SetInt("FangChenMi_Year", 22);
                PlayerPrefs.SetInt("FangChenMi_Type", 2);

                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("激活成年模式成功!");
                Game_PublicClassVar.Get_wwwSet.FangChengMi_Type = 2;

                break;

            case "weijing666":
                Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");
                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(4))
                {

                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                    //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010061", 5);     //发送经验卷轴5个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010071", 5);       //遗失的金币袋子
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010011", 20);      //发送小型止血药20个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010082", 2);       //发送繁荣度勋章2个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010102", 1);       //发送僵尸护符1个
                    
                    //更新UI显示状态
                    Btn_XueLieHaoSet();
                }

                break;

            case "zuozhezhenshuai":
                Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");

                int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                if (roseLv >= 12) {
                    //检测背包格子
                    if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                    {
                        //写入序列号ID
                        Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                        Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 5);       //经验木桩
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010026", 1);       //BOSS冷却卷轴
                        //更新UI显示状态
                        Btn_XueLieHaoSet();
                    }
                }

                break;

            case "weijingweibo":
                Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");

                roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                {
                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 10);       //经验木桩
                    //更新UI显示状态
                    Btn_XueLieHaoSet();
                }

                break;

            case "20180405":

                Game_PublicClassVar.Get_function_UI.GameHint("我曾踏月而来,只因你在山中");
                
                break;


            case "wangzirui":

                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");

                DateTime oldDate = new DateTime(2018, 4, 5);
                DateTime newDate = DateTime.Now;
                TimeSpan ts = newDate - oldDate;
                int differenceInDays = ts.Days;
                Game_PublicClassVar.Get_function_UI.GameHint("今天是我们在一起的第" + differenceInDays + "天");
                /*

                    //检测背包格子
                    if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                    {
                        //写入序列号ID
                        Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                        Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                        //Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 5);       //经验木桩
                        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("3", 500);       //500钻石
                        //更新UI显示状态
                        Btn_XueLieHaoSet();
                    }
                */
                break;

            case "shouji":
                Game_PublicClassVar.Get_function_Rose.JianCeShouJi();
            break;
                

            case "log":
                GameObject logObj = (GameObject)Resources.Load("UGUI/UISet/Other/UI_ErrorLog", typeof(GameObject));
                GameObject errorLogObj = (GameObject)Instantiate(logObj);
                errorLogObj.transform.SetParent(GameObject.Find("Canvas").transform);
                errorLogObj.transform.localPosition = Vector3.zero;
                errorLogObj.transform.localScale = new Vector3(1, 1, 1);
                break;

            case "chongwu":
                
                //获得当前充值额度
                float rosePayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                if (rosePayValue < 30)
                {
                    Game_PublicClassVar.Get_function_UI.GameHint("领取宠物失败,赞助额度不足30！");
                    return;
                }

                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                {
                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010077", 1);       //遗失的金币袋子
                    //更新UI显示状态
                    Btn_XueLieHaoSet();
                    Game_PublicClassVar.Get_function_UI.GameHint("领取宠物成功,祝你游戏愉快！");
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameHint("请预留1个背包格子！");
                }
                break;

            default:

                //验证密齿
                string xuliehao = Game_PublicClassVar.Get_xmlScript.costStr(xulieHaoID);
                Debug.Log("xuliehao = " + xuliehao);
                if (xulieHaoID != "-1")
                {
                    bool sendReward = false;
                    string[] xuliehaoList = xuliehao.Split(';');
                    if (xuliehaoList.Length < 3) {
                        Game_PublicClassVar.Get_function_UI.GameHint("序列号不正确!");
                        return;
                    }
                    //验证序列号
                    sendReward = Game_PublicClassVar.Get_function_Rose.IfTrueXuLieHao(xuliehaoList);

                    if (!sendReward)
                    {
                        Game_PublicClassVar.Get_function_UI.GameHint("序列号密匙不正确!");
                        return;
                    }
                    switch (xuliehaoList[1]) { 
                        //发送道具
                        case "101":
                            

                            //检测背包格子
                            if (!Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1)) {
                                Game_PublicClassVar.Get_function_UI.GameHint("请背包预留至少1个位置!");
                                return;
                            }

                            //获取发送道具
                            if (sendReward)
                            {
                                string[] sendItem = xuliehaoList[2].Split(',');
                                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendItem[0], int.Parse(sendItem[1]));
                                //写入序列号ID
                                Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                            }
                            else {
                                Game_PublicClassVar.Get_function_UI.GameHint("序列号不正确,请检查后在进行输入!");
                            }
                            break;

                        //设置当前关卡
                        case "102":
                            //开启关卡
                            string[] guankaList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter(guankaList[0] + ";" + guankaList[1]);
                            Game_PublicClassVar.Get_function_UI.GameHint("开启关卡成功！");
                            break;

                        //充值指定额度(钻石,额度)
                        case "103":
                            //充值指定额度(钻石,额度)
                            string[] payValue = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag("3", int.Parse(payValue[0]));
                            Game_PublicClassVar.Get_function_Rose.AddPayValue(float.Parse(payValue[1]));
                            Game_PublicClassVar.Get_function_UI.GameHint("领取奖励成功！");
                            //写入序列号ID
                            Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                            break;

                        //直接销毁当前身上某个部位的装备
                        case"201":
                            string[] destoryEquip = xuliehaoList[2].Split(',');
                            //Debug.Log("destoryEquip[0] = " + destoryEquip[0] + "destoryEquip[1] = " + destoryEquip[1]);
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipItemID", destoryEquip[1], "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipQuality", "0", "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIcon", "0", "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", destoryEquip[0], "RoseEquip");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                            break;

                        //直接设置国家等级
                        case "202":
                            string[] counrtyList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryLv", counrtyList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", counrtyList[1], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                            break;

                        //直接设置离线时间戳
                        case "203":
                            string[] offTimeList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("OffGameTime", offTimeList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", counrtyList[1], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            break;
                        //清空抽卡时间
                        case "204":
                            string[] chouKaList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", chouKaList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", chouKaList[1], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                            break;

                        //清空BOSS刷新时间
                        case "205":
                            //string[] chouKaList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                            break;

                        //设置剧情ID
                        case "206":
                            string[] storyIDList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StoryStatus", storyIDList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            break;

                        //设置当前任务
                        case "207":
                            string[] taskIDList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", taskIDList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", "100,0,0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            break;

                        //清空已完成的任务记录
                        case "208":
                            string[] taskSaveList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CompleteTaskID", taskSaveList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            break;

                        //设置快捷任务显示
                        case "209":
                            string[] maintaskSaveList = xuliehaoList[2].Split(',');
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", maintaskSaveList[0], "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                            break;
                    }
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameHint("序列号不正确!");
                }

                Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示
                //更新UI显示状态
                Btn_XueLieHaoSet();
                
            break;
        }
    }

    //点击支持作者按钮
    public void Btn_ZhiChiZuoZhe() {

        string zhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (zhiChiZuoZheID == ZuiHouLiBaoID)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("感谢你为作者做出的赞助,已经够多了,作者在此感谢你！");
            return;
        }
        string[] zhiChiZuoZheValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ZhiChiZuoZhe_" + zhiChiZuoZheID, "GameMainValue").Split(';');
        float nowPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        //检测背包位置是否足够
        int spaceNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (zhiChiZuoZheValue.Length - 1 > spaceNullNum) {
            Game_PublicClassVar.Get_function_UI.GameHint("请预留" + (zhiChiZuoZheValue.Length - 1).ToString()+"个背包空位置！");
            return;
        }


        //检测充值额度是否足够
        //Debug.Log("nowPayValue = " + nowPayValue + "||" + "zhiChiZuoZheValue[0] = " + zhiChiZuoZheValue[0]);
        if (nowPayValue >= float.Parse(zhiChiZuoZheValue[0]))
        {
            //循环发送奖励
            for (int i = 1; i <= zhiChiZuoZheValue.Length - 1; i++)
            {
                string[] rewardStr = zhiChiZuoZheValue[i].Split(',');
                Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardStr[0], int.Parse(rewardStr[1]));
            }

            //写入礼包数据
            zhiChiZuoZheID = (int.Parse(zhiChiZuoZheID) + 1).ToString();
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhiChiZuoZheID", zhiChiZuoZheID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            Btn_ZhiChiZuoZheSet();     //更新显示

        }
        else {
            string hintTextStr = Obj_ZhiChiZuoZhe_BuyText.GetComponent<Text>().text;
            Game_PublicClassVar.Get_function_UI.GameHint("赞助金额不足!   " + hintTextStr);
        }
    }

    /*
    //点击领取登陆
    public void Btn_DengLu() {
        string dengLuReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //获取领取状态
        string dengLuDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuDayStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (dengLuDayStatus == "1") {
            Game_PublicClassVar.Get_function_UI.GameHint("今日登陆奖励已领取!");
        }

        //获取领取数据
        string[] dengLuRewardValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DengLu_" + dengLuReward, "GameMainValue").Split(';');
        //检测背包位置是否足够
        int spaceNullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (dengLuRewardValue.Length - 1 > spaceNullNum)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("请预留" + (dengLuRewardValue.Length - 1).ToString() + "个背包空位置！");
            return;
        }

        //发送奖励
        for (int i = 0; i <= dengLuRewardValue.Length - 1; i++) {
            string[] rewardStr = dengLuRewardValue[i].Split(',');
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardStr[0], int.Parse(rewardStr[1]));
        }
    }
    */
    //点击前往充值
    public void GoToPay() {
        Debug.Log("打开充值界面");
        CloseUI();
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_RmbStore();
    }

    public void CloseUI() {
        Debug.Log("关闭界面");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_HuoDongDaTing();
    }
}
