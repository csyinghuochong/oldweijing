using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_NpcGetTask : MonoBehaviour {

    public string TaskID;
    public GameObject Obj_TaskDes;
    public GameObject Obj_TaskName;
    public GameObject Obj_TaskReward;
    public GameObject UI_ItemTrophySet;				//任务道具奖励父级
    public GameObject Obj_UI_TaskTrophyList;		//任务道具奖励显示

	// Use this for initialization
	void Start () {
        //获取任务名称
        string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", TaskID, "Task_Template");
        Obj_TaskName.GetComponent<Text>().text = taskName;

        //显示任务描述
        string taskDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskDes", "ID", TaskID, "Task_Template");
        Obj_TaskDes.GetComponent<Text>().text =taskDes;

        //获取任务奖励
        string expValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskExp", "ID", TaskID, "Task_Template");
        string moneyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskCoin", "ID", TaskID, "Task_Template");
        string rewardText = "";
        if (expValue != "")
        {
            rewardText = "经验+" + expValue + "\n";
        }
        if (moneyValue != "")
        {
            rewardText = rewardText + "金币+" + moneyValue;
        }

        Obj_TaskReward.GetComponent<Text>().text = rewardText;


        //更新道具奖励的显示
        string[] taskItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", TaskID, "Task_Template").Split(',');
        string[] taskItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", TaskID, "Task_Template").Split(',');

        //循环创建
        for (int i = 0; i <= taskItemID.Length - 1; i++)
        {
            if (taskItemID[i] != "" && taskItemID[i] != "0")
            {
                GameObject taskItemObj = (GameObject)Instantiate(Obj_UI_TaskTrophyList);
                taskItemObj.transform.SetParent(UI_ItemTrophySet.transform);
                taskItemObj.transform.localScale = new Vector3(1, 1, 1);
                taskItemObj.transform.localPosition = new Vector3(i * 70, 0, 0);
                taskItemObj.GetComponent<UI_TaskTrophyList>().ItemID = taskItemID[i];
                taskItemObj.GetComponent<UI_TaskTrophyList>().ItemNum = taskItemNum[i];
            }
        }



	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void GetTask() {
        //领取任务,领取成功关闭UI
        if (Game_PublicClassVar.Get_function_Task.GetTask(TaskID)) {
            Destroy(this.gameObject);   //关闭界面
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "1";        //更新Npc携带的任务
        }


    }

    public void CompleteTask() {
        //完成任务
        if (Game_PublicClassVar.Get_function_Task.TaskRewards(TaskID))
        {
            //写入完成任务
            if (Game_PublicClassVar.Get_function_Task.TaskWriteRoseData(TaskID)) {
                Destroy(this.gameObject,0.1f);   //关闭界面(0.1的延迟是关闭时，上一级的界面还没重新生成好,需要加一个小延迟时间遮挡一下)
                Game_PublicClassVar.Get_game_PositionVar.NpcTaskUpdataStatus = "1";        //更新Npc携带的任务
            }
            
        }
    }
}