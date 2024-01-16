using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AI_NPC : MonoBehaviour {

    public string NpcID;
    private Vector2 vec3_NpcName;
    public GameObject NpcNamePosition;
    private GameObject Obj_NpcName;
    private bool npcNameUpdateStatus;
    public GameObject SelectEffect;             //玩家选中这个NPC播放特效
    public GameObject SelectEffectPosition;     //玩家选中特效播放的点
    public bool IfSeclectNpc;                   //玩家是否选中这个Npc
    public bool IfSeclectNpcEffect;             //玩家选中这个Npc实例化的特效，保证只执行一次
    private GameObject selectEffect;            //实例化的选中特效
    public ArrayList CompleteTaskID;            //完成任务的ID
    public bool CompleteTaskStatus;             //完成任务只执行一次
    public bool waritUpdataTask;               //待更新状态
    private float taskUpdataTime;               //30码内每隔几秒自动是否有任务完成,修复30码内，杀怪任务或其他任务完成后不更新（不得已的办法）
    private float taskUpdataTimeSum;
    private bool npcHeadSpeakStatus;            //NPC头上说话状态

    private string[] npcStoryValue;             //NPC身上绑定的故事ID

	// Use this for initialization
	void Start () {
        npcNameUpdateStatus = true;
        taskUpdataTime = 5.0f;

        //获取NPC自身的故事绑定值
        //string aa = "2;3";
        //npcStoryValue = aa.Split(';');

	}
	
	// Update is called once per frame
	void Update () {

        //判定与角色相距17米进行名称显示
        float distance = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, this.gameObject.transform.position);
        //Debug.Log("distance = " + distance);
        if (distance <= 17.0f)
        {
            //修正物体在屏幕中的位置
            vec3_NpcName = Camera.main.WorldToViewportPoint(NpcNamePosition.transform.position);
            vec3_NpcName = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_NpcName);

            //第一次显示
            if (npcNameUpdateStatus == true)
            {

                npcNameUpdateStatus = false;
                //实例化UI
                Obj_NpcName = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UINpcName);
                Obj_NpcName.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet.transform);
                Obj_NpcName.transform.localPosition = new Vector3(vec3_NpcName.x, vec3_NpcName.y, 0);
                Obj_NpcName.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                UI_NPCName npcNameScri = Obj_NpcName.GetComponent<UI_NPCName>();
                //显示Npc名称
                string NpcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", NpcID, "Npc_Template");
                //Obj_NpcName.transform.Find("Lab_NpcName").transform.GetComponent<Text>().text = NpcName;
                npcNameScri.Obj_NpcName.transform.GetComponent<Text>().text = NpcName;
                npcNameScri.NpcID = NpcID;
                //有街区任务显示图标
                string[] npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');
                //循环创建任务列表
                for (int i = 0; i <= npcTaskIDList.Length - 1; i++)
                {
                    if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0")
                    {
                        string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                        //实例化任务Btn
                        if (taskStatus == "1")
                        {
                            //显示接取任务
                            //Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(true);
                            //Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(false);

                            npcNameScri.Obj_TaskGet.gameObject.SetActive(true);
                            npcNameScri.Obj_TaskComplete.gameObject.SetActive(true);
                        }
                    }
                }
            }
            //修正掉落物体的位置
            if (Obj_NpcName != null) {
                Obj_NpcName.transform.localPosition = new Vector3(vec3_NpcName.x, vec3_NpcName.y, 0);
            }
            
            //修正Npc头顶的任务状态
            if (Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow)
            {
                //有接取任务显示图标
                string[] npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');
                if (npcTaskIDList[0] != "0") {

                    waritUpdataTask = false;
                    //显示接取任务
                    UI_NPCName npcNameScri = Obj_NpcName.GetComponent<UI_NPCName>();
                    npcNameScri.Obj_TaskGet.gameObject.SetActive(false);
                    npcNameScri.Obj_TaskComplete.gameObject.SetActive(false);

                    //循环创建任务列表
                    for (int i = 0; i <= npcTaskIDList.Length - 1; i++)
                    {
                        if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0")
                        {
                            string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                            //实例化任务Btn
                            if (taskStatus == "1")
                            {
                                //显示接取任务
                                npcNameScri.Obj_TaskGet.gameObject.SetActive(true);
                                npcNameScri.Obj_TaskComplete.gameObject.SetActive(false);
                            }
                        }
                    }

                    createCompleteTask();
                }
            }

            //显示快捷对话
            if (distance <= 5.0f)
            {
                if (!npcHeadSpeakStatus)
                {
                    //Debug.Log("头顶说话");
                    if (Obj_NpcName != null) {
                        npcHeadSpeakStatus = true;
                        Obj_NpcName.GetComponent<UI_NPCName>().HeadSpeakStatus = true;
                    }
                }
            }
            else {

                //Obj_NpcName.GetComponent<UI_NPCName>().Obj_HeadSpeak.active = false;
                
            }

            //当接受到更新完成任务状态时,先保存，等进入NPC范围在更新8

            if (waritUpdataTask)
            {
                waritUpdataTask = false;
                createCompleteTask();
            }


            //判定主角是否有任务已经完成,交任务的NPC是否为自己
            if (!CompleteTaskStatus)
            {
                //确保只执行一次
                CompleteTaskStatus = true;
                createCompleteTask();

            }

            //5秒检测附近玩家有没有任务
            /*
            taskUpdataTimeSum = taskUpdataTimeSum +Time.deltaTime;
            if (taskUpdataTimeSum >= taskUpdataTime) {
                taskUpdataTimeSum = 0;
                createCompleteTask();
            }
            */
        }
        else {
            waritUpdataTask = true;
            npcHeadSpeakStatus = false;  //npc头顶说话
            
            if (Obj_NpcName != null) {
                Destroy(Obj_NpcName);
            }
            npcNameUpdateStatus = true;
            
        }

        //NPC被玩家选中
        if (IfSeclectNpc)
        {
            if (!IfSeclectNpcEffect)
            {
                //实例化一个选中特效
                selectEffect = (GameObject)Instantiate(SelectEffect);
                selectEffect.transform.SetParent(SelectEffectPosition.transform);
                selectEffect.transform.localScale = new Vector3(1, 1, 1);
                selectEffect.transform.localPosition = new Vector3(0, 0, 0);
                //Debug.Log("SelectEffect = " + SelectEffect.name);
                selectEffect.GetComponent<Rose_SelectTarget>().SelectEffectSize = 1;
                IfSeclectNpcEffect = true;
            }

            //NPC触发剧情对话
            if (distance <= 2.0f) { 
            
                //镜头拉伸

            }
        }
        else {
            Destroy(selectEffect);
            IfSeclectNpcEffect = false;
        }

        //更新完成任务列表
        if (Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus=="1") {
            createCompleteTask();
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "2";
        }

        //修正Npc头顶的任务状态
        
        if (Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow) {
            waritUpdataTask = true;
        }
        
	}

    //创建完成任务
    void createCompleteTask() {

        //获取主角当前携带的任务
        string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        CompleteTaskID = new ArrayList();
        for (int i = 0; i <= taskIDList.Length - 1; i++)
        {
            if(taskIDList[i]!="" || taskIDList[i]!="0"){
                //判定当前任务是否完成
                if (Game_PublicClassVar.Get_function_Task.TaskComplete(taskIDList[i]))
                {
                    //获取当前交任务的NPC是否是自己
                    string completeNpcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", taskIDList[i], "Task_Template");
                    if (completeNpcID == NpcID)
                    {
                        //Debug.Log("这个NPC有完成任务");
                        CompleteTaskID.Add(taskIDList[i]);
                        //显示交任务图标
                        Obj_NpcName.transform.Find("Img_TaskGet").gameObject.SetActive(false);
                        Obj_NpcName.transform.Find("Img_TaskComplete").gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
