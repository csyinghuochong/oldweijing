using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_NpcTaskList : MonoBehaviour {

    public string TaskID;
    public string TaskStatus;       //外界传入的值,1:未领取 2：已完成 3：已领取 未完成 4: 已领取 已完成
    public GameObject Obj_TaskName;
    public GameObject Obj_TaskGetStatusIcon;   //任务接取图标
    public GameObject Obj_TaskComplateStatusIcon;   //任务完成图标
    private GameObject npcGetTask;

	// Use this for initialization
	void Start () {

        if (TaskStatus == "4")
        {
            //显示已完成的标记
            Obj_TaskGetStatusIcon.SetActive(false);
            Obj_TaskComplateStatusIcon.SetActive(true);
        }
        else {
            Obj_TaskGetStatusIcon.SetActive(true);
            Obj_TaskComplateStatusIcon.SetActive(false);
        }

        //获取任务名称并显示
        string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", TaskID, "Task_Template");
        Obj_TaskName.GetComponent<Text>().text = taskName;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Btn_TaskList(){

        if (npcGetTask != null) {
            return;
        }

        //接取任务
        if (TaskStatus == "1") {
            //打开接取任务界面
            npcGetTask = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcGetTask);
            npcGetTask.transform.SetParent(this.transform.parent.transform.parent);
            npcGetTask.transform.localPosition = new Vector3(1, 1, 1);
            npcGetTask.transform.localScale = new Vector3(1, 1, 1);
            npcGetTask.GetComponent<UI_NpcGetTask>().TaskID = TaskID;
        }

        //完成任务
        if (TaskStatus == "4"){
            //打开接取任务界面
            npcGetTask = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_NpcCompleteTask);
            npcGetTask.transform.SetParent(this.transform.parent.transform.parent);
            npcGetTask.transform.localPosition = new Vector3(1, 1, 1);
            npcGetTask.transform.localScale = new Vector3(1, 1, 1);
            npcGetTask.GetComponent<UI_NpcGetTask>().TaskID = TaskID;
        }
    }
}
