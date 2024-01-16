using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Rose_TaskList_TaskShow : MonoBehaviour {

    //任务描述控件
    public GameObject UI_TaskDescribe;
    //任务目标控件
    public GameObject UI_TaskTarget_1;
    public GameObject UI_TaskTarget_2;
    public GameObject UI_TaskTarget_3;

    //任务奖励
    public GameObject UI_TaskTrophy_Exp;
    public GameObject UI_TaskTrophy_Money;

	//追踪任务按钮
	public GameObject UI_Btn_MainTaskWrite;
	public GameObject UI_Btn_MainTaskDelete;

	public GameObject UI_ItemTrophySet;				//任务道具奖励父级
	public GameObject Obj_UI_TaskTrophyList;		//任务道具奖励显示

	// Use this for initialization
	void Start () {

        string[] achievementTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        if (achievementTaskID[0] != "")
        {
            Game_PublicClassVar.Get_game_PositionVar.NowTaskID = achievementTaskID[0];
        }
        updateTaskData();

	}
	
	// Update is called once per frame
	void Update () {
    
        if (Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata) {
            
            updateTaskData();

            Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata = false;

            Debug.Log("任务信息刷新");
        }
	}

    private void updateTaskData() {

		//获取当前任务第一个任务,作为当前选中任务
		if (Game_PublicClassVar.Get_game_PositionVar.NowTaskID == "") {
			string[] achievementTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
			if (achievementTaskID [0] != "") {
				Game_PublicClassVar.Get_game_PositionVar.NowTaskID=achievementTaskID [0];
			}
		}
		string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;

		//显示追踪任务按钮
        /*
		if (Game_PublicClassVar.Get_function_Task.IfMainUITask(nowTaskID)) {
			UI_Btn_MainTaskWrite.SetActive (false);
			UI_Btn_MainTaskDelete.SetActive (true);
		} else {
			UI_Btn_MainTaskWrite.SetActive(true);
			UI_Btn_MainTaskDelete.SetActive(false);
		}
		if (nowTaskID == "") {
			UI_Btn_MainTaskWrite.SetActive(false);
			UI_Btn_MainTaskDelete.SetActive(false);
		}
        */

        //如果上一次没有任务默认选择第一个任务显示
        if (nowTaskID == "0" || nowTaskID == "") { 
            //获取自身上的任务
            string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (taskID != "") {
                string[] taskIDList = taskID.Split(',');
                //找到第一个任务ID进行设置
                nowTaskID = taskIDList[0];
            }
        }

        if (nowTaskID != "0" && nowTaskID != "")
        {
            //更新任务描述
            string taskDesCribe = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskDes", "ID", nowTaskID, "Task_Template");
            UI_TaskDescribe.GetComponent<Text>().text = taskDesCribe;

            //更新任务目标
            //获取目标1
            string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1","ID",nowTaskID,"Task_Template");
            if (TaskTarget1 != "0") {

                //获取目标值
                string TaskValuePro1 = Game_PublicClassVar.Get_function_Task.TaskReturnValue(nowTaskID,"1");
                string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", nowTaskID, "Task_Template");
                //判定当前任务目标是获得道具还是杀怪
				string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", nowTaskID, "Task_Template");

                if (TaskValuePro1 == "" || TaskValuePro1 == null)
                {
                    TaskValuePro1 = "0";
                }
                if (TargetValue1 == "" || TargetValue1 == null)
                {
                    TargetValue1 = "0";
                }


                switch (targetType){ 
                    
                    //杀怪
                    case "1":
                    
                        //获取杀怪名称
						string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", TaskTarget1, "Monster_Template");
                        if (TaskValuePro1 == ""|| TaskValuePro1 == null) {
                            TaskValuePro1 = "0";
                        }
                        if (TargetValue1 == "" || TargetValue1 == null)
                        {
                            TargetValue1 = "0";
                        }
                        //改变字体颜色
                        if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
                        }
                        else {
                            UI_TaskTarget_1.GetComponent<Text>().color = new Color((float)214 / 255, (float)243 / 255, (float)124 / 255, 1);
                        }

                        //显示当前获取的道具
						UI_TaskTarget_1.GetComponent<Text>().text = "击败" + monsterName + " : " + TaskValuePro1 + " / " + TargetValue1;

                    break;
                    
                //道具
				case "2":
					
					//获取道具名称
					string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", TaskTarget1, "Item_Template");
					//改变字体颜色
					if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
					{
						UI_TaskTarget_1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
					}
					else {
						
						UI_TaskTarget_1.GetComponent<Text>().color = new Color((float)214 / 255, (float)243 / 255, (float)124 / 255, 1);
					}
					
					//显示当前获取的道具
					UI_TaskTarget_1.GetComponent<Text>().text = "获得" + itemName + " : " + TaskValuePro1 + " / " + TargetValue1;
					
					break;
				//对话Npc
				case "3":
					
					//获取道具名称
					string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", TaskTarget1, "Npc_Template");
					//改变字体颜色
					if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
					{
						UI_TaskTarget_1.GetComponent<Text>().color = new Color(1, 1, 1, 1);
					}
					else {
						
						UI_TaskTarget_1.GetComponent<Text>().color = new Color((float)214 / 255, (float)243 / 255, (float)124 / 255, 1);
					}
					
					//显示当前获取的道具
					UI_TaskTarget_1.GetComponent<Text>().text = "对话" + npcName + " : " + TaskValuePro1 + " / " + TargetValue1;
					
					break;
                }
            }
            else
            {
                //清空显示框
                UI_TaskTarget_1.GetComponent<Text>().text = "";
            }

            //获取目标2
            string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", nowTaskID, "Task_Template");
            if (TaskTarget2 != "0")
            {
                //获取目标值
                string TaskValuePro2 = Game_PublicClassVar.Get_function_Task.TaskReturnValue(nowTaskID, "2");
                string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", nowTaskID, "Task_Template");
                //判定当前任务目标是获得道具还是杀怪（根据ID规则判定）

                switch (TaskTarget1[0].ToString())
                {

                    //道具类
                    case "1":

                        //获取道具名称
                        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", TaskTarget2, "Item_Template");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro2) >= int.Parse(TargetValue2))
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = new Color(1, 1, 1, 1);

                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = new Color((float)214 / 255, (float)243 / 255, (float)124 / 255, 1);

                        }
                        //显示当前获取的道具
                        UI_TaskTarget_2.GetComponent<Text>().text = "获得" + itemName + " : " + TaskValuePro2 + " / " + TargetValue2;

                        break;

                    //怪物类（以后添加）

                }

            }
            else { 
                
                //清空显示框
                UI_TaskTarget_2.GetComponent<Text>().text = "";
            }



            //获取目标3
            string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", nowTaskID, "Task_Template");
            if (TaskTarget3 != "0")
            {

                //获取目标值
                string TaskValuePro3 = Game_PublicClassVar.Get_function_Task.TaskReturnValue(nowTaskID, "3");
                string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", nowTaskID, "Task_Template");
                //判定当前任务目标是获得道具还是杀怪（根据ID规则判定）

                switch (TaskTarget1[0].ToString())
                {

                    //道具类
                    case "1":

                        //获取道具名称
                        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", TaskTarget3, "Item_Template");
                        //改变字体颜色
                        if (int.Parse(TaskValuePro3) >= int.Parse(TargetValue3))
                        {
                            UI_TaskTarget_1.GetComponent<Text>().color = new Color(1, 1, 1, 1);

                        }
                        else
                        {

                            UI_TaskTarget_1.GetComponent<Text>().color = new Color((float)214 / 255, (float)243 / 255, (float)124 / 255, 1);

                        }

                        //显示当前获取的道具
                        UI_TaskTarget_3.GetComponent<Text>().text = "获得" + itemName + " : " + TaskValuePro3 + " / " + TargetValue3;

                        break;

                    //怪物类（以后添加）


                }

            }
            else
            {
                //清空显示框
                UI_TaskTarget_3.GetComponent<Text>().text = "";
            }

            //寻人任务单独显示
            string taskTargetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", nowTaskID, "Task_Template");
            if (taskTargetType == "3")
            {
                //获取寻找人的名称
                string completeNpcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", nowTaskID, "Task_Template");
                string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", completeNpcID, "Npc_Template");
                UI_TaskTarget_1.GetComponent<Text>().text = "寻找：" + npcName;
            }


        }
		if (nowTaskID != "") {
			//更新任务奖励
			string exp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskExp", "ID", nowTaskID, "Task_Template");
			string money = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskCoin", "ID", nowTaskID, "Task_Template");
			if (exp != "0") 
			{
				UI_TaskTrophy_Exp.GetComponent<Text>().text = "经验+"+ exp;
			}

			if (money != "0")
			{
				UI_TaskTrophy_Money.GetComponent<Text>().text = "金币+"+ money;
			}

			//更新道具奖励的显示
			string[] taskItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID","ID",nowTaskID,"Task_Template").Split(',');
			string[] taskItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum","ID",nowTaskID,"Task_Template").Split(',');

			//循环创建
			for(int i = 0;i<=taskItemID.Length-1;i++){
				if(taskItemID[i]!=""&&taskItemID[i]!="0"){
					GameObject taskItemObj = (GameObject)Instantiate(Obj_UI_TaskTrophyList);
					taskItemObj.transform.SetParent(UI_ItemTrophySet.transform);
					taskItemObj.transform.localScale = new Vector3(1,1,1);
					taskItemObj.transform.localPosition = new Vector3(i*70,0,0);
					taskItemObj.GetComponent<UI_TaskTrophyList>().ItemID = taskItemID[i];
					taskItemObj.GetComponent<UI_TaskTrophyList>().ItemNum = taskItemNum[i];
				}
			}
		}
    }

	//写入快捷任务
	public void WriteMainUITask(){
		string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;
		if (!Game_PublicClassVar.Get_function_Task.WriteMainUITaskID(nowTaskID)) {
			Debug.Log("最多只能保存3个快捷目标");
		}
		updateTaskData();

	}

	//删除快捷任务
	public void DeleteMainUITask(){
		string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;
		if (!Game_PublicClassVar.Get_function_Task.DeleteMainUITaskID(nowTaskID)) {
			Debug.Log("删除快捷任务失败");
		}
		updateTaskData();
	}

	//放弃自身携带的任务
	public void DeleteRoseTask(){
		string nowTaskID = Game_PublicClassVar.Get_game_PositionVar.NowTaskID;
		if (nowTaskID != "") {
			Game_PublicClassVar.Get_function_Task.DeleteRoseTaskID (nowTaskID);
		}

	}
}
