using System.Collections;
using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;
using UnityEngine.UI;

public class UI_TodayGift : MonoBehaviour
{
    public GameObject Obj_RewardItemObjSet;
    public GameObject Obj_RewardItemObj;
    public GameObject Obj_Num;


    void Start()
    {
        Set();
    }

    public void Set()
    {
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RewardItemObjSet);
        
        string[] rewardStr = "3,500".Split(';');
        
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

        //显示次数
        string num = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TodayGiftStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_Num.GetComponent<Text>().text = "今日领取:" + num +"/3";

    }

    public void OnBtn_Get()
    {
        string yueKaDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TodayGiftStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (int.Parse(yueKaDayStatus) >= 3) {
            Game_PublicClassVar.Get_function_UI.GameHint("每日领取奖励最多3次喔,请明日再来!");
            return;
        }
        
        string[] rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "TodayGiftReward", "GameMainValue").Split(';');
        //检测背包格子
        if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(rewardStr.Length))
        {
            AdManager.Instance.RewardVideoAd_OnRewardArrived = OnRewardArrived;
            AdManager.Instance.LoadRewardAd();
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameHint("背包请预留至少" + rewardStr.Length + "个位置!");
        }
    }
    
    // 观看完成后的回调
    private void OnRewardArrived(bool isRewardValid, int rewardType, IRewardBundleModel extraInfo)
    {
        string[] rewardStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "TodayGiftReward", "GameMainValue").Split(';');

        // 记录领取
        string num = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TodayGiftStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        num = num + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TodayGiftStatus", num, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

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
            .GetComponent<UI_FunctionOpen>().Open_TodayGift();
    }
}