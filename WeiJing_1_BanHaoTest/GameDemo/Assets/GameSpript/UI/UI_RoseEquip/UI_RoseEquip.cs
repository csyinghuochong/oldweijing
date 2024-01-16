using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseEquip : MonoBehaviour {

    public Transform EquipList;

    //子级的控件列表
    private GameObject obj_Equip_Weapon;
    private GameObject obj_Equip_Clothes;
    private GameObject obj_Equip_Amulet;
    private GameObject obj_Equip_GodsStone;
    private GameObject obj_Equip_Ornament3;
    private GameObject obj_Equip_Ornament2;
    private GameObject obj_Equip_Ornament1;
    private GameObject obj_Equip_Shoes;
    private GameObject obj_Equip_Pants;
    private GameObject obj_Equip_Belt;
    private GameObject obj_Equip_Bracelet;
    private GameObject obj_Equip_Helmet;
    private GameObject obj_Equip_Necklace;


    private GameObject EquipQuality;
    private GameObject EquipIcon;
    private GameObject EquipDi;

    private string str_EquipItemID;
    private string str_EquipQuality;
    private string str_EquipIcon;

    private GameObject gameStartVar;
    private Game_PositionVar game_PositionVar;


	//属性部分
	public GameObject Obj_RoseHp;
	public GameObject Obj_RoseAct;
	public GameObject Obj_RoseDef;
    public GameObject Obj_RoseAdf;

	public GameObject Obj_RoseHit;
	public GameObject Obj_RoseCri;
	public GameObject Obj_RoseDodge;
	public GameObject Obj_RoseDamgeAdd;
	public GameObject Obj_RoseDamgeMinu;
	public GameObject Obj_RoseMoveSpeed;
    public GameObject Obj_RoseLucky;


    public GameObject Obj_RoseName;
    public GameObject Obj_RoseLv;
    public GameObject Obj_RoseExp;

	// Use this for initialization
	void Start () {

        //找到对应绑点
        obj_Equip_Weapon = EquipList.Find("Equip_1").gameObject;
        obj_Equip_Clothes = EquipList.Find("Equip_2").gameObject;
        obj_Equip_Amulet = EquipList.Find("Equip_3").gameObject;
        obj_Equip_GodsStone = EquipList.Find("Equip_4").gameObject;
        obj_Equip_Ornament3 = EquipList.Find("Equip_5").gameObject;
        obj_Equip_Ornament2 = EquipList.Find("Equip_6").gameObject;
        obj_Equip_Ornament1 = EquipList.Find("Equip_7").gameObject;
        obj_Equip_Shoes = EquipList.Find("Equip_8").gameObject;
        obj_Equip_Pants = EquipList.Find("Equip_9").gameObject;
        obj_Equip_Belt = EquipList.Find("Equip_10").gameObject;
        obj_Equip_Bracelet = EquipList.Find("Equip_11").gameObject;
        obj_Equip_Helmet = EquipList.Find("Equip_12").gameObject;
        obj_Equip_Necklace = EquipList.Find("Equip_13").gameObject;
        
        //更新装备数据
        UpdateAllRoseEquip();
        //因为刚才已经更新,所以防止二次更新
        if (Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip) {
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = false;
        }

        updataRoseProprety();
	}
	
	// Update is called once per frame
	void Update () {
        //更新属性
        if (Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip) {
            updataRoseProprety();
            Game_PublicClassVar.Get_game_PositionVar.UpdataRoseEquip = false;
        }
	}

    void UpdateAllRoseEquip()
    {
        UpdateRoseEquipDate(obj_Equip_Weapon, 1);
        UpdateRoseEquipDate(obj_Equip_Clothes, 2);
        UpdateRoseEquipDate(obj_Equip_Amulet, 3);
        UpdateRoseEquipDate(obj_Equip_GodsStone, 4);
        UpdateRoseEquipDate(obj_Equip_Ornament3, 5);
        UpdateRoseEquipDate(obj_Equip_Ornament2, 6);
        UpdateRoseEquipDate(obj_Equip_Ornament1, 7);
        UpdateRoseEquipDate(obj_Equip_Shoes, 8);
        UpdateRoseEquipDate(obj_Equip_Pants, 9);
        UpdateRoseEquipDate(obj_Equip_Belt, 10);
        UpdateRoseEquipDate(obj_Equip_Bracelet, 11);
        UpdateRoseEquipDate(obj_Equip_Helmet, 12);
        UpdateRoseEquipDate(obj_Equip_Necklace, 13);
    }

    //显示装备数据
    void UpdateRoseEquipDate(GameObject RoseEquipSpaceName, int bagSpaceNum)
    {
        //获取装备底层显示的文字
        //EquipDi = RoseEquipSpaceName.transform.Find("Img_EquipBackText").gameObject;
        //获取道具数据
        str_EquipItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", bagSpaceNum.ToString(), "RoseEquip");

        RoseEquipSpaceName.GetComponent<UI_RoseEquipShow>().EquipID = str_EquipItemID;
        RoseEquipSpaceName.GetComponent<UI_RoseEquipShow>().UpdataStatus = true;

        /*
        //绑定Tips脚本
        UI_ItemTips ui_ItemTips = RoseEquipSpaceName.transform.Find("Btn_Equip").gameObject.AddComponent<UI_ItemTips>();

        //将信息赋值给脚本
        ui_ItemTips.ItemID = str_EquipItemID;
        ui_ItemTips.ItemType = "2";
        ui_ItemTips.ItemTypeValue = bagSpaceNum.ToString();
        */
        //判定当前装备栏位是否有装备道具
        if (str_EquipItemID != "0")
        {

            //str_EquipQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", str_EquipItemID, "Item_Template");
            //str_EquipIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", str_EquipItemID, "Item_Template");

            //Debug.Log("当前道具序号：" + bagSpaceNum + "当前道具品质" + str_EquipQuality + "当前道具ID" + str_EquipItemID);

            //EquipDi.active = false;

            //显示数据
            //显示道具图标
            /*
            UITexture equipIconTexture = RoseEquipSpaceName.transform.Find("EquipIcon").GetComponent<UITexture>();
            Function_UI function_ui = new Function_UI();
            equipIconTexture.mainTexture = (Texture2D)Resources.Load("ItemIcon/" + str_EquipIcon);
            

            //显示道具品质
            UISprite itemQualitySprite = RoseEquipSpaceName.transform.Find("EquipQuality").GetComponent<UISprite>();
            itemQualitySprite.spriteName = "ItemQuality_" + str_EquipQuality;
            */
        }
        else
        {

            //当被格子没有到道具清空显示信息
            /*
            UITexture equipIconTexture = RoseEquipSpaceName.transform.Find("EquipIcon").GetComponent<UITexture>();
            equipIconTexture.mainTexture = (Texture2D)Resources.Load("");
            UISprite uispriteQuality = RoseEquipSpaceName.transform.Find("EquipQuality").GetComponent<UISprite>();
            uispriteQuality.spriteName = "";
            */
            //EquipDi.active = true;
        }

    }

    //更新角色属性
    void updataRoseProprety() {

        Game_PublicClassVar.Get_function_Rose.UpdateRoseProperty();

        Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        Obj_RoseHp.GetComponent<Text>().text = rose_Proprety.Rose_Hp.ToString();
        Obj_RoseAct.GetComponent<Text>().text = rose_Proprety.Rose_ActMin.ToString() + "-" + rose_Proprety.Rose_ActMax.ToString();
        Obj_RoseDef.GetComponent<Text>().text = rose_Proprety.Rose_DefMin.ToString() + "-" + rose_Proprety.Rose_DefMax.ToString();
        Obj_RoseAdf.GetComponent<Text>().text = rose_Proprety.Rose_AdfMin.ToString() + "-" + rose_Proprety.Rose_AdfMax.ToString();
        Obj_RoseHit.GetComponent<Text>().text = (rose_Proprety.Rose_Hit * 100).ToString() + "%";
        Obj_RoseCri.GetComponent<Text>().text = (rose_Proprety.Rose_Cri * 100).ToString() + "%";
        Obj_RoseDodge.GetComponent<Text>().text = (rose_Proprety.Rose_Dodge * 100).ToString() + "%";
        Obj_RoseDamgeAdd.GetComponent<Text>().text = (rose_Proprety.Rose_DamgeAdd * 100).ToString() + "%";
        Obj_RoseDamgeMinu.GetComponent<Text>().text = (rose_Proprety.Rose_DamgeSub * 100).ToString() + "%";
        Obj_RoseMoveSpeed.GetComponent<Text>().text = rose_Proprety.Rose_MoveSpeed.ToString();
        Obj_RoseLucky.GetComponent<Text>().text = rose_Proprety.Rose_Lucky.ToString() + "/9";

        //更新名称、等级、经验
        Obj_RoseName.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_RoseLv.GetComponent<Text>().text = "等级：" + Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        int nowExp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ExpNow;
        int sumExp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Exp;
        Obj_RoseExp.GetComponent<Text>().text = nowExp.ToString() + "/" + sumExp.ToString();
    }

	public void CloseUI(){
        Destroy (this.gameObject);
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RoseEquip_Status = false;
        //获取当前Tips栏内是否有Tips,如果有就清空处理
        GameObject parentObj = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet;
        for (int i = 0; i < parentObj.transform.childCount; i++)
        {
            GameObject go = parentObj.transform.GetChild(i).gameObject;
            Destroy(go);
        }
	}

    public void UpdataEquipOne(string updataEquipNum) {
        //更新装备
        switch (updataEquipNum) { 
            
            case "1":
                UpdateRoseEquipDate(obj_Equip_Weapon, 1);
                break;

            case "2":
                UpdateRoseEquipDate(obj_Equip_Clothes, 2);
                break;
                    
            case "3":
                UpdateRoseEquipDate(obj_Equip_Amulet, 3);
                break;
                
            case "4":
                UpdateRoseEquipDate(obj_Equip_GodsStone, 4);
                break;

            case "5":
                UpdateRoseEquipDate(obj_Equip_Ornament3, 5);
                break;

            case "6":
                UpdateRoseEquipDate(obj_Equip_Ornament2, 6);
                break;

            case "7":
                UpdateRoseEquipDate(obj_Equip_Ornament1, 7);
                break;

            case "8":
                UpdateRoseEquipDate(obj_Equip_Shoes, 8);
                break;
                
            case "9":
                UpdateRoseEquipDate(obj_Equip_Pants, 9);
                break;
  
            case "10":
                UpdateRoseEquipDate(obj_Equip_Belt, 10);
                break;

            case "11":
                UpdateRoseEquipDate(obj_Equip_Bracelet, 11);
                break;

            case "12":
                UpdateRoseEquipDate(obj_Equip_Helmet, 12);
                break;

            case "13":
                UpdateRoseEquipDate(obj_Equip_Necklace, 13);
                break;

        }
        
    }
}
