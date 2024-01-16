using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DayTaskList : MonoBehaviour
{
    public string dayTaskID;
    public GameObject Obj_dayTaskIcon;
    public GameObject Obj_dayTaskDes;
    public GameObject Obj_dayTaskReward;
    private int dayTaskNum;
    private int dayTaskNumMax;
    private string dayTaskRewardType;
    private int dayTaskRewardValue;

    // Use this for initialization
    void Start()
    {
        //显示图标
        string dayTaskType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskType", "ID", dayTaskID, "TaskCountry_Template");
        object obj = Resources.Load("GameUI/Image/DayTaskIcon_" + dayTaskType, typeof(Sprite));
        Sprite taskIcon = obj as Sprite;
        Obj_dayTaskIcon.GetComponent<Image>().sprite = taskIcon;

        string[] dayTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        string[] dayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');

        for (int i = 0; i <= dayTaskIDList.Length - 1; i++) {
            if (dayTaskIDList[i] == dayTaskID) {
                dayTaskNum = int.Parse(dayTaskValue[i]);
            }
        }
        dayTaskNumMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetValue1", "ID", dayTaskID, "TaskCountry_Template"));
        //显示描述
        string dayTaskDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TaskDes", "ID", dayTaskID, "TaskCountry_Template");
        Obj_dayTaskDes.GetComponent<Text>().text = "任务内容：" + dayTaskDes + "(" + dayTaskNum + "/" + dayTaskNumMax+")";

        //指定击杀某个怪物需要特殊显示要不会显示出ID
        if (dayTaskType == "3")
        {
            string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", dayTaskID, "TaskCountry_Template");
            if (targetType == "4")
            {
                //指定怪物特殊处理(指定击杀目标仅限击杀1次)
                if (dayTaskNum != 0)
                {
                    //dayTaskNum = 1;
                    Obj_dayTaskDes.GetComponent<Text>().text = "任务内容：" + dayTaskDes + "(" + "1" + "/" + "1" + ")";
                }
                else {
                    Obj_dayTaskDes.GetComponent<Text>().text = "任务内容：" + dayTaskDes + "(" + dayTaskNum + "/" + "1" + ")";
                }
                
            }
        }

        //显示奖励
        dayTaskRewardType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardType", "ID", dayTaskID, "TaskCountry_Template");
        float rewardValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RewardValue", "ID", dayTaskID, "TaskCountry_Template"));
        string showRewardStr = "";
        switch (dayTaskRewardType) {
            //角色经验
            case "1":
                int lv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                int pro1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpPro", "RoseLv",lv.ToString(), "RoseExp_Template"));
                dayTaskRewardValue = (int)(rewardValue * pro1*45);
                showRewardStr = "角色经验：";
                break;
            //金币
            case "2":
                lv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                pro1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseGoldPro", "RoseLv",lv.ToString(), "RoseExp_Template"));
                dayTaskRewardValue = (int)(rewardValue * pro1*30);
                showRewardStr = "金币：";
                break;
            //国家繁荣度
            case "3":
                lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                pro1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureExp", "ID", lv.ToString(), "Country_Template"));
                dayTaskRewardValue = (int)(rewardValue * pro1);
                showRewardStr = "国家繁荣度：";
                break;
            //荣誉
            case "4":
                lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
                pro1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureHonor", "ID", lv.ToString(), "Country_Template"));
                dayTaskRewardValue = (int)(rewardValue * pro1);
                showRewardStr = "荣誉：";
                break;
        }
        showRewardStr = showRewardStr + dayTaskRewardValue;
        Obj_dayTaskReward.GetComponent<Text>().text = showRewardStr;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Btn_TaskReward() {
        if (dayTaskNum >= dayTaskNumMax)
        {
            //发送奖励
            //Debug.Log("1");
            switch (dayTaskRewardType)
            {
                //角色经验
                case "1":
                    Game_PublicClassVar.Get_function_Rose.AddExp(dayTaskRewardValue);
                    break;
                //金币
                case "2":
                    Game_PublicClassVar.Get_function_Rose.SendReward("1", dayTaskRewardValue.ToString());
                    break;
                //国家繁荣度
                case "3":
                    Game_PublicClassVar.Get_function_Country.addCoutryExp(dayTaskRewardValue);
                    break;
                //荣誉
                case "4":
                    Game_PublicClassVar.Get_function_Country.AddCountryHonor(dayTaskRewardValue);
                    break;
            }
            //Debug.Log("2");
            //删除任务更新列表
            Destroy(this.gameObject);
            //删除对应任务数据
            string write_dayTaskIDList = "";
            string write_dayTaskValue = "";
            //Debug.Log("4");
            string[] dayTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
            string[] dayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
            for (int i = 0; i <= dayTaskIDList.Length - 1; i++)
            {
                //实例化任务列表
                if (dayTaskIDList[i] != "" && dayTaskIDList[i] != "0")
                {
                    if (dayTaskIDList[i] != dayTaskID)
                    {
                        write_dayTaskIDList = write_dayTaskIDList + dayTaskIDList[i] + ";";
                        write_dayTaskValue = write_dayTaskValue + dayTaskValue[i] + ";";
                    }
                }
            }
            //Debug.Log("5");
            if (write_dayTaskIDList != "")
            {
                write_dayTaskIDList = write_dayTaskIDList.Substring(0, write_dayTaskIDList.Length - 1);
                write_dayTaskValue = write_dayTaskValue.Substring(0, write_dayTaskValue.Length - 1);
            }
            //Debug.Log("6");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskID", write_dayTaskIDList, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayTaskValue", write_dayTaskValue, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        }
        else {
            Game_PublicClassVar.Get_function_UI.GameHint("领取奖励条件不足");
        }
    }
}