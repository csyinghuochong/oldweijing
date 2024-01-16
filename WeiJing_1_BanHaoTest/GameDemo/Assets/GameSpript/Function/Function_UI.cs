using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Function_UI 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //根据道具品质返回对应的品质框
    //ItemQuality  道具品质
    public string ItemQualiytoPath(string ItemQuality) {

        string path ="";

        switch (ItemQuality)
        {
            case "1":
                path = "ItemQuality/ItemQuality_1";
            break;

            case "2":
                path = "ItemQuality/ItemQuality_2";
            break;

            case "3":
                path = "ItemQuality/ItemQuality_3";
            break;

            case "4":
                path = "ItemQuality/ItemQuality_4";
            break;

            case "5":
                path = "ItemQuality/ItemQuality_5";
            break;
        
        }

        return path;

    }

    //根据ID返回装备ICON路径
    public string EquipIconToPath(string equipIcon) {

        string path = "";
		path = "ItemIcon/" + equipIcon;
        return path;
    }

    //展示道具Tips
    public GameObject UI_ItemTips(string itemID, GameObject parentObj,bool ifShowEquipTips = true,string equipHindID = "0")
    {

        //如果传入的值为0则直接返回空
		if (itemID == "0") {
			return null;
		}
		//获取道具类型
        string ItemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
        //打开UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(true);
        
		//Debug.Log ("ID:"+itemID+" 类型为"+ItemType);
        GameObject itemTips_1 = null;

        switch (ItemType)
        {
            //消耗品道具
            case "1":
                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemTips);
                UI_ItemTips ui_ItemTipsShow_1 = itemTips_1.GetComponent<UI_ItemTips>();
                ui_ItemTipsShow_1.ItemID = itemID;
                
                //设置UI出现的位置
                Vector2 mouseVec2 = Input.mousePosition;
                itemTips_1.transform.SetParent(parentObj.transform);
				itemTips_1.transform.localScale= new Vector3(1,1,1);
				Vector3 v3 = new Vector3(mouseVec2.x,mouseVec2.y,0);
                //v3 = RetrunScreenV2(v3);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);
				break;
            //材料道具
            case "2":

                itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIItemTips);
                ui_ItemTipsShow_1 = itemTips_1.GetComponent<UI_ItemTips>();
                ui_ItemTipsShow_1.ItemID = itemID;
                
                //设置UI出现的位置
                mouseVec2 = Input.mousePosition;
                itemTips_1.transform.SetParent(parentObj.transform);
				itemTips_1.transform.localScale= new Vector3(1,1,1);
				v3 = new Vector3(mouseVec2.x,mouseVec2.y,0);
                //v3 = RetrunScreenV2(v3);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);
				
                break;

            //装备道具
            case "3":
				mouseVec2 = Input.mousePosition;
				itemTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIEquipTips);
				itemTips_1.transform.SetParent(parentObj.transform);
				itemTips_1.transform.localScale= new Vector3(1,1,1);
				itemTips_1.GetComponent<UI_EquipTips>().ItemID = itemID;
                if (equipHindID != "0" && equipHindID!="")
                {
                    itemTips_1.GetComponent<UI_EquipTips>().ItemHideID = equipHindID;
                }
                
				v3 = new Vector3(mouseVec2.x,mouseVec2.y,0);
                itemTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);

                //----------------显示对比装备---------------
                if (ifShowEquipTips)
                {
                    //获取装备位置
                    string ItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                    //获取当前身上对应位置的装备
                    string roseEquipID = EquipSubType(ItemSubType);
                    if (roseEquipID != "5" && roseEquipID != "0")
                    {
                        string equipID_DuiBi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", roseEquipID, "RoseEquip");
                        string equipHideID_DuiBi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", roseEquipID, "RoseEquip");
                        //不为空展示Tips
                        if (equipID_DuiBi != "0" && equipID_DuiBi != "")
                        {
                            GameObject itemTips_2 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIEquipTips);
                            itemTips_2.transform.SetParent(parentObj.transform);
                            itemTips_2.transform.localScale = new Vector3(1, 1, 1);
                            itemTips_2.GetComponent<UI_EquipTips>().ItemID = equipID_DuiBi;
                            itemTips_2.GetComponent<UI_EquipTips>().ItemHideID = equipHideID_DuiBi;
                            v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
                            itemTips_2.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen_DuiBi(v3);
                            itemTips_2.GetComponent<UI_EquipTips>().Obj_ImgYiChuanDai.SetActive(true);          //设置已装备图标
                            itemTips_2.GetComponent<UI_EquipTips>().EquipTipsType = "5";
                            itemTips_2.transform.SetSiblingIndex(0);
                        }
                    }
                }

				break;

            default:

                return itemTips_1;

                break;
        }
        //设置父级Tips
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.GetComponent<UI_CloseTips>().TipsParent = itemTips_1;
        
        return itemTips_1;

    }

	
	//展示技能Tips
    public GameObject UI_SkillTips(string skillID, GameObject parentObj) {

        //如果传入的值为0则直接返回空
        if (skillID == "0")
        {
            return null;
        }
        //获取道具类型
        GameObject skillTips_1 = null;
        skillTips_1 = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UISkillTips);
        UI_SkillTips ui_ItemTipsShow_1 = skillTips_1.GetComponent<UI_SkillTips>();
        ui_ItemTipsShow_1.SkillID = skillID;

        //设置UI出现的位置
        Vector2 mouseVec2 = Input.mousePosition;
        skillTips_1.transform.SetParent(parentObj.transform);
        skillTips_1.transform.localScale = new Vector3(1, 1, 1);
        Vector3 v3 = new Vector3(mouseVec2.x, mouseVec2.y, 0);
        skillTips_1.GetComponent<RectTransform>().anchoredPosition3D = UITipsScreen(v3);

        return skillTips_1;
    
    }
	
	
    //交换用鼠标移动道具的位置的前后顺序
    public bool UI_ItemMouseMove() {

        //Debug.Log("进来了");
        bool moveStatus = true;
        bool ifUpdataProperty = false;      //是否更新属性
        string sourceValue = "1";

        //检索移动不成功的条件
        if (Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("洗练装备时禁止移动道具操作！");
            return false;
        }

        //判定是需要交换的两个道具的容器类型

        //string ItemMoveID_Middle = "";
        //string ItemMoveNum_Middle = "";

        string ItemMoveID_Initial ="";
        string ItemMoveNum_Initial = "";
        string ItemMoveHideID_Initial = "";

        string ItemMoveID_End = "";
        string ItemMoveNum_End = "";
        string ItemMoveHideID_End = "";

        //获取相关数据
        switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial)
        {
            //背包
            case "1":
                ItemMoveID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                ItemMoveNum_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                ItemMoveHideID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                break;
            //装备
            case "2":
                ItemMoveID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                ItemMoveNum_Initial = "1";
                ItemMoveHideID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                break;
            //仓库
            case "3":
                ItemMoveID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                ItemMoveNum_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                ItemMoveHideID_Initial = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                break;

        }

        switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End)
        {
            //背包
            case "1":
                ItemMoveID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                ItemMoveNum_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                ItemMoveHideID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                break;
            //装备
            case "2":
                ItemMoveID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                ItemMoveNum_End = "0";
                ItemMoveHideID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                break;
            //仓库
            case "3":
                ItemMoveID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                ItemMoveNum_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                ItemMoveHideID_End = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                break;
        }


        //获得数据,检测不满足交换的条件
        //背包→装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "2")
        {
            bool ifMoveItem = bagToEquip(ItemMoveID_Initial, ItemMoveID_End);
            //Debug.Log("ItemMoveID_Initial = " + ItemMoveID_Initial + "     ItemMoveID_End = " + ItemMoveID_End);
            if (ifMoveItem)
            {
                //Debug.Log("可以交换");
                //如果当前打开技能界面则关闭,让他刷新一下技能界面
                if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status) {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();
                }
                //更新角色数据
                //Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();
                //Game_PublicClassVar.Get_function_UI.PlaySource("10005", "1");
                sourceValue = "2";    //切换播放音效
                ifUpdataProperty = true;        //更新属性
            }
            else {
                //Debug.Log("不可以交换");
                moveStatus = false;     //暂时关闭交换数据
            }
        }

        //装备→背包
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "2" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "1")
        {
            bool ifMoveItem = equipToBag(ItemMoveID_Initial, ItemMoveID_End);
            if (ifMoveItem)
            {
                //Debug.Log("可以交换");
                sourceValue = "3";    //切换播放音效
                //如果当前打开技能界面则关闭,让他刷新一下技能界面
                if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status)
                {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();
                }
            }
            else
            {
                //Debug.Log("不可以交换");
                moveStatus = false;     //暂时关闭交换数据
            }
        }

		//背包→背包
		if(Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1"&&Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "1"){

            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End == Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial) {
                //Debug.Log("移动位置相同");
                return false;   
            }

			//获取交换的道具ID检测是否为同类
			if(ItemMoveID_Initial == ItemMoveID_End){
                //交换道具为0直接结束方法
                if (ItemMoveID_Initial == "0") {
                    return false; 
                }
				//获取当前背包道具的最大堆叠数量
				int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID",ItemMoveID_End, "Item_Template"));
				if(int.Parse(ItemMoveNum_End)<itemPileSum){
					if(itemPileSum-int.Parse(ItemMoveNum_End)>=int.Parse(ItemMoveNum_Initial)){
						//可以叠加
						int numValue = int.Parse(ItemMoveNum_End)+int.Parse(ItemMoveNum_Initial);
						ItemMoveNum_End = numValue.ToString();
						ItemMoveNum_Initial = "0";

					}else{
						//额外叠加
						int numValue = int.Parse(ItemMoveNum_Initial)-(itemPileSum-int.Parse(ItemMoveNum_End));
						ItemMoveNum_Initial = numValue.ToString();
						ItemMoveNum_End = itemPileSum.ToString();
					}

					//执行交换数据
					if(ItemMoveNum_Initial!="0"){
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
					}else{
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
						Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
					}

					Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
					Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
					Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
					moveStatus = false;
				}
			}
		}

        //装备→装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "2" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "2") {
            //不同部位不可以交换
            string typeSon1 = EquipBagType(Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial);
            string typeSon2 = EquipBagType(Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End);
            Debug.Log("typeSon1 = " + typeSon1 + ";typeSon2 = " + typeSon2);
            if (typeSon1 != typeSon2) {
                moveStatus = false;
            }
        }

        //仓库→装备（仓库不能直接穿戴装备）
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "3" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "2")
        {
            moveStatus = false;
        }

        //装备→仓库（装备不能直接存进仓库）
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "2" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "3")
        {
            moveStatus = false;
        }

        //仓库→背包
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "3" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "1")
        {
            //获取交换的道具ID检测是否为同类
            if (ItemMoveID_Initial == ItemMoveID_End)
            {
                if (ItemMoveID_Initial == "0")
                {
                    //return false;
                }
                //获取当前背包道具的最大堆叠数量
                int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID", ItemMoveID_End, "Item_Template"));
                if (int.Parse(ItemMoveNum_End) < itemPileSum)
                {
                    if (itemPileSum - int.Parse(ItemMoveNum_End) >= int.Parse(ItemMoveNum_Initial))
                    {
                        //可以叠加
                        int numValue = int.Parse(ItemMoveNum_End) + int.Parse(ItemMoveNum_Initial);
                        ItemMoveNum_End = numValue.ToString();
                        ItemMoveNum_Initial = "0";

                    }
                    else
                    {
                        //额外叠加
                        int numValue = int.Parse(ItemMoveNum_Initial) - (itemPileSum - int.Parse(ItemMoveNum_End));
                        ItemMoveNum_Initial = numValue.ToString();
                        ItemMoveNum_End = itemPileSum.ToString();
                    }

                    //执行交换数据
                    if (ItemMoveNum_Initial != "0")
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }
                    else
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    moveStatus = false;
                }
            }
        }

        //仓库→仓库
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "3" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "3")
        {
            //Debug.Log("啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊");
            //Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

            if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End == Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial)
            {
                //Debug.Log("移动位置相同");
                return false;
            }

            //获取交换的道具ID检测是否为同类
            if (ItemMoveID_Initial == ItemMoveID_End)
            {
                if (ItemMoveID_Initial == "0") {
                    return false;
                }
                //获取当前背包道具的最大堆叠数量
                int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID", ItemMoveID_End, "Item_Template"));
                if (int.Parse(ItemMoveNum_End) < itemPileSum)
                {
                    if (itemPileSum - int.Parse(ItemMoveNum_End) >= int.Parse(ItemMoveNum_Initial))
                    {
                        //可以叠加
                        int numValue = int.Parse(ItemMoveNum_End) + int.Parse(ItemMoveNum_Initial);
                        ItemMoveNum_End = numValue.ToString();
                        ItemMoveNum_Initial = "0";

                    }
                    else
                    {
                        //额外叠加
                        int numValue = int.Parse(ItemMoveNum_Initial) - (itemPileSum - int.Parse(ItemMoveNum_End));
                        ItemMoveNum_Initial = numValue.ToString();
                        ItemMoveNum_End = itemPileSum.ToString();
                    }

                    //执行交换数据
                    if (ItemMoveNum_Initial != "0")
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }
                    else
                    {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    moveStatus = false;
                    
                }
            }
        }

        //背包→仓库
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial == "1" && Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End == "3")
        {
            //获取交换的道具ID检测是否为同类
            if (ItemMoveID_Initial == ItemMoveID_End)
            {
                if (ItemMoveID_Initial == "0") {
                    return false;
                }
                Debug.Log("ItemMoveID_Initial = " + ItemMoveID_Initial + "/ItemMoveID_End = " + ItemMoveID_End);
                //获取当前背包道具的最大堆叠数量
                int itemPileSum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPileSum", "ID", ItemMoveID_End, "Item_Template"));
                if (int.Parse(ItemMoveNum_End) < itemPileSum)
                {
                    if (itemPileSum - int.Parse(ItemMoveNum_End) >= int.Parse(ItemMoveNum_Initial))
                    {
                        //可以叠加
                        int numValue = int.Parse(ItemMoveNum_End) + int.Parse(ItemMoveNum_Initial);
                        ItemMoveNum_End = numValue.ToString();
                        ItemMoveNum_Initial = "0";
                        Debug.Log("可以叠加");

                    }
                    else
                    {
                        //额外叠加
                        int numValue = int.Parse(ItemMoveNum_Initial) - (itemPileSum - int.Parse(ItemMoveNum_End));
                        ItemMoveNum_Initial = numValue.ToString();
                        ItemMoveNum_End = itemPileSum.ToString();
                        Debug.Log("不可以叠加");
                    }

                    //执行交换数据
                    if (ItemMoveNum_Initial != "0")
                    {
                        //Debug.Log("11111:" + ItemMoveID_Initial + "--------" + Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial + "----" + ItemMoveNum_Initial);
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    }
                    else
                    {
                        //Debug.Log("22222:" + ItemMoveID_Initial + "--------" + Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial);
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    moveStatus = false;
                }
            }
        }

        //判定移动道具的类型是否为装备

		//判定当前交换是否为

        //Debug.Log("ItemMoveNum_Initial = " + ItemMoveNum_Initial + ";" + "ItemMoveNum_End = " + ItemMoveNum_End);
        //执行交换
        if (moveStatus)
        {

            //执行2的交换
            switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial)
            {

                case "1":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    break;

                case "2":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                    break;
                case "3":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_End, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    break;
            }

            //执行1的交换
            switch (Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End)
            {
                case "1":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
                    break;

                case "2":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseEquip");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquip");
                    break;

                case "3":
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", ItemMoveID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", ItemMoveNum_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", ItemMoveHideID_Initial, "ID", Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End, "RoseStoreHouse");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    break;
            }
        }
        else { 
            //交换失败进行提示
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("交换条件不足");
            return false;
        }

        //更新装备属性
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;

        //交换完毕清空数据
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_Initial = "";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_Initial = "";
        /*
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
        Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
        */
        Game_PublicClassVar.Get_game_PositionVar.UpdataRoseItem = true;
        //播放UI音效
        switch (sourceValue) { 
            //交换
            case "1":
                Game_PublicClassVar.Get_function_UI.PlaySource("10007", "1");
                break;
            //穿戴
            case "2":
                Game_PublicClassVar.Get_function_UI.PlaySource("10005", "1");
                break;
            //卸下
            case "3":
                Game_PublicClassVar.Get_function_UI.PlaySource("10006", "1");
                break;
        }
        if (ifUpdataProperty) {
            Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty(true);
            //Debug.Log("更新了属性");
        }
        return true;   
    }

    //根据装备栏的位置,返回对应的装备类型
    public string EquipBagType(string equipBagValue) {

        switch (equipBagValue) {
            //武器
            case "1":
                return "1";
            break;

            case "2":
                return "2";
            break;

            case "3":
            return "3";
            break;

            case "4":
            return "4";
            break;

            case "5":
            return "5";
            break;

            case "6":
            return "5";
            break;

            case "7":
            return "5";
            break;

            case "8":
            return "6";
            break;

            case "9":
            return "7";
            break;

            case "10":
            return "8";
            break;

            case "11":
            return "9";
            break;

            case "12":
            return "10";
            break;

            case "13":
            return "11";
            break;
        }
        return "0";
    }


    //根据装备栏的位置,返回对应的装备类型
    public string EquipSubType(string equipSubType)
    {

        switch (equipSubType)
        {
            //武器
            case "1":
                return "1";
                break;
            //衣服
            case "2":
                return "2";
                break;
            //护符
            case "3":
                return "3";
                break;
            //灵石
            case "4":
                return "4";
                break;
            //饰品
            case "5":
                return "5";
                break;
            //鞋子
            case "6":
                return "8";
                break;
            //裤子
            case "7":
                return "9";
                break;
            //腰带
            case "8":
                return "10";
                break;
            //手镯
            case "9":
                return "11";
                break;
            //头盔
            case "10":
                return "12";
                break;
            //项链
            case "11":
                return "13";
                break;
        }
        return "0";
    }

    //判定是否可以交换，参数1,背包数据  参数2，装备数据
    private bool bagToEquip(string moveItem1,string moveItem2) {

        string type1 = "";
        string type2 = "";
        string typeSon1 = "";
        string typeSon2 = "";
        
        //Debug.Log("1");
        //判定移动时背包内是否拖动的是空格子
        if (moveItem1 != "0")
        {
            type1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem1, "Item_Template");
            typeSon1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem1, "Item_Template");
            //Debug.Log("2");
            //判定移动到装备是否为空格子
            if (moveItem2 != "0")
            {
                type2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem2, "Item_Template");
                typeSon2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem2, "Item_Template");
                //Debug.Log("3");
            }
            else
            {
                type2 = "3";
                typeSon2 = EquipBagType(Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End);
                //Debug.Log("4");
            }
            //道具类型一样执行下一步筛选
            if (type1 == type2)
            {
                //Debug.Log("5");
                //获取装备子类,查看是否是一个子类
                if (typeSon1 == typeSon2)
                {
                    //Debug.Log("6");
                    //判定装备等级是否达到
                    int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
                    int equipLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", moveItem1, "Item_Template"));
                    if (roseLv >= equipLv) {
                        //判定对应的属性是否达到
                        string equipLimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyLimit", "ID", moveItem1, "Item_Template");
                        if(equipLimit!="0"){
                            string needPropertyType = equipLimit.Split(',')[0];
                            string needPropertyValue = equipLimit.Split(',')[1];
                            switch (needPropertyType) { 
                                //目前先只支持攻击
                                case "1":
                                    //获取自身攻击力
                                    int roseAct = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMax;
                                    if (roseAct >= int.Parse(needPropertyValue)) {
                                        return true;
                                    }
                                    break;
                            }
                        }else{
                            return true;
                        }
                        
                    }
                }
                    /*
                else
                {
                    return false;     //交换失败
                }
                     */
            }
                /*
            else
            {
                return false;     //交换失败
            }
                 */
        }
            /*
        else
        {
            return false;     //交换失败
        }
             */
        return false;     //交换失败
    }


    //判定是否可以交换，参数1,装备数据  参数2，背包数据
    private bool equipToBag(string moveItem1, string moveItem2)
    {
        string type1 = "";
        string type2 = "";
        string typeSon1 = "";
        string typeSon2 = "";

        //判定移动时背包内是否拖动的是空格子
        if (moveItem1 != "0")
        {
            type1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem1, "Item_Template");
            typeSon1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem1, "Item_Template");
            //判定移动到装备是否为空格子
            if (moveItem2 != "0")
            {
                type2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem2, "Item_Template");
                typeSon2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem2, "Item_Template");
            }
            else
            {
                //检测移动装备移动背包内,背包的格子为空，则判定移入成功
                type2 = type1;
                typeSon2 = typeSon1;
            }
            //道具类型一样执行下一步筛选
            if (type1 == type2)
            {
                //获取装备子类,查看是否是一个子类
                if (typeSon1 == typeSon2)
                {
                    return true;
                }
                else
                {
                    return false;     //交换失败
                }
            }
            else
            {
                return false;     //交换失败
            }
        }
        else
        {
            return false;     //交换失败
        }

    }

    
    //判定是否可以交换，参数1,装备数据  参数2，背包数据
    private bool storeHouseToBag(string moveItem1, string moveItem2)
    {
        string type1 = "";
        string type2 = "";
        string typeSon1 = "";
        string typeSon2 = "";

        //判定移动时背包内是否拖动的是空格子
        if (moveItem1 != "0")
        {
            type1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem1, "Item_Template");
            typeSon1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem1, "Item_Template");
            //判定移动到装备是否为空格子
            if (moveItem2 != "0")
            {
                type2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", moveItem2, "Item_Template");
                typeSon2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", moveItem2, "Item_Template");
            }
            else
            {
                //检测移动装备移动背包内,背包的格子为空，则判定移入成功
                type2 = type1;
                typeSon2 = typeSon1;
            }
            //道具类型一样执行下一步筛选
            if (type1 == type2)
            {
                //获取装备子类,查看是否是一个子类
                if (typeSon1 == typeSon2)
                {
                    return true;
                }
                else
                {
                    return false;     //交换失败
                }
            }
            else
            {
                return false;     //交换失败
            }
        }
        else
        {
            return false;     //交换失败
        }

    }
    

    //判定道具是否移入到技能栏中  moveType(0:表示道具类型  1：表示技能类型)
    public bool UI_MoveToMainSkill(string moveType)
    {
        if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus) {
            bool moveStatus = true;
            //判定交换失败条件
            if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial == "0" && Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial == "")
            {
                moveStatus = false;
            }
            if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End == "") {
                moveStatus = false;
            }

            //Debug.Log("Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = " + Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial);
            if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial == "0")
            {
                moveStatus = false;
                //清空值
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = false;
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = "";
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = "";
                return false;
            }

            for (int i = 1; i <= 8; i++) {

                GameObject skillObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseSkillSet.transform.Find("UI_MainRoseSkill_" + i).gameObject;
                if (skillObj.GetComponent<MainUI_SkillGrid>().SkillID == Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial)
                {
                    //获取技能是否有CD
                    if (skillObj.GetComponent<MainUI_SkillGrid>().skillCDStatus) {
                        moveStatus = false;
                        Game_PublicClassVar.Get_function_UI.GameHint("技能CD中,不能移动技能");
                    }
                } 
            }


            switch (moveType)
            {
                //判定道具是否为消耗品
                case "0":
                    string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial, "Item_Template");
                    if (itemType != "1")
                    {
                        moveStatus = false;
                    }
                    //判定是否有技能
                    string itemSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial, "Item_Template");
                    if (itemSkillID == "0")
                    {
                        moveStatus = false;
                    }
                    break;

                //技能,不做处理（后期需要判定是否为被动技能）
                case "1":



                    break;
            }

            //写入快捷键的值
            if (moveStatus) {

                //获取当前快捷键内是否有相同的技能或道具ID
                for (int i = 1; i <= 8; i++) {
                    string mainSkillId = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_"+ i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    if (mainSkillId == Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial) {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + i,"", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    }
                }
                //相同ID删除之前的ID

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End, Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
        }
        //清空值
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = "";
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = "";
        return true;
    }


    //获取当前背包剩余的格子数
    public int BagSpaceNullNum() {
        //暂时设定背包格子有64个,以后可能会改
        int bagNullNum = 0;
        for (int i = 1; i <= 64; i++) {
            string bagValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID","ID",i.ToString(),"RoseBag");
            if (bagValue == "0") {
                bagNullNum = bagNullNum + 1;
            }
        }
        return bagNullNum;
    }

	//根据品质返回对应的颜色值
	public string QualityReturnColorText(string ItenQuality){
		string color = "";
		switch (ItenQuality)
		{
		case "1":
			color = "<color=#FFFFFF>" ;
			break;
		case "2":
			color = "<color=#00FF00>";
			break;
		case "3":
			color = "<color=#871F78>";
			break;
		case "4":
			color = "<color=#FF7F00>";
			break;
		case "5":
			color = "<color=#CD7F32>";
			break;
		}
		return color;

	}

	//根据品质返回一个Color
	public Color QualityReturnColor(string ItenQuality){
		Color color = new Color(1,1,1);
		switch (ItenQuality)
		{
		case "1":
			color = new Color(1,1,1);
			break;
		case "2":
			color = new Color(0,1,0);
			break;
		case "3":
			color = new Color(0.937f,0.5f,1.0f);
			break;
		case "4":
			color = new Color(1,0.49f,0);
			break;
		case "5":
			color = new Color(0.80f,0.49f,0.19f);
			break;
		}
		return color;
	}

	//输入文本弹出对应的游戏通用提示
	public void GameHint(string hintText){
		GameObject objHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGameHint);
		objHint.transform.SetParent (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_GameHintSet.transform);
		objHint.transform.localScale = new Vector3(1,1,1);
		objHint.transform.localPosition = new Vector3(0,0,0);
		objHint.GetComponent<UI_GameHint> ().HintText = hintText;
	}

	//传入屏幕的百分比位置，显示当前对应的位置
	public Vector3 RetrunScreenV2(Vector3 v3){
		//获取当前屏幕的坐标
		int screen_X = Screen.width;
		int screen_Y = Screen.height;
        //Debug.Log("screen_X = " + screen_X + ";" + screen_Y);
		Vector3 UI_V3 = new Vector3();
        
		UI_V3.x = v3.x * 1366;
        UI_V3.y = v3.y * 1366 * screen_Y / screen_X;

		return UI_V3;
	}

	//传入UI坐标值,显示当前装备在左边还是右边
	public Vector3 UITipsScreen(Vector3 v2){

		Vector3 vec3 = new Vector3 ();
		float v3_X = v2.x;
        float v3_Y = v2.y;
		if (v2.x >= (Screen.width / 2)) {
            v3_X = v3_X - 250.0f * Screen.width / 1366;
		} else {
            v3_X = v3_X + 250.0f * Screen.width / 1366;
		}
        v3_X = v3_X / Screen.width * 1366;
        int screen_X = Screen.width;
        int screen_Y = Screen.height;
        v3_Y = v3_Y / Screen.height * 1366 * screen_Y / screen_X;
        //v3_Y = v3_Y / Screen.height * 768;
        vec3 = new Vector3(v3_X, v3_Y, 0);
        //vec3 = RetrunScreenV2(vec3);
		return vec3;

	}

    //传入UI坐标值,显示当前装备在左边还是右边
    public Vector3 UITipsScreen_DuiBi(Vector3 v2)
    {

        Vector3 vec3 = new Vector3();
        float v3_X = v2.x;
        float v3_Y = v2.y;
        if (v2.x >= (Screen.width / 2))
        {
            v3_X = v3_X - 550.0f * Screen.width / 1366;
        }
        else
        {
            v3_X = v3_X + 550.0f * Screen.width / 1366;
        }
        v3_X = v3_X / Screen.width * 1366;
        int screen_X = Screen.width;
        int screen_Y = Screen.height;
        v3_Y = v3_Y / Screen.height * 1366 * screen_Y / screen_X;
        //v3_Y = v3_Y / Screen.height * 768;
        vec3 = new Vector3(v3_X, v3_Y, 0);
        //vec3 = RetrunScreenV2(vec3);
        return vec3;

    }

    public Vector3 UIMoveIconPosition(float x, float y) { 
        x = x / Screen.width * 1366;
        int screen_X = Screen.width;
        int screen_Y = Screen.height;
        y = y / Screen.height *1366 * screen_Y / screen_X;
        //y = y / Screen.height * 768;
        Vector2 v3 = new Vector3(x,y,0);
        //Debug.Log("Screen.width = " + Screen.width + "Screen.height = " + Screen.height);
        return v3;
    }

    //传入X值返回实际X值
    public float ReturnScreen_X(float Value_X) {
        Value_X = Value_X / 1366 * Screen.width;
        return Value_X;
    }

    //传入Y值返回实际X值
    public float ReturnScreen_Y(float Value_Y){
        int screen_X = Screen.width;
        int screen_Y = Screen.height;
        //Value_Y = Value_Y / 1366 * screen_Y / screen_X * Screen.height;
        Value_Y = Value_Y / 768 * Screen.height;
        return Value_Y;
    }

    //显示故事模式以及背景字母
    public void CreateStoryBack() {

        //获取显示的内容
        string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string isShowBack = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IsStoryBackText", "ID", roseStoryStatus, "GameStory_Template");
        if (isShowBack == "1") {

            //实例化一个背景
            //Debug.Log("开始实例化");
            GameObject obj_storyBack = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_StoreTextBack);
            obj_storyBack.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_StorySpeakSet.transform);
            obj_storyBack.transform.localPosition = new Vector3(0, 0, 0);
            //obj_storyBack.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(683, 682);
            //obj_storyBack.transform.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(683, 0, 0);
            //obj_storyBack.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            //获取当前分辨率
            //Debug.Log("长=" + Screen.width + "宽=" + Screen.height);
            float chang = Screen.width / 1366.0f;
            float kuan = Screen.height / 768.0f;
            if (chang < 1) {
                chang = 1;
            }
            if (kuan < 1) {
                kuan = 1;
            }
            
            //obj_storyBack.transform.localScale = new Vector3(chang, kuan, 1);
            obj_storyBack.transform.localScale = new Vector3(1, 1, 1);
            //obj_storyBack.transform.lossyScale = new(1,1,1);
            UI_StoreTextBack ui_StoreTextBack = obj_storyBack.GetComponent<UI_StoreTextBack>();
            ui_StoreTextBack.ExitType = "2";        //默认显示点击取消
            string showText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryBackText", "ID", roseStoryStatus, "GameStory_Template");
            ui_StoreTextBack.StoreText = showText;

            //更新主角故事
            Game_PublicClassVar.Get_function_Rose.UpdataRoseStoryStatus();
            //Debug.Log("结束实例化");
            //获取是否有任务需要接取
            string taskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryBackTaskID", "ID", roseStoryStatus, "GameStory_Template");
            if (taskID != "0") {
                Game_PublicClassVar.Get_function_Task.GetTask(taskID);
            }
        }
    }

    //创建通用组提示
    public void GameGirdHint(string hintText,string colorVale = "FFFFFFFF") {

        GameObject hintObj = MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGameGirdHintSing);
        hintObj.SetActive(false);
        hintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint.transform);
        UI_GameGirdHint ui_gameGirdHint = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint.GetComponent<UI_GameGirdHint>();
        //ui_gameGirdHint.GameHintObj.Length - 1
        //循环判定提示位置
        for (int i = 0; i <= 99; i++)
        {
            if (ui_gameGirdHint.GameHintObj[i] == null)
            {
                ui_gameGirdHint.GameHintObj[i] = hintObj;
                //设定位置
                
                hintObj.transform.localPosition = new Vector3(0,0,0);
                hintObj.GetComponent<UI_GameGirdHintSingle>().HintText = hintText;
                hintObj.GetComponent<UI_GameGirdHintSingle>().HintColorValue = colorVale;
                //判定提示状态是否打开
                if (!ui_gameGirdHint.HintStatus) {
                    ui_gameGirdHint.HintStatus = true;
                    //Debug.Log("状态打开！");
                }
                break;      //跳出循环
            }
        }
    }


    //创建通用组提示
    public void GameGirdHint_Front(string hintText)
    {

        GameObject hintObj = MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGameGirdHintSing);
        hintObj.SetActive(false);
        hintObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint_Front.transform);
        UI_GameGirdHint ui_gameGirdHint = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameGirdHint_Front.GetComponent<UI_GameGirdHint>();
        //ui_gameGirdHint.GameHintObj.Length - 1
        //循环判定提示位置
        for (int i = 0; i <= 99; i++)
        {
            if (ui_gameGirdHint.GameHintObj[i] == null)
            {
                ui_gameGirdHint.GameHintObj[i] = hintObj;
                //设定位置

                hintObj.transform.localPosition = new Vector3(0, 0, 0);
                hintObj.GetComponent<UI_GameGirdHintSingle>().HintText = hintText;
                //判定提示状态是否打开
                if (!ui_gameGirdHint.HintStatus)
                {
                    ui_gameGirdHint.HintStatus = true;
                    //Debug.Log("状态打开！");
                }
                break;      //跳出循环
            }
        }
    }

    //根据资源类型返回资源图标路径
    public string ResourceTypeReturnIconPath(string resourceType) {

        switch (resourceType) {
            //建筑金币
            case "1":
                return "ItemIcon/Resouce_1";
                break;
            //农民
            case "2":
                return "ItemIcon/Resouce_6";
                break;
            //粮食
            case "3":
                return "ItemIcon/Resouce_2";
                break;
            //木材
            case "4":
                return "ItemIcon/Resouce_3";
                break;
            //石头
            case "5":
                return "ItemIcon/Resouce_4";
                break;
            //钢铁
            case "6":
                return "ItemIcon/Resouce_5";
                break;
        }
        return "";
    }

    //特殊飘字
    //参数一：飘字的Obj  参数二：飘字的父级Obj  参数三：飘的文字类型
    public void SpecialFlyText(GameObject flyObj, GameObject flyObjParent, string flyType)
    {

        GameObject HitObject_p = (GameObject)MonoBehaviour.Instantiate(flyObj);
        Text label = HitObject_p.GetComponent<Text>();
        Outline outLine = HitObject_p.GetComponent<Outline>();
        switch (flyType) { 
            //眩晕
            case "1":
                label.text = "眩晕";
                label.fontSize = 30;
                label.color = Color.green;
                outLine.effectColor = Color.black;
                break;
        }


        if (flyObj != null)
        {
            HitObject_p.transform.SetParent(flyObjParent.transform);
            HitObject_p.transform.localPosition = new Vector3(-30, 40, 0);
            HitObject_p.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    //传入值获取属性名称
    public string ReturnEquipNeedPropertyName(string proprety) {

        string propertyName = "";
        switch (proprety) { 
            
            case "1":
                propertyName = "攻击";
                break;

            case "2":
                propertyName = "物防";
                break;

            case "3":
                propertyName = "魔防";
                break;
        }
        return propertyName;
    }

    //传入装备类型返回对应的角色装备格子
    public string ReturnEquipSpaceNum(string equipType) {

        string equipSpaceNum = "0";
        switch (equipType) { 
            //武器
            case "1":
                equipSpaceNum = "1";
                break;
            //衣服
            case "2":
                equipSpaceNum = "2";
                break;
            //护符
            case "3":
                equipSpaceNum = "3";
                break;
            //灵石
            case "4":
                equipSpaceNum = "4";
                break;
            //饰品
            case "5":
                equipSpaceNum = "5";
                break;
            //鞋子
            case "6":
                equipSpaceNum = "8";
                break;
            //裤子
            case "7":
                equipSpaceNum = "9";
                break;
            //腰带
            case "8":
                equipSpaceNum = "10";
                break;
            //手镯
            case "9":
                equipSpaceNum = "11";
                break;
            //头盔
            case "10":
                equipSpaceNum = "12";
                break;
            //项链
            case "11":
                equipSpaceNum = "13";
                break;
        
        }

        return equipSpaceNum;

    }

    //播放音效(音效名称、音效类型、播放时间)
    public GameObject PlaySource(string sourceName,string sourceType,float playTime=0) {

        //获取当前播放音效的组建
        GameObject sourceObj = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceObj);
        AudioSource audioSource = sourceObj.GetComponent<AudioSource>();
        sourceObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet.transform);
        float sourceSize = Game_PublicClassVar.Get_game_PositionVar.GameSourceValue;
        /*
        if (sourceSize == 0) {
            return null;
        }
         */
        switch (sourceType) { 
            
            //播放UI音效
            case "1":
                AudioClip audioClip = (AudioClip)Resources.Load("GameSource/UI/" + sourceName, typeof(AudioClip));
                audioSource.clip = audioClip;
                if (playTime > 0) {
                    audioSource.loop = true;    //循环播放 
                }
                audioSource.GetComponent<GameSourceObj>().playTime = playTime;
                audioSource.volume = 1 * sourceSize;
                audioSource.Play();
                break;

            //播放游戏音效
            case "2":
                audioClip = (AudioClip)Resources.Load("GameSource/Game/" + sourceName, typeof(AudioClip));
                audioSource.clip = audioClip;
                if (playTime > 0)
                {
                    audioSource.loop = true;    //循环播放 
                }
                audioSource.GetComponent<GameSourceObj>().playTime = playTime;
                audioSource.volume = 1 * sourceSize;
                audioSource.Play();
                break;

            //播放场景背景音效
            case "3":
                audioClip = (AudioClip)Resources.Load("GameSource/BGM/" + sourceName, typeof(AudioClip));
                audioSource.clip = audioClip;
                audioSource.loop = true;    //循环播放
                audioSource.volume = 0.5f * sourceSize;
                audioSource.Play();
                break;
        }

        return sourceObj;
    }

    //钻石不足提示
    public void AddRMBHint() {
        //钻石不足统一提示,以后可能在这里统一添加界面跳转

        GameHint("钻石不足,请点击商城充值！");
    }

    //循环删除目标物体下的所有Obj
    public void DestoryTargetObj(GameObject targetObj) {
        for (int i = 0; i < targetObj.transform.childCount; i++)
        {
            GameObject go = targetObj.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }
    }

    //循环道具Tips
    public void DestoryTipsUI()
    {
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }

        //打开UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);

    }

    //清空UI
    public void ClearnUIStatus()
    {
        //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        //清空名字显示
        GameObject npcNameObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcNameSet;
        for (int i = 0; i < npcNameObj.transform.childCount; i++)
        {
            GameObject go = npcNameObj.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }
        //清空血条显示
        GameObject aiHpObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet;
        for (int i = 0; i < aiHpObj.transform.childCount; i++)
        {
            GameObject go = aiHpObj.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }
        //清空游戏声音
        GameObject gameSourceSet = Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet;
        for (int i = 0; i < gameSourceSet.transform.childCount; i++)
        {
            GameObject go = gameSourceSet.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }
        //清空掉落显示
        GameObject dropItemSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_DropItemSet;
        for (int i = 0; i < dropItemSet.transform.childCount; i++)
        {
            GameObject go = dropItemSet.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
            //Debug.Log("清空掉落");
        }
        //清空名称显示
        GameObject aiHpSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet;
        for (int i = 0; i < aiHpSet.transform.childCount; i++)
        {
            GameObject go = aiHpSet.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }

        //清空任务显示
        GameObject npcTaskSet = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_NpcTaskSet;
        for (int i = 0; i < npcTaskSet.transform.childCount; i++)
        {
            GameObject go = npcTaskSet.transform.GetChild(i).gameObject;
            MonoBehaviour.Destroy(go);
        }

        //重置摇杆状态
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_YaoGan.GetComponent<UI_YaoGan>().Exit();

        //删除保存的东西
        for (int i = 0; i <= Game_PublicClassVar.Get_game_PositionVar.Obj_Keep.Length; i++) {
            MonoBehaviour.Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Keep[i]);
        }
    }

}

