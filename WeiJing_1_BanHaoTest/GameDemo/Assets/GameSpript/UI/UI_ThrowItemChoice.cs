using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ThrowItemChoice : MonoBehaviour {
    public string ItemID;
    public GameObject Obj_ThrowItemName;
    public string BagSpaceNum;
	// Use this for initialization
	void Start () {
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Obj_ThrowItemName.GetComponent<Text>().text = itemName;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //丢弃一个
    public void ThrowOne() {
        Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, BagSpaceNum, false);
        //获取道具的出售金额
        string sellValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", ItemID, "Item_Template");
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", int.Parse(sellValue));
        //Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(UIBagSpaceNum);
        //获取当前格子剩余数量
        string spaceNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", BagSpaceNum, "RoseBag");
        if (int.Parse(spaceNum) <= 0) {
            //获取当前Tips栏内是否有Tips,如果有就清空处理
            GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
            for (int i = 0; i < parentObj.transform.childCount; i++)
            {
                GameObject go = parentObj.transform.GetChild(i).gameObject;
                Destroy(go);
            }
        }

        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;   //更新背包数据

    }

    //丢弃全部个
    public void ThrowAll() {
        //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, BagSpaceNum, true);
        Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(BagSpaceNum);
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;   //更新背包数据
    }
}
