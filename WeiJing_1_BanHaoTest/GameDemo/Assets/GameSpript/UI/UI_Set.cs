using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UI_Set : MonoBehaviour {

    public GameObject Obj_UI_TipsSet;
    public GameObject Obj_UI_RoseHp;
    public GameObject Obj_DropItemSet;
    public GameObject Obj_BuildingNameSet;
    public GameObject Obj_NpcNameSet;
    public GameObject Obj_NpcTaskSet;
	public GameObject Obj_RoseGetItemHint;
	public GameObject Obj_UI_BossHp;
	public GameObject Obj_UI_RoseExp;
	public GameObject Obj_UI_GameHintSet;
	public GameObject Obj_UI_GetherItemSet;
    public GameObject Obj_UI_AIHpSet;
    public GameObject Obj_MainUI;
    public GameObject Obj_FunctionOpen;
    public GameObject Obj_StorySpeakSet;
    public GameObject Obj_RoseSkillSet;
    public GameObject Obj_BuildingMainUISet;
    public GameObject Obj_BuildingMainUISet_2;
    public GameObject Obj_EnterGame;
    public GameObject Obj_GameGirdHint;
    public GameObject Obj_GameGirdHint_Front;
    public GameObject Obj_BtnCunMinDeXin;
    public GameObject Obj_HuoBiSet;
    public GameObject Obj_HeadSet;
    public GameObject Obj_RightDownSet;
    public GameObject Obj_MainUIBtn;
    public GameObject Obj_EquipMakeSet;
    public GameObject Obj_MapName;
    public GameObject Obj_UI_CloseTips;
    public GameObject Obj_MainUISet;
    public GameObject Obj_BuildingBGM;
    public GameObject Obj_ShouSuo;
    public GameObject Obj_ShouSuoImg;
    public GameObject Obj_YaoGan;
    public GameObject Obj_RmbStore;

    //public GameObject Obj_RightDownSet;
    public GameObject Obj_MainFunctionUI;
    public GameObject Obj_RoseTask;
    public GameObject Obj_UIMapName;
    public GameObject Obj_UIGameNanDu;

	// Use this for initialization
	void Start () {

        FangChengMiHint();

    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void FangChengMiHint()
    {

        string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_10", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        nowTime = PlayerPrefs.GetInt("FangChenMi_Time").ToString();

        if (nowTime == "")
        {
            nowTime = "0";
        }

        int nowTimeSum = int.Parse(nowTime);

        //防沉迷提示
        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 0)
        {

            if (nowTimeSum < 45)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》,未实名用户游戏体验时间不得超过60分钟,立即实名获得完整的游戏体验。", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "稍后实名", "立即实名");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            if (nowTimeSum >= 45 && nowTimeSum < 60)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》,未实名用户游戏体验时间不得超过60分钟,您已体验45分钟,立即实名获得完整的游戏体验。", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "稍后实名", "立即实名");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }


            if (nowTimeSum >= 60)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,您的体验时间已经结束,继续游戏请进行实名认证。", Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "立即实名", "退出游戏");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 1)
        {
            if (nowTimeSum >= 60 && nowTimeSum < 90)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,未成年人每日登陆时间不得超过90分钟,您已登陆" + nowTimeSum + "分钟。", null, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "确认");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            if (nowTimeSum >= 90)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,未成年人每日登陆时间不得超过90分钟,您已登陆90分钟,请合理分配游戏时间。", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "确认");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            //验证登陆时间
            DateTime nowData = DateTime.Now;
            if (Game_PublicClassVar.Get_wwwSet.DataTime == null)
            {
                nowData = DateTime.Now;
            }
            else
            {
                nowData = Game_PublicClassVar.Get_wwwSet.DataTime;
            }

            if (nowData.Hour >= 22 || nowData.Hour < 8)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,22:00-8:00不能向未成年人提供游戏服务,请在规定时间外登陆游戏。", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "确认");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            //if(nowData.TIME)
        }
    }


    public void FangChengMiHintNew()
    {

        string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_10", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        nowTime = PlayerPrefs.GetInt("FangChenMi_Time").ToString();

        if (nowTime == "")
        {
            nowTime = "0";
        }

        int nowTimeSum = int.Parse(nowTime);

        //防沉迷提示
        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 0)
        {

            if (nowTimeSum < 45)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》,未实名用户游戏体验时间不得超过60分钟,立即实名获得完整的游戏体验。", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "稍后实名", "立即实名");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            if (nowTimeSum >= 45 && nowTimeSum < 60)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》,未实名用户游戏体验时间不得超过60分钟,您已体验45分钟,立即实名获得完整的游戏体验。", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "稍后实名", "立即实名");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }


            if (nowTimeSum >= 60)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,您的体验时间已经结束,继续游戏请进行实名认证。", Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "立即实名", "退出游戏");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 1)
        {
            if (nowTimeSum >= 60 && nowTimeSum < 90)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,未成年人每日登陆时间不得超过90分钟,您已登陆" + nowTimeSum + "分钟。", null, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "确认");
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            if (nowTimeSum >= 90)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,未成年人每日登陆时间不得超过90分钟,您已登陆90分钟,请合理分配游戏时间。", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "确认");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            //验证登陆时间
            DateTime nowData = DateTime.Now;
            if (Game_PublicClassVar.Get_wwwSet.DataTime == null)
            {
                nowData = DateTime.Now;
            }
            else
            {
                nowData = Game_PublicClassVar.Get_wwwSet.DataTime;
            }

            if (nowData.Hour >= 22 || nowData.Hour < 8)
            {
                //弹出对应提示
                GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》规定,22:00-8:00不能向未成年人提供游戏服务,请在规定时间外登陆游戏。", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "温馨提示", "确认", "确认");
                uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
            }

            //if(nowData.TIME)
        }
    }

}
