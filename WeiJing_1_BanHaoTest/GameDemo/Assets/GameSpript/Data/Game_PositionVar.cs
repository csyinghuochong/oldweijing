using UnityEngine;
using System.Collections;
using System.Data;
using UnityEngine.UI;



//此脚本用于存放游戏内的通用的Transform和GameObject，方便其他程序调用，统一规范化管理
public class Game_PositionVar : MonoBehaviour {

    public DataSet DataSetXml;              //XML缓存集
    public string RoseID;                   //角色ID
    public GameObject Obj_Rose;             //角色Obj
    public string Get_XmlPath;              //只读Xml的位置
    public string Set_XmlPath;              //可读可存Xml的位置
    public GameObject[] Obj_Keep;           //切换场景保存的Obj
    public GameObject Obj_GameSourceSet;    //游戏音效的Obj
    public float SourceSize;                //游戏音效声音大小值
    public GameObject Obj_UIItemTips;       //UI道具Tips的源文件
    public GameObject Obj_UIItemTipsUse;    //UI道具Tips使用的OBJ
    public GameObject Obj_UIThrowItemChoice;    //丢弃道具时选择丢弃数量的UI
	public GameObject Obj_UIEquipTips;		//UI装备Tips的源文件
    public GameObject Obj_UIEquipMake;      //装备制造源Obj
    public GameObject Obj_UIHeZi;           //经验盒子源Obj
    public GameObject Obj_UICunMinDeXin;    //村民的信源Obj
    public GameObject Obj_UISkillTips;      //UI技能Tips的源文件
    public GameObject OBJ_UIMoveIcon;       //UI移动时显示的ICON
    public GameObject OBJ_UI_Set;           //UITips的集合
	public GameObject Obj_UI_FunctionOpen;	//UI功能开启
    public GameObject Obj_Model_Drop;       //掉落Obj
    public Transform Tr_Drop;               //掉落的父级
    public GameObject Obj_UIDropName;       //掉落道具的UI
    public bool UpdataRoseItem;             //更新角色道具
    private int updataRoseItemSum;
    public GameObject Obj_UINpcName;        //Npc名称
	public GameObject Obj_UINpcStoreShow;	//商店界面
    public GameObject Obj_StoreTextBack;    //黑屏显示故事文字
    public GameObject Obj_StorySpeakSet;    //对话UI
	public bool UpdataBagAll;				//背包内容全部更新
	private int UpdataBagAllSum;			//背包内容更新累计值
    public bool UpdataRoseEquip;            //更新当前角色角色界面显示的属性值
    public GameObject Obj_BuildingName;     //建筑名称的OBJ
    public GameObject Obj_RosePetSet;       //宠物父级
    public GameObject Obj_MapCamera;        //小地图摄像机


    //public string GameStatusValue;          //0：进入游戏界面  1： 创建角色界面  2：正式在游戏中
    //UI道具交换使用的变量
    public bool ItemMoveStatus;                     // 是否在进行移动道具
    public string ItemMoveType_Initial;             //1：代表背包  2：代表英雄栏位
    public string ItemMoveValue_Initial;
    public string ItemMoveType_End;                 //1：代表背包  2：代表英雄栏位
    public string ItemMoveValue_End;

    //UI技能上交换
    public bool SkillMoveStatus;                    //是否在进行技能交换
    public string SkillMoveValue_Initial;                    
    public string SkillMoveValue_End;               

    public bool Rose_TaskListUpdata;                //是否更新任务列表
    public GameObject Obj_UITaskList_TaskType;      //任务列表UI
    public GameObject Obj_UITaskList_TaskRow;       //任务行UI
    public string NowTaskID;                        //当前选中任务ID
    public bool Rose_TaskDataUpdata;                //更新任务详细数据状态
    public string RoseTaskListShow_1;               //任务列表是否展开——主线任务
    public string RoseTaskListShow_2;               //任务列表是否展开——支线任务
    public string RoseTaskListShow_3;               //任务列表是否展开——其他任务
    public GameObject Obj_UI_NpcTaskList;           //接取任务用到的条
    public GameObject Obj_UI_NpcGetTask;            //接取任务用到的UI
    public GameObject Obj_UI_NpcCompleteTask;       //完成任务用到的UI
    public GameObject Obj_UI_NpcTask;               //NPC显示任务及对话的界面
    public string NpcTaskUpdataStatus;              //Npc显示的任务更新状态,0：表位未更新  1 - 2 表示执行到的步骤
	public bool MainUITaskUpdata;					//主界面任务显示更新
	public bool NpcTaskMainUIShow;					//主界面Npc头顶完成任务图标显示
	private int NpcTaskMainUIShowSum;				//主界面Npc头顶完成任务图标显示累计值，用于执行完当前帧重置其状态
	public bool UpdataRoseProperty;					//开启时更新当前角色属性
	public GameObject Obj_UIGameHint;				//主界面通用UI提示
    public GameObject Obj_UIGameGirdHintSing;		//主界面通用UI提示(组提示)
    public GameObject Obj_UICommonHint;             //通用提示,带选项(是否)
    public GameObject Obj_UICommonHint_2;             //通用提示,带选项(是否)
    public GameObject Obj_UIGetherItem;				//打开道具的进度条UI
    public GameObject Obj_GameSourceObj;            //游戏音效Obj
    public bool EnterScenceStatus;                  //切换场景
    public string EnterScencePositionName;          //切换场景的坐标名称
    public bool SellItemStatus;                     //出售道具时开启此状态
    public bool StoreHouseStatus;                   //打开仓库时开启此状态
    public bool EquipXiLianStatus;                  //打开洗练时开启此状态
    public bool UpdataSelectedStatus;               //开启后,所有怪物更新当前选中目标是不是自己,如果不是吧自身的选中状态去掉
    private int UpdataSelectedStatusSum;            //开启怪物更新选中状态的计数器
    public bool UpdataSellUIStatus;                 //开启更新出售道具后,回购的列表
    public bool EnterGameStatus;                    //进入游戏界面状态
    public bool Rose_PublicSkillCDStatus;           //角色技能公共CD状态
    public float Rose_PublicSkillCDTime;            //角色技能公共CD时间
    public float Rose_PublicSkillCDTimeSum;         //角色技能公共CD累加值
    public bool UpdataMainSkillUI;                  //开启后更新主界面上的技能IconUI
    private int UpdataMainSkillUISum;               //开启后更新主界面上的技能IconUI的累加值,保证在下一帧执行一次
    private float doorWayNextDelayTime;             //延迟一帧设置玩家坐标，要不玩家会掉下去
    //private float deathMonsterTimeSum;              //怪物死亡时间累计
    public GameObject Obj_MonsterDeathTime;         //怪物死亡倒计时显示UI
    public GameObject Obj_GameSetting;              //游戏设置UI
    public float GameSourceValue;                   //游戏声音值 1：表示正常  0表示静音
    public GameObject Obj_YaoGanSet;                //摇杆相关控件
    public bool YaoGanStatus;                       //摇杆状态    
    public GameObject Obj_MonsterModelSheXiangJi;   //怪物模型摄像机
    public GameObject Obj_RoseModelSheXiangJi;      //角色摄像机
    public bool roseOpenOnlyUI;                     //角色打开唯一UI
    public string GameNanduValue;                   //怪物难度

    //public bool DestroyKeepObj;                   //当次开关打开是注销Keep保存的Obj,此处用在战斗界面切换建筑界面中


    //建筑相关参数
    public bool UpdataBuildingIDStatus;             //开启后更新建筑ID
    private bool BuildingHintGirdStatus;            //开启时播放资源获取提示
    private float Minute_TimeSum;                   //单次奖励时间累积
    private float HintShowIntervalTime;             //提示间隔时间
    private bool gameOffResourceStatus;             //游戏离线资源是否发放  true 表示已发放
    private float minuteValue;
    public GameObject Obj_OffGameResource;              //离线资源显示的Obj
    public GameObject Obj_UIFortressBuildReward;        //要塞战斗结果Obj

    private float hour_TimeSum;             //小时计时器
    private float tiLi_TimeSum;             //体力计时器

    public string GovernmentLvID;   	//市政厅等级
    public string ResidentHouseID;  	//民居
    public string FarmlandID;      		//农田
    public string LoggingFieldID;		//伐木场
    public string StonePitID;		    //采石场
    public string SmeltingPlantID;		//冶炼厂
    public string CollegeID;   		    //学院
    public string FortressID;           //要塞
    public string TrainCampID;          //训练场

    public int BuildingGold;            //建筑金币
    public int Farm;             	    //农民
    public int Food;	                //粮食
    public int Wood;	                //木材
    public int Stone;	                //石头
    public int Iron;	                //钢铁

    public int BuildingGold_Add;            //建筑金币
    public int Farm_Add;             	    //农民
    public int Food_Add;	                //粮食
    public int Wood_Add;	                //木材
    public int Stone_Add;	                //石头
    public int Iron_Add;	                //钢铁
    public int RoseExp_Add;                 //角色经验

    public int MinuteBuildingGold;            //建筑金币 (分钟收益)
    public int MinuteFarm;             	      //农民 (分钟收益)
    public int MinuteFood;	                  //粮食 (分钟收益)
    public int MinuteWood;	                  //木材 (分钟收益)
    public int MinuteStone;	                  //石头 (分钟收益)
    public int MinuteIron;	                  //钢铁 (分钟收益)
    public int MinuteRoseExp;                 //角色经验
    public int MinuteRoseTiLi;                //角色体力

    public int CountryExp;                      //每分钟国家经验
    public int CountryHonor;                    //每分钟荣誉产出
    public int MinuteCountryExp;             //每分钟国家经验
    public int MinuteCountryHonor;           //每分钟荣誉产出
    public int CountryExp_Add;              
    public int CountryHonor_Add;

    public bool emenyActStatus;            //此状态为True时,表示触发了要塞防御
    private float emenyActTimeSum;
    public float emenyActTime;
    private bool firstEnterGame;            //第一次进入游戏获取怪物进攻时间
    public bool UpdatTaskStatus;            //任务更新状态

    public bool OpenEmenyActStatus;
    private float secUpdataTimeSum;
    public bool RoseDayPracticeRewardStatus;        //每日领经验和金币状态
    private bool ifOneGameToDay;                    //为True表示今天第一次登陆游戏
    //村民的信
    private float CunMinEmailSum;
    private float CunMinSaveSum;
    private bool CunMinStatus;

    //抽卡调用
    public bool ChouKaUIOpenStatus;
    public bool ChouKaStatus;
    public string ChouKaStr;
    public string ChouKaHindIdStr;

    //支付调用
    public bool PayStatus;          //交易状态
    public string PayStr;
    public string PayValueNow;         //当前交易值
    public string PayDingDanIDNow;      //当前交易订单
    public bool PayQueryStatus;     //交易订单查询状态
    public string PayStrQueryStatus;     //交易订单查询字符串
    private string testStrPay;

    public GameObject MonsterSet;
    public bool testFunction;           //测试功能
    public bool ifXiuGaiGame;           //是否修改游戏
    public string ErrorLogStr;          //错误日志

    public bool TestWeiTuo;

    public GameObject FangChengMiObj_1;
    public GameObject FangChengMiObj_2;
    public bool UpdateFangchenMiStatus;

    //在Start方法之前执行
    void Awake() {
        
        //保证切换场景以下预设体不消失
        for (int i = 1; i <= Obj_Keep.Length; i++) {
            DontDestroyOnLoad(Obj_Keep[i-1]);
        }
        
        //设置路径
        Get_XmlPath = Application.dataPath + "/GameData/Xml/Get_Xml/";        //只读Xml的位置
        Set_XmlPath = Application.dataPath + "/GameData/Xml/Set_Xml/";        //可读可存Xml的位置

        //加载游戏首次缓存数据集
        //Game_PublicClassVar.Get_function_DataSet.DataSet_AllReadXml();

        //临时数据
        //RoseID = "10001";       //初始化角色ID
        RoseID = Game_PublicClassVar.Get_wwwSet.RoseID;
        SourceSize = 1.0f;      //设置音效的大小值
        Rose_PublicSkillCDTime = 1;     //设置技能公共冷却CD为1秒

        OBJ_UI_Set = GameObject.FindWithTag("UI_Set");
		Obj_UI_FunctionOpen = GameObject.FindWithTag("UI_FunctionOpen");
        if (GameObject.FindWithTag("DropItemSet") != null) {
            Tr_Drop = GameObject.FindWithTag("DropItemSet").transform;
        }

        //初始化进入界面
        //EnterGameStatus = true;
        
        //初始化任务列表是打开的
        RoseTaskListShow_1 = "1";
        RoseTaskListShow_2 = "1";
        RoseTaskListShow_3 = "1";

        //设定建筑资源获取间隔时间
        HintShowIntervalTime = 60.0f;

        GameSourceValue = 0;  //默认声音正常,以后需要添加配置修改这里即可
        if (Application.loadedLevelName == "EnterGame") {
            //读取当前声音状态
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                GameSourceValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SourceSize", "ID", RoseID, "RoseConfig"));
            }
        }

        //初始化支付状态
        PayStr = "";
        PayStatus = false;

        //默认难度
        GameNanduValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");



    }

	// Use this for initialization
	void Start () {

        //测试数据（建筑相关）
        //Minute_Gold = 10.0f;    //每分钟金币产出,测试数据

        //获取离线发送资源状态
        gameOffResourceStatus = Game_PublicClassVar.Get_wwwSet.GameOffResourceStatus;

        //检测外挂
        Game_PublicClassVar.Get_wwwSet.IfUpdataGameWaiGua = false;

        UpdateFangchenMiStatus = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (TestWeiTuo) {
            TestWeiTuo = false;
            GameObject uiCommonHint = (GameObject)Instantiate(Obj_UICommonHint);
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("进入委托", testWeiTuoA, null);
        }

        if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
        {
            //Debug.Log("Updata读取数据表中……");
            return;
        }
		//当前帧执行完重置其状态
		if (NpcTaskMainUIShow) {
			NpcTaskMainUIShowSum = NpcTaskMainUIShowSum +1;
			if(NpcTaskMainUIShowSum > 1){
				NpcTaskMainUIShow=false;
				NpcTaskMainUIShowSum =0;
			}
		}
		//当前背包内容全更新重置其状态
		if (UpdataBagAll) {
            //Debug.Log("更新！！！");
			UpdataBagAllSum = UpdataBagAllSum +1;
			if(UpdataBagAllSum > 1){
				UpdataBagAll = false;
				UpdataBagAllSum = 0;
			}
		}

        if (UpdataRoseItem) {
            //Debug.Log("更新！！！");
            updataRoseItemSum = updataRoseItemSum + 1;
            if (updataRoseItemSum > 1)
            {
                UpdataRoseItem = false;
                updataRoseItemSum = 0;
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
                Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            }
        }

        //切换场景,更新自身的位置
        if (EnterScenceStatus) {

            doorWayNextDelayTime = doorWayNextDelayTime + Time.deltaTime;

            if (doorWayNextDelayTime > 1)
            {
                Obj_Rose.SetActive(true);
                EnterScenceStatus = false;


                /*
                GameObject scenceRosePosition = GameObject.FindWithTag("ScenceRosePosition");
                GameObject obj_Position = scenceRosePosition.transform.Find(EnterScencePositionName).gameObject;
                 
                //Debug.Log("obj_Position = " + obj_Position.transform.position);
                Obj_Rose.transform.position = obj_Position.transform.position;
                 
                //Debug.Log("设置的位置为" + obj_Position.transform.position + ",角色的坐标为：" + Obj_Rose.transform.position);
                EnterScencePositionName = "";       //清空
                */
                doorWayNextDelayTime = 0;
            }

        }
        if (Obj_Rose != null) {
            //Debug.Log("角色的坐标为：" + Obj_Rose.transform.position);
        }


        //更新建筑ID
        if (!UpdataBuildingIDStatus)
        {
            UpdataBuildingIDStatus = true;
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus == true) {
                try
                {
                    updateBuildingID();
                }
                catch {
                    Debug.Log("报错！！！更新建筑报错");
                }
            }
        }

        //每分钟在线收益
        Minute_TimeSum = Minute_TimeSum + Time.deltaTime;
        if (Minute_TimeSum >= HintShowIntervalTime)
        {
            Minute_TimeSum = 0.0f;
            minuteValue = HintShowIntervalTime /60.0f;
            //调用持久化数据
            updataPlayPrefsData();
            //添加资源
            /*
            BuildingGold_Add = (int)(BuildingGold_Add + MinuteBuildingGold * minuteValue);
            Food_Add = (int)(Food_Add + MinuteFood * minuteValue);
            Wood_Add = (int)(Wood_Add + MinuteWood * minuteValue);
            Stone_Add = (int)(Stone_Add + MinuteStone * minuteValue);
            Iron_Add = (int)(Iron_Add + MinuteIron * minuteValue);
            RoseExp_Add = (int)(RoseExp_Add + MinuteRoseExp * minuteValue);

            //持久化数据保存
            PlayerPrefs.SetInt("BuildingGold_Add", BuildingGold_Add);
            PlayerPrefs.SetInt("Food_Add", Food_Add);
            PlayerPrefs.SetInt("Wood_Add", Wood_Add);
            PlayerPrefs.SetInt("Stone_Add", Stone_Add);
            PlayerPrefs.SetInt("Iron_Add", Iron_Add);
            PlayerPrefs.SetInt("RoseExp_Add", RoseExp_Add);
            */
            //Debug.Log("CountryHonor_Add = " + CountryHonor_Add);
            CountryExp_Add = (int)(CountryExp_Add + MinuteCountryExp * minuteValue);
            CountryHonor_Add = (int)(CountryHonor_Add + MinuteCountryHonor * minuteValue);
            //持久化数据
            PlayerPrefs.SetInt("CountryExp_Add", CountryExp_Add);
            PlayerPrefs.SetInt("CountryHonor_Add", CountryHonor_Add);

            //判定当前场景是否是建筑场景
            if (Application.loadedLevelName == "EnterGame") {
                BuildingHintGirdStatus = true;
                //Debug.Log("开启存储资源数据");
            }

            ActTimeSet();       //更新怪物进攻时间,每10秒


            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {

                //验证防沉迷
                string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_10", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                nowTime = PlayerPrefs.GetInt("FangChenMi_Time").ToString();
                if (nowTime == "")
                {
                    nowTime = "0";
                }
                int nowTimeSum = int.Parse(nowTime);
                nowTimeSum = nowTimeSum + 1;
                PlayerPrefs.SetInt("FangChenMi_Time", nowTimeSum);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_10", nowTimeSum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

                //没有进行实名认证
                if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 0)
                {

                    if (nowTimeSum == 45)
                    {
                        //弹出对应提示
                        OBJ_UI_Set.GetComponent<UI_Set>().FangChengMiHint();
                    }

                    if (nowTimeSum == 60)
                    {
                        //弹出对应提示
                        OBJ_UI_Set.GetComponent<UI_Set>().FangChengMiHint();
                    }
                }
                //未成年
                if (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type == 1)
                {

                    if (nowTimeSum == 60)
                    {
                        //弹出对应提示
                        OBJ_UI_Set.GetComponent<UI_Set>().FangChengMiHint();
                    }

                    if (nowTimeSum == 90)
                    {
                        //弹出对应提示
                        OBJ_UI_Set.GetComponent<UI_Set>().FangChengMiHint();
                    }

                    //验证登陆时间
                    System.DateTime nowData = System.DateTime.Now;
                    /*
                    if (Game_PublicClassVar.gameLinkServer.ServerLinkTimeStamp == "")
                    {
                        nowData = DateTime.Now;
                    }
                    else
                    {
                        nowData = Game_PublicClassVar.Get_wwwSet.GetTime(Game_PublicClassVar.gameLinkServer.ServerLinkTimeStamp);
                    }
                    */

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
                }
            }

        }

        //每小时在线收益
        hour_TimeSum = hour_TimeSum + Time.deltaTime;
        if (hour_TimeSum >= 3600.0f)
        {
            hour_TimeSum = 0;

            //获取当前农民数量及上限
            string farmerNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmerNumMax", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            string farm = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            if (int.Parse(farm) < int.Parse(farmerNumMax)) {

                //获取差值,当差值快满足上限时,只弥补差值部分
                int differenceValue = int.Parse(farmerNumMax) - int.Parse(farm);
                if (differenceValue < Farm_Add) {
                    Farm_Add = differenceValue;
                }

                Farm_Add = (int)(Farm_Add + MinuteFarm);
                PlayerPrefs.SetInt("Farm_Add", Farm_Add);

                buildingHintGird("农民", Farm_Add);
                //更新界面人数显示
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Obj_TrainCamp.GetComponent<UI_TrainCamp>().UpdataShowStatus = true;
            }
        }

        //更新体力
        tiLi_TimeSum = tiLi_TimeSum + Time.deltaTime;
        //每5分钟更新1点体力
        if(tiLi_TimeSum>=300.0f)
        {
            tiLi_TimeSum = 0;
            //获取当前体力,+1后进行存储
            
            Game_PublicClassVar.Get_function_Rose.AddTili(1);
        }

        if (BuildingHintGirdStatus) {

            /*
            buildingHintGird("紫金", BuildingGold_Add);
            buildingHintGird("粮食", Food_Add);
            buildingHintGird("木材", Wood_Add);
            buildingHintGird("石头", Stone_Add);
            buildingHintGird("钢铁", Iron_Add);
            buildingHintGird("角色经验", RoseExp_Add);
            */

            buildingHintGird("繁荣度", CountryExp_Add);
            buildingHintGird("荣誉", CountryHonor_Add);
            
            BuildingHintGirdStatus = false;

            //开启存储数据
            updateBuildingResource();   //更新当前存储资源
            /*
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BuildingGold", (BuildingGold_Add + BuildingGold).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Farm", (Farm_Add + Farm).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Food", (Food_Add + Food).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Wood", (Wood_Add + Wood).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Stone", (Stone_Add + Stone).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Iron", (Iron_Add + Iron).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");

            //添加角色经验
            Game_PublicClassVar.Get_function_Rose.AddExp(RoseExp_Add,"1");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
            */

            //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", (CountryExp_Add + CountryExp).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            //Debug.Log("增加经验：" + CountryExp_Add + CountryExp);
            Game_PublicClassVar.Get_function_Country.addCoutryExp(CountryExp_Add);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", (CountryHonor_Add + CountryHonor).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

            /*
            //清空数据
            BuildingGold_Add = 0;
            Farm_Add = 0;
            Food_Add = 0;
            Wood_Add = 0;
            Stone_Add = 0;
            Iron_Add = 0;

            //持久化数据保存清空
            PlayerPrefs.SetInt("BuildingGold_Add", 0);
            PlayerPrefs.SetInt("Farm_Add", 0);
            PlayerPrefs.SetInt("Food_Add", 0);
            PlayerPrefs.SetInt("Wood_Add", 0);
            PlayerPrefs.SetInt("Stone_Add", 0);
            PlayerPrefs.SetInt("Iron_Add", 0);
            PlayerPrefs.SetInt("RoseExp_Add", 0);
            */

            //清空数据
            CountryExp_Add = 0;
            CountryHonor_Add = 0;
            PlayerPrefs.SetInt("CountryExp_Add", 0);
            PlayerPrefs.SetInt("CountryHonor_Add", 0);
        }


         //判定离线资源是否已经发放
        if (!gameOffResourceStatus)
        {
            //判定当前场景是否是建筑场景
            if (Application.loadedLevelName == "EnterGame")
            {
                //判定是否读取到世界时间
                if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
                {
                    UpdataOffGameResource();
                }
            }
        }

        //第一次进入游戏触发
        if (!firstEnterGame) {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus == true) {

                firstEnterGame = true;

                //更新进攻时间
                emenyActTime = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EnemyTime", "ID", RoseID, "RoseBuilding"));

                //检测当前是否第一次登陆游戏,清空每日奖励数据
                /*
                if (ifOneGameToDay) {
                    Debug.Log("今天第一次登陆游戏！");
                    dayClearnGameData();    //清空每日数据
                }
                */

                if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus) {
                    //Debug.Log("ssssss123s");
                    dayClearnGameData();    //清空每日数据
                }
                

                //检测当前首次进入游戏
                string oneEnterGame = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirstEnterGame", "ID", RoseID, "RoseConfig");
                if (oneEnterGame == "0") {
                    try
                    {
                        dayClearnGameData(true);    //清空每日数据
                    }
                    catch {
                        Debug.Log("清空每日数据报错");
                    }
                    
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FirstEnterGame", "1","ID", RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }

                //更新每日奖励领取状态
                string expNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_ExpNum", "GameMainValue");
                string expNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string goldNumMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "Day_GoldNum", "GameMainValue");
                string goldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

                //如果今日领取次数满了则不在触发时间累计
                if (int.Parse(expNum) >= int.Parse(expNumMax) && int.Parse(goldNum) >= int.Parse(goldNumMax))
                {
                    RoseDayPracticeRewardStatus = true;
                }

                //清空出售道具的数据
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                
            }
        }

        //判定当前凌晨更新游戏数据
        if (Game_PublicClassVar.Get_wwwSet.DayUpdataStatus) {
            Game_PublicClassVar.Get_wwwSet.DayUpdataStatus = false;
            dayClearnGameData(false,true);    //清空每日数据
        }

        //判定要塞防御状态
        if (OpenEmenyActStatus) {
            if (!emenyActStatus)
            {
                //开始倒计时
                emenyActTime = emenyActTime - Time.deltaTime;
                //if()
                if (emenyActTime <= 0.0f)
                {
                    //屏蔽要塞 emenyActStatus = true;
                }
            }
        }

        //测试功能
        if (testFunction) {
            testFunction = false;
            //dayClearnGameData();
        }


	}

    void LateUpdate() {

        if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
        {
            //Debug.Log("LateUpdate读取数据表中……");
            return;
        }

		//更新当前角色属性
		if (UpdataRoseProperty) {
			UpdataRoseProperty = false;
			Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();
		}

        //怪物选中状态开启后执行一个循环后关闭
        if (UpdataSelectedStatus) {
            UpdataSelectedStatusSum = UpdataSelectedStatusSum + 1;
            if (UpdataSelectedStatusSum > 1) {
                UpdataSelectedStatus = false;
                UpdataSelectedStatusSum = 0;
            }
        }

        //刷新技能公共冷却时间
        if (Rose_PublicSkillCDStatus)
        {
            Rose_PublicSkillCDTimeSum = Rose_PublicSkillCDTimeSum + Time.deltaTime;
            if (Rose_PublicSkillCDTimeSum >= Rose_PublicSkillCDTime)
            {
                Rose_PublicSkillCDTimeSum = 0.0f;
                Rose_PublicSkillCDStatus = false;
            }
        }

        //更新主界面的Icon显示
        if (UpdataMainSkillUI) {
            if (UpdataMainSkillUISum >= 2) {
                UpdataMainSkillUI = false;
                UpdataMainSkillUISum = 0;
            }
            UpdataMainSkillUISum = UpdataMainSkillUISum + 1;
        }

        //更新任务显示
        if (UpdatTaskStatus)
        {
            UpdatTaskStatus = false;
            Game_PublicClassVar.Get_function_Task.updataTaskItemID();
        }

        //更新怪物死亡时间
        /*
        deathMonsterTimeSum = deathMonsterTimeSum + Time.deltaTime;
        if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
            //Debug.Log("读取死亡时间");
            if (deathMonsterTimeSum >= 1.0f)
            {
                deathMonsterTimeSum = 0;
                //更新怪物复活时间
                Game_PublicClassVar.Get_function_AI.UpdataMonsterDeathTime(deathMonsterTimeSum);
                //Debug.Log("读取死亡时间完毕");
            }
        }
        */
        /*
        //战斗界面切换建筑界面是注销的Obj
        if (DestroyKeepObj) {

            //保证切换场景以下预设体不消失
            for (int i = 1; i <= Obj_Keep.Length; i++)
            {
                DontDestroyOnLoad(Obj_Keep[i - 1]);
            }
        
        }
        */
        //村民的信
        //CunMinEmailSum = CunMinEmailSum + Time.deltaTime;
        if (!CunMinStatus) {
            CunMinSaveSum = CunMinSaveSum + Time.deltaTime;
            //每10秒记录一次事件
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
                if (CunMinSaveSum > 3600.0f)
                {
                    float specialEventTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpecialEventTime", "ID", RoseID, "RoseData"));
                    specialEventTime = specialEventTime + CunMinSaveSum;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventTime", specialEventTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                    CunMinEmailSum = specialEventTime;
                    //Debug.Log("CunMinEmailSum = " + CunMinEmailSum);
                    CunMinSaveSum = 0;
                }
            }
        }
        //设置时间触发
        if (CunMinEmailSum > 30) {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
                if (Random.value <= 1)
                {
                    //触发村民的信
                    if (Application.loadedLevelName != "StartGame") {
                        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.GetComponent<UI_CunMinDeXinBtn>().EventStatus == false)
                        {
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.SetActive(true);
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.GetComponent<UI_CunMinDeXinBtn>().EventStatus = true;
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnCunMinDeXin.GetComponent<UI_CunMinDeXinBtn>().EventUpdaDataStatus = true;
                            CunMinEmailSum = 0; //清空
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventTime", CunMinEmailSum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            //CunMinStatus = true;
                        }
                    }
                }
            }
        }

        //每秒执行一次
        secUpdataTimeSum = secUpdataTimeSum + Time.deltaTime;
        if (secUpdataTimeSum >= 10)
        {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
                if (!RoseDayPracticeRewardStatus)
                {
                    //写入更新数据
                    string expTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    string goldTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    float writeExpTime = float.Parse(expTime) + secUpdataTimeSum;
                    float writeGoldTime = float.Parse(goldTime) + secUpdataTimeSum;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpTime", writeExpTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldTime", writeGoldTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                }

                //写入抽卡时间
                float chouKaTime_One = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                float chouKaTime_Ten = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                chouKaTime_One = chouKaTime_One + secUpdataTimeSum;
                chouKaTime_Ten = chouKaTime_Ten + secUpdataTimeSum;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", chouKaTime_One.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", chouKaTime_Ten.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                //Debug.Log("更新数据");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                //写入怪物死亡时间
                string deathMonsterIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string[] deathMonsterID = deathMonsterIDList.Split(';');
                string deathMonsterIDStr = "";
                if (deathMonsterIDList != "")
                {
                    //循环获取当前怪物死亡时间
                    for (int i = 0; i <= deathMonsterID.Length-1; i++)
                    {
                        if (deathMonsterID[i] != "")
                        {
                            string monsterID = deathMonsterID[i].Split(',')[0];                             
                            float monsterTime = float.Parse(deathMonsterID[i].Split(',')[1]);               //获取在线BOSS复活时间
                            float monsterOffLineTime = float.Parse(deathMonsterID[i].Split(',')[2]);        //获取离线BOSS复活时间
                            //如果离线时间少于在线时间,在线时间取离线时间的值
                            if (monsterOffLineTime < monsterTime) {
                                monsterTime = monsterOffLineTime;
                            }
                            monsterTime = monsterTime - secUpdataTimeSum;
                            monsterOffLineTime = monsterOffLineTime - secUpdataTimeSum;
                            if (monsterTime <= 0)
                            {
                                monsterTime = 0;
                            }
                            else
                            {
                                deathMonsterIDStr = deathMonsterIDStr + monsterID + "," + monsterTime + ","+ monsterOffLineTime + ";";
                            }
                        }
                    }
                    //记录怪物死亡数据
                    if (deathMonsterIDStr != "")
                    {
                        deathMonsterIDStr = deathMonsterIDStr.Substring(0, deathMonsterIDStr.Length - 1);
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    }
                    else
                    {
                        deathMonsterIDStr = "";
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    }
                }


                //清空值
                secUpdataTimeSum = 0;
            }
        }

        //if (UpdateFangchenMiStatus) {
        //UpdateFangchenMiStatus = false;
        //0 未验证  1 已验证  未满18  2 已满正  满18
        if (Application.loadedLevelName != "StarGame"&& FangChengMiObj_1!=null)
        {
            string showStr = "";
            switch (Game_PublicClassVar.Get_wwwSet.FangChengMi_Type)
            {
                case 0:
                    showStr = "未验证";
                    break;
                case 1:
                    showStr = "已验证(未满18周岁)";
                    break;
                case 2:
                    showStr = "验证通过";
                    break;
                default:
                    showStr = "验证失效";
                    break;
            }
            FangChengMiObj_1.GetComponent<Text>().text = "实名验证:" + showStr;
            FangChengMiObj_2.GetComponent<Text>().text = "实名验证:" + showStr;
        }
        //}
    }
    /*
    void OnGUI() {
        //testStrPay = "111111" + "\n" + "222222";
        GUI.Box(new Rect(10, 10, 500, 670), testStrPay);
    }
    */
    //每天凌晨需要清空的游戏数据(参数1：是否第一次进入游戏  参数2：是否为当天24点进入的游戏)
    void dayClearnGameData(bool ifFirst = false, bool ifUpdataTime24 = false)
    {
        //Debug.Log("ssssssss");
        //获取当前时间
        string nowDayValue = Game_PublicClassVar.Get_wwwSet.DataTime.Day.ToString();
        //年份小于设置天数为第0天防止重复
        if (Game_PublicClassVar.Get_wwwSet.DataTime.Year < 2017)
        {
            nowDayValue = "0";
        }
        else
        {
            //如果是当前时间为24点清空时间时,需要按照月份的不同让时间变的不同
            if (ifUpdataTime24)
            {
                int nowMonthValue = Game_PublicClassVar.Get_wwwSet.DataTime.Month;
                //Debug.Log("nowMonthValue = " + nowMonthValue);
                if (nowMonthValue == 1 || nowMonthValue == 3 || nowMonthValue == 5 || nowMonthValue == 7 || nowMonthValue == 8 || nowMonthValue == 10 || nowMonthValue == 12)
                {
                    if (int.Parse(nowDayValue) >= 31)
                    {
                        nowDayValue = "1";
                    }
                    else
                    {
                        //Debug.Log("nowDayValue1 = " + nowDayValue);
                        nowDayValue = (Game_PublicClassVar.Get_wwwSet.DataTime.Day + 1).ToString();
                        //Debug.Log("nowDayValue2 = " + nowDayValue);
                    }
                }
                if (nowMonthValue == 4 || nowMonthValue == 6 || nowMonthValue == 9 || nowMonthValue == 11)
                {
                    //Debug.Log("nowDayValue = " + nowDayValue);
                    if (int.Parse(nowDayValue) >= 29)
                    {
                        nowDayValue = "1";
                        //Debug.Log("nowDayValue1111 = " + nowDayValue);
                    }
                    else
                    {
                        nowDayValue = (Game_PublicClassVar.Get_wwwSet.DataTime.Day + 1).ToString();
                    }
                }
                if (nowMonthValue == 2)
                {
                    if (int.Parse(nowDayValue) >= 28)
                    {
                        nowDayValue = "1";
                    }
                    else
                    {
                        nowDayValue = (Game_PublicClassVar.Get_wwwSet.DataTime.Day + 1).ToString();
                    }
                }
            }
        }


        if (!ifFirst) {

            //Debug.Log("Game_PublicClassVar.Get_wwwSet = " + Game_PublicClassVar.Get_wwwSet.LastOffGameTime);

            Debug.Log("aaa=" + Game_PublicClassVar.Get_wwwSet.LastOffGameTime + "bbb=" + Game_PublicClassVar.Get_wwwSet.DataTime);
            //年份小于2017不会领取
            if (Game_PublicClassVar.Get_wwwSet.DataTime.Year < 2017)
            {
                Debug.Log("获取时间小于2017");
                return;
            }

            //获取当前日期
            string lastday = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

            //判定月份是否一样,如果月份一样,判断日期时间是否一样
            Debug.Log("lastday = " + lastday + "nowDayValue = " + nowDayValue);
            if (lastday == nowDayValue)
            {
                Debug.Log("天数一致");
                return;
            }

            if (!ifUpdataTime24) {
                if (int.Parse(lastday) > int.Parse(nowDayValue))
                {
                    //判定月份属否一致
                    if (Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Month == Game_PublicClassVar.Get_wwwSet.DataTime.Month)
                    {
                        Debug.Log("时区的天数一致");
                        return;
                    }
                }
            }


            /*
            //判定不同时区离线的日期是否为一天
            if (Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Month == Game_PublicClassVar.Get_wwwSet.DataTime.Month) {
                //if (Game_PublicClassVar.Get_wwwSet.DataTime.Day == int.Parse(lastday))
                //{
                    if (Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Day == Game_PublicClassVar.Get_wwwSet.DataTime.Day)
                    {
                        Debug.Log("时区的天数一致");
                        return;
                    }
                //}
            }*/
        }

        Debug.Log("BiuBiuBiuBiu~~~~~~~~~~~~~~开始清空数据！");
        PlayerPrefs.SetInt("FangChenMi_Time", 0);

        //清空每日付费数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_7", "0;0;0;0;0;0;0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_8", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_10", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //清空每日奖励数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_ExpTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_GoldTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //清空分享数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_1", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_2", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("FenXiang_3", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //清空每日副本次数
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Day_FuBenNum", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        PlayerPrefs.SetInt("FuBenKillBossNum", 0);
        PlayerPrefs.SetInt("LingQuFuBenExp", 0);
        
        //清空每日任务
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string[] dayTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayTask_ID", "GameMainValue").Split(';');
        string dayTaskIDStr = "";
        string dayTaskValueStr = "";
        //随机三个任务ID
        int DayTask_Num = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayTask_Num", "GameMainValue"));
        float taskNum = dayTaskID.Length - 0.01f;
        for (int i = 0; i <= DayTask_Num-1; i++)
        {
            int randValue = (int)(Random.value * taskNum);
            //获取任务概率
            float taskRandValue = Random.value;
            string writeTaskID = dayTaskID[i];
            string writeTaskID_Next = writeTaskID;
            float triggerPro = 0;
            int kajiNum = 0;
            do{
                writeTaskID = writeTaskID_Next;
                triggerPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerPro", "ID", writeTaskID, "TaskCountry_Template"));
                //Debug.Log("任务概率：" + taskRandValue + "/" + triggerPro);
                writeTaskID_Next = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextTask", "ID", writeTaskID, "TaskCountry_Template");

                //防止卡机
                if (kajiNum >= 10) {
                    taskRandValue = -1;
                }
                if (writeTaskID_Next == "0") {
                    taskRandValue = -1;
                }
            } while (taskRandValue >= triggerPro);

            dayTaskIDStr = dayTaskIDStr + writeTaskID + ";";
            dayTaskValueStr = dayTaskValueStr + "0" + ";";
        }

        //添加隐藏任务
        string countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        float hideTaskPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideTaskPro", "ID", countryLv, "Country_Template"));
        //hideTaskPro = 1;
        if (Random.value <= hideTaskPro) {
            //Debug.Log("触发隐藏任务");
            //获取隐藏任务
            string[] DayHideTaskList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "DayTask_HideID", "GameMainValue").Split(';');
            float hidTaskNum = DayHideTaskList.Length - 0.01f;

            string DayHideTaskListStr = "";
            for (int i = 0; i <= 10; i++) {
                int randValue = (int)(Random.value * hidTaskNum);
                DayHideTaskListStr = DayHideTaskList[randValue];

                string taskTriggerType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerType", "ID", DayHideTaskListStr, "TaskCountry_Template");
                if (taskTriggerType == "1") {
                    int taskLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerValue", "ID", DayHideTaskListStr, "TaskCountry_Template"));
                    if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= taskLv)
                    {
                        i = 11; //跳出循环
                    }
                    else {
                        DayHideTaskListStr = "";
                    }
                }
            }
            //如果DayHideTaskListStr为0,就默认指定一个任务
            if (DayHideTaskListStr == "") {
                DayHideTaskListStr = "30001";
            }

            dayTaskIDStr = dayTaskIDStr + DayHideTaskListStr + ";";
            dayTaskValueStr = dayTaskValueStr + "0;";
        }

        if (dayTaskIDStr != "") {
            dayTaskIDStr = dayTaskIDStr.Substring(0, dayTaskIDStr.Length - 1);
        }
        if (dayTaskValueStr != "")
        {
            dayTaskValueStr = dayTaskValueStr.Substring(0, dayTaskValueStr.Length - 1);
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskID", dayTaskIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", dayTaskValueStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //写入月卡每日领取数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YueKaDayStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //清空登陆数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DengLuDayStatus", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        //记录每日领取数据
        //Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        Debug.Log("nowDayValue = " + nowDayValue);
        
        //存入每日奖励日期
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_2", nowDayValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }

    void updateBuildingID() {

        //更新建筑ID
        GovernmentLvID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GovernmentLvID", "ID", RoseID, "RoseBuilding");
        ResidentHouseID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ResidentHouseID", "ID", RoseID, "RoseBuilding");
        FarmlandID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FarmlandID", "ID", RoseID, "RoseBuilding");
        LoggingFieldID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LoggingFieldID", "ID", RoseID, "RoseBuilding");
        StonePitID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StonePitID", "ID", RoseID, "RoseBuilding");
        SmeltingPlantID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SmeltingPlantID", "ID", RoseID, "RoseBuilding");
        CollegeID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CollegeID", "ID", RoseID, "RoseBuilding");
        FortressID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FortressID", "ID", RoseID, "RoseBuilding");
        TrainCampID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TrainCampID", "ID", RoseID, "RoseBuilding");

        /*
        BuildingGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingGold", "ID", RoseID, "RoseBuilding"));
        Farm = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", RoseID, "RoseBuilding"));
        Food = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Food", "ID", RoseID, "RoseBuilding"));
        Wood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Wood", "ID", RoseID, "RoseBuilding"));
        Stone = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Stone", "ID", RoseID, "RoseBuilding"));
        Iron = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Iron", "ID", RoseID, "RoseBuilding"));

        
        MinuteBuildingGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteBuildingGold", "ID", RoseID, "RoseBuilding"));
        MinuteFarm = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteFarm", "ID", RoseID, "RoseBuilding"));
        MinuteFood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteFood", "ID", RoseID, "RoseBuilding"));
        MinuteWood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteWood", "ID", RoseID, "RoseBuilding"));
        MinuteStone = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteStone", "ID", RoseID, "RoseBuilding"));
        MinuteIron = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteIron", "ID", RoseID, "RoseBuilding"));
        MinuteRoseExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MinuteRoseExp", "ID", RoseID, "RoseBuilding"));
        */

        CountryExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryExp", "ID", RoseID, "RoseDayReward"));
        CountryHonor = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", RoseID, "RoseDayReward"));


        //更新游戏分钟产出资源
        Game_PublicClassVar.Get_function_Building.UpdataMinuteResource();
        Game_PublicClassVar.Get_function_Country.UpdataMinuteData();

    }


    //更新建筑资源
    void updateBuildingResource() {

        /*
        BuildingGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuildingGold", "ID", RoseID, "RoseBuilding"));
        Farm = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Farm", "ID", RoseID, "RoseBuilding"));
        Food = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Food", "ID", RoseID, "RoseBuilding"));
        Wood = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Wood", "ID", RoseID, "RoseBuilding"));
        Stone = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Stone", "ID", RoseID, "RoseBuilding"));
        Iron = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Iron", "ID", RoseID, "RoseBuilding"));
        */

        CountryExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryExp", "ID", RoseID, "RoseDayReward"));
        CountryHonor = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", RoseID, "RoseDayReward"));

    }

    //更新持久化数据
    void updataPlayPrefsData() {

        /*
        BuildingGold_Add = PlayerPrefs.GetInt("BuildingGold_Add");
        Farm_Add = PlayerPrefs.GetInt("Farm_Add");
        Food_Add = PlayerPrefs.GetInt("Food_Add");
        Wood_Add = PlayerPrefs.GetInt("Wood_Add");
        Stone_Add = PlayerPrefs.GetInt("Stone_Add");
        Iron_Add = PlayerPrefs.GetInt("Iron_Add");
        RoseExp_Add = PlayerPrefs.GetInt("RoseExp_Add");
        */
        
        CountryExp_Add = PlayerPrefs.GetInt("CountryExp_Add");
        CountryHonor_Add = PlayerPrefs.GetInt("CountryHonor_Add");
    
    }

    void buildingHintGird(string resourceName,int resourceValue) {

        if (resourceValue > 0)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint(resourceName + "+" + resourceValue.ToString());
        }
    }

    void UpdataOffGameResource() {

        //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID1 = " + Game_PublicClassVar.Get_wwwSet.RoseID);

        //判定是否读取到世界时间
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
        {
            //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID2 = " + Game_PublicClassVar.Get_wwwSet.LastOffGameTime);
            //获取上一次离线时间,如果小于2016表示第一次登陆将直接跳转
            if (Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Year >= 2016)
            {
                //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID3 = " + Game_PublicClassVar.Get_wwwSet.RoseID);
                //计算离线时间(最多离线收益为24小时)
                int year = Game_PublicClassVar.Get_wwwSet.DataTime.Year - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Year;
                int month = Game_PublicClassVar.Get_wwwSet.DataTime.Month - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Month;
                int day = Game_PublicClassVar.Get_wwwSet.DataTime.Day - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Day;
                int hour = Game_PublicClassVar.Get_wwwSet.DataTime.Hour - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Hour;
                int minute = Game_PublicClassVar.Get_wwwSet.DataTime.Minute - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Minute;
                int second = Game_PublicClassVar.Get_wwwSet.DataTime.Second - Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Second;

                System.TimeSpan offGameTime = Game_PublicClassVar.Get_wwwSet.DataTime - Game_PublicClassVar.Get_wwwSet.LastOffGameTime;

                //Debug.Log("Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Day = " + Game_PublicClassVar.Get_wwwSet.LastOffGameTime.Day);
                //Debug.Log("Game_PublicClassVar.Get_wwwSet.DataTime.Day = " + Game_PublicClassVar.Get_wwwSet.DataTime.Day);
                //Debug.Log("year = " + year + "  month = " + month + "   Day = " + day);

                //判定当前是否第一次登陆
                if (year >= 1) {
                    ifOneGameToDay = true;
                }
                if (month >= 1) {
                    ifOneGameToDay = true;
                }
                if (day >= 1) {
                    ifOneGameToDay = true;
                }

                //Debug.Log("离线时间戳 = " + offGameTime.TotalSeconds);

                int offGameTimeInt = (int)(offGameTime.TotalSeconds);

                //发送资源时减去已经开启的时间
                offGameTimeInt = offGameTimeInt - (int)(Time.time);
                //Debug.Log("离线时间：" + offGameTimeInt);
                //写入抽卡时间
                float chouKaTime_One = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                float chouKaTime_Ten = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                chouKaTime_One = chouKaTime_One + offGameTimeInt;
                chouKaTime_Ten = chouKaTime_Ten + offGameTimeInt;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_One", chouKaTime_One.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChouKaTime_Ten", chouKaTime_Ten.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                //循环减去判定怪物的离线复活时间
                Game_PublicClassVar.Get_function_AI.UpdataMonsterDeathOffLineTime(offGameTimeInt);

                //限制离线收益(8小时)
                if (offGameTimeInt > 28800)
                {
                    offGameTimeInt = 28800;
                }

                if (offGameTimeInt <= 0) {
                    offGameTimeInt = 0;
                }

                /*
                int BuildingGold_off = (int)(MinuteBuildingGold * offGameTimeInt / 60.0f);
                int Food_off = (int)(MinuteFood * offGameTimeInt / 60.0f);
                int Wood_off = (int)(MinuteWood * offGameTimeInt / 60.0f);
                int Stone_off = (int)(MinuteStone * offGameTimeInt / 60.0f);
                int Iron_off = (int)(MinuteIron * offGameTimeInt / 60.0f);
                int RoseExp_off = (int)(MinuteRoseExp * offGameTimeInt / 60.0f);
                */
                
                int CountryExp_off = (int)(MinuteCountryExp * offGameTimeInt / 60.0f);
                int CountryHonor_off = (int)(MinuteCountryHonor * offGameTimeInt / 60.0f);

                //发送离线获得体力
                int addTili = (int)(offGameTimeInt / 300.0f);
                Game_PublicClassVar.Get_function_Rose.AddTili(addTili);

                //表示资源已经发放
                gameOffResourceStatus = true;
                Game_PublicClassVar.Get_wwwSet.GameOffResourceStatus = true;

                //开启存储数据
                updateBuildingResource();   //更新当前存储资源
                /*
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BuildingGold", (BuildingGold_off + BuildingGold).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Farm", (Farm_off + Farm).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Food", (Food_off + Food).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Wood", (Wood_off + Wood).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Stone", (Stone_off + Stone).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Iron", (Iron_off + Iron).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
                //添加角色经验
                Game_PublicClassVar.Get_function_Rose.AddExp(RoseExp_Add,"1");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
                */

                //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", (CountryExp_off + CountryExp).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

                //写入离线数据
                Game_PublicClassVar.Get_wwwSet.writeOffGameTime(0);
                
                Game_PublicClassVar.Get_function_Country.addCoutryExp(CountryExp_off);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", (CountryHonor_off + CountryHonor).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");


                /*
                offGameResourceObj.GetComponent<UI_OffGameResource>().BuildingGold = BuildingGold_off;
                offGameResourceObj.GetComponent<UI_OffGameResource>().Food = Food_off;
                offGameResourceObj.GetComponent<UI_OffGameResource>().Wood = Wood_off;
                offGameResourceObj.GetComponent<UI_OffGameResource>().Stone = Stone_off;
                offGameResourceObj.GetComponent<UI_OffGameResource>().Iron = Iron_off;
                offGameResourceObj.GetComponent<UI_OffGameResource>().RoseExp = RoseExp_off;
                */

                if (offGameTimeInt > 0) {
                    //弹出离线UI
                    GameObject offGameResourceObj = (GameObject)Instantiate(Obj_OffGameResource);
                    offGameResourceObj.transform.SetParent(OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
                    offGameResourceObj.transform.localPosition = Vector3.zero;
                    offGameResourceObj.transform.localScale = new Vector3(1, 1, 1);
                    offGameResourceObj.GetComponent<UI_OffGameResource>().CountryExp = CountryExp_off;
                    offGameResourceObj.GetComponent<UI_OffGameResource>().CountryHonor = CountryHonor_off;
                }


            }
        }
    }
    //记录怪物进攻时间
    public void ActTimeSet()
    {
        
        //获取当前进攻时间
        int actTimeValue = (int)(Game_PublicClassVar.Get_game_PositionVar.emenyActTime);
        //actTimeValue = 10;  //测试
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EnemyTime", actTimeValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
    }


    //支付回执
    public void OnPayResultReturn(string str)
    {
        testStrPay = testStrPay + ";" + str;
        //Debug.Log("调用了支付返回值");
        //IOS强制支付成功,因为失败会调用下面另一个函数
        #if UNITY_IPHONE
            str = "1;"+str;
        #endif

        PayStr = str;

        //payStr = str;
        if (str != "")
        {
            PayStatus = true;   //开启支付状态
            /*（修改交易方式不需要存储订单号了）
            //存储交易订单号
            if (str.Split(';')[0]=="4") {
                PayDingDanIDNow = str.Split(';')[1];
                Game_PublicClassVar.Get_function_Rose.WritePayID(PayDingDanIDNow, PayValueNow);
            }
            */
        }
    }


#if UNITY_IPHONE
    //支付回执（IOS失败回执）
    public void OnPayResultReturnFail(string str)
    {
        testStrPay = testStrPay + ";" + str;
        //Debug.Log("调用了支付返回值");
        PayStr = str;
        //payStr = str;
        if (str != "")
        {
            PayStatus = true;   //开启支付状态
            /*（修改交易方式不需要存储订单号了）
            //存储交易订单号
            if (str.Split(';')[0]=="4") {
                PayDingDanIDNow = str.Split(';')[1];
                Game_PublicClassVar.Get_function_Rose.WritePayID(PayDingDanIDNow, PayValueNow);
            }
            */
        }
    }
#endif

    //查询订单回执
    public void OnPayQueryReturn(string str)
    {
        //Debug.Log("调用了支付查询值");
        PayStrQueryStatus = str;
        //testStrPay = testStrPay + ";" + str + "\n";
        //payStr = str;
        if (str != "")
        {
            PayQueryStatus = true;   //开启查询状态
        }

        string dingdanID = str.Split(';')[0];
        string dingdanStatus = str.Split(';')[1];
        if (dingdanStatus == "SUCCESS") {
            string dingDanValue = Game_PublicClassVar.Get_function_Rose.DingDanReturnPayValue(dingdanID);
            //删除订单记录
            Game_PublicClassVar.Get_function_Rose.DeletePayID(dingdanID);
            if (dingDanValue != "0" && dingDanValue != "") {
                Game_PublicClassVar.Get_function_Rose.DingDanSendPayValue(dingDanValue);
            }
        }
    }

    //安装微信插件清空点击状态
    public void ClearnWeiXinPayStatus(string str)
    {
        Debug.Log("微信清空点击状态！，当前版本：" + str);
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>().buyStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>().ClearnPayValue();
    }


    public void testWeiTuoA()
    {
        Debug.Log("testWeiTuoBtestWeiTuoBtestWeiTuoA");
    }
    public void testWeiTuoB() {
        Debug.Log("testWeiTuoBtestWeiTuoBtestWeiTuoB");
    }
}
