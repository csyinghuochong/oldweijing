using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_EnterGame : MonoBehaviour {

    private AsyncOperation mAsyn;
    private bool enterGameStatus;
    public GameObject Obj_Loading;
    private GameObject obj_loading;
    private bool loadStatus;
    private float loadingSumTime;       //加载延迟显示时间
    private float loadingSumTimeSum;
    private UI_Set ui_set;
    public GameObject Obj_RoseEnterGameSet;     //点击创建角色时,隐藏进入游戏的对应按钮
    public GameObject Obj_CreateRoseSet;        //点击创建角色时,显示的创角相关界面
    public string SceneTransferID;
    public bool roseObjUpdataStatus;
    private bool enterBuildGameStatus;
    public GameObject[] KeepObj;
    private bool DestroyKeepObj;
    public GameObject Obj_UpdataDataText;
    public string EnterSceneID;
    private bool clearnStatus;          //清理UI状态

	// Use this for initialization
	void Start () {

        loadingSumTime = 0.5f;

        //开启选择角色状态
        Game_PublicClassVar.Get_game_PositionVar.EnterGameStatus = true;

        //隐藏主界面和任务血条
        if (Application.loadedLevelName != "StartGame") {
            ui_set = GameObject.FindWithTag("UI_Set").GetComponent<UI_Set>();
            ui_set.Obj_MainUI.SetActive(false);
            ui_set.Obj_UI_RoseHp.SetActive(false);
            //更新角色当前属性
            Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();
        }
	}
	
	// Update is called once per frame
	void Update () {

        //开启Loding状态
        if (enterGameStatus) {
            //Debug.Log("设置坐标点10");
            loadingSumTimeSum = loadingSumTimeSum + Time.deltaTime;
            //Debug.Log("loadingSumTimeSum = " + loadingSumTimeSum + "        loadingSumTime = " + loadingSumTime);
            if (loadingSumTimeSum >= loadingSumTime) {
                //Debug.Log("设置坐标点2");
                if (loadStatus) {
                    loadStatus = false;
                    loadingSumTimeSum = 0;      //确保不会执行第二次
                    StartCoroutine("Load");
                    //Debug.Log("设置坐标点1");
                }
            }
            if (mAsyn != null && !mAsyn.isDone)
            {
                //Debug.Log("设置坐标点2");
                float value = (float)mAsyn.progress/2.0f;
                //Debug.Log(value);
                if (value != 0) {
                    //Debug.Log("设置坐标点3");
                    obj_loading.GetComponent<UI_Loading>().LoadingValue = value + loadingSumTime+0.1f;
                    //Debug.Log("Loding:" + obj_loading.GetComponent<UI_Loading>().LoadingValue);
                    if (mAsyn.progress >= 0.9f) {
                        obj_loading.GetComponent<UI_Loading>().LoadingValue = 1;
                        mAsyn.allowSceneActivation = true;
                        //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.SetActive(false);
                        //测试
                        if (!roseObjUpdataStatus) {
                            //死亡进入地图时设置血量
                            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow <= 0) {
                                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp;
                                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";
                            }
                            //设置角色状态
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";

                            float transfer_X = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_X", "ID", SceneTransferID, "SceneTransfer_Template"));
                            float transfer_Y = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_Y", "ID", SceneTransferID, "SceneTransfer_Template"));
                            float transfer_Z = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapTransfer_Z", "ID", SceneTransferID, "SceneTransfer_Template"));
                            //Debug.Log("设置坐标点");
                            Debug.Log("坐标111:"+ Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position = new Vector3(transfer_X, transfer_Y, transfer_Z);
                            Debug.Log("坐标222:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                            if (!clearnStatus) {
                                ClearnUI();
                                clearnStatus = true;
                            }

                            //设定宠物位置
                            string ifPetChuZhan = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                            //Debug.Log("ifPetChuZhan = " + ifPetChuZhan);
                            if (ifPetChuZhan == "1")
                            {
                                //设置宠物位置
                                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj != null)
                                {
                                    //设置宠物坐标
                                    Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                                    for (int i = 0; i <= roseStatus.RosePetObj.Length-1; i++)
                                    {
                                        GameObject go = roseStatus.RosePetObj[i];
                                        if (go != null) {
                                            go.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                                            go.GetComponent<AIPet>().AI_Hp_Status = false;
                                            go.SetActive(false);
                                            go.SetActive(true);
                                            go.GetComponent<AIPet>().updateStarMoveStatus = true;
                                            /*
                                            //设置自身离寻路最近的坐标点,要不离地面检测区太远会报错
                                            UnityEngine.AI.NavMeshHit hit;
                                            UnityEngine.AI.NavMesh.SamplePosition(go.transform.position, out hit, 10.0f, 1);
                                            try
                                            {
                                                go.GetComponent<AIPet>().ai_NavMesh.Warp(hit.position);
                                                //go.transform.position.y = hit.position.y;
                                                Vector3 selfVec3 = go.transform.position;
                                                selfVec3.y = hit.position.y;
                                                go.GetComponent<AIPet>().ai_NavMesh.SetDestination(selfVec3);
                                                //go.GetComponent<AIPet>().ai_NavMesh.SetDestination(go.transform.position);
                                                //设置AI的初始坐标，要不返回时会报错
                                                //ai_StarPosition = this.transform.position;

                                            }
                                            catch
                                            {
                                                //Debug.Log("移动报错！怪物ID：" + AI_ID + "name = " + AI_Name);
                                            }
                                            */
                                        }
                                    }

                                    /*
                                    GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet;
                                    //循环删除宠物
                                    for (int i = 0; i < monsterSetObj.transform.childCount; i++)
                                    {
                                        GameObject go = monsterSetObj.transform.GetChild(i).gameObject;
                                        go.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                                        go.GetComponent<AIPet>().AI_Hp_Status = false;
 
                                    }
                                    */

                                    //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                                    //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj.GetComponent<AIPet>().AI_Hp_Status = false;
                                }
                            }


                            //更新主界面经验显示
                            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseExp.GetComponent<UI_MainUIRoseExp>().UpdataRoseExp = true;
                        }
                    }
                }
            }
            else {
                if (loadingSumTimeSum < loadingSumTime)         //加判定是为了防止在最后一帧执行这一步,结果会是0.66
                {
                    obj_loading.GetComponent<UI_Loading>().LoadingValue = loadingSumTimeSum;
                }
            }

            //下载完毕关闭进入开关
            if (mAsyn != null) {
                if (mAsyn.isDone)
                {
                    Debug.Log("坐标333:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    enterGameStatus = false;
                    Destroy(obj_loading,1);     //延迟1秒关闭,防止穿帮
                    Game_PublicClassVar.Get_game_PositionVar.EnterScenceStatus = true;
                                    //更新场景名称
                    string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName","ID",Application.loadedLevelName,"Scene_Template");
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MapName.GetComponent<Text>().text = sceneName;
                    //Game_PublicClassVar.Get_game_PositionVar.EnterScenceID = EnterSceneID;       //设置地图ID
                    //设置怪物
                    Game_PublicClassVar.Get_game_PositionVar.MonsterSet = GameObject.Find("Monster");
                    clearnStatus = false;       //重置清空状态

                    //设置难度
                    switch(Game_PublicClassVar.Get_game_PositionVar.GameNanduValue){

                        case "0":
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:普通";
                            break;

                        case "1":
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:普通";
                            break;

                        case "2":
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:挑战";
                            break;

                        case "3":
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UIGameNanDu.GetComponent<Text>().text = "游戏模式:地狱";
                            break;
                    }

                    Debug.Log("坐标444:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                }
            }
        }
        //Debug.Log("Loding:"+obj_loading.GetComponent<UI_Loading>().LoadingValue);

        if (enterBuildGameStatus) {
            loadingSumTimeSum = loadingSumTimeSum + Time.deltaTime/1;
            if (loadingSumTimeSum > 1) {
                loadingSumTimeSum = 1;
            }
            //设置进度条的值,如果不加0.2f 加载条读不到100% 会提前加载完成
            float loadingProValue = loadingSumTimeSum + 0.2f;
            if (loadingProValue >= 1) {
                loadingProValue = 1;
            }
            obj_loading.GetComponent<UI_Loading>().LoadingValue = loadingProValue;

            //加载完成延迟1秒注销Loding
            if (loadingSumTimeSum>=1)
            {
                //Destroy(obj_loading, 1);     //延迟1秒关闭,防止穿帮
                enterBuildGameStatus = false;
                DestroyKeepObj = true;
                //Application.LoadLevel("EnterGame"); //加载场景
                SceneManager.LoadScene("EnterGame");
            }
        }
	}

    void LateUpdate()
    {
        if (DestroyKeepObj)
        {
            DestroyKeepObj = false;
            //保证切换场景以下预设体不消失
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++)
            {
                Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i - 1]);
            }
        }
    }

    //进入建筑界面
    public void EnterBuildGame() {
        
        //实例化一个Loding界面
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        obj_loading = (GameObject)Instantiate(Obj_Loading);
        //更换Loding背景图
        if (mapName == "EnterGame")
        {
            object obj = Resources.Load("GameUI/Back/LodingBack_2", typeof(Sprite));
            Sprite itemIcon = obj as Sprite;
            obj_loading.GetComponent<UI_Loading>().Obj_ImgBack.GetComponent<Image>().sprite = itemIcon;
        }
        obj_loading.transform.SetParent(this.transform);
        obj_loading.transform.localScale = new Vector3(1, 1, 1);
        obj_loading.transform.localPosition = new Vector3(0, 0, 0);
        enterBuildGameStatus = true;
        //Application.LoadLevel("EnterGame"); //加载场景
        //DontDestroyOnLoad(this.gameObject);
        loadingSumTimeSum = 0;
        EnterSceneID = mapName;
    }

    //进入游戏
    public void EnterGame() {
        if (Obj_UpdataDataText != null) {
            Obj_UpdataDataText.SetActive(false);
        }
        
        mAsyn = null;
        enterGameStatus = true;
        //加载Loding的UI界面
        //Debug.Log("RoseID22222 = " + Game_PublicClassVar.Get_wwwSet.RoseID);
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Debug.Log("mapName = " + mapName);
        obj_loading = (GameObject)Instantiate(Obj_Loading);
        //更换Loding背景图
        if (mapName == "EnterGame") { 
          	object obj = Resources.Load("GameUI/Back/LodingBack_2", typeof(Sprite));
		    Sprite itemIcon = obj as Sprite;
            obj_loading.GetComponent<UI_Loading>().Obj_ImgBack.GetComponent<Image>().sprite = itemIcon;

            //隐藏UI
            /*
            ui_set.Obj_MainFunctionUI.SetActive(false);
            ui_set.Obj_MapName.SetActive(false);
            ui_set.Obj_RoseTask.SetActive(false);
            */
        }
        obj_loading.transform.SetParent(this.transform);
        obj_loading.transform.localScale = new Vector3(1, 1, 1);
        obj_loading.transform.localPosition = new Vector3(0, 0, 0);
        loadStatus = true;
        this.transform.Find("EnterGameSet").gameObject.SetActive(false);
        //清空UI
        if (ui_set != null) {
            ui_set.Obj_BuildingMainUISet.SetActive(false);        //隐藏建筑UI
            ui_set.Obj_BuildingNameSet.SetActive(false);        //隐藏建筑名称UI
            ui_set.Obj_HuoBiSet.SetActive(false);               //隐藏货币
            ui_set.Obj_MainFunctionUI.SetActive(true);
            ui_set.Obj_UIMapName.SetActive(true);
            ui_set.Obj_RoseTask.SetActive(true);
            ui_set.Obj_RightDownSet.SetActive(false);
        }

        //Debug.Log("Game_PublicClassVar.Get_wwwSet.RoseID = " + Game_PublicClassVar.Get_wwwSet.RoseID);
        //Debug.Log("mapName = " + mapName);

        EnterSceneID = mapName;
        if (mapName != "EnterGame")
        {
            //清理UI
            ClearnUI();
        }
    }
    
    IEnumerator Load()
    {
        //获取角色当前进入地图
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string mapPositionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapPositionName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_game_PositionVar.EnterScencePositionName = mapPositionName;
        //yield return new WaitForEndOfFrame();
        //mAsyn = Application.LoadLevelAsync(mapName);
        mAsyn = SceneManager.LoadSceneAsync(mapName);
        mAsyn.allowSceneActivation = false;
        //进入游戏界面恢复主界面正常显示
        Game_PublicClassVar.Get_game_PositionVar.EnterGameStatus = false;
        if (ui_set != null) {
            ui_set.Obj_MainUI.SetActive(true);
            ui_set.Obj_UI_RoseHp.SetActive(true);
        }
        //Debug.Log("开始载入游戏地图");
        yield return mAsyn;
    }


    //创建角色
    public void CreateRoseID() { 
        //判定是否输入角色名称
        Obj_RoseEnterGameSet.SetActive(false);
        Obj_CreateRoseSet.SetActive(true);
    }

    //清空UI
    public void ClearnUI() {
        //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        //清空名字显示
        GameObject npcNameObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet;
        for (int i = 0; i < npcNameObj.transform.childCount; i++)
        {
            GameObject go = npcNameObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        //清空血条显示
        GameObject aiHpObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet;
        for (int i = 0; i < aiHpObj.transform.childCount; i++)
        {
            GameObject go = aiHpObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        //清空游戏声音
        GameObject gameSourceSet = Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet;
        for (int i = 0; i < gameSourceSet.transform.childCount; i++)
        {
            GameObject go = gameSourceSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        //清空掉落显示
        GameObject dropItemSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_DropItemSet;
        for (int i = 0; i < dropItemSet.transform.childCount; i++)
        {
            GameObject go = dropItemSet.transform.GetChild(i).gameObject;
            Destroy(go);
            //Debug.Log("清空掉落");
        }
        //清空名称显示
        GameObject aiHpSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet;
        for (int i = 0; i < aiHpSet.transform.childCount; i++)
        {
            GameObject go = aiHpSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        
        //清空任务显示
        GameObject npcTaskSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet;
        for (int i = 0; i < npcTaskSet.transform.childCount; i++)
        {
            GameObject go = npcTaskSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //重置摇杆状态
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_YaoGan.GetComponent<UI_YaoGan>().Exit();
    }

}
