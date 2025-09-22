using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Text;
using System.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
//using Umeng;
using System.Runtime.InteropServices;
using System.Configuration;
using Umeng;

public class WWWSet:MonoBehaviour{

    //读取的XML
    public WWW WWW_Monster_Template;
    public WWW WWW_Drop_Template;
    public WWW WWW_Item_Template;
    public WWW WWW_Skill_Template;
    public WWW WWW_Task_Template;
    public WWW WWW_Npc_Template;
    public WWW WWW_RoseExp_Template;
	public WWW WWW_Equip_Template;
    public WWW WWW_EquipSuit_Template;
    public WWW WWW_EquipSuitProperty_Template;
	public WWW WWW_Occupation_Template;
    public WWW WWW_SceneItem_Template;
    public WWW WWW_SkillBuff_Template;
    public WWW WWW_GameStory_Template;
    public WWW WWW_Scene_Template;
    public WWW WWW_SceneTransfer_Template;
    public WWW WWW_Building_Template;
    public WWW WWW_Chapter_Template;
    public WWW WWW_ChapterSon_Template;
    public WWW WWW_TaskMovePosition_Template;
    public WWW WWW_EquipMake_Template;
    public WWW WWW_GameMainValue;
    public WWW WWW_SpecialEvent_Template;
    public WWW WWW_TaskCountry_Template;
    public WWW WWW_TakeCard_Template;
    public WWW WWW_HonorStore_Template;
    public WWW WWW_Country_Template;
    public WWW WWW_ShouJiItem_Template;
    public WWW WWW_ShouJiItemPro_Template;

    //读写的XML
    public WWW WWWSet_RoseData;
    public WWW WWWSet_RoseBag;
    public WWW WWWSet_RoseEquip;
	public WWW WWWSet_RoseConfig;
    public WWW WWWSet_RoseBuilding;
    public WWW WWWSet_RoseStoreHouse;
    public WWW WWWSet_RoseEquipHideProperty;
    public WWW WWWSet_RoseDayReward;

    //角色创建的XML
    public WWW WWWSet_GameConfig_1;     //战士初始化
    public WWW WWWSet_GameConfig_2;     //法师初始化


    //路径
    public string Get_XmlPath;
    public string Set_XmlPath;
    public string Assets_XmlPath;       
    public DataSet DataSetXml;
    public bool IfUpdata;               //是否初始化数据
    public string RoseID;
    public bool IfAddKey;
    private bool IfUpdateWorldTime;     //是否初始化更新世界时间（测试期间关闭此选项要不会卡）

    private bool updataStatus;
    public int updataNum;              //更新数量
    public int updataNumSum;           //更新数量

    public bool DataUpdataStatus;       //数据更新状态,如果数据读取完毕,打开此开关
    public bool DataUpdataStatusOnce;                    //第一次加载数据时调用此字段为True

    public bool WorldTimeStatus;            //世界时间状态
    private float wordldTimeStatus;         //如果未连接网络多久间隔自动测试一次网络是否连接
    public bool GameOffResourceStatus;      //离线收益状态, True 表示当前游戏的离线资源已发放
    public DateTime DataTime;               //时间变量
    public DateTime DataTime_Last;          //上一次时间变量
    //public string DataTime_Str;           //时间变量时间戳
    public DateTime LastOffGameTime;        //上一次离开游戏的时间
    private bool lastOffGameTimeStatus;     //获取上一次离线时间的状态
    //public DateTime NowWorldTime;         //获取当前世界时间
    public string enterGameTimeStamp;       //进入游戏第一次打开网络的时间戳
    private float saveOffGameTimeSum;       //保存离线数据累计值
    private bool updataOffGameTimeStatus;   //离线时间
    private float worldTimeOnceSum;         //请求世界时间间隔
    public bool DayUpdataStatus;            //第二天凌晨更新
    public bool dayUpdataOne;

    public float dayUpdataTime;             //第二天剩余时间
    public bool upXmlDataStatus;            //更新游戏数据

    public bool IfSaveXmlStatus;            //是否覆盖当前数据内的XML
    private int xmlVersion;                 //XML存档版本号

    private bool IfSendYouMengData;         //是否向友盟发送了数据
    public bool IfHindMainBtn;              //是否隐藏主功能区按钮
    public bool IfUpdataGameWaiGua;

    public bool IfSaveRoseData;             //是否存储游戏数据
    public bool IfSaveGetRoseData;          //是否从存储的数据中获取角色数据
    //public int XXPERMISSIONCODE = 0;
	// Use this for initialization
    //private string wangluoTestStr;


    public WWW WWW_xml; //外网有打不开游戏的

    public bool IfChangeOcc;            //更改职业
    public bool IfChangeOccStatus;  
    public string CreateRoseNameStr;    //创建角色名称字符
    public bool CreateRoseStatus;       //创建角色状态
    public int CreateRoseDataNum;

    public bool TryWorldTimeStatus;         //尝试连接网络时间
    public string GameServerVersionStr;       //游戏版本


    ///0	0 - 7 
    ///8	8 - 15 
    ///16	16 -- 17 
    ///18	18+
    public int AgeRange = -1;
    public int RemainingTime;
    public float RemainingTimeSum;

    //UI类
    public GameObject Obj_BeiFenData;               //备份UI

    //防沉迷相关
    public GameObject FangChengMiYanZhengObj;
    public int FangChengMi_Type;         //0 未验证  1 已验证  未满18  2 已满正  满18
    public int FangChengMi_Year;

    public GameObject Obj_CommonHintHint_2;           //提示UI

    void Awake()
    {

        //设置屏幕自动旋转， 并置支持的方向
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

#if UNITY_EDITOR
        IfAddKey = false;            //默认False不加密文件
#else
        IfAddKey = true;            //默认False不加密文件
#endif
        IfUpdateWorldTime = true;   //关闭此选项,不会链接网络 要不会卡

        RoseID = "10001";
        dayUpdataTime = 0;
        wordldTimeStatus = 4;
        Get_XmlPath = Application.persistentDataPath + "/GameData/Xml/Get_Xml/";
        Set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + RoseID + "/";

        DontDestroyOnLoad(this.gameObject);

        xmlVersion = 110;       //只要此值比之前存储的值大就会覆盖XML数据（角色数据不会覆盖）
        int xmlVersionNum = PlayerPrefs.GetInt("XmlVersionNum");
        if (xmlVersion > xmlVersionNum)
        {
            //Debug.Log("开始覆盖更新数据");
            PlayerPrefs.SetInt("XmlVersionNum", xmlVersion);
            IfSaveXmlStatus = true; //是否覆盖配置文件
        }

        //Debug.Log("时区：" + TimeZoneInfo.Local.ToString());

        //设置程序后台执行
        //Application.runInBackground = true;

        Debug.Log("开始缓存读取的数据表");    //必须加这个Debug  要不读取文件会报错,原因不明
                                    //Game_PublicClassVar.Get_function_DataSet.DataSet_AllReadXml()

        //协同加载时间状态
        //this.StartCoroutine(LoadWorldTime());

        //WorldTimeStatus = true;

        //加载数据
        this.StartCoroutine(Set_GameConfig_1());
        this.StartCoroutine(Set_GameConfig_2());

        //StartCoroutine(OnTapLoginButtonClick());
    }


    IEnumerator OnTapLoginButtonClick()
    {
        yield return new WaitForSeconds(0.2f);
    }


    void Start () {

        //配置表总数
        updataNum = 37;

        //初始化加载防沉迷
        FangChengMi_Type = PlayerPrefs.GetInt("FangChenMi_Type");
        FangChengMi_Year = PlayerPrefs.GetInt("FangChenMi_Year");

    }
	
	// Update is called once per frame
    void Update()
    {
     
        /*
        //开始协同
        if (!upXmlDataStatus)
        {
            upXmlDataStatus = true;        //更新下一个数据表
            //Debug.Log("updataNumSum = " + updataNumSum);
            switch (updataNumSum)
            {
                case 0:
                    this.StartCoroutine(LoadMonster_Template());
                    break;
                case 1:
                    this.StartCoroutine(LoadDrop_Template());
                    break;
                case 2:
                    this.StartCoroutine(LoadItem_Template());
                    break;
                case 3:
                    this.StartCoroutine(LoadSkill_Template());
                    break;
                case 4:
                    this.StartCoroutine(LoadTask_Template());
                    break;
                case 5:
                    this.StartCoroutine(LoadNpc_Template());
                    break;
                case 6:
                    this.StartCoroutine(LoadRoseExp_Template());
                    break;
                case 7:
                    this.StartCoroutine(LoadEquip_Template());
                    break;
                case 8:
                    this.StartCoroutine(LoadEquipSuit_Template());
                    break;
                case 9:
                    this.StartCoroutine(LoadEquipSuitProperty_Template());
                    break;
                case 10:
                    this.StartCoroutine(LoadOccupation_Template());
                    break;
                case 11:
                    this.StartCoroutine(LoadSceneItem_Template());
                    break;
                case 12:
                    this.StartCoroutine(LoadSkillBuff_Template());
                    break;
                case 13:
                    this.StartCoroutine(LoadGameStory_Template());
                    break;
                case 14:
                    this.StartCoroutine(LoadScene_Template());
                    break;
                case 15:
                    this.StartCoroutine(LoadSceneTransfer_Template());
                    break;
                case 16:
                    this.StartCoroutine(LoadBuilding_Template());
                    break;
                case 17:
                    this.StartCoroutine(LoadChapter_Template());
                    break;
                case 18:
                    this.StartCoroutine(LoadChapterSon_Template());
                    break;
                case 19:
                    this.StartCoroutine(LoadTaskMovePosition_Template());
                    break;
                case 20:
                    this.StartCoroutine(LoadEquipMake_Template());
                    break;
                case 21:
                    this.StartCoroutine(LoadGameMainValue());
                    break;
                case 22:
                    this.StartCoroutine(LoadSpecialEvent_Template());
                    break;
                case 23:
                    this.StartCoroutine(LoadTaskCountry_Template());
                    break;
                case 24:
                    this.StartCoroutine(LoadTakeCard_Template());
                    break;
                case 25:
                    this.StartCoroutine(LoadHonorStore_Template());
                    break;
                case 26:
                    this.StartCoroutine(LoadCountry_Template());
                    break;
                //开始添加能记录的XML文件
                case 27:
                    this.StartCoroutine(Set_RoseData());
                    break;
                case 28:
                    this.StartCoroutine(Set_RoseBag());
                    break;
                case 29:
                    this.StartCoroutine(Set_RoseEquip());
                    break;
                case 30:
                    this.StartCoroutine(Set_RoseConfig());
                    break;
                case 31:
                    this.StartCoroutine(Set_RoseBuilding());
                    break;
                case 32:
                    this.StartCoroutine(Set_RoseStoreHouse());
                    break;
                case 33:
                    this.StartCoroutine(Set_RoseEquipHideProperty());
                    break;
                case 34:
                    this.StartCoroutine(Set_RoseDayReward());
                    break;
            }
        }
        
        */


        
        //测试加载10002
        if (IfChangeOcc == true) {
            IfChangeOcc = false;
            IfChangeOccStatus = true;
            //清空数据
            //RoseID = "10002";
            upXmlDataStatus = false;
            updataNumSum = 0;
            Game_PublicClassVar.Get_game_PositionVar.RoseID = RoseID;
            Set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + RoseID + "/";

        }

        if (IfChangeOccStatus)
        {
            //开始协同
            if (!upXmlDataStatus)
            {
                upXmlDataStatus = true;        //更新下一个数据表
                //Debug.Log("updataNumSum = " + updataNumSum);
                switch (updataNumSum)
                {
                    case 0:
                        this.StartCoroutine(LoadMonster_Template());
                        break;
                    case 1:
                        this.StartCoroutine(LoadDrop_Template());
                        break;
                    case 2:
                        this.StartCoroutine(LoadItem_Template());
                        break;
                    case 3:
                        this.StartCoroutine(LoadSkill_Template());
                        break;
                    case 4:
                        this.StartCoroutine(LoadTask_Template());
                        break;
                    case 5:
                        this.StartCoroutine(LoadNpc_Template());
                        break;
                    case 6:
                        this.StartCoroutine(LoadRoseExp_Template());
                        break;
                    case 7:
                        this.StartCoroutine(LoadEquip_Template());
                        break;
                    case 8:
                        this.StartCoroutine(LoadEquipSuit_Template());
                        break;
                    case 9:
                        this.StartCoroutine(LoadEquipSuitProperty_Template());
                        break;
                    case 10:
                        this.StartCoroutine(LoadOccupation_Template());
                        break;
                    case 11:
                        this.StartCoroutine(LoadSceneItem_Template());
                        break;
                    case 12:
                        this.StartCoroutine(LoadSkillBuff_Template());
                        break;
                    case 13:
                        this.StartCoroutine(LoadGameStory_Template());
                        break;
                    case 14:
                        this.StartCoroutine(LoadScene_Template());
                        break;
                    case 15:
                        this.StartCoroutine(LoadSceneTransfer_Template());
                        break;
                    case 16:
                        this.StartCoroutine(LoadBuilding_Template());
                        break;
                    case 17:
                        this.StartCoroutine(LoadChapter_Template());
                        break;
                    case 18:
                        this.StartCoroutine(LoadChapterSon_Template());
                        break;
                    case 19:
                        this.StartCoroutine(LoadTaskMovePosition_Template());
                        break;
                    case 20:
                        this.StartCoroutine(LoadEquipMake_Template());
                        break;
                    case 21:
                        this.StartCoroutine(LoadGameMainValue());
                        break;
                    case 22:
                        this.StartCoroutine(LoadSpecialEvent_Template());
                        break;
                    case 23:
                        this.StartCoroutine(LoadTaskCountry_Template());
                        break;
                    case 24:
                        this.StartCoroutine(LoadTakeCard_Template());
                        break;
                    case 25:
                        this.StartCoroutine(LoadHonorStore_Template());
                        break;
                    case 26:
                        this.StartCoroutine(LoadCountry_Template());
                        break;
                    //开始添加能记录的XML文件
                    case 27:
                        this.StartCoroutine(Set_RoseData());
                        break;
                    case 28:
                        this.StartCoroutine(Set_RoseBag());
                        break;
                    case 29:
                        this.StartCoroutine(Set_RoseEquip());
                        break;
                    case 30:
                        this.StartCoroutine(Set_RoseConfig());
                        break;
                    case 31:
                        this.StartCoroutine(Set_RoseBuilding());
                        break;
                    case 32:
                        this.StartCoroutine(Set_RoseStoreHouse());
                        break;
                    case 33:
                        this.StartCoroutine(Set_RoseEquipHideProperty());
                        break;
                    case 34:
                        this.StartCoroutine(Set_RoseDayReward());
                        break;
                    case 35:
                        this.StartCoroutine(LoadShouJiItem_Template());
                        break;
                    case 36:
                        this.StartCoroutine(LoadShouJiItemPro_Template());
                        break;
                }
            }
        }
        
        //外网有打不开游戏的生成文件
        /*
        this.StartCoroutine(addXmlJiaMi());
        */
        if (!updataStatus)
        {
            //Debug.Log("updataNumSum = " + updataNumSum);
            //if (updataNumSum >= updataNum|| updataNumSum==0)
            if (updataNumSum >= updataNum)
            {
                //Debug.Log("开始缓存所有表,当前updataNumSum值为：" + updataNumSum);
                //初始化缓存所有表
                if (!IfUpdata)
                {
                    Debug.Log("开始缓存所有数据！！！！！");
                    try
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_AllReadXml();
                        IfSaveRoseData = true;      //开启储备数据
                    }
                    catch (Exception ex)
                    {
                        IfSaveGetRoseData = true;
                        Debug.Log("账号数据异常,从备份中获取数据！");
                        Debug.Log("账号数据异常,从备份中获取数据！" +  ex.ToString());
                        Time.timeScale = 0;
                        GameObject beifenObj = (GameObject)Instantiate(Obj_BeiFenData);
                        beifenObj.transform.SetParent(GameObject.Find("Canvas").transform);
                        beifenObj.transform.localScale = new Vector3(1, 1, 1);
                        beifenObj.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(1, 1, 1);
                    }
                    
                    IfUpdata = true;
                    Debug.Log("数据加载完毕");
                    //设置当前账号不是第一次创建登录
                    Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "FirstGame", "1");

                    DataUpdataStatus = true;

                    //第一次进游戏随机一个8为ID给角色
                    string zhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", RoseID, "RoseData");
                    //因为17915632是第一次生成的时候获得加密文件的值T.T 好囧
                    if (zhangHaoID == "17915632" || zhangHaoID == "0")
                    {
                        //Debug.Log("ssssss");
                        try
                        {
                            zhangHaoID = Game_PublicClassVar.Get_function_Rose.GetZhangHuID();
                            //写入账号ID
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhangHaoID", zhangHaoID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            //清空初始化数据  
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("OffGameTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                            //存储角色名称
                            if (CreateRoseStatus) {
                                //CreateRoseStatus = false;
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Name", CreateRoseNameStr , "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                //Debug.Log("CreateRoseNameStr = " + CreateRoseNameStr);
                            }
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            
                            //存储角色通用数据
                            Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Name", CreateRoseNameStr);
                            //Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "FirstGame", "1");
            

                            string nametest = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", RoseID, "RoseData");
                            //Debug.Log("nametest = " + nametest);
                        }
                        catch {
                            Debug.Log("报错！！！第一次进入游戏报错");
                        }
                    }
                    
                    if (WorldTimeStatus)
                    {
                        try
                        {
                     
                            GA.StartWithAppKeyAndChannelId("5cd984c10cafb2a438000fa4", "umeng");        //设置appID信息
                            GA.SetLogEnabled(Debug.isDebugBuild);
                            //设置Umeng Appkey
                            //GA.StartWithAppKeyAndChannelId("58ef5a47c895761626001bc0", "App Store");        //设置appID信息

                            //调试时开启日志
                            //GA.SetLogEnabled(false);            //不输出Logo信息
                            //GA.SetLogEncryptEnabled(true);      //加密传输信息
                            //GA.ProfileSignIn(zhangHaoID);       //写入账号ID
                            //IfSendYouMengData = true;           //开启友盟发送

                        }
                        catch
                        {
                            Debug.Log("有报错");
                        }
                    }
                    

                    //GA.ProfileSignIn(zhangHaoID, "app strore");

                    //print("GA.ProfileSignOff();");

                    //GA.ProfileSignOff();


                }

                Debug.Log(Application.persistentDataPath);

                //开启主界面追踪UI显示
                Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;

                updataStatus = true;

                updataOffGameTimeStatus = true;     //更新离线时间
            }
        }

        if (DataUpdataStatus)
        {
            //修改检测
            if (!IfUpdataGameWaiGua)
            {
                //Debug.Log("检测角色数据");
                IfUpdataGameWaiGua = true;

                string payValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string roseRmb = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //充值少于100
                if (float.Parse(payValue) <= 6)
                {
                    //检测钻石
                    if (float.Parse(roseRmb) >= float.Parse(payValue) * 1000 + 190000)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame = true;
                        //Debug.Log("检测角色修改了数据");
                    }

                    //检测攻击(等级小于30级,攻击大于998判定为数据篡改)
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                        if (roseLv < 30)
                        {
                            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMax > 998)
                            {
                                Game_PublicClassVar.Get_game_PositionVar.ifXiuGaiGame = true;
                            }
                        }
                    }
                }
            }
        }

        //如果当前进入游戏时没有链接到网络,则不断尝试的进行网络连接
        if (!WorldTimeStatus)
        {
            //Debug.Log("wordldTimeStatus = " + wordldTimeStatus);unicode
            if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus)
            {
                //Debug.Log("Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkTimeStamp = " + Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkTimeStamp);
                //读取服务器返回的时间
                DataTime = GetTime(Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkTimeStamp);
                //记录时间
                //Debug.Log((int)DataTime.Year + "年" + (int)DataTime.Month + "月" + (int)DataTime.Day + "日" + (int)DataTime.Hour + "时" + (int)DataTime.Minute + "分" + (int)DataTime.Second + "秒");//输出时间
                dayUpdataTime = 86400 - ((int)DataTime.Hour * 3600 + (int)DataTime.Minute * 60 + (int)DataTime.Second);
                //Debug.Log("DataWorldTimeStatus333333333 = " + DataTime);

                //保证时间戳都是第一次连接获得
                if (enterGameTimeStamp == "")
                {
                    enterGameTimeStamp = Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkTimeStamp;
                }


                if ((int)DataTime.Year > 2015)
                {
                    //dayUpdataOne = true;
                    WorldTimeStatus = true;
                }

                TryWorldTimeStatus = true;
            }
            else {
                wordldTimeStatus = wordldTimeStatus + Time.deltaTime;
                if (wordldTimeStatus >= 5.0f)
                {
                    //协同加载时间状态
                    try
                    {
                        if (IfUpdateWorldTime)
                        {
                            this.StartCoroutine(LoadWorldTime());
                        }
                    }
                    catch
                    {
                        Debug.Log("连接网络读取时间异常");
                        //wangluoTestStr = "连接网络读取时间异常";
                    }
                    wordldTimeStatus = 0;
                    TryWorldTimeStatus = true;
                }
            }

            if (WorldTimeStatus)
            {
                dayUpdataOne = true;
            }

        }
        else
        {
            //判定是否读取到了表
            /*
            if (DataUpdataStatus)
            {
                //每分钟请求一次时间
                
                worldTimeOnceSum = worldTimeOnceSum + Time.deltaTime;
                
                if (!IfSendYouMengData)
                {
                    try
                    {
                        //if (worldTimeOnceSum >= 60) {
                        //worldTimeOnceSum = 0;
                        //GetNowWorldTime();
                        //设置Umeng Appkey

                        //Debug.Log("更新账号信息");
                        //string zhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", RoseID, "RoseData");
                        GA.StartWithAppKeyAndChannelId("58ef5a47c895761626001bc0", "App Store");        //设置appID信息
                        //Debug.Log("更新账号信息2222");
                        //调试时开启日志
                        GA.SetLogEnabled(false);            //不输出Logo信息
                        //Debug.Log("更新账号信息3333");
                        GA.SetLogEncryptEnabled(true);      //加密传输信息
                        //Debug.Log("更新账号信息4444");
                        //GA.ProfileSignIn(zhangHaoID);       //写入账号ID
                        IfSendYouMengData = true;           //开启友盟发送
                        //Debug.Log("更新账号信息5555");

                        //}
                    }
                    catch
                    {
                        Debug.Log("World友盟有报错");
                    }
                }
                
            }
            */
        }

        //设置距离第二天的离线时间
        if (WorldTimeStatus)
        {
            if (dayUpdataTime > 0)
            {
                //Debug.Log("dayUpdataTime = " + dayUpdataTime);
                float updataDayTime = dayUpdataTime - Time.realtimeSinceStartup;
                //Debug.Log("dayUpdataTime = " + updataDayTime);
                if (updataDayTime <= 0)
                {
                    //GetNowWorldTime();
                    DayUpdataStatus = true; //设置更新
                    Debug.Log("第二天到了！！！");
                    dayUpdataTime = 86400;
                }
            }
        }
        else
        {
            //Debug.Log("时间未连接");
        }


        //每分钟请求一次时间
        saveOffGameTimeSum = saveOffGameTimeSum + Time.deltaTime;
        if (DataUpdataStatus)
        {
            //Debug.Log("Data111111111111111111");
            if (saveOffGameTimeSum >= 10.0f)
            {
                if (lastOffGameTimeStatus) {
                    //Debug.Log("Data2222222222222222");
                    //Debug.Log("LastOffGameTime = " + LastOffGameTime);
                    writeOffGameTime(saveOffGameTimeSum);
                    saveOffGameTimeSum = 0;
                }
            }
        }


        //获取上次离线时间
        if (updataOffGameTimeStatus)
        {
            updataOffGameTime();
            updataOffGameTimeStatus = false;
        }


        //从当前数据拷贝备份数据
        if (IfSaveRoseData) {
            IfSaveRoseData = false;

            /*
            //安卓读取
            if (Application.platform == RuntimePlatform.Android)
            {
                beifen_1 = "jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/";
            }
            //PC读取
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {

            }


            //Mac OS 读取
            if (Application.platform == RuntimePlatform.OSXEditor)
            {

            }

            //IOS 读取
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {

            }
            */

            string beifenFileName =  Set_XmlPath.Substring(0,Set_XmlPath.Length-1)+"_beifen/";
            //Debug.Log("beifenFileName = " + beifenFileName);
            Game_PublicClassVar.Get_xmlScript.CopyFile(new DirectoryInfo(Set_XmlPath), beifenFileName);
        }

        //从备份获取数据替换当前数据
        if (IfSaveGetRoseData) {
            IfSaveGetRoseData = false;
            string beifenFileName = Set_XmlPath.Substring(0, Set_XmlPath.Length - 1) + "_beifen/";
            //Debug.Log("beifenFileName = " + beifenFileName);
            //检测是否有存档文件
            if (Directory.Exists(beifenFileName))
            {
                Game_PublicClassVar.Get_xmlScript.CopyFile(new DirectoryInfo(beifenFileName), Set_XmlPath);
                Application.Quit(); //退出游戏,以后要改成重启需要调用安卓方法
            }
        }


        //测试防沉迷
        if (AgeRange>=1 && AgeRange < 18)
        {
            RemainingTimeSum = RemainingTimeSum + Time.deltaTime;

            //10秒后自动关闭游戏

            if (RemainingTimeSum >= RemainingTime)
            {
                ExitGame();
            }

            if (RemainingTimeSum + 10 >= RemainingTime) {

                //弹出防沉迷认证
                GameObject OBJ_UI_Set = GameObject.FindWithTag("UI_Set");
                if (OBJ_UI_Set != null)
                {
                    OBJ_UI_Set.GetComponent<UI_Set>().FangChengMiHintNew();
                }
            }
        }
    }

    //获取当前时间
    /*
    public void GetNowWorldTime() {
        this.StartCoroutine(LoadWorldTime());
    }
    */

    //注销时调用,保存上次时间
    void OnDestory(){
        //Debug.Log("写入离线数据");
        //writeOffGameTime(0);
    }

    //写入离线时间
    public void writeOffGameTime(float addTimeValue) {
        //Debug.Log("写入离线时间:" + addTimeValue);
        if (WorldTimeStatus)
        {
            //Debug.Log("aaaaaaaa = " + float.Parse(enterGameTimeStamp));
            int gameTime = (int)(Time.realtimeSinceStartup);
            int offTiem = int.Parse(enterGameTimeStamp) + gameTime;
            //Debug.Log(" offTiem = " + offTiem);
            int offTimeInt = (int)(offTiem);
            //存储离线时间

            //Debug.Log("enterGameTimeStamp = " + enterGameTimeStamp +"    Time.realtimeSinceStartup = " + Time.realtimeSinceStartup);
            //Debug.Log("lixian = " + offTimeInt.ToString());
            //每分钟写入角色数据,顺便写入当前血量的数据
            //Debug.Log("offTimeInt = " + offTimeInt);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("OffGameTime", offTimeInt.ToString(), "ID", RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            //Debug.Log("开始存储");
        }
    }

    //开始协同怪物表
    private IEnumerator LoadMonster_Template()
    {
        //Debug.Log("测试读取表");
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Monster_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
            //Debug.Log("测试读取表成功");

        }
        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            //Debug.Log("PC读取怪物表成功！进行中……");
            WWW_Monster_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
            //Debug.Log("PC读取怪物表成功！");

        }

        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Monster_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Monster_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Monster_Template.xml");
            yield return WWW_Monster_Template;
        }

        if (WWW_Monster_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Monster_Template.xml", WWW_Monster_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取怪物表成功！执行下一个表");
            //string sourceFile = Application.persistentDataPath + "/GameData/Xml/Get_Xml/AAAA.xml";
            //byte[] btFile = File.ReadAllBytes(sourceFile);
            //string aaa = Encoding.UTF8.GetString(btFile);
            //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZWWW_Monster_Template = " + aaa);
        }

    }


    //开始协同道具表
    private IEnumerator LoadItem_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Item_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Item_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Item_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Item_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Item_Template.xml");
            yield return WWW_Item_Template;
        }


        if (WWW_Item_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Item_Template.xml", WWW_Item_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取道具表成功！");
        }

    }


    //开始协同道具表
    private IEnumerator LoadSkill_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Skill_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Skill_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Skill_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Skill_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Skill_Template.xml");
            yield return WWW_Skill_Template;
        }


        if (WWW_Skill_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Skill_Template.xml", WWW_Skill_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取技能表成功！");
        }

    }

    //开始协同掉落表
    private IEnumerator LoadDrop_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Drop_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Drop_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Drop_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Drop_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Drop_Template.xml");
            yield return WWW_Drop_Template;
        }


        if (WWW_Drop_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Drop_Template.xml", WWW_Drop_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取掉落表成功！");
        }

    }

    //开始协同任务表
    private IEnumerator LoadTask_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Task_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Task_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Task_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Task_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Task_Template.xml");
            yield return WWW_Task_Template;
        }


        if (WWW_Task_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Task_Template.xml", WWW_Task_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取任务表成功！");
        }

    }

    //开始协同NPC表
    private IEnumerator LoadNpc_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Npc_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Npc_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Npc_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Npc_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Npc_Template.xml");
            yield return WWW_Npc_Template;
        }


        if (WWW_Npc_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Npc_Template.xml", WWW_Npc_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取NPC表成功！");
        }

    }

    //开始协同角色经验表
    private IEnumerator LoadRoseExp_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_RoseExp_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_RoseExp_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_RoseExp_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_RoseExp_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "RoseExp_Template.xml");
            yield return WWW_RoseExp_Template;
        }


        if (WWW_RoseExp_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "RoseExp_Template.xml", WWW_RoseExp_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取角色经验表成功！");
        }
    }

	//开始协同装备表
	private IEnumerator LoadEquip_Template()
	{
		
		
		//安卓读取
		if (Application.platform == RuntimePlatform.Android)
		{
			WWW_Equip_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Equip_Template.xml");
			yield return WWW_Equip_Template;
		}
        if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			//PC读取
			WWW_Equip_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Equip_Template.xml");
			yield return WWW_Equip_Template;
		}


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Equip_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Equip_Template.xml");
            yield return WWW_Equip_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Equip_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Equip_Template.xml");
            yield return WWW_Equip_Template;
        }


        if (WWW_Equip_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Equip_Template.xml", WWW_Equip_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取装备表成功！");
        }
		
	}

    //开始协套装备表
    private IEnumerator LoadEquipSuit_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipSuit_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipSuit_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipSuit_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipSuit_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "EquipSuit_Template.xml");
            yield return WWW_EquipSuit_Template;
        }


        if (WWW_EquipSuit_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipSuit_Template.xml", WWW_EquipSuit_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取装备表成功！");
        }

    }

    //开始协套装属性表
    private IEnumerator LoadEquipSuitProperty_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipSuitProperty_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipSuitProperty_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipSuitProperty_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipSuitProperty_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "EquipSuitProperty_Template.xml");
            yield return WWW_EquipSuitProperty_Template;
        }


        if (WWW_EquipSuitProperty_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipSuitProperty_Template.xml", WWW_EquipSuitProperty_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取装备表成功！");
        }

    }

	//开始协同职业表
	private IEnumerator LoadOccupation_Template()
	{
		
		
		//安卓读取
		if (Application.platform == RuntimePlatform.Android)
		{
			WWW_Occupation_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Occupation_Template.xml");
			yield return WWW_Occupation_Template;
		}
        if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			//PC读取
			WWW_Occupation_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Occupation_Template.xml");
			yield return WWW_Occupation_Template;

		}


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Occupation_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Occupation_Template.xml");
            yield return WWW_Occupation_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Occupation_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Occupation_Template.xml");
            yield return WWW_Occupation_Template;
        }


        if (WWW_Occupation_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Occupation_Template.xml", WWW_Occupation_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取职业表成功！");
        }
		
	}


    //开始协同场景道具表
    private IEnumerator LoadSceneItem_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SceneItem_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SceneItem_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SceneItem_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SceneItem_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "SceneItem_Template.xml");
            yield return WWW_SceneItem_Template;
        }


        if (WWW_SceneItem_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SceneItem_Template.xml", WWW_SceneItem_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取道具表成功！");
        }

    }


    //开始协同场景Buff表
    private IEnumerator LoadSkillBuff_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SkillBuff_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;
        }


        if (WWW_SkillBuff_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SkillBuff_Template.xml", WWW_SkillBuff_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //开始协同故事对话表
    private IEnumerator LoadGameStory_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_GameStory_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_GameStory_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_GameStory_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_GameStory_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "GameStory_Template.xml");
            yield return WWW_GameStory_Template;
        }


        if (WWW_GameStory_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "GameStory_Template.xml", WWW_GameStory_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取故事对话表表成功！");
        }

    }


    //开始协同故事对话表
    private IEnumerator LoadScene_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Scene_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Scene_Template.xml");
            yield return WWW_Scene_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Scene_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Scene_Template.xml");
            yield return WWW_Scene_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Scene_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Scene_Template.xml");
            yield return WWW_Scene_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Scene_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Scene_Template.xml");
            yield return WWW_Scene_Template;
        }


        if (WWW_Scene_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Scene_Template.xml", WWW_Scene_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //开始协同故事对话表
    private IEnumerator LoadSceneTransfer_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SceneTransfer_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SceneTransfer_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SceneTransfer_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SceneTransfer_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "SceneTransfer_Template.xml");
            yield return WWW_SceneTransfer_Template;
        }


        if (WWW_SceneTransfer_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SceneTransfer_Template.xml", WWW_SceneTransfer_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //建筑表
    private IEnumerator LoadBuilding_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Building_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Building_Template.xml");
            yield return WWW_Building_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Building_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Building_Template.xml");
            yield return WWW_Building_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Building_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Building_Template.xml");
            yield return WWW_Building_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Building_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Building_Template.xml");
            yield return WWW_Building_Template;
        }


        if (WWW_Building_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Building_Template.xml", WWW_Building_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //建筑表
    private IEnumerator LoadChapter_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Chapter_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Chapter_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Chapter_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Chapter_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Chapter_Template.xml");
            yield return WWW_Chapter_Template;
        }


        if (WWW_Chapter_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Chapter_Template.xml", WWW_Chapter_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    private IEnumerator LoadChapterSon_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ChapterSon_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ChapterSon_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ChapterSon_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ChapterSon_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "ChapterSon_Template.xml");
            yield return WWW_ChapterSon_Template;
        }


        if (WWW_ChapterSon_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ChapterSon_Template.xml", WWW_ChapterSon_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }
    }


    private IEnumerator LoadTaskMovePosition_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_TaskMovePosition_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_TaskMovePosition_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_TaskMovePosition_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_TaskMovePosition_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "TaskMovePosition_Template.xml");
            yield return WWW_TaskMovePosition_Template;
        }


        if (WWW_TaskMovePosition_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "TaskMovePosition_Template.xml", WWW_TaskMovePosition_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    private IEnumerator LoadEquipMake_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_EquipMake_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_EquipMake_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_EquipMake_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_EquipMake_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "EquipMake_Template.xml");
            yield return WWW_EquipMake_Template;
        }


        if (WWW_EquipMake_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "EquipMake_Template.xml", WWW_EquipMake_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    private IEnumerator LoadGameMainValue()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_GameMainValue = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "GameMainValue.xml");
            yield return WWW_GameMainValue;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_GameMainValue = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "GameMainValue.xml");
            yield return WWW_GameMainValue;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_GameMainValue = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "GameMainValue.xml");
            yield return WWW_GameMainValue;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_GameMainValue = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "GameMainValue.xml");
            yield return WWW_GameMainValue;
        }


        if (WWW_GameMainValue.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "GameMainValue.xml", WWW_GameMainValue);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    private IEnumerator LoadSpecialEvent_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SpecialEvent_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_SpecialEvent_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_SpecialEvent_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_SpecialEvent_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "SpecialEvent_Template.xml");
            yield return WWW_SpecialEvent_Template;
        }


        if (WWW_SpecialEvent_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SpecialEvent_Template.xml", WWW_SpecialEvent_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //每日任务表
    private IEnumerator LoadTaskCountry_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_TaskCountry_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_TaskCountry_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_TaskCountry_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_TaskCountry_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "TaskCountry_Template.xml");
            yield return WWW_TaskCountry_Template;
        }


        if (WWW_TaskCountry_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "TaskCountry_Template.xml", WWW_TaskCountry_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //抽卡
    private IEnumerator LoadTakeCard_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_TakeCard_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_TakeCard_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_TakeCard_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_TakeCard_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "TakeCard_Template.xml");
            yield return WWW_TakeCard_Template;
        }


        if (WWW_TaskCountry_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "TakeCard_Template.xml", WWW_TakeCard_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }


    //荣誉兑换
    private IEnumerator LoadHonorStore_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_HonorStore_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_HonorStore_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_HonorStore_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_HonorStore_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "HonorStore_Template.xml");
            yield return WWW_HonorStore_Template;
        }


        if (WWW_HonorStore_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "HonorStore_Template.xml", WWW_HonorStore_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //抽卡
    private IEnumerator LoadCountry_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_Country_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "Country_Template.xml");
            yield return WWW_Country_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_Country_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "Country_Template.xml");
            yield return WWW_Country_Template;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_Country_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "Country_Template.xml");
            yield return WWW_Country_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_Country_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "Country_Template.xml");
            yield return WWW_Country_Template;
        }


        if (WWW_Country_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "Country_Template.xml", WWW_Country_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //收集大厅
    private IEnumerator LoadShouJiItem_Template()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ShouJiItem_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ShouJiItem_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ShouJiItem_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ShouJiItem_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "ShouJiItem_Template.xml");
            yield return WWW_ShouJiItem_Template;
        }


        if (WWW_ShouJiItem_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ShouJiItem_Template.xml", WWW_ShouJiItem_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    //收集大厅
    private IEnumerator LoadShouJiItemPro_Template()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_ShouJiItemPro_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWW_ShouJiItemPro_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWW_ShouJiItemPro_Template = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Get_Xml/" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWW_ShouJiItemPro_Template = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Get_Xml/" + "ShouJiItemPro_Template.xml");
            yield return WWW_ShouJiItemPro_Template;
        }


        if (WWW_ShouJiItem_Template.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "ShouJiItemPro_Template.xml", WWW_ShouJiItemPro_Template);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取Buff表成功！");
        }

    }

    /*
    //开始协同Buff表
    private IEnumerator LoadSkillBuff_Template()
    {

        //Debug.Log("开始读取Buff表");
        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW_SkillBuff_Template = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Get_Xml/" + " SkillBuff_Template.xml");
            //Debug.Log("开始读取Buff表(安卓)");
            yield return WWW_SkillBuff_Template;
        }
        else
        {
            //PC读取
            WWW_SkillBuff_Template = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Get_Xml\\" + "SkillBuff_Template.xml");
            yield return WWW_SkillBuff_Template;

        }

        if (WWW_SkillBuff_Template.isDone)
        {
            //Debug.Log("读取Buff表完成");
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Get_XmlPath + "SkillBuff_Template.xml", WWW_SkillBuff_Template);
            updataNumSum = updataNumSum + 1;
            //Debug.Log("读取Buff表成功！");
        }

    }
    */

    //开始初始化主角数据表
    private IEnumerator Set_RoseData()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseData = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseData.xml");
            yield return WWWSet_RoseData;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseData = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseData.xml");
            yield return WWWSet_RoseData;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseData = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseData.xml");
            yield return WWWSet_RoseData;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseData = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseData.xml");
            yield return WWWSet_RoseData;
        }


        if (WWWSet_RoseData.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseData.xml", WWWSet_RoseData);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取主数据表成功！");

            //LastOffGameTime = GetTime("0000000000");        //测试
            
        }

    }

    //开始初始化主角背包数据
    private IEnumerator Set_RoseBag()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseBag = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseBag.xml");
            yield return WWWSet_RoseBag;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseBag = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseBag.xml");
            yield return WWWSet_RoseBag;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseBag = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseBag.xml");
            yield return WWWSet_RoseBag;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseBag = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseBag.xml");
            yield return WWWSet_RoseBag;
        }


        //复制RoseBag表
        if (WWWSet_RoseBag.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseBag.xml", WWWSet_RoseBag);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取背包表成功！");
        }

    }


    //开始初始化主角装备数据
    private IEnumerator Set_RoseEquip()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseEquip = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseEquip = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;

        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseEquip = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseEquip = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseEquip.xml");
            yield return WWWSet_RoseEquip;
        }


        if (WWWSet_RoseEquip.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseEquip.xml", WWWSet_RoseEquip);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取主角装备表成功！");
        }

    }


	//开始初始化主角配置数据
	private IEnumerator Set_RoseConfig()
	{
		//安卓读取
		if (Application.platform == RuntimePlatform.Android)
		{
			WWWSet_RoseConfig = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseConfig.xml");
			yield return WWWSet_RoseConfig;
		}
        if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			//PC读取
			WWWSet_RoseConfig = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseConfig.xml");
			yield return WWWSet_RoseConfig;
		}


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseConfig = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseConfig.xml");
            yield return WWWSet_RoseConfig;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseConfig = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseConfig.xml");
            yield return WWWSet_RoseConfig;
        }


        if (WWWSet_RoseConfig.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseConfig.xml", WWWSet_RoseConfig);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！");
        }
	}

    //开始初始化建筑相关数据
    private IEnumerator Set_RoseBuilding()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseBuilding = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseBuilding = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseBuilding = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseBuilding = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseBuilding.xml");
            yield return WWWSet_RoseBuilding;
        }


        if (WWWSet_RoseBuilding.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseBuilding.xml", WWWSet_RoseBuilding);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！");
        }

    }

    //开始初始化仓库相关数据
    private IEnumerator Set_RoseStoreHouse()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseStoreHouse = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseStoreHouse = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseStoreHouse = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseStoreHouse = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseStoreHouse.xml");
            yield return WWWSet_RoseStoreHouse;
        }


        if (WWWSet_RoseStoreHouse.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseStoreHouse.xml", WWWSet_RoseStoreHouse);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！" + updataNumSum);
        }

    }

    //初始化极品装备
    private IEnumerator Set_RoseEquipHideProperty()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseEquipHideProperty = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseEquipHideProperty = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseEquipHideProperty = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseEquipHideProperty = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseEquipHideProperty.xml");
            yield return WWWSet_RoseEquipHideProperty;
        }


        if (WWWSet_RoseEquipHideProperty.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseEquipHideProperty.xml", WWWSet_RoseEquipHideProperty);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取游戏配置表成功！" + updataNumSum);
        }

    }

    //初始化每日奖励
    private IEnumerator Set_RoseDayReward()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_RoseDayReward = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_RoseDayReward = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + RoseID + "\\" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_RoseDayReward = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_RoseDayReward = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + RoseID + "/" + "RoseDayReward.xml");
            yield return WWWSet_RoseDayReward;
        }


        if (WWWSet_RoseDayReward.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_Create(Set_XmlPath + "RoseDayReward.xml", WWWSet_RoseDayReward);
            updataNumSum = updataNumSum + 1;
            upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }


    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_GameConfig_1()
    {

        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_GameConfig_1 = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + "10001" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + "10001" + "\\" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + "10001" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_GameConfig_1 = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + "10001" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_1;
        }


        if (WWWSet_GameConfig_1.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/10001/" + "GameConfig.xml", WWWSet_GameConfig_1);
            CreateRoseDataNum = CreateRoseDataNum + 1;
            //updataNumSum = updataNumSum + 1;
            //upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }

    //游戏配置WWWSet_GameConfig_1
    private IEnumerator Set_GameConfig_2()
    {


        //安卓读取
        if (Application.platform == RuntimePlatform.Android)
        {
            WWWSet_GameConfig_2 = new WWW("jar:file://" + Application.dataPath + "!/assets/GameData/Xml/Set_Xml/" + "10002" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //PC读取
            WWWSet_GameConfig_2 = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Xml\\Set_Xml\\" + "10002" + "\\" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }


        //Mac OS 读取
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            WWWSet_GameConfig_2 = new WWW("file://" + Application.dataPath + "/StreamingAssets/GameData/Xml/Set_Xml/" + "10002" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }

        //IOS 读取
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            WWWSet_GameConfig_2 = new WWW("file://" + Application.dataPath + "/Raw/GameData/Xml/Set_Xml/" + "10002" + "/" + "GameConfig.xml");
            yield return WWWSet_GameConfig_2;
        }


        if (WWWSet_GameConfig_2.isDone)
        {
            Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/Xml/Set_Xml/10002/" + "GameConfig.xml", WWWSet_GameConfig_2);
            CreateRoseDataNum = CreateRoseDataNum + 1;
            //updataNumSum = updataNumSum + 1;
            //upXmlDataStatus = false;        //更新下一个数据表
            //Debug.Log("读取RoseDayReward游戏配置表成功！" + updataNumSum);
        }

    }


    //外网有打不开的游戏的,需要手动加密Xml
    private IEnumerator addXmlJiaMi()
    {
        WWW_xml = new WWW("file://" + Application.dataPath + "\\StreamingAssets\\GameData\\Test\\" + "aaa.xml");
        yield return WWW_xml;
    }


    private void updataOffGameTime(){
    
        //假设一个上一次时间,持久化数据OffGameTime
        //Debug.Log("RoseID = " + RoseID);
        string value = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffGameTime", "ID", RoseID, "RoseData");

        Debug.Log("OffValue = " + value);
        if (value == "0")
        {
            //初次登陆设置时间为1970,会被识别为第一次登陆
            //PlayerPrefs.SetString("LastOffTime", "0000000000");
            LastOffGameTime = GetTime("0000000000");
            lastOffGameTimeStatus = true;       //获取离线时间戳值
        }
        else
        {
            LastOffGameTime = GetTime(value);
            lastOffGameTimeStatus = true;       //获取离线时间戳值
            Debug.Log("LastOffGameTime = " + LastOffGameTime);
        }
    }


    //协同程序 获取当前时间
    private IEnumerator LoadWorldTime()
    {
        /*
        string str = GetWebClient("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2");
        string timeStamp = str.Split('=')[1].Substring(0, 10);  //网页获取的数据长度超了，所以要裁剪
        bool ifGetWorldTimeStr = true;

        if (str == "0000000000")
        {
            //wangluoTestStr = "未连接网络!";
            Debug.Log("未连接网络");
            yield return WorldTimeStatus;
        }
        else {

            //保证时间戳都是第一次连接获得
            if (enterGameTimeStamp == "")
            {
                enterGameTimeStamp = timeStamp;
            }
            //wangluoTestStr = "连接数据：4";
            DataTime = GetTime(timeStamp);
            //wangluoTestStr = "连接数据：5";
            //记录时间
            Debug.Log((int)DataTime.Year + "年" + (int)DataTime.Month + "月" + (int)DataTime.Day + "日" + (int)DataTime.Hour + "时" + (int)DataTime.Minute + "分" + (int)DataTime.Second + "秒");//输出时间
            dayUpdataTime = 86400 - ((int)DataTime.Hour * 3600 + (int)DataTime.Minute * 60 + (int)DataTime.Second);
            Debug.Log("dayUpdataTime = " + dayUpdataTime);
            //wangluoTestStr = "连接数据：6";

            Debug.Log("DataWorldTimeStatus22222 = " + DataTime);
            if ((int)DataTime.Year > 2015)
            {
                //dayUpdataOne = true;
                WorldTimeStatus = true;
                yield return WorldTimeStatus;
            }
        }
        */

        yield return "0000000000";
    }


    //协同程序 获取当前时间
    public string XuliehaoTime()
    {
        /*
        //wangluoTestStr = "网络加载中……";
        //Debug.Log("aaaaaaaaaaaaaaaa");
        string str = GetWebClient("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2");
        Debug.Log("str = " + str);
        string timeStamp = str.Split('=')[1].Substring(0, 10);  //网页获取的数据长度超了，所以要裁剪
        //bool ifGetWorldTimeStr = true;
        //Debug.Log("str = " + str);
        //Debug.Log("timeStamp = " + timeStamp);
        //string str = GetWebClient("http://shijianchuo.911cha.com/");
        if (str == "0000000000")
        {
            //wangluoTestStr = "未连接网络!";
            Debug.Log("未连接网络");
            return str;
        }


        return timeStamp;
        */

        return "0000000000";
    }


    //获取当前时间（用于其他地方调用）
    public string GetWorldTime()
    {
        /*
        //Debug.Log("aaaaaaaaaaaaaaaa");
        //string str = GetWebClient("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=2");
        //string timeStamp = str.Split('=')[1].Substring(0, 10);  //网页获取的数据长度超了，所以要裁剪
        string str = GetWebClient("http://shijianchuo.911cha.com/");
        if (str == "0000000000" && str != "1 = 0000000000")
        {
            Debug.Log("未连接网络");
            return str;
        }
        else
        {
            string[] strShuZu = str.Split(new string[] { "→" }, StringSplitOptions.RemoveEmptyEntries);
            if (strShuZu.Length >= 2)
            {
                string timeStamp = strShuZu[1].Substring(1, 10);  //网页获取的数据长度超了，所以要裁剪
                Debug.Log("timeStamp = " + timeStamp);
                return timeStamp;

            }
        }

        return str;
        */
        return "0000000000";
    }


    private static string GetWebClient(string url)
    {
        string strHTML = "";
        WebClient myWebClient = new WebClient();
        try
        {
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
        }
        catch(Exception ex) {
            ////Debug.Log("报错信息" + ex + "详细信息" + ex.Message);
            strHTML = "1=0000000000";
        }

        return strHTML;
    }

    /// <summary>
    /// 获取时间戳
    /// 本方法只是为了测试时间戳样式
    /// </summary>
    /// <returns></returns>
    private static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    /// <param name="timeStamp">Unix时间戳格式</param>
    /// <returns>C#格式时间</returns>
    public static DateTime GetTime(string timeStamp)
    {
        //验证时间戳格式是否正确
        int errorValue=999;
        bool timeValue = int.TryParse(timeStamp,out errorValue);
        if (!timeValue) {
            //Debug.Log("时间戳验证错误!");
            timeStamp = "0000000000";   //强制等于1970的时间
        }

        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc));

        string timeStampSum = timeStamp + "0000000";
        long lTime = long.Parse(timeStampSum);
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }
    /*
    public Boolean requsterPermiss(string permisson) { 
        //if(contextc)
    }
    */

        public void ExitGame()
    {
        Debug.Log("退出游戏！退出游戏！退出游戏！退出游戏！");
        Application.Quit();
    }

    //打开防沉迷
    public void OpenFangChenMiYanZheng()
    {
        Debug.Log("OpenFangChenMiYanZheng111111");
        GameObject fangObj = (GameObject)Instantiate(FangChengMiYanZhengObj);
        fangObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        fangObj.transform.localScale = new Vector3(1, 1, 1);
        fangObj.transform.localPosition = Vector3.zero;
        fangObj.SetActive(true);
        Debug.Log("OpenFangChenMiYanZheng222222222");
    }


}