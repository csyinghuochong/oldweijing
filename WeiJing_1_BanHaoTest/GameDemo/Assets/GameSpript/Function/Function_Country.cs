using UnityEngine;
using System.Collections;

public class Function_Country
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //添加指定繁荣度并检测是否升级
    public bool addCoutryExp(int addValue, bool ifHint = false)
    {

        string guoWangLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        //超过25级不得任何经验
        if (int.Parse(guoWangLv) >= 25) {
            return false;
        }

        string guoWangExpNow = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //获得当前建筑的经验
        //string guoWangExpMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryUp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "Country_Template");
        string guoWangExpMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryUp", "ID", guoWangLv, "Country_Template");
        int expValue = int.Parse(guoWangExpNow) + addValue;
        if (ifHint) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("繁荣度提升增加:" + addValue);
        }
        
        if (expValue > int.Parse(guoWangExpMax))
        {
            //判定当前升级的角色等级限制
            int countryRoseLvMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryRoseLvMax", "ID", guoWangLv, "Country_Template"));
            int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            if (roseLv >= countryRoseLvMax)
            {
                //升级
                int outherValue = expValue - int.Parse(guoWangExpMax);
                int guoLv = int.Parse(guoWangLv) + 1;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", outherValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryLv", guoLv.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                return true;
            }
            else {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", expValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                return false;
            }
        }
        else {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryExp", expValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
            //Debug.Log("expValue = " + expValue);
            return false;
        }
    }

    public void SetGuoJiang(int setLv) {
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryLv", setLv.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
    }

    //更新每分钟产出
    public void UpdataMinuteData() {
        
        string guoWangLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string houreExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureExp", "ID", guoWangLv, "Country_Template");
        string houreHonor = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureHonor", "ID", guoWangLv, "Country_Template");
        //Debug.Log("houreHonor =" + houreHonor);
        Game_PublicClassVar.Get_game_PositionVar.MinuteCountryExp = (int)(int.Parse(houreExp) / 60);
        Game_PublicClassVar.Get_game_PositionVar.MinuteCountryHonor = (int)(int.Parse(houreHonor) / 60);
        
    }

    //扣除荣誉值
    public bool CostCountryHonor(int costValue) {

        string countryHonor = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        int value = int.Parse(countryHonor) - costValue;
        if (value >= 0)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
            return true;
        }
        else {
            return false;
        }
    }

    //增加荣誉值
    public bool AddCountryHonor(int addValue, bool ifHint = false)
    {
        string countryHonor = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        int value = int.Parse(countryHonor) + addValue;
        if (value >= 0)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
            if (ifHint) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("荣誉值增加:" + addValue);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //增加任务进度 3,2,1
    public void UpdataTaskValue(string addTaskType, string addTargetType, string addTaskValue,string targetMonsterID = "0")
    {
        
        //string writeDayTaskID = "";
        string writeDayTaskValue = "";
        string writeDayTaskValueStr = "";
        //获取当前任务ID
        string[] dayTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        //获取当前任务进度
        string[] dayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        //匹配任务进度
        for (int i = 0; i <= dayTaskIDList.Length - 1; i++) {
            //Debug.Log("sssssssssss");
            if (dayTaskIDList[i] != "" && dayTaskIDList[i] != "0") {
                //Debug.Log("s111111111");
                writeDayTaskValue = dayTaskValue[i];
                //获取当前任务大类
                string taskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", dayTaskIDList[i], "TaskCountry_Template");
                if (addTaskType == taskType)
                {
                    //Debug.Log("s2222222222");
                    //获取子类
                    string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", dayTaskIDList[i], "TaskCountry_Template");
                    if (targetType == addTargetType)
                    {
                        //获取当前数量
                        writeDayTaskValue = (int.Parse(dayTaskValue[i]) + int.Parse(addTaskValue)).ToString();
                        //Debug.Log("s3333333333 : " + writeDayTaskValue);
                    }
                    //Debug.Log("addTaskType = " + addTaskType + "    addTargetType = " + addTargetType);
                    //检测指定目标事件处理
                    if (addTaskType == "3") {
                        switch (addTargetType) {
                            //指定抽卡
                            case "3":
                                //获取子类
                               targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", dayTaskIDList[i], "TaskCountry_Template");
                                if (targetType == addTargetType)
                                {
                                    //获取当前数量
                                    writeDayTaskValue = (int.Parse(dayTaskValue[i]) + int.Parse(addTaskValue)).ToString();
                                }
                            break;


                            //指定怪物
                            case "4":
                                if (targetMonsterID != "0") {

                                    string targetValue1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", dayTaskIDList[i], "TaskCountry_Template");
                                    if (targetMonsterID != targetValue1)
                                    {
                                        Debug.Log("dayTaskValue[3] = " + dayTaskValue[3] + "    targetValue1 = " + targetValue1);
                                        //writeDayTaskValue = "0";
                                        if (dayTaskValue[3] != targetValue1)
                                        {
                                            writeDayTaskValue = "0";
                                        }
                                        else {
                                            writeDayTaskValue = targetValue1;
                                        }
                                        Debug.Log("s4444444444 : " + writeDayTaskValue);
                                    }
                                    else {
                                        writeDayTaskValue = targetMonsterID;
                                        //Debug.Log("s5555555555 : " + writeDayTaskValue);
                                    }
                                }
                            break;
                        }
                    }
                }
                writeDayTaskValueStr = writeDayTaskValueStr + writeDayTaskValue + ";";
            }
        }
        if (writeDayTaskValueStr != "") {
            writeDayTaskValueStr = writeDayTaskValueStr.Substring(0, writeDayTaskValueStr.Length - 1);
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", writeDayTaskValueStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        /*
            switch (addTaskType)
            {
                //建设任务
                case "1":
                    switch (addTargetType)
                    {
                        //抽卡
                        case "1":
                            break;
                        //国王大厅升级
                        case "2":
                            break;
                        case "3":
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                    }
                    break;
                //杀怪任务
                case "2":

                    break;
                //隐藏人物
                case "3":

                    break;
            }
        */
    }
}
