using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_MainUITaskShow : MonoBehaviour {

	public string TaskID;
	public bool UpdataTask;
	public GameObject Obj_TaskName;
	public GameObject Obj_TaskTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//更新显示任务
		if (UpdataTask) {
			UpdataTask = false;
			string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", TaskID, "Task_Template");
			string taskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", TaskID, "Task_Template");
			string taskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", TaskID, "Task_Template");
			string taskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", TaskID, "Task_Template");
			string taskTargetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", TaskID, "Task_Template");

			//获取任务当前目标文本
			string taskText = "";
			string value1 = "";
			string value2 = "";
			string value3 = "";

			if(taskTarget1!="0"){
				value1= returnTaskTagetText(taskTarget1,taskTargetType,"1");
			}
			if(taskTarget2!="0"){
				value2= returnTaskTagetText(taskTarget2,taskTargetType,"2");
			}
			if(taskTarget3!="0"){
				value3= returnTaskTagetText(taskTarget3,taskTargetType,"3");
			}

			taskText = value1 + value2 +value3;

            //显示找人任务
            if (taskTargetType == "3")
            {
                //获取寻找人的名称
                string completeNpcID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteNpcID", "ID", TaskID, "Task_Template");
                string npcName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcName", "ID", completeNpcID, "Npc_Template");
                taskText = "寻找：" + npcName;
            }

			//显示任务名称
			Obj_TaskName.GetComponent<Text>().text = taskName;
			//显示任务进度
			Obj_TaskTarget.GetComponent<Text>().text = taskText;
		}
	}

	//返回任务目标描述文本，参数1：任务目标ID  参数2：任务目标类型  参数3：当前第几个目标
	private string returnTaskTagetText(string targetID,string targetType,string targetNum){
		string taskText = "";
		//根据任务目标类型返回任务前缀的文字、
		string targetText = "";
		string taskTargetName = "";
		string taskTargetValue = "";
		string taskNowTargetValue = "";
		if (targetID != "0") {
			//获取任务完成度
			taskTargetValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("TargetValue" + targetNum, "ID", TaskID, "Task_Template");
            taskNowTargetValue = Game_PublicClassVar.Get_function_Task.TaskReturnValue(TaskID, targetNum);
			//根据任务目标类型到各个表中获取任务名称
			switch (targetType) {
			case "1":
				targetText = "击败";
				taskTargetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("MonsterName", "ID", targetID, "Monster_Template");
				break;

			case "2":
				targetText = "获得";
				taskTargetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemName", "ID", targetID, "Item_Template");
				break;

			case "3":
				targetText = "对话";
				taskTargetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("NpcName", "ID", targetID, "Npc_Template");
				break;

            case "4":
                targetText = "等级";
                taskTargetName = "提升至";
                break;
			}

			if (targetText != "") {
                
				taskText = targetText + taskTargetName + "(" + taskNowTargetValue + "/" + taskTargetValue + ")" + "\n";
                
                if (int.Parse(taskNowTargetValue) >= int.Parse(taskTargetValue)) {
                    taskText = targetText + taskTargetName + "(" + taskNowTargetValue + "/" + taskTargetValue + ")" + "<color=#00ff00ff>" + "（已完成）</color>" + "\n";
                }
                
                if (targetType == "4")
                {
                    taskText = targetText + taskTargetName + taskTargetValue + "级\n";
                    /*
                    int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
                    if (roseLv >= int.Parse(taskTargetValue))
                    { 
                        
                    }
                    */
                    //

                }
			}
		}
		return taskText;
	}

    //点击任务移动按钮
    public void Btn_TaskMove(){
        //string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", TaskID, "Task_Template");
        //Debug.Log("我点击了任务：" + taskName);

        //获取任务是否完成,如果完成直接奔向交任务的NPC处
        string incompleteValue = Game_PublicClassVar.Get_function_Task.TaskIncompleteReturnValue(TaskID);
        switch (incompleteValue) { 
        
            case "-1":
                //Debug.Log(TaskID + "数据错误");
                break;
            //任务全部完成
            case "0":
                //Debug.Log("任务完成");
                string movePosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1Position", "ID", TaskID, "Task_Template");
                //获取场景并对比是否为当前场景
                taskMovePosition("CompleteTaskPosition");
                break;
            //任务条件1未完成
            case "1":
                //Debug.Log("任务未完成：1");
                taskMovePosition("Target1Position");
                break;
            //任务条件2未完成
            case "2":
                //Debug.Log("任务未完成：2");
                taskMovePosition("Target2Position");
                break;
            //任务条件3未完成
            case "3":
                //Debug.Log("任务未完成：3");
                taskMovePosition("Target3Position");
                break;

        }
    }

    void taskMovePosition(string moveName) {
        //获取场景并对比是否为当前场景
        string taskMoveID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(moveName, "ID", TaskID, "Task_Template");
        if (taskMoveID == "0") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("此任务不可以自动寻路！");
            return;
        }
        string mapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", taskMoveID, "TaskMovePosition_Template");
        if (Application.loadedLevelName == mapName)
        {
            moveMap(taskMoveID);
            /*
            //获取移动到的目标点
            string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PositionName", "ID", taskMoveID, "TaskMovePosition_Template");
            GameObject scenceRosePosition = GameObject.FindWithTag("ScenceRosePosition");
            GameObject obj_Position = scenceRosePosition.transform.Find(positionName).gameObject;
            Vector3 movePositionVec3 = new Vector3();
            if (obj_Position != null)
            {
                //如果目标点在场景中存在则直接获取此坐标点
                movePositionVec3 = obj_Position.transform.position;
            }
            else {
                //读取XYZ的值进行赋值
                float p_x = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_X", "ID", taskMoveID, "TaskMovePosition_Template"));
                float p_y = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Y", "ID", taskMoveID, "TaskMovePosition_Template"));
                float p_z = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Z", "ID", taskMoveID, "TaskMovePosition_Template"));
                movePositionVec3 = new Vector3(p_x, p_y, p_z);
                //movePositionVec3 = new Vector3(-10.21f, 31f, -10.38f);
            }
            
            //移动到目标点
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunPositionVec3 = movePositionVec3;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunStatus = "1";
            */
        }
        else {
            //获取移动其他地图的移动点
            string[] OtherMapMoveList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OtherMapMove", "ID", taskMoveID, "TaskMovePosition_Template").Split(';');
            string movePositionID = "";
            bool moveStatus = false;
            for(int i = 0;i<=OtherMapMoveList.Length-1;i++){
                string[] moveMapValue = OtherMapMoveList[i].Split(',');
                if (moveMapValue[0] == Application.loadedLevelName) {
                    moveStatus = true;
                    movePositionID = moveMapValue[1];
                }
            }

            if (moveStatus)
            {
                if (movePositionID != "")
                {
                    moveMap(movePositionID);        //开始移动               
                }
            }
            else {
                string sceneName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneName", "ID", mapName, "Scene_Template");
                Game_PublicClassVar.Get_function_UI.GameGirdHint("请先前往地图：" + sceneName);
                if (mapName == "20004") {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("地牢位于热荒沙漠的入口处,打开左上角的小地图即可看到!");
                }
            }
        }
    }

    void moveMap(string taskMoveID)
    {
        //获取移动到的目标点
        string positionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PositionName", "ID", taskMoveID, "TaskMovePosition_Template");
        GameObject scenceRosePosition = GameObject.FindWithTag("ScenceRosePosition");
        GameObject obj_Position = scenceRosePosition.transform.Find(positionName).gameObject;
        Vector3 movePositionVec3 = new Vector3();
        if (obj_Position != null)
        {
            //如果目标点在场景中存在则直接获取此坐标点
            movePositionVec3 = obj_Position.transform.position;
        }
        else
        {
            //读取XYZ的值进行赋值
            float p_x = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_X", "ID", taskMoveID, "TaskMovePosition_Template"));
            float p_y = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Y", "ID", taskMoveID, "TaskMovePosition_Template"));
            float p_z = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Z", "ID", taskMoveID, "TaskMovePosition_Template"));
            movePositionVec3 = new Vector3(p_x, p_y, p_z);
            //movePositionVec3 = new Vector3(-10.21f, 31f, -10.38f);
        }

        //移动到目标点
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunPositionVec3 = movePositionVec3;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().TaskRunStatus = "1";
    }
}