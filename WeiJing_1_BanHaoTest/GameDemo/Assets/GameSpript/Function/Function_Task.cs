using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//任务相关的子程序
public class Function_Task {

    private int taskNum;  //任务数量上限
    private string roseName; //主角名称
    private string roseID; //主角名称

    //绑点专用
    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;

    
    //接取任务
    public bool GetTask(string taskID) {

		Game_PublicClassVar.Get_function_UI.GameHint("接取了一个任务");
        //获取任务ID在第几个位置
        string taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (taskIDList != "")
        {
            taskIDList = taskIDList + "," + taskID;
        }
        else {
            taskIDList = taskID;
        }
        
        //获取任务当前完成值
        string taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (taskValueList != "")
        {
            taskValueList = taskValueList + ";0,0,0";
        }
        else {
            taskValueList = "0,0,0";
        }

        //写入任务值
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", taskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", taskValueList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//更新Npc头部任务状态显示
		Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;

		//获取当前追踪任务ID数量，少于3将刚接取的任务直接列入追踪任务中
		WriteMainUITaskID (taskID);

		//如果有获取道具任务更新任务显示
		string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID",taskID, "Task_Template");
		if (targetType == "2") {
			updataTaskItemID();
		}
        if (targetType == "4")
        {
            updataTaskItemID();
        }

        Game_PublicClassVar.Get_function_UI.PlaySource("10010", "1");

        return true;
    
    }

    //写入角色任务进度
    public bool WariteTaskPro(string taskID,string proID,string proValue) {
        /*
        IniScript iniScript = new IniScript();

        //获取绑点
        GameObject gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        Game_PositionVar game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        iniScript.InIWrite(taskID, proID, proValue, Application.dataPath + "/Game_DB/INI/" + game_PositionVar.Rose_ID + ".ini");
         */
        return true;
       
    }

    

    //查询当前NPC身上是否有自身的任务
    //NpcID 需要查询的NpcID
    public string[] TaskRoseQuery(string NpcID)
    {
        /*
        //获取当前自身的任务
        //获取绑点
        gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        //获取NPC身上的任务
        //DB_ReadDate db_ReadDate = new DB_ReadDate();
        IniScript iniScript = new IniScript();
        XMLScript xmlScript = new XMLScript();

        char[] fenge = { ';' };
        string[] npc_TaskID = xmlScript.Xml_GetDate("TaskID", "ID", NpcID, game_PositionVar.Xml_Path + "Npc_Template.xml").Split(fenge);

        string rose_TaskIDstr = "";
        string queryValue = "";

        for (int i = 1; i <= game_PositionVar.Rose_TaskNum; i++)
        {
            queryValue = iniScript.InIRead("Rose_Task", "TaskID" + i.ToString(), Game_PublicClassVar.Get_game_PositionVar.Ini_Path + "RoseTask_" + Game_PublicClassVar.Get_game_PositionVar.Rose_ID + ".ini");

            if (queryValue == "0")
            {

            }
            else {

                rose_TaskIDstr = rose_TaskIDstr + queryValue + ";";

            }

        }

        //处理多余的字符串
        if(rose_TaskIDstr != "" ){

            rose_TaskIDstr = rose_TaskIDstr.Substring(0, rose_TaskIDstr.Length-1);
        
        }

        string[] rose_TaskID = rose_TaskIDstr.Split(fenge);

        //对比重复的数组

        string QueryTaskStr = ""; //存储重复的数据

        for (int i = 1; i <= rose_TaskID.Length; i++) {

            for (int n = 1; n <= npc_TaskID.Length; n++) {

                if (rose_TaskID[i - 1] == npc_TaskID[n - 1]) {

                    QueryTaskStr = QueryTaskStr + rose_TaskID[i - 1] + ";"; 

                }

            }
        
        }

        if (QueryTaskStr != "")
        {

            QueryTaskStr = QueryTaskStr.Substring(0, QueryTaskStr.Length-1);

        }

        string[] QueryTask = QueryTaskStr.Split(fenge);
        
        return QueryTask;
         */

        //删
        char[] fenge = { ';' };
        string[] QueryTask = NpcID.Split(fenge);
        return QueryTask;

    }

    //查询任务是否完成
    //TaskID 任务ID
    public bool TaskComplete(string TaskID) {
        
        if (TaskID != "")
        {
            string TaskValuePro1 = TaskReturnValue(TaskID, "1");
            string TaskValuePro2 = TaskReturnValue(TaskID, "2");
            string TaskValuePro3 = TaskReturnValue(TaskID, "3");

            string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
            string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", TaskID, "Task_Template");
            string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", TaskID, "Task_Template");

            if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
            {
                if (int.Parse(TaskValuePro2) >= int.Parse(TargetValue2))
                {
                    if (int.Parse(TaskValuePro3) >= int.Parse(TargetValue3))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else {
            return false;
        }
    }


    //查询任务是否完成,全部完成返回0,返回1-3表示对应条件的任务未完成,返回-1表示执行错误
    //TaskID 任务ID
    public string TaskIncompleteReturnValue(string TaskID)
    {

        string IncompleteValue = "-1";
        if (TaskID != "")
        {
            string TaskValuePro1 = TaskReturnValue(TaskID, "1");
            string TaskValuePro2 = TaskReturnValue(TaskID, "2");
            string TaskValuePro3 = TaskReturnValue(TaskID, "3");

            string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", TaskID, "Task_Template");
            string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", TaskID, "Task_Template");
            string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", TaskID, "Task_Template");

            if (int.Parse(TaskValuePro1) >= int.Parse(TargetValue1))
            {
                if (int.Parse(TaskValuePro2) >= int.Parse(TargetValue2))
                {
                    if (int.Parse(TaskValuePro3) >= int.Parse(TargetValue3))
                    {
                        IncompleteValue = "0";
                    }
                    else
                    {
                        IncompleteValue = "3";
                    }
                }
                else
                {
                    IncompleteValue = "2";
                }
            }
            else
            {
                IncompleteValue = "1";
            }
        }

        return IncompleteValue;

    }

    //根据任务ID发放任务奖励
    public bool TaskRewards(string TaskID) {

        //根据任务ID获取对应的奖励 
        string[] rewardItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", TaskID, "Task_Template").Split(',');
        string[] rewardItemValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", TaskID, "Task_Template").Split(',');

        //获取背包剩余格子数量，判定奖励数量是否大于格子数量
        if (Game_PublicClassVar.Get_function_UI.BagSpaceNullNum() < rewardItemID.Length)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请先清空背包在提交任务！");
            return false;       //背包格子不足,领取奖励失败
        }

		//发送经验或金币奖励
		string taskTrophyCoin = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskCoin", "ID", TaskID, "Task_Template");
		string taskTrophyExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskExp", "ID", TaskID, "Task_Template");

		Game_PublicClassVar.Get_function_Rose.SendReward ("1", taskTrophyCoin);
		Game_PublicClassVar.Get_function_Rose.AddExp(int.Parse(taskTrophyExp));

        //获取奖励类型
        string taskTrophyType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskTrophyType", "ID", TaskID, "Task_Template");
        if (taskTrophyType == "1") {

            //发送奖励
            if (rewardItemID != null) {
                for (int i = 0; i <= rewardItemID.Length - 1; i++) {
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardItemID[i], int.Parse(rewardItemValue[i]));
                }
            }
        }

        //根据任务ID开启下面的章节关卡
        string openPveChapter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenPVEChapter", "ID", TaskID, "Task_Template");
        //Debug.Log("openPveChapter = " + openPveChapter);
        if (openPveChapter != "0") {
            Game_PublicClassVar.Get_function_Rose.UpdataPVEChapter(openPveChapter);
        }
        //Debug.Log("任务奖励结束！");
        return true;
    }

    //检测当前NPC是否有可接的任务，有的返回任务数组，没有返回1个数组，第一个值为空
    //NpcID       NPC的ID
    //NpcTaskID   NPC对应的任务ID数组
    public string[] IfTask(string NpcID,string[] NpcTaskID)
    {
        
        //获取自身已完成任务
        string[] roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        //获取自身接取任务
        string[] roseGetTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        
        /*
        //检测已完成的任务是否
        gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        roseID = game_PositionVar.Rose_ID;

        //*****************************************************************检测角色已完成的任务列表*****************************************************************
        
        //获得角色以完成的任务
        string path = game_PositionVar.Ini_RoseTask;
        IniScript iniScript = new IniScript();
        //string npcTask = iniScript.InIRead(roseID, NpcID, path);
        string npcTask = Game_PublicClassVar.Get_IniSprict.InIRead("RoseCompleteNpcTask", NpcID,Game_PublicClassVar.Get_game_PositionVar.Ini_Path+"RoseData_"+Game_PublicClassVar.Get_game_PositionVar.Rose_ID+".ini");
        //将字符串转换成数组
        char[] fenge = { ';' };
        string[] npcTaskList = npcTask.Split(fenge);

        //比较是否已完成该任务ID
         
        //newTaskList[0] = "aaaa";
        string newTaskID="";
        //string newTaskID2 = "";
        //int newTaskListNum = 0;
        for (int i=1; i <= NpcTaskID.Length; i++) {

            for (int n = 1; n <= npcTaskList.Length; n++) {

                if (NpcTaskID[i - 1] == npcTaskList[n - 1])
                {
                    NpcTaskID[i - 1] = "0";

                    //n = npcTaskList.Length;

                }
                else
                {   

                }
            }

            //如果是末尾不加“，”号，要不会多生成一个数组成员

            if (NpcTaskID[i - 1] != "0") {

                if (i == NpcTaskID.Length)
                {
                    newTaskID = newTaskID + NpcTaskID[i - 1];
                    //newTaskListNum = newTaskListNum + 1;
                }
                else
                {
                    newTaskID = newTaskID + NpcTaskID[i - 1] + ";";
                    //newTaskListNum = newTaskListNum + 1;  

                }
            
            }

        }

        //for(int i = 1 ;NpcTaskID )

        

        //将字符串转换成数组
        char[] fenge1 = { ';' };
        string[] newTaskList = newTaskID.Split(fenge);

        //*****************************************************************检测角色身上的任务ID*****************************************************************

        //检测当前绑定角色身上的任务
        //读取数据脚本
        //db_ReadDate = new DB_ReadDate();

        for (int i = 1; i <= game_PositionVar.Rose_TaskNum; i++) {

            //string date = db_ReadDate.ReadDate(game_PositionVar.Sql_Rose_Task, game_PositionVar.SqlKey_TaskID + i.ToString(), "RoseID", game_PositionVar.Rose_ID);
            string date = iniScript.InIRead("Rose_Task", "TaskID" + i.ToString(), Game_PublicClassVar.Get_game_PositionVar.Ini_Path + "RoseTask_" + Game_PublicClassVar.Get_game_PositionVar.Rose_ID + ".ini");

            for (int n = 1; n<=newTaskList.Length; n++) {

                if (date == newTaskList[n - 1]) {

                    newTaskList[n - 1] = "0";

                }
            
            }
        
        }

        string newTaskID2="";
        //将数组内为0的值进行删除
        for (int i = 1; i <= newTaskList.Length; i++) {

            if (newTaskList[i - 1] == "0")
            {

            }
            else { 
                    
                    if(i==newTaskList.Length){

                        newTaskID2 = newTaskID2 + newTaskList[i-1];

                    }else{
                    
                        newTaskID2 = newTaskID2+newTaskList[i-1]+";";
                    }

            }


        }

        //将字符串转换成数组
        
        char[] fenge2 = { ';' };
        string[] newTaskList2 = newTaskID2.Split(fenge);

        
        
        return newTaskList2;
        */


        //后期注意删除
        char[] fenge = { ';' };
        string[] newTaskList2 = NpcID.Split(fenge);;
        return newTaskList2;

    }

    //根据数字类型返回文字的任务类型
    //typeValue 为任务的数字类型
    public string TaskTypeToString(string typeValue) {
        /*
        string value="";

        switch (typeValue)
        {

            case "1":
                //case "1":

               value= "主线";

            break;

            case "2":

                value= "支线";

            break;

            case "3":

                value= "每日";

            break;
        }

        return value;
        */

        return ""; //后期删除
    }

    //根据传入的怪物ID判定增加当前任务击杀数量
    
    public bool TaskMonsterNum( string MonsterID,int MonsterNum ) {
		
        //获取自身携带的任务ID
        string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        string taskValue = "";
        //循环遍历当前任务ID的要求击杀的目标是否是当前怪物
        for (int i = 0; i <= taskIDList.Length - 1; i++)
        {
			if(taskIDList[i]!=""){
				string taskValueSon = "";
				//获取当前任务要求的目标
				string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskIDList[i], "Task_Template");
				string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskIDList[i], "Task_Template");
				string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskIDList[i], "Task_Template");
				string[] TaskValue = taskValueList[i].Split(',');

				if (TaskTarget1 == MonsterID)
				{
					//获取任务目标完成值
					string TaskTargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", taskIDList[i], "Task_Template");
					//获取任务完成值
					string TaskValue1 = TaskValue[0];
					//判定完成值是否大于目标值
					if (int.Parse(TaskTargetValue1) > int.Parse(TaskValue1))
					{
						//写入数据
						taskValueSon = (int.Parse(TaskValue1) + MonsterNum).ToString();
					}
					else
					{
						taskValueSon = TaskValue1;
					}
					
					//更新主界面任务显示数据
					Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
				}
				else {
					taskValueSon = TaskValue[0];
				}
				
				if (TaskTarget2 == MonsterID)
				{
					//获取任务目标完成值
					string TaskTargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", taskIDList[i], "Task_Template");
					//获取任务完成值
					string TaskValue1 = TaskValue[1];
					//判定完成值是否大于目标值
					if (int.Parse(TaskTargetValue1) > int.Parse(TaskValue1))
					{
						//写入数据
						taskValueSon = taskValueSon + "," + (int.Parse(TaskValue1) + MonsterNum).ToString();
					}
					else
					{
						taskValueSon = taskValueSon + "," + TaskValue1;
					}
					
					//更新主界面任务显示数据
					Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
				}
				else {
					taskValueSon = taskValueSon + "," + TaskValue[1];
				}
				if (TaskTarget3 == MonsterID)
				{
					//获取任务目标完成值
					string TaskTargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", taskIDList[i], "Task_Template");
					//获取任务完成值
					string TaskValue1 = TaskValue[2];
					//判定完成值是否大于目标值
					if (int.Parse(TaskTargetValue1) > int.Parse(TaskValue1))
					{
						//写入数据
						taskValueSon = taskValueSon + "," + (int.Parse(TaskValue1) + MonsterNum).ToString();
					}
					else
					{
						taskValueSon = taskValueSon + "," + TaskValue1 + ";";
					}
					
					//更新主界面任务显示数据
					Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
				}
				else {
					taskValueSon = taskValueSon + "," + TaskValue[2] + ";";
				}
				
				taskValue = taskValue + taskValueSon;
			}

        }

        //末尾去掉;符号
        if (taskValue != "") {
            taskValue = taskValue.Substring(0, taskValue.Length - 1);
        }

        //写入对应的任务目标值
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", taskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //更新Npc头部任务状态显示
        Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;


        return true;
    
    }

	//更新当前任务目标完成值
	public bool updataTaskItemID(){

		//获取自身携带的任务ID
		string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
		string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
		string taskValue = "";

		//循环遍历当前任务ID的要求击杀的目标是否是当前怪物
		for (int i = 0; i <= taskIDList.Length - 1; i++)
		{
            if (taskIDList[i] != "" && taskIDList[i] != "0")
            {
				string taskValueSon = "";
				//获取当前任务要求的目标
				string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskIDList[i], "Task_Template");
				string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskIDList[i], "Task_Template");
				string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskIDList[i], "Task_Template");
				
				string[] TaskValue = taskValueList[i].Split(',');
				
				//获取任务类型是否为寻找道具
				string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskIDList[i], "Task_Template");
				if(targetType=="2"){
                    int bagItemNum = 0;
                    if (TaskTarget1 != "0")
                    {
                        //获取目标ID
                        bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(TaskTarget1);
                        //int bagItemNum = 0;
                        if (bagItemNum != 0)
                        {
                            taskValueSon = bagItemNum.ToString();
                            //更新主界面任务显示数据
                            Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                        }
                        else
                        {
                            taskValueSon = TaskValue[0];
                        }
                    }
                    else {
                        taskValueSon = TaskValue[0];
                    }


                    if (TaskTarget2 != "0")
                    {
                        bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(TaskTarget2);
                        if (bagItemNum != 0)
                        {
                            taskValueSon = taskValueSon + "," + bagItemNum.ToString();
                            //更新主界面任务显示数据
                            Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                        }
                        else
                        {
                            taskValueSon = taskValueSon + "," + TaskValue[1];
                        }
                    }
                    else {
                        taskValueSon = taskValueSon + "," + TaskValue[1];
                    }

                    if (TaskTarget2 != "0")
                    {
                        bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(TaskTarget3);
                        if (bagItemNum != 0)
                        {

                            taskValueSon = taskValueSon + "," + bagItemNum.ToString() + ";";
                            //更新主界面任务显示数据
                            Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
                        }
                        else
                        {
                            taskValueSon = taskValueSon + "," + TaskValue[2] + ";";
                        }

                        
                    }
                    else {
                        taskValueSon = taskValueSon + "," + TaskValue[2] + ";";
                    }

                    taskValue = taskValue + taskValueSon;
					
				}
                
                if (targetType == "1" || targetType == "3")
                {
                    taskValue = taskValue + taskValueList[i] + ";";
                }

                //等级类任务
                if (targetType == "4")
                {
                    //获取当前等级
                    string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    taskValue = taskValue + roseLv + ",0,0;";
                }
			}
			//Debug.Log("taskValue = " + taskValue);
		}
	
		//末尾去掉;符号
		if (taskValue != "") {
			taskValue = taskValue.Substring(0, taskValue.Length - 1);  
		}
		
		//写入对应的任务目标值
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", taskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
		
		return true;
	}

    //获取当前角色身上的任务ID
    public List<string> GetRoseTaskID() {

        List<string> taskID_List = new List<string>();
        string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        char[] fenge = { ',' };
        string[] taskIDList = taskID.Split(fenge);
        for (int i = 0; i <= taskIDList.Length - 1; i++) {
            taskID_List.Add(taskIDList[i]);
        }
        return taskID_List;
    }

    //返回当前自身携带对应类型的任务
    public List<string> GetRoseTaskTypeID(string taskType)
    {
        List<string> taskID_List = new List<string>();
        string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        char[] fenge = {','};
        string[] taskIDList = taskID.Split(fenge);
        for (int i = 0; i <= taskIDList.Length - 1; i++)
        {
			if(taskIDList[i]!=""){
				//获取任务类型
				string targerID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", taskIDList[i], "Task_Template");
				if (targerID == taskType)
				{
					taskID_List.Add(taskIDList[i]);
				}
			}
        }
        return taskID_List;
    }

    //根据任务ID返回任务名称
    public string TaskIDtoTaskName(string taskID) {
        
        string taskName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskName", "ID", taskID,"Task_Template");
        return taskName;
    }

    //根据任务ID返回任务等级
    public string TaskIDtoTaskLv(string taskID)
    {
        string taskLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskLv", "ID", taskID, "Task_Template");
        return taskLv;
    }

    public string TaskIDtoTaskDescribe(string taskID)
    {
        /*
        string taskDescribe = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("TaskDes", "ID", taskID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Task_Template.xml");

        return taskDescribe;
        */
        return ""; //后期删除
    }

    //传入任务ID获取当前进度值  (参数1：传入的任务ID,  参数2：需要查询当前任务的第几个任务目标)
    public string TaskReturnValue(String taskID, String taskTargetValue) {
        string taskProValue = "";
        //检测自身是否携带此任务
        bool findStatus = false;
        List<string> roseTaskList = GetRoseTaskID();
        foreach(string roseTaskID in roseTaskList){
            if (roseTaskID == taskID) {
                findStatus = true;
            }
        }
        //开始查询
        if (findStatus)
        { 
            //获取任务ID在第几个位置
            string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
            //获取任务当前完成值
            string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
            for (int i = 0; i <= taskIDList.Length - 1; i++)
            {
                //符合条件
                if (taskIDList[i] == taskID)
                {
                    string taskValue = taskValueList[i];
                    //当前进度赋值
                    switch (taskTargetValue) {
                        case "1":
                            taskProValue = taskValue.Split(',')[0];
                        break;

                        case "2":
                            taskProValue = taskValue.Split(',')[1];
                        break;

                        case "3":
                            taskProValue = taskValue.Split(',')[2];
                        break;
                    }
                }
            }
        }
        return taskProValue;
    }

    //传入任务ID返回任务状态 (参数1:任务ID)  返回值 1：未接取 2：已完成 3：已接取,未完成 4：已接取,已完成 5:未达到接取条件
    public string TaskReturnStatus(string taskID) {

        //获取自身已完成任务
        string[] roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');

        for (int i = 0; i <= roseCompleteTaskID.Length - 1; i++) {
            if (roseCompleteTaskID[i] == taskID) {
                return "2";
            }
        }

        //获取自身接取任务
        string[] roseGetTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        for (int i = 0; i <= roseGetTaskID.Length - 1; i++)
        {
            if (roseGetTaskID[i] == taskID)
            {

                //检测任务是否完成
                if (TaskComplete(taskID))
                {
                    return "4";
                }
                else {
                    return "3";
                }
            }
        }

        //判定是否满足接取任务条件
        string triggerTaskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerType", "ID", taskID, "Task_Template");
        string triggerTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerValue", "ID", taskID, "Task_Template");
        string taskLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskLv", "ID", taskID, "Task_Template");
        switch (triggerTaskType) { 
            //等级触发，检测自身等级是否达到
            case "1":
                string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (int.Parse(roseLv) >= int.Parse(taskLv))
                {
                    return "1";
                }
            break;
            //任务触发，检测完成的任务ID是否包含前置任务
            case "2":
                for (int i = 0; i <= roseCompleteTaskID.Length - 1; i++)
                {
                    if (roseCompleteTaskID[i] == triggerTaskValue)
                    {
                        return "1";
                    }
                }
            break;
        }
        return "5";

    }

    //传入任务ID,写入完成的任务
    public bool TaskWriteRoseData(string taskID) {

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().obj_NpcTarget == null) {
            //Debug.Log("玩家当前没有选中任务NPC");
            return false;
        }

        //写入完成任务
        string roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string writeTaskID = "";
        if (roseCompleteTaskID != "")
        {
            writeTaskID = roseCompleteTaskID + "," + taskID;
        }
        else {
            writeTaskID = taskID;
        }
        //查询自身是否携带此任务,如携带此任务将清空其数据
        //获取任务ID在第几个位置
        string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        //获取任务当前完成值
        string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        string getTaskIDList = "";
        string getTaskValueList = "";
        for (int i = 0; i <= taskIDList.Length - 1; i++) {
            //完成的任务ID不是当前携带的任务,进行数据记录，如果是自身携带记录,则不进行记录
            if (taskIDList[i] != taskID) {

                if (getTaskIDList == "")
                {
                    getTaskIDList = taskIDList[i];
                    getTaskValueList = taskValueList[i];
                }
                else
                {
                    getTaskIDList = getTaskIDList + "," + taskIDList[i];
                    getTaskValueList = getTaskValueList + ";" + taskValueList[i];
                }
			}else{
				//触发完成任务，扣除任务消耗
				//获取任务类型是否为寻找道具
				string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskIDList[i], "Task_Template");
				//是否为消耗道具任务
				if(targetType == "2"){
					//获取任务要求道具数量
					string TaskTarget1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskIDList[i], "Task_Template");
					string TaskTarget2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskIDList[i], "Task_Template");
					string TaskTarget3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskIDList[i], "Task_Template");

					string TargetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", taskIDList[i], "Task_Template");
					string TargetValue2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue2", "ID", taskIDList[i], "Task_Template");
					string TargetValue3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue3", "ID", taskIDList[i], "Task_Template");

					if(TaskTarget1!="0"){
						bool costItem = Game_PublicClassVar.Get_function_Rose.CostBagItem(TaskTarget1,int.Parse(TargetValue1));
						if(!costItem){
							//Debug.Log("背包内任务道具不足");
						}
					}
					if(TaskTarget2!="0"){
						bool costItem = Game_PublicClassVar.Get_function_Rose.CostBagItem(TaskTarget2,int.Parse(TargetValue2));
						if(!costItem){
							//Debug.Log("背包内任务道具不足");
						}
					}
					if(TaskTarget3!="0"){
						bool costItem = Game_PublicClassVar.Get_function_Rose.CostBagItem(TaskTarget3,int.Parse(TargetValue3));
						if(!costItem){
							//Debug.Log("背包内任务道具不足");
						}
					}
				}
			}
        }

        //更新玩家剧情状态
        string TriggerStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TriggerStoryID", "ID", taskID, "Task_Template");
        //Debug.Log("TriggerStoryID = " + TriggerStoryID);
        if(TriggerStoryID!="0"){
            //获取玩家当前剧情状态值
            string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Debug.Log("TriggerStoryID = " + TriggerStoryID);
            if (roseStoryStatus == TriggerStoryID) {
                Game_PublicClassVar.Get_function_Rose.UpdataRoseStoryStatus();
                //Debug.Log("玩家完成指定任务,更新了剧情值");
            }
        }

        //
        //获取当前选中的Npc是否有剧情对话
        bool isStory = false;
        string storyIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StorySpeakID", "ID", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().obj_NpcTarget.GetComponent<AI_NPC>().NpcID, "Npc_Template");
        if (storyIDStr != "0")
        {
            string[] storyID = (storyIDStr).Split(';');
            for (int i = 0; i <= storyID.Length - 1; i++)
            {
                if (storyID[i] == Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"))
                {
                    isStory = true;
                }
            }
        }

        if (isStory)
        {
            //实例化一个剧情UI
            GameObject obj_StorySpeakSet = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_StorySpeakSet);
            obj_StorySpeakSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_StorySpeakSet.transform);
            obj_StorySpeakSet.transform.localPosition = Vector3.zero;
            obj_StorySpeakSet.transform.localScale = new Vector3(1, 1, 1);
            obj_StorySpeakSet.GetComponent<UI_StorySpeakSet>().GameStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            GameObject obj_NpcTarget = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().obj_NpcTarget;
            if (obj_NpcTarget.GetComponent<AI_NPC>().NpcID != "" && obj_NpcTarget.GetComponent<AI_NPC>().NpcID != "0")
            {
                obj_StorySpeakSet.GetComponent<UI_StorySpeakSet>().NpcID = obj_NpcTarget.GetComponent<AI_NPC>().NpcID;

            }
            //隐藏主界面
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUIBtn.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.SetActive(false);
        }



        //写入完成任务
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CompleteTaskID", writeTaskID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", getTaskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", getTaskValueList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//更新Npc头部任务状态显示
		Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;

		//删除主界面快捷显示的任务
		DeleteMainUITaskID (taskID);

        Game_PublicClassVar.Get_function_UI.PlaySource("10011", "1");

        return true;

    }

	//获取当前主界面跟踪任务
	public string[] ReturnMainUITask(){

		string[] mainUITask = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainUITaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig").Split(',');
		return mainUITask;
	}

	//获取当前任务有几个目标
	public int ReturnTaskTargetNum(string taskID){

        //如果是找人任务 返回1
        string taskTargetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", taskID, "Task_Template");
        if (taskTargetType == "3")
        {
            return 1;
        }

		int targetNum = 0;
		string target_1 =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target1", "ID", taskID, "Task_Template");
		string target_2 =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target2", "ID", taskID, "Task_Template");
		string target_3 =  Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Target3", "ID", taskID, "Task_Template");
		if (target_1 != "0") {
			targetNum = targetNum +1;
		}
		if (target_2 != "0") {
			targetNum = targetNum +1;
		}
		if (target_3 != "0") {
			targetNum = targetNum +1;
		}

		return targetNum;
	}

	//写入追踪任务
	public bool WriteMainUITaskID(string taskID){

		if (ReturnMainUITask ().Length < 3) {
			string mainUITask = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainUITaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
			if(mainUITask!=""){
				mainUITask = mainUITask + "," + taskID;
			}else{
				mainUITask = taskID;
			}
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", mainUITask, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
			Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

			//更新主界面任务显示
			Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;
			return true;
		} else {
			return false;
		}
	}

	//删除追踪任务
	public bool DeleteMainUITaskID(string taskID){

		string mainTask = "";
		string[] mainUITaskID = ReturnMainUITask();
		for (int i = 0; i<=mainUITaskID.Length-1; i++) {
			if(mainUITaskID[i]!=taskID){
				mainTask = mainTask + mainUITaskID[i]+",";
			}
		}
		if (mainTask != "") {
			mainTask = mainTask.Substring(0,mainTask.Length-1);
		}

		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainUITaskID", mainTask, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

		//更新主界面任务显示
		Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;

		return true;
	}

	//传入任务ID判定是否为追踪任务
	public bool IfMainUITask(string taskID){
		string[] mainUITaskID = ReturnMainUITask();
		for (int i = 0; i<=mainUITaskID.Length-1; i++) {
			if(mainUITaskID[i]==taskID){
				return true;
			}
		}
		return false;
	}

	//取消自身携带的任务
	public bool DeleteRoseTaskID(string taskID){
		//写入完成任务
		string roseCompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		//查询自身是否携带此任务,如携带此任务将清空其数据
		//获取任务ID在第几个位置
		string[] taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
		//获取任务当前完成值
		string[] taskValueList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
		string getTaskIDList = "";
		string getTaskValueList = "";
		for (int i = 0; i <= taskIDList.Length - 1; i++) {
			//完成的任务ID不是当前携带的任务,进行数据记录，如果是自身携带记录,则不进行记录
			if (taskIDList[i] != taskID) {
				
				if (getTaskIDList == "")
				{
					getTaskIDList = taskIDList[i];
					getTaskValueList = taskValueList[i];
				}
				else
				{
					getTaskIDList = getTaskIDList + ";" + taskIDList[i];
					getTaskValueList = getTaskValueList + ";" + taskValueList[i];
				}
			}
		}
		
		//写入完成任务
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskID", getTaskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AchievementTaskValue", getTaskValueList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//删除当前追踪任务ID
		DeleteMainUITaskID(taskID);

		//更新主界面任务显示
		Game_PublicClassVar.Get_game_PositionVar.MainUITaskUpdata = true;

		//刷新任务栏
		Game_PublicClassVar.Get_game_PositionVar.Rose_TaskDataUpdata = true;

		//清空当前选中任务
		if (Game_PublicClassVar.Get_game_PositionVar.NowTaskID == taskID) {
			Game_PublicClassVar.Get_game_PositionVar.NowTaskID = "";
			//再次获取自身的任务
			taskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
			if(taskIDList[0]!=""){
				Game_PublicClassVar.Get_game_PositionVar.NowTaskID = taskIDList[0];
			}
		}

		//更新主界面任务列表
		UI_FunctionOpen ui_FunctionOpen = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
		ui_FunctionOpen.Obj_roseTask.GetComponent<Rose_TaskList>().Rose_TaskList_Update = true;
		//更新Npc头部任务状态显示
		Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true;

		return true;

	}

}
