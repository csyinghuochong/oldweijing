using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_NpcTask : MonoBehaviour {

    public string NpcID;                    //NpcID
    public ArrayList CompleteTaskID;        //Npc完成的任务
    private string[] npcTaskIDList;         //Npc携带的任务列表
    public GameObject Obj_NpcTaskSet;       
    private int creatNpcTask;               //创建UINpcTask的数量
    public GameObject Obj_NpcSpeak;         //Npc对话
    public bool UpdataStatus;               //更新状态,打开开关更新Npc携带的任务
	public GameObject Obj_Function;			//功能按钮
	private string npcType;
    public GameObject Obj_NpcName;          //Npc名称Obj
    public GameObject Obj_RoseStoreHouse;   //角色仓库Obj
    public GameObject Obj_EquipXiLi;        //装备洗炼

	// Use this for initialization
	void Start () {

        //显示Npc对话
        string npcSpeakText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpeakText", "ID", NpcID, "Npc_Template");
        string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", NpcID, "Npc_Template");
        Obj_NpcSpeak.GetComponent<Text>().text = "    "+npcSpeakText;
        Obj_NpcName.GetComponent<Text>().text = npcName;
        updataTask();

		//获取Npc类型
		npcType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("NpcType", "ID", NpcID, "Npc_Template");
        //npcType = "4";
		switch (npcType) {
            //商店类型NPC
			case "2":
				//Obj_Function.GetComponent<Text>().text = "购买物品";
				//Obj_Function.SetActive(true);
                UseFunction();
                Destroy(this.gameObject);
			break;
		    //仓库类型NPC
            case "3":
                UseFunction();
                Destroy(this.gameObject);
                //开启仓库状态
                Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = true;
            break;
            //装备洗练类型
            case "4":
                UseFunction();
                Destroy(this.gameObject);
                //开启仓库状态
                //Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus = true;
            break;
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        //更新Npc携带的完成任务
        if (UpdataStatus)
        {
            UpdataStatus = false;
            //循环清空任务列表下的控件
            for (int i = 0; i < Obj_NpcTaskSet.transform.childCount; i++)
            {
                GameObject go = Obj_NpcTaskSet.transform.GetChild(i).gameObject;
                Destroy(go);
            }

            //更新任务
            updataTask();
        }
	}

    void updataTask() {

        creatNpcTask = 0;

        if (NpcID != "0") {
            //获取NPC的任务
            npcTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskID", "ID", NpcID, "Npc_Template").Split(';');

            if (CompleteTaskID != null) {
                //循环创建完成任务
                foreach (string taskID in CompleteTaskID)
                {
                    //创建接取任务的Btn
                    GameObject getTaskUI = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
                    getTaskUI.transform.SetParent(Obj_NpcTaskSet.transform);
                    getTaskUI.transform.localPosition = new Vector3(0, creatNpcTask * -45, 0);
                    getTaskUI.transform.localScale = new Vector3(1, 1, 1);
                    getTaskUI.GetComponent<UI_NpcTaskList>().TaskID = taskID;
                    getTaskUI.GetComponent<UI_NpcTaskList>().TaskStatus = "4";      //4代表任务已接取已完成
                    creatNpcTask = creatNpcTask + 1;
                }
            }

            //循环创建任务列表
            for (int i = 0; i <= npcTaskIDList.Length - 1; i++)
            {
                if (npcTaskIDList[i] != "" && npcTaskIDList[i] != "0") {
                    string taskStatus = Game_PublicClassVar.Get_function_Task.TaskReturnStatus(npcTaskIDList[i]);
                    //实例化任务Btn
                    if (taskStatus == "1")
                    {
                        //创建接取任务的Btn
                        GameObject getTaskUI = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcTaskList);
                        getTaskUI.transform.SetParent(Obj_NpcTaskSet.transform);
                        getTaskUI.transform.localPosition = new Vector3(0, creatNpcTask * -45, 0);
                        getTaskUI.transform.localScale = new Vector3(1, 1, 1);
                        getTaskUI.GetComponent<UI_NpcTaskList>().TaskID = npcTaskIDList[i];
                        getTaskUI.GetComponent<UI_NpcTaskList>().TaskStatus = taskStatus;
                        creatNpcTask = creatNpcTask + 1;
                    }
                }
            }
        }
    }

    //关闭UI
    public void CloseUI() {
        Destroy(this.gameObject);
    }

	//使用Npc功能
	public void UseFunction(){

        if (Game_PublicClassVar.Get_game_PositionVar.roseOpenOnlyUI)
        {
            return;
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.roseOpenOnlyUI = true;
        }

		switch (npcType) {
        //商店
		case "2":
			GameObject npcStoreShow = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UINpcStoreShow);
			npcStoreShow.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
			npcStoreShow.transform.localPosition = new Vector3(-300, 5, 0);
			npcStoreShow.transform.localScale = new Vector3(1, 1, 1);
			npcStoreShow.GetComponent<UI_NpcStoreShow>().NpcID = NpcID;

		break;
        //仓库
        case "3":
            GameObject npcStoreHouse = (GameObject)Instantiate(Obj_RoseStoreHouse);
            npcStoreHouse.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            npcStoreHouse.transform.localPosition = new Vector3(-300, 5, 0);
            npcStoreHouse.transform.localScale = new Vector3(1, 1, 1);
            //npcStoreHouse.GetComponent<UI_NpcStoreShow>().NpcID = NpcID;
        break;
        //洗练
        case "4":
            GameObject npcXiLian = (GameObject)Instantiate(Obj_EquipXiLi);
            npcXiLian.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            npcXiLian.transform.localPosition = new Vector3(-300, 5, 0);
            npcXiLian.transform.localScale = new Vector3(1, 1, 1);
        break;
		}
	}

}
