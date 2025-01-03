﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EquipMake : MonoBehaviour {

    public string ItemID;
    private string makeEquipID;                 //制作书ID
    public GameObject Obj_MakeItemNameTitle;    //制作书标题
    public GameObject Obj_MakeItemLv;           //制造书等级
    public GameObject Obj_MakeItemStar;         //制造书星级
    public GameObject Obj_MakeItemNum;          //制造书等级
    public GameObject Obj_MakeItemName;         //制造书名称
    public GameObject Obj_MakeItemQuality;      //制造道具品质显示
    public GameObject Obj_MakeItemIcon;         //制造道具图标显示
    public GameObject Obj_MakeSuccesPro;
    public GameObject Obj_MakeNeedGold;
    public GameObject Obj_MakeNeedItem;         //制造需求的道具源Obj
    public GameObject Obj_MakeEquipNeedItemSet;
    private string makeItemID;                     //制造道具的ID
    private string makeItemName;
    private int makeItemLv;
    private int makeEquipNum;
    private float makeSuccessPro;
    private int makeNeedGold;                 //制造道具需要金币
    private GameObject obj_ItemTips;         //制造道具的Tips
    private bool ClickMakeBtn;              //制造按钮,防止在卡顿的时候多次执行制作操作

	// Use this for initialization
	void Start () {
        GetMakeData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //获取制作数据
    void GetMakeData() {

        //获取制造ID
        makeEquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        string makeStar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeStar", "ID", makeEquipID, "EquipMake_Template");
        makeItemLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeLv", "ID", makeEquipID, "EquipMake_Template"));
        makeEquipNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeEquipNum", "ID", makeEquipID, "EquipMake_Template"));
        makeSuccessPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeSuccessPro", "ID", makeEquipID, "EquipMake_Template"));
        //makeNeedGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeNeedGold", "ID", makeEquipID, "EquipMake_Template"));
        makeNeedGold = 1000;
        makeItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeItemID", "ID", makeEquipID, "EquipMake_Template");
        makeItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", makeItemID, "Item_Template");
        string makeItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", makeItemID, "Item_Template");
        string makeItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", makeItemID, "Item_Template");

        //UI赋值
        Obj_MakeItemNameTitle.GetComponent<Text>().text = itemName;
        if (makeItemLv <= 1)
        {
            Obj_MakeItemLv.GetComponent<Text>().text = "制造等级：无限制";
        }
        else {
            Obj_MakeItemLv.GetComponent<Text>().text = "制造等级：" + makeItemLv;
        }
        //Obj_MakeItemStar.GetComponent<Text>().text = "制造数量：" + makeEquipNum;


        //最低显示1%成功概率
        int showMakeSuccessPro = (int)(makeSuccessPro * 100);
        if (showMakeSuccessPro <= 0) {
            showMakeSuccessPro = 1;
        }

        Obj_MakeItemNum.GetComponent<Text>().text = "制造数量：" + makeEquipNum + "个";
        Obj_MakeSuccesPro.GetComponent<Text>().text = "成功概率：" + showMakeSuccessPro + "%";
        Obj_MakeItemStar.GetComponent<Text>().text = "制造难度：" + makeStar + "星";
        Obj_MakeNeedGold.GetComponent<Text>().text = "消耗金币：" + makeNeedGold;
        //合成道具名称
        Obj_MakeItemName.GetComponent<Text>().text = makeItemName;

        //显示道具Icon
        object obj = Resources.Load("ItemIcon/" + makeItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_MakeItemIcon.GetComponent<Image>().sprite = itemIcon;

        //显示品质
        object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(makeItemQuality), typeof(Sprite));
        Sprite itemQuality = obj2 as Sprite;
        Obj_MakeItemQuality.GetComponent<Image>().sprite = itemQuality;

        //检测金币是否足够
        //检测金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < makeNeedGold)
        {
            Obj_MakeNeedGold.GetComponent<Text>().color = Color.red;
            Obj_MakeNeedGold.GetComponent<Text>().text += "(金币不足)";
        }
        

        updataNeedItem();   //更新需求道具
    }

    //更新需求道具
    void updataNeedItem() {
        //获取需求道具的数量
        int needItemNum = 0;
        for (int i = 1; i <= 3; i++) {
            if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template") != "0")
            {
                needItemNum = needItemNum + 1;
            }
        }

        GameObject objMakeNeedItem_1 = null;
        GameObject objMakeNeedItem_2 = null;
        GameObject objMakeNeedItem_3 = null;

        //string needItem 

        switch (needItemNum) { 
            case 1:
                createNeedItemObj(1);
                break;
            case 2:
                createNeedItemObj(2);
                break;
            case 3:
                createNeedItemObj(3);
                break;
        }
    }

    //创建制作书需求材料
    void createNeedItemObj(int createNum) {
        for (int i = 1; i <= createNum; i++)
        {
            GameObject objMakeNeedItem = (GameObject)Instantiate(Obj_MakeNeedItem);
            objMakeNeedItem.transform.SetParent(Obj_MakeEquipNeedItemSet.transform);
            objMakeNeedItem.transform.localScale = new Vector3(1, 1, 1);
            switch (createNum) { 
                case 1:
                    objMakeNeedItem.transform.localPosition = Vector3.zero;
                    break;
                case 2:
                    objMakeNeedItem.transform.localPosition = new Vector3(-300 + 200 * i, 0, 0);
                    break;
                case 3:
                    objMakeNeedItem.transform.localPosition = new Vector3(-300 + 150 * i, 0, 0);
                    break;
            }
            string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
            string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum_" + i, "ID", makeEquipID, "EquipMake_Template");
            objMakeNeedItem.GetComponent<MakeEquipNeedItem>().ItemID = needItemID;
            objMakeNeedItem.GetComponent<MakeEquipNeedItem>().NeedItemNum = int.Parse(needItemNum);
        }
    }

    //显示制造道具的Tips
    public void Btn_MakeItemTips() {
        if (obj_ItemTips == null)
        {
            //获取当前Tips栏内是否有Tips,如果有就清空处理
            
            GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
            for (int i = 0; i < parentObj.transform.childCount; i++)
            {
                GameObject go = parentObj.transform.GetChild(i).gameObject;
                Destroy(go);
            }
            
            //实例化Tips
            Debug.Log("makeEquipID = " + makeEquipID + ";" + ItemID);
            string itemShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeItemID", "ID", makeEquipID, "EquipMake_Template");
            obj_ItemTips = Game_PublicClassVar.Get_function_UI.UI_ItemTips(itemShowID, Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet);
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemShowID, "Item_Template");

            //获取目标是否是装备
            if (itemType == "3")
            {
                //obj_ItemTips.GetComponent<UI_EquipTips>().UIBagSpaceNum = BagPosition;
                obj_ItemTips.GetComponent<UI_EquipTips>().EquipTipsType = "3";
                //获取极品属性
                //string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", BagPosition, "RoseBag");
                //obj_ItemTips.GetComponent<UI_EquipTips>().ItemHideID = hideID;
            }
            else
            {
                //其余默认为道具,如果其他道具需做特殊处理
                //obj_ItemTips.GetComponent<UI_ItemTips>().UIBagSpaceNum = BagPosition;
                obj_ItemTips.GetComponent<UI_ItemTips>().EquipTipsType = "3";
            }
        }
        else {
            Destroy(obj_ItemTips);
        }
    }


    //点击制造按钮
    public void Btn_MakeItem() {

        //如果当前处于制作中,则不返回任何制造
        if (ClickMakeBtn) {
            return;
        }

       //Debug.Log("我制造了一件装备");
        bool makeStatus = true;
        ClickMakeBtn = true;

        //检测等级是否达到
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv < makeItemLv) {
            makeStatus = false;
            Game_PublicClassVar.Get_function_UI.GameHint("制作书要求角色等级不足！");
            ClickMakeBtn = false;
            return;
        }

        //检测本身制造制作书是否存在
        int makeNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        //当某一材料未达成合成显示失败
        if (makeNum < 1)
        {
            makeStatus = false;
            Game_PublicClassVar.Get_function_UI.GameHint("制造书不在背包当中!");
            ClickMakeBtn = false;
            return;
        }

        //检测道具是否足够
        for (int i = 1; i <= 3; i++)
        {
            string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
            if (needItemID != "0") {
                string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum_" + i, "ID", makeEquipID, "EquipMake_Template");
                int selfItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(needItemID);
                //当某一材料未达成合成显示失败
                if (selfItemNum < int.Parse(needItemNum))
                {
                    makeStatus = false;
                }
            }
        }

        //检测金币是否足够
        if (Game_PublicClassVar.Get_function_Rose.GetRoseMoney() < makeNeedGold)
        {
            //金币不足
            Game_PublicClassVar.Get_function_UI.GameHint("制造需要的金币不足!");
            makeStatus = false;
            ClickMakeBtn = false;
            return;
        }




        //Debug.Log("makeStatus = " + makeStatus);
        //制造成功
        if (makeStatus)
        {
            //扣除指定金币
            /*
            bool costGoldStatus = Game_PublicClassVar.Get_function_Rose.CostReward("1", makeNeedGold.ToString());
            if (!costGoldStatus)
            {
                //金币不足
                Game_PublicClassVar.Get_function_UI.GameHint("制造需要的金币不足!");
                return;
            }
            */
            //扣除对应道具
            for (int i = 1; i <= 3; i++)
            {
                string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID_" + i, "ID", makeEquipID, "EquipMake_Template");
                if (needItemID != "0") {
                    string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum_" + i, "ID", makeEquipID, "EquipMake_Template");
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(needItemID, int.Parse(needItemNum));
                }
            }
            Debug.Log("makeSuccessPro = " + makeSuccessPro);
            //获取成功概率
            if (Random.value <= makeSuccessPro)
            {
                string hintStr = "制造成功！获得装备：" + makeItemName;
                //发送对应奖励
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", makeItemID, "Item_Template");
                if (itemType == "3")
                {
                    //触发随机属性
                    string makeHintPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MakeHintPro", "ID", makeEquipID, "EquipMake_Template");
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum, "1", float.Parse(makeHintPro));
                    /*
                    if (makeHintPro!="0"){
                        //hintStr = "恭喜你制造的装备获得隐藏属性！获得装备：" + makeItemName;
                    }
                    */
                    Btn_CloseUI();
                }
                else {
                    //其余道具直接发送到背包中
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag(makeItemID, makeEquipNum);
                }

                //扣除背包制造卷轴
                Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                Game_PublicClassVar.Get_function_UI.GameHint(hintStr);

            }
            else { 
                //制造失败
                Game_PublicClassVar.Get_function_UI.GameHint("制造失败,下次制造概率提升！");
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameHint("制造材料不足");
        }

        //制造完毕,防止卡顿时多次制造道具
        ClickMakeBtn = false;
    }

    //关闭UI
    public void Btn_CloseUI() {

        //获取当前Tips栏内是否有Tips,如果有就清空处理

        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        Destroy(this.gameObject);
    }
}
