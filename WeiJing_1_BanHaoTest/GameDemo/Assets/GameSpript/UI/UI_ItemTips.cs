using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ItemTips : MonoBehaviour {

    //public string ItemQuality;
    public GameObject ItemName;
    public GameObject ItemDes;
	public GameObject ItemStory;
	public GameObject ItemItemLv;
	public GameObject ItemDi;
    public GameObject Obj_BagOpenSet;
    public GameObject Obj_SaveStoreHouse;
    public GameObject Obj_Diu;
    public GameObject Obj_Btn_StoreHouseSet;

    public string ItemID;
	private string itemQuality;
    private string Text_ItemName;
    private string Text_ItemDes;
	private string Text_ItemStory;
	private string Text_ItemLv;
    private string itemType;            //道具大类
    private string itemSubType;         //道具子类
    public string UIBagSpaceNum;
    public string EquipTipsType;        //1.背包打开  2.装备栏打开 3.无任何按钮显示,点击商店列表弹出

    private bool clickBtnStatus;        //点击按钮状态

    // Use this for initialization
    void Start()
    {

        //获取道具信息
        Text_ItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
        Text_ItemDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemDes", "ID", ItemID, "Item_Template");
		Text_ItemStory = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemBlackDes", "ID", ItemID, "Item_Template");
		Text_ItemLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemLv", "ID", ItemID, "Item_Template");
		itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", ItemID, "Item_Template");
        itemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", ItemID, "Item_Template");

        //获取道具描述的分隔符
        string[] itemDesArray = Text_ItemDes.Split(';');
        string itemMiaoShu = "";
        for (int i = 0; i <= itemDesArray.Length - 1; i++)
        {
            if (itemMiaoShu == "")
            {
                itemMiaoShu = itemDesArray[i];
            }
            else {
                itemMiaoShu = itemMiaoShu + "\n" + itemDesArray[i];
            }
        }
        //数组大于2表示有换行符,否则显示原来的描述
        if (itemDesArray.Length >= 2) {
            Text_ItemDes = itemMiaoShu;
        }

		//根据Tips描述长度缩放底的大小
		int i1 = 0;
		int i2 = 0;
		i1 = (int)((Text_ItemDes.Length)/16)+1;
        if (itemDesArray.Length > i1) {
            i1 = itemDesArray.Length;
        }
		ItemDes.GetComponent<RectTransform>().sizeDelta = new Vector2(252.0f,20.0f*i1);
        Vector2 i1Vec2 = new Vector2 (148.0f,-65-10.0f*i1);
        ItemDes.transform.GetComponent<RectTransform>().anchoredPosition = i1Vec2;


        //赞助宝箱设置描述为绿色
        if (itemSubType == "9") {
            ItemDes.GetComponent<Text>().color = Color.green;
        }
        


        //显示道具描述
        i2 = (int)((Text_ItemStory.Length) / 20) + 1;
		ItemStory.GetComponent<RectTransform>().sizeDelta = new Vector2(260.0f,120.0f+16.0f*i2);
        Vector2 i2Vec2 = new Vector2(148.0f, -205 - 10.0f * i1 - 8.0f*i2);
        ItemStory.transform.GetComponent<RectTransform>().anchoredPosition = i2Vec2;
        float ItemBottomTextNum = 30.0f;
        //显示按钮
        switch (EquipTipsType)
        {
            //背包打开显示对应功能按钮
            case "1":
                Obj_BagOpenSet.SetActive(true);
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
                Obj_BagOpenSet.SetActive(true);
                Obj_Btn_StoreHouseSet.SetActive(false);
                break;
            //商店查看属性
            case "3":
                Obj_BagOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(false);
                ItemBottomTextNum = 0;
                break;

            //仓库查看属性
            case "4":
                Obj_BagOpenSet.SetActive(false);
                Obj_Btn_StoreHouseSet.SetActive(true);
                //ItemBottomTextNum = 0;
                break;
        }

		//设置底的长度
        ItemDi.GetComponent<RectTransform>().sizeDelta = new Vector2(301.0f, 170.0f + i1 * 20.0f + i2 * 16.0f + ItemBottomTextNum);

        //显示道具信息
		ItemName.GetComponent<Text>().text = Text_ItemName;
		ItemName.GetComponent<Text>().color = Game_PublicClassVar.Get_function_UI.QualityReturnColor(itemQuality);
        ItemDes.GetComponent<Text>().text = Text_ItemDes;
		ItemStory.GetComponent<Text>().text = Text_ItemStory;
		if (int.Parse (Text_ItemLv) > 0) {
            ItemItemLv.GetComponent<Text>().text = Text_ItemLv+"级使用";
		} else {
			//ItemItemLv.GetComponent<Text>().text = "无使用等级限制";
            ItemItemLv.GetComponent<Text>().text = "";
		}
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    //使用道具按钮
    public void UseItem()
    {
        if (clickBtnStatus) {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        bool CloseUIStatus = true;

        if (itemType == "1") {

            switch (itemSubType) { 
                //触发技能
                case "0":
                    //判定当前是否在建筑地图
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }
                    
                    string useSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", ItemID, "Item_Template");
                    if (useSkillID != "0") {
                        GameObject skill_0 = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseSkillSet.transform.Find("UI_MainRoseSkill_0").gameObject;
                        skill_0.GetComponent<MainUI_SkillGrid>().SkillID = ItemID;
                        skill_0.GetComponent<MainUI_SkillGrid>().UseSkillID = useSkillID;
                        skill_0.GetComponent<MainUI_SkillGrid>().updataSkill();
                        skill_0.GetComponent<MainUI_SkillGrid>().cleckbutton();
                    }
                    break;
                //触发掉落(掉落地上)
                case "1":
                    //判定当前是否在建筑地图
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }
                    string ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_AI.DropIDToDropItem(ItemUsePar, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID,1);        //销魂自身道具
                break;

                //经验盒子
                case "2":
                    //ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    //实例化制作UI
                    GameObject heZiObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIHeZi);
                    heZiObj.GetComponent<UI_HeZi>().ItemID = ItemID;
                    heZiObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                    heZiObj.transform.localScale = new Vector3(1, 1, 1);
                    heZiObj.transform.localPosition = Vector3.zero;

                break;

                //金币盒子
                case "3":
                    //ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    heZiObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIHeZi);
                    heZiObj.GetComponent<UI_HeZi>().ItemID = ItemID;
                    heZiObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                    heZiObj.transform.localScale = new Vector3(1, 1, 1);
                    heZiObj.transform.localPosition = Vector3.zero;
                break;

                //回城卷轴
                case "4":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID,1);        //销魂自身道具
                    //写入场景
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";                     //设置角色为待机状态
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;           //设置角色不能移动
                    string ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", ItemUsePar, "SceneTransfer_Template");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = ItemUsePar;
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
                    //Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");
                break;

                //制作书
                case "5":
                    //Debug.Log("调用制作书");
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    //清空之前打开的制作书
                    Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EquipMakeSet);
                    //实例化制作UI
                    GameObject equipMakeObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIEquipMake);
                    equipMakeObj.GetComponent<UI_EquipMake>().ItemID = ItemID;
                    equipMakeObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EquipMakeSet.transform);
                    equipMakeObj.transform.localScale = new Vector3(1, 1, 1);
                    equipMakeObj.transform.localPosition = Vector3.zero;
                break;

                //直接获得经验
                case "6":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID,1);        //销魂自身道具
                    Game_PublicClassVar.Get_function_Rose.AddExp(int.Parse(ItemUsePar));
                    //判定自己背包内是否还有道具
                    int itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                    if (itemBagNum > 0) {
                        CloseUIStatus = false;
                    }

                break;

                //直接获得金币
                case "7":
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID,1);        //销魂自身道具
                    Game_PublicClassVar.Get_function_Rose.SendReward("1", ItemUsePar);
                    //判定自己背包内是否还有道具
                    itemBagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                    if (itemBagNum > 0)
                    {
                        CloseUIStatus = false;
                    }

                break;

                //集齐道具触发一个掉落ID
                case "8":
                    //判定当前是否在建筑地图
                    if (Application.loadedLevelName == "EnterGame") {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    string needItemNum = ItemUsePar.Split(';')[0];
                    string itemDropID = ItemUsePar.Split(';')[1];
                    string hideID = ItemUsePar.Split(';')[2];
                    //获取背包道具是否足够
                    int bagItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
                    if (bagItemNum >= int.Parse(needItemNum)) {

                        //获取当前背包是否足够
                        Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, int.Parse(needItemNum));
                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem(itemDropID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position,hideID);
                        
                    }
                    break;
                //充值额度,触发掉落
                case "9":
                    //判定当前是否在建筑地图
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }

                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");

                    string needPayValue = ItemUsePar.Split(';')[0];
                    string rosePayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    itemDropID = ItemUsePar.Split(';')[1];

                    if (float.Parse(rosePayValue) >= float.Parse(needPayValue))
                    {
                        //获取当前背包是否足够
                        Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem(itemDropID, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                        
                    }
                    else {
                        Game_PublicClassVar.Get_function_UI.GameHint("这是作者的感谢宝箱,赞助任意额度支持作者后可即可开启！");
                    }

                break;

                //荣誉
                case "10":
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                    //获取当前一小时产出荣誉
                    string countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    string rongYu_Hour = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureHonor", "ID", countryLv, "Country_Template");
                    //随机获得
                    float value = Random.value + 0.5f;
                    int addRongYuHourValue = (int)(int.Parse(rongYu_Hour) * value);
                    Game_PublicClassVar.Get_function_Country.AddCountryHonor(addRongYuHourValue,true);
                break;


                //繁荣度
                case "11":
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                    //获取当前一小时产出繁荣度
                    countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    string fanRongDu_Hour = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureExp", "ID", countryLv, "Country_Template");
                    //随机获得
                    value = Random.value + 0.5f;
                    int addFanRongDuHourValue = (int)(int.Parse(fanRongDu_Hour) * value);
                    Game_PublicClassVar.Get_function_Country.addCoutryExp(addFanRongDuHourValue,true);
                break;

                //BOSS冷却
                case "12":
                    //判定当前是否在主城场景
                if (Application.loadedLevelName == "EnterGame")
                {
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("道具发挥了效果,所有怪物冷却时间已刷新!");
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("使用此道具请移动到主城场景中使用！");
                }
                break;

                //体力药水
                case "13":
                    //获取当前体力
                    if (Game_PublicClassVar.Get_function_Rose.GetRoseTili() >= 100) {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint("当前体力已满！");
                        return;
                    }
                    Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
                    ItemUsePar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemUsePar", "ID", ItemID, "Item_Template");
                    Game_PublicClassVar.Get_function_Rose.AddTili(int.Parse(ItemUsePar));
                break;

                //宠物召唤
                case "14":

                    //判定当前是否在建筑地图
                    if (Application.loadedLevelName == "EnterGame")
                    {
                        Game_PublicClassVar.Get_function_UI.GameHint("请进入关卡地图在使用此道具");
                        break;
                    }
                    //召唤怪物
                    Game_PublicClassVar.Get_function_Rose.RosePetCreate();
                    /*
                    GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet;
                    //循环删除宠物
                    for (int i = 0; i < monsterSetObj.transform.childCount; i++)
                    {
                        GameObject go = monsterSetObj.transform.GetChild(i).gameObject;
                        //清空AI血条显示
                        Destroy(go.GetComponent<AIPet>().UI_Hp);
                        Destroy(go);
                    }

                    string ifPetChuZhan = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                    if (ifPetChuZhan == "0")
                    {
                        //获取怪物
                        GameObject monsterObj = Instantiate((GameObject)Resources.Load("PetSet/" + "PetObj_1", typeof(GameObject)));
                        monsterObj.transform.SetParent(monsterSetObj.transform);
                        Vector3 CreateVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                        monsterObj.transform.position = CreateVec3;
                        monsterObj.SetActive(false);
                        monsterObj.SetActive(true);

                        //设置当前宠物
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj = monsterObj;
                        //设置宠物出战
                        //string lastday = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_3","1","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                        Game_PublicClassVar.Get_function_UI.GameHint("召唤宠物成功！");
                    }
                    else {
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_3", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                        Game_PublicClassVar.Get_function_UI.GameHint("收回宠物,再次使用可以再次召唤!");
                    }
                    */

                break;

            }
        }

        //防止2次执行
        clickBtnStatus = false;

        //关闭界面
        if (CloseUIStatus)
        {
            CloseUI();
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

        //获取当前道具数量
        string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseBag");
        if (int.Parse(bagItemNum) <= 1)
        {
            //Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(ItemID, 1, UIBagSpaceNum, true);
            Game_PublicClassVar.Get_function_Rose.SellBagSpaceItemToMoney(UIBagSpaceNum);
            CloseUI();
        }
        else {
            GameObject throwItemChoice = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIThrowItemChoice);
            throwItemChoice.transform.SetParent(this.gameObject.transform);
            throwItemChoice.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);        //在中心,后期分辨率不一样可能需要调整
            throwItemChoice.transform.localScale = new Vector3(1, 1, 1);
            throwItemChoice.GetComponent<UI_ThrowItemChoice>().BagSpaceNum = UIBagSpaceNum;
            throwItemChoice.GetComponent<UI_ThrowItemChoice>().ItemID = ItemID;
        }
    }

    //存入仓库
    public void Btn_SaveStoreHouse()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        //获取仓库是否已经满了
        int nullNum = Game_PublicClassVar.Get_function_Rose.StoreHouseNullNum();
        if (nullNum <= 0) {
            Game_PublicClassVar.Get_function_UI.GameHint("仓库已满！");
            return;
        }

        //获取存入道具的数据
        string save_ItemID = ItemID;
        string save_ItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseBag");
        //string save_ItemHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseBag");
        //删除指定道具
        Game_PublicClassVar.Get_function_Rose.CostBagSpaceNumItem(save_ItemID, int.Parse(save_ItemNum), UIBagSpaceNum, true);
        //添加指定道具到仓库
        Game_PublicClassVar.Get_function_Rose.SendRewardToStoreHouse(save_ItemID, int.Parse(save_ItemNum), "1");
        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        //关闭UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
    }

    //取出仓库
    public void Btn_TaskStoreHouse()
    {

        if (clickBtnStatus)
        {
            return;
        }
        clickBtnStatus = true;  //防止因为卡顿二次执行

        int nullNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
        if (nullNum <= 0)
        {
            Game_PublicClassVar.Get_function_UI.GameHint("背包已满！");
            return;
        }
        //获取存入道具的数据
        string save_ItemID = ItemID;
        string save_ItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseStoreHouse");
        //string save_ItemHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", UIBagSpaceNum, "RoseBag");
        //删除指定道具
        Game_PublicClassVar.Get_function_Rose.CostStoreHouseSpaceNumItem(save_ItemID, int.Parse(save_ItemNum), UIBagSpaceNum, true);
        //添加指定道具到背包
        Game_PublicClassVar.Get_function_Rose.SendRewardToBag(save_ItemID, int.Parse(save_ItemNum), "1");
        //更新显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        Destroy(this.gameObject);
        //关闭UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
    }


    public void CloseUI() {
        Destroy(this.gameObject);
        //关闭UI背景图片
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_CloseTips.SetActive(false);
    }
}
