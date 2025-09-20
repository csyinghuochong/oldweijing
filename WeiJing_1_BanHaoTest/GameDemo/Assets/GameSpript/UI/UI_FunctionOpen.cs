using UnityEngine;
using System.Collections;

public class UI_FunctionOpen : MonoBehaviour {

	public Transform UISet;                 //UI生成的绑点
	
	public bool RoseBag_Status;             //角色背包打开状态
	public GameObject Obj_roseBag;          //实例化的背包
    public GameObject Obj_Bag;              //背包

	public bool RoseEquip_Status;             //角色装备打开状态
	public GameObject Obj_roseEquip;          //实例化的装备
	public GameObject Obj_Equip;              //装备

	public bool RoseTask_Status;             //角色任务打开状态
	public GameObject Obj_roseTask;          //实例化的任务
	public GameObject Obj_Task;              //任务

	public bool RoseSkill_Status;             //角色任务打开状态
	public GameObject Obj_roseSkill;          //实例化的任务
	public GameObject Obj_Skill;              //任务

    public bool RoseRmbStore_Status;        //人民币商城
    public GameObject Obj_RomStore;

    public bool RoseFenXiang_Status;        //分享界面
    public GameObject Obj_roseFenXiang;
    public GameObject Obj_FenXiang;

    public bool RoseHuoDongDaTing_Status;   //活动大厅状态
    public GameObject Obj_HuoDongDaTing;    //活动大厅
    private GameObject obj_RoseHuoDongDaTing;   //活动大厅实例化
    
    public bool RoseTodayGift_Status;   //今日礼包状态
    public GameObject Obj_TodayGift;    //今日礼包
    private GameObject obj_RoseTodayGift;   //今日礼包实例化

    public bool NewGame_Status;   //今日礼包状态
    public GameObject Obj_NewGame;    //今日礼包
    private GameObject obj_NewGame;   //今日礼包实例化

    public bool RoseMap_Status;             //小地图打开状态
    public GameObject Obj_Map;              //小地图源
    private GameObject obj_Map;             //小地图实例化

	// Use this for initialization
	void Start () {
        //显示功能区域按钮是否隐藏
        if (Game_PublicClassVar.Get_wwwSet.IfHindMainBtn)
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //打开角色背包
    public void OpenBag() {
		//当前背包是否打开
		if (!RoseBag_Status)
		{
            if (Obj_roseBag == null) {
                //载入背包UI
                RoseBag_Status = true;
                Obj_roseBag = (GameObject)Instantiate(Obj_Bag);
                Obj_roseBag.transform.SetParent(UISet);
                Obj_roseBag.transform.localScale = new Vector3(1, 1, 1);
                Obj_roseBag.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(340f, 5, 0);
                playUISource_Open();    //播放音效
            }

		}
		else {
			//注销背包UI
			RoseBag_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseBag);
		}
    }

	public void OpenRoseEquip(){
		//当前角色装备是否打开
		if (!RoseEquip_Status)
		{
			//载入背包UI
			RoseEquip_Status = true;
			Obj_roseEquip = (GameObject)Instantiate(Obj_Equip);
			Obj_roseEquip.transform.SetParent(UISet);
			Obj_roseEquip.transform.localScale = new Vector3(1,1,1);
			Obj_roseEquip.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-500f, 5, 0);
            playUISource_Open();    //播放音效
            //显示怪物模型
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.SetActive(true);
		}
		else {
			//注销背包UI
			RoseEquip_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseEquip);
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.SetActive(false);
		}
	}

	public void OpenRoseTask(){
		//当前角色装备是否打开
		if (!RoseTask_Status)
		{
			//载入背包UI
			RoseTask_Status = true;
			Obj_roseTask = (GameObject)Instantiate(Obj_Task);
			Obj_roseTask.transform.SetParent(UISet);
			Obj_roseTask.transform.localScale = new Vector3(1,1,1);
			Obj_roseTask.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
		}
		else {
			//注销背包UI
			RoseTask_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseTask);
		}
	}

	public void OpenRoseSkill(){

		//当前角色装备是否打开
		if (!RoseSkill_Status)
		{
			//载入背包UI
			RoseSkill_Status = true;
			Obj_roseSkill = (GameObject)Instantiate(Obj_Skill);
			Obj_roseSkill.transform.SetParent(UISet);
			Obj_roseSkill.transform.localScale = new Vector3(1,1,1);
			Obj_roseSkill.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 5, 0);
            playUISource_Open();    //播放音效
		}
		else {
			//注销背包UI
			RoseSkill_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseSkill);
		}
	}

    //自动战斗
    public void AutomaticFight() {

        //角色状态
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        if (Game_PublicClassVar.Get_game_PositionVar.YaoGanStatus) {
            if (roseStatus.YaoGanStopMoveTime > 0)
            {
                return;
            }
        }

        //死亡状态不能触发按钮
        if (roseStatus != null) {
            if (roseStatus.RoseDeathStatus)
            {
                return;
            }
        }

        //获取当前怪物的最近距离
        //Debug.Log("asdasdasdasd_11111");
        if (roseStatus.NextAutomaticMonsterDis <= 15.0f)
        {
            //Debug.Log("asdasdasdasd_22222");
            //获取角色当前状态
            if (roseStatus.RoseStatus != "3") {
                //Debug.Log("asdasdasdasd_33333");
                //获取自动战斗是否开启
                if (roseStatus.ifAutomatic == false)
                {
                    roseStatus.ifAutomatic = true;
                    //Debug.Log("asdasdasdasd44444");
                }
            }
        }
        else {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("附近没有怪物！");
        }
    }



    //自动拾取
    public void AutomaticTake() {

        //角色状态
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        //死亡状态不能触发按钮
        if (roseStatus != null)
        {
            if (roseStatus.RoseDeathStatus)
            {
                return;
            }
        }

        //死亡状态不能触发按钮
        if (roseStatus != null)
        {
            if (roseStatus.RoseDeathStatus)
            {
                return;
            }
        }
        
        //获取当前怪物的最近距离
        if (roseStatus.NextAutomaticDropDis <= 15.0f)
        {
            //获取自动战斗是否开启
            if (roseStatus.IfAutomaticTake == false)
            {
                roseStatus.IfAutomaticTake = true;
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("附近没有可拾取的道具！");
        }
    }

    //打开商城
    public void Open_RmbStore()
    {
        if (RoseRmbStore_Status)
        {
            Obj_RomStore.SetActive(false);
            RoseRmbStore_Status = false;
        }
        else {
            Obj_RomStore.SetActive(true);
            RoseRmbStore_Status = true;
        }
    }

    //打开商城分享
    public void Open_FenXiang()
    {
        Debug.Log("我点击了分享按钮");
        if (!RoseFenXiang_Status)
        {
            //载入背包UI
            RoseFenXiang_Status = true;
            Obj_roseFenXiang = (GameObject)Instantiate(Obj_FenXiang);
            Debug.Log("我点击了分享按钮1111");
            Obj_roseFenXiang.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
            Obj_roseFenXiang.transform.localScale = new Vector3(1, 1, 1);
            Obj_roseFenXiang.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            Obj_roseFenXiang.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
            Obj_roseFenXiang.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            playUISource_Open();    //播放音效

            //Obj_FenXiang.SetActive(false);
            //RoseFenXiang_Status = false;
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseFenXiang);
            RoseFenXiang_Status = false;
        }
    }

    //打开活动大厅
    public void Open_HuoDongDaTing() {
        if (!RoseHuoDongDaTing_Status)
        {
            //载入背包UI
            RoseHuoDongDaTing_Status = true;
            obj_RoseHuoDongDaTing = (GameObject)Instantiate(Obj_HuoDongDaTing);
            Debug.Log("我点击了分享按钮1111");
            obj_RoseHuoDongDaTing.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
            obj_RoseHuoDongDaTing.transform.localScale = new Vector3(1, 1, 1);
            obj_RoseHuoDongDaTing.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            obj_RoseHuoDongDaTing.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
            obj_RoseHuoDongDaTing.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            playUISource_Open();    //播放音效
        }
        else {
            playUISource_Close();   //播放音效
            Destroy(obj_RoseHuoDongDaTing);
            RoseHuoDongDaTing_Status = false;
        }
    }

    // 今日礼包 看广告
    public void Open_TodayGift()
    {
	    if (!RoseTodayGift_Status)
	    {
		    //载入背包UI
		    RoseTodayGift_Status = true;
		    obj_RoseTodayGift = (GameObject)Instantiate(Obj_TodayGift);
		    Debug.Log("我点击了分享按钮1111");
		    obj_RoseTodayGift.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
		    obj_RoseTodayGift.transform.localScale = new Vector3(1, 1, 1);
		    obj_RoseTodayGift.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
		    obj_RoseTodayGift.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
		    obj_RoseTodayGift.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
		    playUISource_Open();    //播放音效
	    }
	    else {
		    playUISource_Close();   //播放音效
		    Destroy(obj_RoseTodayGift);
		    RoseTodayGift_Status = false;
	    }
    }

    // 前往新游戏
    public void Open_GotoNewGame()
    {
        if (!NewGame_Status)
        {
            //载入背包UI
            NewGame_Status = true;
            obj_NewGame = (GameObject)Instantiate(Obj_NewGame);
            Debug.Log("我点击了新游戏");
            obj_NewGame.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet_2.transform);
            obj_NewGame.transform.localScale = new Vector3(1, 1, 1);
            obj_NewGame.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            obj_NewGame.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
            obj_NewGame.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
            playUISource_Open();    //播放音效
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(obj_NewGame);
            NewGame_Status = false;
        }
    }

    //隐藏功能按钮
    public void HindFunctionBtn() {

        if (!Game_PublicClassVar.Get_wwwSet.IfHindMainBtn)
        {
            Game_PublicClassVar.Get_wwwSet.IfHindMainBtn = true;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuo.transform.Find("Img_ShouSuo").GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else {
            Game_PublicClassVar.Get_wwwSet.IfHindMainBtn = false;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuo.transform.Find("Img_ShouSuo").GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    //打开地图
    public void Open_Map()
    {
        if (!RoseMap_Status)
        {
            //载入地图UI
            RoseMap_Status = true;
            obj_Map = (GameObject)Instantiate(Obj_Map);
            obj_Map.transform.SetParent(UISet);
            obj_Map.transform.localScale = new Vector3(1, 1, 1);
            obj_Map.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(obj_Map);
            RoseMap_Status = false;
        }
    }

    //打开
    void playUISource_Open() {
        Game_PublicClassVar.Get_function_UI.PlaySource("10001", "1");
    }

    //关闭
    void playUISource_Close()
    {
        Game_PublicClassVar.Get_function_UI.PlaySource("10002", "1");
    }

}
