using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EquipTips : MonoBehaviour {

	public string ItemID;
    public string ItemHideID;
	public GameObject Obj_Imgback;
	public GameObject Obj_EquipIcon;
	public GameObject Obj_EquipQuality;
	public GameObject Obj_EquipName;
	public GameObject Obj_EquipType;
	public GameObject Obj_EquipProperty;
	public GameObject Obj_EquipWearNeed;
	public GameObject Obj_EquipDes;
    public GameObject Obj_EquipWearNeedProperty;

    public GameObject Obj_HideObj;
    public GameObject Obj_HideProperty;

	public GameObject Obj_EquiBase;
	public GameObject Obj_EquipNeed;
	public GameObject Obj_EquipBottom;
    public GameObject Obj_BtnSet;
    public GameObject Obj_EquipPropertyText;
    public GameObject Obj_BagOpenSet;
    public GameObject Obj_RoseEquipOpenSet;
    public GameObject Obj_SaveStoreHouse;
    public GameObject Obj_Diu;
    public GameObject Obj_Btn_StoreHouseSet;

    public string UIBagSpaceNum;
    public GameObject Obj_UIBagSpace;
    public GameObject Obj_UIRoseEquipShow;
    public GameObject Obj_UIOrnamentChoice;

    public GameObject Obj_UIEquipSuit;              //套装组建
    public GameObject Obj_UIEquipSuitName;          //套装名称
    public GameObject Obj_EquipSuitPropertyText;

    public GameObject Obj_UISuitEquipName;
    public GameObject EquipSuitShowSet_Right;
    public GameObject EquipSuitShowSet_Lelt;

    public string EquipTipsType;    //1.背包打开  2.装备栏打开 3.无任何按钮显示,点击商店列表弹出  4.仓库
	private GameObject itemTips_1;
    private GameObject showEquipObj;
    public GameObject Obj_ImgYiChuanDai;

    private string equipSuitID;
    private int properShowNum;      //属性列表展示次数

    private bool clickBtnStatus;        //点击按钮状态

	// Use this for initialization
	void Start () {
        //Debug.Log("打开套装");
		string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", ItemID, "Item_Template");
		string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
		
		//获取当前装备的各项属性
		string equip_ID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", ItemID, "Item_Template");
		string equipName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
		string equipLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", ItemID, "Item_Template");
		string ItemBlackDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemBlackDes", "ID", ItemID, "Item_Template");
		string equip_Hp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Hp", "ID", equip_ID, "Equip_Template");
		string equip_MinAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MinAct", "ID", equip_ID, "Equip_Template");
		string equip_MaxAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxAct", "ID", equip_ID, "Equip_Template");
		string equip_MinDef = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MinDef", "ID", equip_ID, "Equip_Template");
		string equip_MaxDef = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxDef", "ID", equip_ID, "Equip_Template");
		string equip_MinAdf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MinAdf", "ID", equip_ID, "Equip_Template");
		string equip_MaxAdf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equip_ID, "Equip_Template");
		string equip_Cri = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Cri", "ID", equip_ID, "Equip_Template");
		string equip_Hit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Hit", "ID", equip_ID, "Equip_Template");
		string equip_Dodge = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Dodge", "ID", equip_ID, "Equip_Template");
		string equip_DamgeAdd = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equip_ID, "Equip_Template");
		string equip_DamgeSub = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equip_ID, "Equip_Template");
		string equip_Speed = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Speed", "ID", equip_ID, "Equip_Template");
		string equip_Lucky = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Equip_Lucky", "ID", equip_ID, "Equip_Template");

        //获取极品装备属性
        //string hidePropertyStr = "1,1;4,1;5,1";         //临时
        string hidePropertyStr = "";
        string[] hideProperty;
        if (ItemHideID != "0" && ItemHideID!="")
        {
            hidePropertyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", ItemHideID, "RoseEquipHideProperty");
            hideProperty = hidePropertyStr.Split(';');
            //隐藏属性
            /*
            1:血量上限
            2:物理攻击最大值
            3:物理防御最大值
            4:魔法防御最大值
            */
            //循环加入各个隐藏属性
            if (hidePropertyStr != "")
            {
                for (int i = 0; i <= hideProperty.Length - 1; i++)
                {
                    string hidePropertyType = hideProperty[i].Split(',')[0];
                    string hidePropertyValue = hideProperty[i].Split(',')[1];

                    switch (hidePropertyType)
                    {
                        //血量上限
                        case "1":
                            equip_Hp = (int.Parse(equip_Hp) + int.Parse(hidePropertyValue)).ToString();
                            break;
                        //物理攻击最大值
                        case "2":
                            equip_MaxAct = (int.Parse(equip_MaxAct) + int.Parse(hidePropertyValue)).ToString();
                            break;
                        case "3":
                            //物理防御最大值
                            equip_MaxDef = (int.Parse(equip_MaxDef) + int.Parse(hidePropertyValue)).ToString();
                            break;
                        //魔法防御最大值
                        case "4":
                            equip_MaxAdf = (int.Parse(equip_MaxAdf) + int.Parse(hidePropertyValue)).ToString();
                            break;

                        //幸运值
                        case "101":
                            equip_Lucky = (int.Parse(equip_Lucky) + int.Parse(hidePropertyValue)).ToString();
                            break;
                    }
                }
            }
        }
        else {
            hideProperty = hidePropertyStr.Split(';');      //防止下面读取不到数据报错
        }


		//判定各项属性是否为0，为0的不显示
		string textShow = "";

		//血量
		if (equip_Hp != "0")
		{
			textShow = "血量 ： " + equip_Hp ;
			//textNum = textNum + 1;
            //ShowPropertyText(textShow);
            bool ifHideProperty = false;
            //显示极品属性
            if (hidePropertyStr != "") {
                for (int i = 0; i <= hideProperty.Length - 1; i++)
                {
                    string hidePropertyType = hideProperty[i].Split(',')[0];
                    string hidePropertyValue = hideProperty[i].Split(',')[1];
                    if (hidePropertyType == "1")
                    {
                        textShow = "血量 ： " + equip_Hp + "(+" + hidePropertyValue + ")";
                        ifHideProperty = true;
                    }
                }
            }

            if (ifHideProperty)
            {
                ShowPropertyText(textShow,"1");
            }
            else {
                ShowPropertyText(textShow);                
            }
		}
		//攻击
		if (equip_MinAct != "0"||equip_MaxAct != "0")
		{
				textShow = "攻击 ： " + equip_MinAct + " - " + equip_MaxAct ;
				//textNum = textNum + 1;
                //ShowPropertyText(textShow);
                bool ifHideProperty = false;
                //显示极品属性
                if (hidePropertyStr != "")
                {
                    for (int i = 0; i <= hideProperty.Length - 1; i++)
                    {
                        string hidePropertyType = hideProperty[i].Split(',')[0];
                        string hidePropertyValue = hideProperty[i].Split(',')[1];
                        if (hidePropertyType == "2")
                        {
                            textShow = "攻击 ： " + equip_MinAct + " - " + equip_MaxAct + "(+" + hidePropertyValue + ")";
                            ifHideProperty = true;
                        }
                    }
                }
                if (ifHideProperty)
                {
                    ShowPropertyText(textShow, "1");
                }
                else
                {
                    ShowPropertyText(textShow);
                }
		}
		//防御
		if (equip_MinDef != "0"||equip_MaxDef != "0")
		{
				textShow = "防御 ： " + equip_MinDef + " - " + equip_MaxDef ;
				//textNum = textNum + 1;
                //ShowPropertyText(textShow);
                bool ifHideProperty = false;
                //显示极品属性
                if (hidePropertyStr != "")
                {
                    for (int i = 0; i <= hideProperty.Length - 1; i++)
                    {
                        string hidePropertyType = hideProperty[i].Split(',')[0];
                        string hidePropertyValue = hideProperty[i].Split(',')[1];
                        if (hidePropertyType == "3")
                        {
                            textShow = "防御 ： " + equip_MinDef + " - " + equip_MaxDef + "(+" + hidePropertyValue + ")";
                            ifHideProperty = true;
                        }
                    }
                }
                if (ifHideProperty)
                {
                    ShowPropertyText(textShow, "1");
                }
                else
                {
                    ShowPropertyText(textShow);
                }
		}
		//魔防
		if (equip_MinAdf != "0"||equip_MaxAdf != "0")
		{
				textShow = "魔防 ： " + equip_MinAdf + " - " + equip_MaxAdf ;
				//textNum = textNum + 1;
                //ShowPropertyText(textShow);
                bool ifHideProperty = false;
                //显示极品属性
                if (hidePropertyStr != "")
                {
                    for (int i = 0; i <= hideProperty.Length - 1; i++)
                    {
                        string hidePropertyType = hideProperty[i].Split(',')[0];
                        string hidePropertyValue = hideProperty[i].Split(',')[1];
                        if (hidePropertyType == "4")
                        {
                            textShow = "魔防 ： " + equip_MinAdf + " - " + equip_MaxAdf + "(+" + hidePropertyValue + ")";
                            ifHideProperty = true;
                        }
                    }
                }
                if (ifHideProperty)
                {
                    ShowPropertyText(textShow, "1");
                }
                else
                {
                    ShowPropertyText(textShow);
                }
		}
		//暴击
		if (equip_Cri != "0")
		{
            textShow = "暴击 ： " + float.Parse(equip_Cri) * 100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//命中
		if (equip_Hit != "0")
		{
			textShow = "命中 ： " + float.Parse(equip_Hit)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//闪避
		if (equip_Dodge != "0")
		{
			textShow = "闪避 ： " + float.Parse(equip_Dodge)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//伤害加成
		if (equip_DamgeAdd != "0")
		{
			textShow = "伤害加成 ： " + float.Parse(equip_DamgeAdd)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//伤害减免
		if (equip_DamgeSub != "0")
		{
			textShow = "伤害减免 ： " + float.Parse(equip_DamgeSub)*100 + "%\n";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//速度
		if (equip_Speed != "0")
		{
			textShow = "移动速度 ： " + equip_Dodge ;
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		//幸运值
		if (equip_Lucky != "0")
		{
            textShow = "<color=#00ff00ff>幸运值 ： " + equip_Lucky + "</color>";
			//textNum = textNum + 1;
            ShowPropertyText(textShow);
		}
		
		//最后一行去掉/n
		if (textShow != "")
		{
			textShow = textShow.Substring(0, textShow.Length - 1);
		}
		
		//判定当前装备是什么部位
		string textEquipType = "";
		string itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
		switch (itemSubType)
		{
		case "1":
			textEquipType = "武器";
			break;
			
		case "2":
			textEquipType = "衣服";
			break;
			
		case "3":
			textEquipType = "护符";
			break;
			
		case "4":
			textEquipType = "灵石";
			break;
			
		case "5":
			textEquipType = "饰品";
			break;
			
		case "6":
			textEquipType = "鞋子";
			break;
			
		case "7":
			textEquipType = "裤子";
			break;
			
		case "8":
			textEquipType = "腰带";
			break;
			
		case "9":
			textEquipType = "手套";
			break;
			
		case "10":
			textEquipType = "头盔";
			break;
			
		case "11":
			textEquipType = "项链";
			break;
		}

        //显示隐藏技能
        float HintTextNum = 0;
        string skillIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", ItemID, "Item_Template");
        if (skillIDStr != "0")
        {
            //标题长度
            HintTextNum = HintTextNum + 50.0f;
            //获取技能ID列表
            string[] skillID = skillIDStr.Split(',');
            //设置位置
            //Obj_HideObj.transform.localPosition = new Vector3(8, -20 - 11 * textNum, 0);
            Vector2 hint_vec2 = new Vector2(150.0f, -170 - 20 * properShowNum);
            Obj_HideObj.transform.GetComponent<RectTransform>().anchoredPosition = hint_vec2;


            //逐个显示
            string showHintTxt = "";
            for (int i = 0; i <= skillID.Length - 1; i++) {
                //每增加一个隐藏属性,长度增加22
                HintTextNum = HintTextNum + 20.0f;
                //获取技能名称
                //Debug.Log("skillID[i] = " + skillID[i]);
                //skillID[i] = "60091201";        //测试,配置完成后时候屏蔽即可
                string skillName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillName", "ID", skillID[i], "Skill_Template");
                showHintTxt = showHintTxt + skillName + "\n";
            }

            Obj_HideProperty.GetComponent<Text>().text = showHintTxt;
            Obj_HideObj.SetActive(true);
            Obj_HideProperty.SetActive(true);
        }
        else {
            Obj_HideObj.SetActive(false);
            Obj_HideProperty.SetActive(false);
        }
        

        //显示套装属性
        float equipSuitTextNum = 0;
        float suitShowTextNumSum = 0;
        equipSuitID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitID", "ID", equip_ID, "Equip_Template");
        if (equipSuitID != "0")
        {
            //设置套装显示位置
            //Debug.Log("properShowNum = " + properShowNum);
            Vector2 equipSuit_vec2 = new Vector2(150.0f, -170 - 20 * properShowNum - HintTextNum);
            Obj_UIEquipSuit.transform.GetComponent<RectTransform>().anchoredPosition = equipSuit_vec2;

            Obj_UIEquipSuit.SetActive(true);
            equipSuitTextNum = equipSuitTextNum + 50.0f;
            //获取套装名称
            string equipSuitName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", equipSuitID, "EquipSuit_Template");
            //获取套装ID
            string[] needEquipIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipID", "ID", equipSuitID, "EquipSuit_Template").Split(';');
            string[] needEquipNumSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipNum", "ID", equipSuitID, "EquipSuit_Template").Split(';');
            string[] suitPropertyIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuitPropertyID", "ID", equipSuitID, "EquipSuit_Template").Split(';');
            //获取自身套装数量
            int equipSuitNum = Game_PublicClassVar.Get_function_Rose.returnEquipSuitNum(needEquipIDSet, needEquipNumSet);
            //显示套装名称及拥有数量
            Obj_UIEquipSuitName.GetComponent<Text>().text = equipSuitName + "(" + equipSuitNum + "/" + needEquipIDSet.Length + ")" + "           查看部件";
            
            for(int i = 0; i<=suitPropertyIDSet.Length-1;i++){
                string triggerSuitNum = suitPropertyIDSet[i].Split(',')[0];
                string triggerSuitPropertyID = suitPropertyIDSet[i].Split(',')[1];
                //显示套装属性
                GameObject propertyObj = (GameObject)Instantiate(Obj_EquipSuitPropertyText);
                propertyObj.transform.SetParent(Obj_UIEquipSuit.transform);
                propertyObj.transform.localScale = new Vector3(1, 1, 1);
                string equipSuitDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitDes", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template");
                string ifShowSuitNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ifShowSuitNum", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template");
                if (ifShowSuitNum == "0")
                {
                    propertyObj.GetComponent<Text>().text = triggerSuitNum + "件套：" + equipSuitDes;
                }
                else {
                    propertyObj.GetComponent<Text>().text = "            " + equipSuitDes;
                }
                
                //float suitShowTextNum = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowTextNum", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template"));
                float suitShowTextNum = 1;
                suitShowTextNumSum = suitShowTextNumSum + suitShowTextNum;
                propertyObj.transform.localPosition = new Vector3(10, -30 - 25 * suitShowTextNumSum, 0);
                //propertyObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 25.0f * suitShowTextNumSum);
                //满足条件显示绿色
                if (equipSuitNum >= int.Parse(triggerSuitNum)) {
                    propertyObj.GetComponent<Text>().color = Color.green;
                }
            }

            //累计获取套装显示占用的长度
            equipSuitTextNum = equipSuitTextNum + suitShowTextNumSum * 25.0f+10.0f;
            //Debug.Log("没有套装！");
        }
        else {
            Obj_UIEquipSuit.SetActive(false);
            //Debug.Log("有套装！");
        }


		//显示对应装备属性
        
		Obj_EquipName.GetComponent<Text>().text = equipName;
		Obj_EquipName.GetComponent<Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(ItemQuality);
		Obj_EquipType.GetComponent<Text>().text = "部位："+textEquipType;

        Vector2 equipNeedvec2 = new Vector2(150.0f, -170 - 20 * properShowNum - HintTextNum - equipSuitTextNum);
        Obj_EquipNeed.transform.GetComponent<RectTransform>().anchoredPosition = equipNeedvec2;

		Obj_EquipWearNeed.GetComponent<Text>().text = "等级 ： " + equipLv;
        //获取角色等级
        int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        if (roseLv < int.Parse(equipLv)) {
            //Obj_EquipWearNeed.GetComponent<Text>().color = Color.red;
            Obj_EquipWearNeed.GetComponent<Text>().text = "等级 ： " + equipLv + "<color=#ff0000ff>  (等级不足)</color>";
        }
        int EquipNeedTextNum = 30;
        //此处后面需补充判定某种属性的条件
        //string[] needProperty = "1,40".Split(',');
        string propertyLimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PropertyLimit", "ID", ItemID, "Item_Template");
        string[] needProperty = propertyLimit.Split(',');

        if (needProperty[0] != "0")
        {
            //根据属性值返回属性名称
            string propertyName = Game_PublicClassVar.Get_function_UI.ReturnEquipNeedPropertyName(needProperty[0]);
            Obj_EquipWearNeedProperty.GetComponent<Text>().text = "需要" + propertyName + "达到 ： " + needProperty[1];

            switch (needProperty[0])
            { 
                //最大攻击
                case "1":
                    int value = Game_PublicClassVar.Get_function_Rose.ReturnRosePropertyValue("1");
                    if (value < int.Parse(needProperty[1])) {
                        //Obj_EquipWearNeedProperty.GetComponent<Text>().color = Color.red;
                        Obj_EquipWearNeedProperty.GetComponent<Text>().text = "需要" + propertyName + "达到 ： " + needProperty[1] + "<color=#ff0000ff>  (攻击力不足)</color>";
                    }
                break;
                //后期属性在补充
            }

            //显示值
            Obj_EquipWearNeedProperty.SetActive(true);

            //设置长度
            EquipNeedTextNum = EquipNeedTextNum + 20;
        }
        else {
            Obj_EquipWearNeedProperty.SetActive(false);
        }
		

        //显示道具来源描述文字
        int ItemBlackNum = 0;
        float EquipBottomTextNum = 0;
        //Debug.Log("ItemBlackDes = " + ItemBlackDes);
        if (ItemBlackDes != "0" && ItemBlackDes != "")
        {
            ItemBlackNum = (int)((ItemBlackDes.Length - 16) / 16) + 1;
            Vector2 equipBottomvec2 = new Vector2(150.0f, -170 - 20 * properShowNum - HintTextNum - EquipNeedTextNum - ItemBlackNum * 8 - equipSuitTextNum);
            Obj_EquipBottom.transform.GetComponent<RectTransform>().anchoredPosition = equipBottomvec2;
            EquipBottomTextNum = EquipBottomTextNum + 50.0f;
            Obj_EquipDes.GetComponent<Text>().text = ItemBlackDes;
        }
        else
        {
            Obj_EquipBottom.SetActive(false);
        }
        if (ItemBlackDes.Length > 32)
        {
            ItemBlackNum = (int)((ItemBlackDes.Length - 32) / 16) + 1;
            Obj_EquipDes.GetComponent<RectTransform>().sizeDelta = new Vector2(240.0f, 40.0f + 16.0f * ItemBlackNum);
            Obj_EquipDes.GetComponent<Text>().text = ItemBlackDes;
            EquipBottomTextNum = EquipBottomTextNum + 16 * ItemBlackNum;
        }

        //显示按钮
        switch (EquipTipsType)
        {
            //背包打开显示对应功能按钮
            case "1":
                Obj_BagOpenSet.SetActive(true);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                //判定当前是否打开仓库
                if (Game_PublicClassVar.Get_game_PositionVar.StoreHouseStatus)
                {
                    Obj_SaveStoreHouse.SetActive(true);
                    Obj_Diu.SetActive(false);
                }
                else {
                    Obj_SaveStoreHouse.SetActive(false);
                    Obj_Diu.SetActive(true);
                }
                break;
            //角色栏打开显示对应功能按钮
            case "2":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(true);
                Obj_Btn_StoreHouseSet.SetActive(false);
                break;
            //商店查看属性
            case "3":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                EquipBottomTextNum = 0;
                break;
            //仓库
            case "4":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(true);
                //EquipBottomTextNum = 0;
                break;

            //对比查看
            case "5":
                Obj_BagOpenSet.SetActive(false);
                Obj_RoseEquipOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                //EquipBottomTextNum = 0;
                break;
        }

        //设置底图长度
        float DiHight = 250 + 20 * properShowNum + HintTextNum + EquipNeedTextNum + EquipBottomTextNum + equipSuitTextNum;
        Obj_Imgback.GetComponent<RectTransform>().sizeDelta = new Vector2(301.0f, DiHight);
		//显示道具Icon
		object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
		Sprite itemIcon = obj as Sprite;
		Obj_EquipIcon.GetComponent<Image>().sprite = itemIcon;

		//显示品质
		object obj2 = Resources.Load(Game_PublicClassVar.Get_function_UI.ItemQualiytoPath(ItemQuality), typeof(Sprite));
		Sprite itemQuality = obj2 as Sprite;
		Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;




        //监测UI是否超过底部显示
        float UIHeadValue = 768 - this.transform.localPosition.y - DiHight/2;            //UI和顶部的距离
        float UIHightValue = UIHeadValue + DiHight+50.0f;
        //Debug.Log("UIHeadValue = " + UIHeadValue + "DiHight = " + DiHight);
        if (UIHightValue >= 768) {
            //Debug.Log("UI触底了");
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 50 + DiHight / 2, 0);
        }
        //监测UI是否超过了顶部显示
        if (UIHeadValue <= 0) {
            //Debug.Log("UI触顶了");
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, 768 - DiHight / 2, 0);
        }

	}
	
	// Update is called once per frame
	void Update () {



	}

    //穿戴装备
    public void UseEquip()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //Debug.Log("我穿了一件装备！");

        Game_PositionVar game_PublicClassVar = Game_PublicClassVar.Get_game_PositionVar;
        //获取装备类型
        string equipType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template"); ;
        string equipSpaceType =  Game_PublicClassVar.Get_function_UI.ReturnEquipSpaceNum(equipType);

        //获取此装备的类型,如果是饰品需要弹出选择界面
        if (equipSpaceType == "5")
        {
            GameObject ornamentObj = (GameObject)Instantiate(Obj_UIOrnamentChoice);
            ornamentObj.transform.SetParent(this.gameObject.transform);
            ornamentObj.transform.position = new Vector3(Screen.width/2,Screen.height/2,0);        //在中心,后期分辨率不一样可能需要调整
            ornamentObj.transform.localScale = new Vector3(1, 1, 1);
            ornamentObj.GetComponent<UI_OrnamentChoice>().UIBagSpaceNum = UIBagSpaceNum;
        }
        else {
            game_PublicClassVar.ItemMoveValue_Initial = UIBagSpaceNum;
            game_PublicClassVar.ItemMoveType_Initial = "1";
            game_PublicClassVar.ItemMoveValue_End = equipSpaceType;
            game_PublicClassVar.ItemMoveType_End = "2";
            Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
            //删除Tips
            Destroy(this.gameObject);
            //关闭Tips
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
            //更新背包
            //Obj_UIBagSpace.GetComponent<UI_BagSpace>().UpdataItemShow = true;
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            //角色装备更新指定某个装备
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseEquip.GetComponent<UI_RoseEquip>().UpdataEquipOne(equipSpaceType);
            //更新装备属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;
        }
    }

    //丢弃道具按钮
    public void ThrowItem()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行
        //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, UIBagSpaceNum, true);
        Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(UIBagSpaceNum);
        CloseUI();
    }
    public void CloseUI()
    {
        Destroy(this.gameObject);
    }

    //实例化属性文本
    public void ShowPropertyText(string showText,string showType = "0") {
 
        //实例化一个Obj
        GameObject propertyObj = (GameObject)Instantiate(Obj_EquipPropertyText);
        propertyObj.transform.SetParent(Obj_EquiBase.transform);
        propertyObj.transform.localScale = new Vector3(1, 1, 1);
        propertyObj.GetComponent<Text>().text = showText;
        propertyObj.transform.localPosition = new Vector3(10, -30 - 20 * properShowNum, 0);

        properShowNum = properShowNum + 1;

        if (showType == "1") {
            propertyObj.GetComponent<Text>().color = Color.green;
        }

        //Debug.Log("实例化坐标：" + properShowNum);
        
    }

    //卸下装备
    public void Btn_TakeEquip() {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //判断背包是否已满
        string bagFirstNullNum = Game_PublicClassVar.Get_function_Rose.BagFirstNullNum();
        if (bagFirstNullNum != "-1")
        {
            //获取装备类型
            string equipType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");
            string equipSpaceType = Game_PublicClassVar.Get_function_UI.ReturnEquipSpaceNum(equipType);
            Game_PositionVar game_PublicClassVar = Game_PublicClassVar.Get_game_PositionVar;
            game_PublicClassVar.ItemMoveValue_Initial = Obj_UIRoseEquipShow.GetComponent<UI_RoseEquipShow>().EquipSpaceNum;
            game_PublicClassVar.ItemMoveType_Initial = "2";
            game_PublicClassVar.ItemMoveValue_End = bagFirstNullNum;
            game_PublicClassVar.ItemMoveType_End = "1";
            Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
            //删除Tips
            Destroy(this.gameObject);
            //关闭UI背景图片
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
            //更新背包
            //Obj_UIBagSpace.GetComponent<UI_BagSpace>().UpdataItemShow = true;
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            //角色装备更新指定某个装备
            Obj_UIRoseEquipShow.GetComponent<UI_RoseEquipShow>().UpdataStatus = true;
            //更新装备属性
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = true;

            //获取装备是否携带技能
            string equip_skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", ItemID, "Item_Template");
            if (equip_skillID != "0" && equip_skillID != "") {
                Game_PublicClassVar.Get_function_Skill.UpdataMainUIEquipSkillID(equip_skillID);
            }

            //判定技能栏是否打开,如果打开就关闭,防止出现BUG（装备技能卸下重装会叠加skillADDID）
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseSkill_Status) {
                Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();
            }

        }
    }

    //展示装备套装
    public void Btn_ShowEquipSuit() {
        if (showEquipObj == null)
        {
            showEquipObj = (GameObject)Instantiate(Obj_UISuitEquipName);

            //判定自身是在左边还是右边
            if (this.gameObject.transform.position.x >= Screen.width / 2)
            {
                //左边显示左侧
                showEquipObj.transform.SetParent(EquipSuitShowSet_Lelt.transform);
                showEquipObj.transform.localScale = new Vector3(1, 1, 1);
                showEquipObj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20, -106);
                showEquipObj.GetComponent<UI_SuitNeedEquipName>().SuitShowDirection = "1";
                //Debug.Log("在左边");
            }
            else
            {
                //左面显示右侧
                showEquipObj.transform.SetParent(EquipSuitShowSet_Right.transform);
                showEquipObj.transform.localScale = new Vector3(1, 1, 1);
                showEquipObj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(27, -106);
                showEquipObj.GetComponent<UI_SuitNeedEquipName>().SuitShowDirection = "2";
                //Debug.Log("在右边");
            }
            showEquipObj.GetComponent<UI_SuitNeedEquipName>().SuitID = equipSuitID;
        }
        else {
            Destroy(showEquipObj);
        }
    }

    //存入仓库
    public void Btn_SaveStoreHouse() {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行


        //获取仓库是否已经满了
        int nullNum = Game_PublicClassVar.Get_function_Rose.StoreHouseNullNum();
        if (nullNum <= 0)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("仓库已满！");
            return;
        }

        //获取存入道具的数据
        string save_ItemID = ItemID;
        //获取隐藏属性ID
        string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseBag");
        //删除指定道具
        Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(save_ItemID, 1, UIBagSpaceNum, true);
        //添加指定道具到仓库
        //Debug.Log("UIBagSpaceNum = " + UIBagSpaceNum + ";   hideID = " + hideID);
        Game_PublicClassVar.Get_function_Rose.SendRewardToStoreHouse(save_ItemID, 1, "1", hideID);
        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
    }

    //取出仓库
    public void Btn_TaskStoreHouse() {

        if (clickBtnStatus) {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //获取存入道具的数据
        string save_ItemID = ItemID;
        //获取隐藏属性ID
        string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseStoreHouse");
        //删除指定道具
        Game_PublicClassVar.Get_function_Rose.CostStoreHouseSpaceNumItem(save_ItemID, 1, UIBagSpaceNum, true);
        //添加指定道具到背包
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(save_ItemID, 1, "1",0, hideID);
        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();    //删除所有Tips
    }
}
