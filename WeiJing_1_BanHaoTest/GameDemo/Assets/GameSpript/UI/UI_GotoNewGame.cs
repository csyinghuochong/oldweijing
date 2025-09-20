using System.Collections;
using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;

public class UI_GotoNewGame : MonoBehaviour
{
    public GameObject Obj_RewardItemObjSet;
    public GameObject Obj_RewardItemObj;
    public string[] rewardStr;

    void Start()
    {
        rewardStr = "3,500".Split(';');
        Set();
    }

    public void Set()
    {
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RewardItemObjSet);

        //显示奖励
        for (int i = 0; i <= rewardStr.Length - 1; i++)
        {
            string[] rewardYueKaStr = rewardStr[i].Split(',');
            GameObject itemObj = (GameObject)Instantiate(Obj_RewardItemObj);
            itemObj.transform.SetParent(Obj_RewardItemObjSet.transform);
            // itemObj.transform.localPosition = new Vector3(100 * i - 50, 0, 0);
            itemObj.transform.localScale = new Vector3(1, 1, 1);
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemID = rewardYueKaStr[0];
            itemObj.GetComponent<UI_ChouKaItemObj>().ItemNum = rewardYueKaStr[1];
        }
    }

    public void OnBtn_Get()
    {
        string yueKaDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NewGameStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (yueKaDayStatus == "1") {
            Game_PublicClassVar.Get_function_UI.GameHint("已经领取");
            return;
        }

        // 记录领取
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NewGameStatus", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        Application.OpenURL("https://www.taptap.cn/app/271100");

        //领取奖励
        OnRewardArrived();
    }
    
    // 观看完成后的回调
    private void OnRewardArrived()
    {
        //发送奖励
        for (int i = 0; i <= rewardStr.Length - 1; i++)
        {
            string[] rewardYueKaStr = rewardStr[i].Split(',');
            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardYueKaStr[0], int.Parse(rewardYueKaStr[1]));
        }
    }

    public void CloseUI()
    {
        Debug.Log("关闭界面");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen
            .GetComponent<UI_FunctionOpen>().Open_GotoNewGame();
    }
}