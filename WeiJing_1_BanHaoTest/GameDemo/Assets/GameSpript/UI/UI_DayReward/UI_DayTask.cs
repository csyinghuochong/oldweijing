using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DayTask : MonoBehaviour
{
    public GameObject Obj_DayTaskListSet;
    public GameObject Obj_DayTaskList;
    private float taskNum;
    private string[] dayTaskIDList;
    private string[] dayTaskValue;
    // Use this for initialization
    void Start()
    {
        dayTaskIDList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        dayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward").Split(';');
        for (int i = 0; i <= dayTaskIDList.Length - 1; i++) { 
            //实例化任务列表
            if (dayTaskIDList[i] != "" && dayTaskIDList[i] != "0") {
                GameObject dayTaskObj = (GameObject)Instantiate(Obj_DayTaskList);
                dayTaskObj.transform.SetParent(Obj_DayTaskListSet.transform);
                dayTaskObj.transform.localScale = new Vector3(1, 1, 1);
                dayTaskObj.GetComponent<UI_DayTaskList>().dayTaskID = dayTaskIDList[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Btn_CloseUI() {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }

}