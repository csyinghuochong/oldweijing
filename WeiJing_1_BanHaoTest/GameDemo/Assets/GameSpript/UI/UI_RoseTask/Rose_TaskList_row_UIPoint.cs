using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Rose_TaskList_row_UIPoint : MonoBehaviour {

    public GameObject UI_TaskName;
    public GameObject UI_TaskLv;
    public GameObject UI_TaskIfFinish;
    public GameObject UIImg_SelectStatus;

    public string UI_TaskID;

	// Use this for initialization
	void Start () {

        //判定当前任务目标是否已达成

        bool taskStatus = Game_PublicClassVar.Get_function_Task.TaskComplete(UI_TaskID);

        if (taskStatus)
        {

            UI_TaskIfFinish.SetActive(true);

        }
        else
        {

            UI_TaskIfFinish.SetActive(false);

        }

	}
	
	// Update is called once per frame
	void Update () {



	}

    public void UI_SelectTask() {
        
            //更改任务名称的颜色及显示选中的任务
            UI_TaskLv.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            UI_TaskName.GetComponent<Text>().color = new Color(1, 1, 1, 1);
            UIImg_SelectStatus.SetActive(true);

            //修改当前选中的任务ID
            Game_PublicClassVar.Get_game_PositionVar.NowTaskID = UI_TaskID;
            //Debug.Log("当前任务ID" + UI_TaskID);

            //更新任务日志列表
            Game_PublicClassVar.Get_game_PositionVar.Rose_TaskListUpdata = true;
            //更新任务信息
            Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata = true;

            //显示当前UI

            //判定当前任务目标是否已达成
        
            bool taskStatus = Game_PublicClassVar.Get_function_Task.TaskComplete(UI_TaskID);
            if (taskStatus)
            {

                UI_TaskIfFinish.SetActive(true);

            }
            else {

                UI_TaskIfFinish.SetActive(false);
        
            }
            
    }


}
