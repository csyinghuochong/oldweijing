using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EquipXiLian : MonoBehaviour {

    public GameObject Obj_EquipItem;            //放入的装备图标
    public GameObject Obj_EquipQuality;            //放入的装备图标
    public GameObject Obj_EquipXiLianGold;      //金币洗练消耗金币
    public GameObject Obj_EquipXiLianItem;      //道具洗练消耗道具数量
    public GameObject Obj_EquipXiLianBtn_Gold;  //金币洗练
    public GameObject Obj_EquipXiLianBtn_Item;  //道具洗练
    private string bagSpaceNum;                 //背包格子
    private string moveItemID;                  //移动道具ID
    public string xiLianItemID;                //洗练道具ID
    private int xiLianNeedGold;                 //洗练金币
    private string[] xiLianNeedItem;                 //洗练需要道具
    private GameObject obj_ItemTips;            //道具Tips

	// Use this for initialization
	void Start () {
	    
        //默认打开背包
        GameObject functionOpen = GameObject.FindWithTag("UI_FunctionOpen");
        if (!functionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status)
        {
            functionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
        }
        Obj_EquipXiLianGold.GetComponent<Text>().text = "消耗金币：0";
        Obj_EquipXiLianItem.GetComponent<Text>().text = "消耗洗炼石：0";

        Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus = true;      //打开装备洗练状态
        XiLian_Item();  //默认显示道具洗练

	}
	
	// Update is called once per frame
	void Update () {
        //触发移动注销此界面
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus != "1")
        {
            Destroy(this.gameObject);

            //关闭背包
            if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseBag_Status == true)
            {
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenBag();
            }
        }
	}

    //被销毁时调用
    void OnDisable()
    {
        Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus = false;      //关闭装备洗练状态
    }

    public void MouseEnter() {

        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1") {
            bagSpaceNum = Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial;
            moveItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
            if (moveItemID != "")
            { 
                //判定ID是否为装备
                if (moveItemID[0] == '1') {
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                    if (itemType == "3") {
                        //显示道具Icon
                        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", moveItemID, "Item_Template");
                        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
                        Sprite itemIcon = obj as Sprite;
                        Obj_EquipItem.GetComponent<Image>().sprite = itemIcon;

                        //显示品质
                        string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", moveItemID, "Item_Template");
                        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
                        Sprite itemQuality = obj2 as Sprite;
                        Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;

                        //显示洗练金币
                        xiLianNeedGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianMoney", "ID", moveItemID, "Item_Template"));
                        Obj_EquipXiLianGold.GetComponent<Text>().text = "消耗金币：" + xiLianNeedGold;
                        //显示洗练道具
                        //Debug.Log("ssssssssssssss = " + Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianMoney", "ID", moveItemID, "Item_Template"));
                        xiLianNeedItem = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianStoneNeedNum", "ID", moveItemID, "Item_Template").Split(',');
                        int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
                        Obj_EquipXiLianItem.GetComponent<Text>().text = "消耗洗炼石：" + itemBagNum + "/" + xiLianNeedItem[1];
                    }
                }
            }

            //防止背包移动
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
        }
    }

    public void Btn_EquipXiLian() {
        //不显示Tips
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }

        //判定洗练金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < xiLianNeedGold) {
            Game_PublicClassVar.Get_function_UI.GameHint("金币不足");
            return;
        }

        //金币洗炼40%概率成功
        if (Random.value > 0.4f) {
            Game_PublicClassVar.Get_function_UI.GameHint("装备洗练失败！");
            //扣除金币
            Game_PublicClassVar.Get_function_Rose.CostReward("1", xiLianNeedGold.ToString());
            return;
        }
        
        if (bagSpaceNum != "" && bagSpaceNum != "0") {
            xiLianItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
            if (xiLianItemID != "")
            {
                //判定ID是否为装备
                if (xiLianItemID[0] == '1')
                {
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", xiLianItemID, "Item_Template");
                    if (itemType == "3")
                    {
                        if (xiLianItemID == moveItemID) {
                            //获得洗练装备ID
                            Debug.Log("洗练装备出发,洗练背包第：" + bagSpaceNum);
                            string hideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID(xiLianItemID);
                            if (hideID == "0") {
                                Game_PublicClassVar.Get_function_UI.GameHint("装备洗练失败！");
                                return;
                            }
                            Debug.Log("hideID = " + hideID);
                            //写入极品属性ID
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", hideID, "ID", bagSpaceNum, "RoseBag");
                            //扣除金币
                            Game_PublicClassVar.Get_function_Rose.CostReward("1",xiLianNeedGold.ToString());
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                            Game_PublicClassVar.Get_function_UI.GameHint("装备洗练成功！");
                            return;
                        }
                    }
                }
            }
        }
        Game_PublicClassVar.Get_function_UI.GameGirdHint("请放入将要洗练的装备");
        return;
    }


    public void Btn_EquipXiLian_Item()
    {
        //不显示Tips
        if (obj_ItemTips != null)
        {
            Destroy(obj_ItemTips);
        }

        //判定洗练道具是否足够
        int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
        if (itemBagNum < int.Parse(xiLianNeedItem[1]))
        {
            Game_PublicClassVar.Get_function_UI.GameHint("洗炼石不足");
            return;
        }

        if (bagSpaceNum != "" && bagSpaceNum != "0")
        {
            xiLianItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", bagSpaceNum, "RoseBag");
            if (xiLianItemID != "")
            {
                //判定ID是否为装备
                if (xiLianItemID[0] == '1')
                {
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", xiLianItemID, "Item_Template");
                    if (itemType == "3")
                    {
                        if (xiLianItemID == moveItemID)
                        {
                            //获得洗练装备ID
                            Debug.Log("洗练装备出发,洗练背包第：" + bagSpaceNum);
                            string hideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID(xiLianItemID);
                            if (hideID == "0")
                            {
                                //循环洗练10次,防止不出洗练属性
                                for (int i = 0; i <= 10; i++) {
                                    hideID = Game_PublicClassVar.Get_function_Rose.ReturnHidePropertyID(xiLianItemID);
                                    if (hideID != "0") {
                                        i = 11;     //跳出循环
                                    }
                                }
                                if (hideID == "0") {
                                    Game_PublicClassVar.Get_function_UI.GameHint("装备洗练成功,但是属性并没有发生变化T.T！");
                                    return;
                                }
                                
                            }
                            Debug.Log("hideID = " + hideID);
                            //写入极品属性ID
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", hideID, "ID", bagSpaceNum, "RoseBag");
                            //扣除道具
                            Game_PublicClassVar.Get_function_Rose.CostBagItem(xiLianNeedItem[0], int.Parse(xiLianNeedItem[1]));
                            Game_PublicClassVar.Get_function_Rose.CostReward(xiLianNeedItem[0], xiLianNeedItem[1]);
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                            Game_PublicClassVar.Get_function_UI.GameHint("装备洗练成功！");
                            //更新洗练道具数量
                            int xiLianBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(xiLianNeedItem[0]);
                            Obj_EquipXiLianItem.GetComponent<Text>().text = "消耗洗炼石：" + xiLianBagNum + "/" + xiLianNeedItem[1];
                            return;
                        }
                    }
                }
            }
        }
        Game_PublicClassVar.Get_function_UI.GameGirdHint("请放入将要洗练的装备");
        return;
    }

    //显示装备ID
    public void Btn_Click() {
        if (obj_ItemTips == null)
        {
            if (moveItemID != "" && moveItemID != "0")
            {
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItemID, "Item_Template");
                obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(moveItemID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
                //获取目标是否是装备
                if (itemType == "3")
                {
                    obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = bagSpaceNum;
                    obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "1";
                    //获取极品属性
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", bagSpaceNum, "RoseBag");
                    obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
                }
            }
        }
        else {
            Destroy(obj_ItemTips);
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
        }
    }

    //点击洗练金币
    public void XiLian_Gold() {
        Obj_EquipXiLianBtn_Gold.SetActive(true);
        Obj_EquipXiLianBtn_Item.SetActive(false);
        Obj_EquipXiLianGold.SetActive(true);
        Obj_EquipXiLianItem.SetActive(false);

    }

    //点击洗练道具
    public void XiLian_Item()
    {
        Obj_EquipXiLianBtn_Gold.SetActive(false);
        Obj_EquipXiLianBtn_Item.SetActive(true);
        Obj_EquipXiLianGold.SetActive(false);
        Obj_EquipXiLianItem.SetActive(true);
    }

    public void Btn_Close() {
        Destroy(obj_ItemTips);
        Destroy(this.gameObject);
    }
}
