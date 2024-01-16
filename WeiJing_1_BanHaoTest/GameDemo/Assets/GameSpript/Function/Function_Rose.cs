using UnityEngine;
using System.Collections;
using CodeStage.AntiCheat.ObscuredTypes;

public class Function_Rose {

    private Game_PositionVar game_PositionVar;
    private Rose_Proprety rose_Proprety;

	// Use this for initialization
	void Start () {
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    //获取一个随机账号ID
    public string GetZhangHuID() {

        string zhangHaoID = "";
        for (int i = 0; i <= 7; i++)
        {
            int randomValue = (int)((Random.value-0.001f)*10);
            zhangHaoID = zhangHaoID + randomValue.ToString();

        }
        return zhangHaoID;
    }



    //获取当前钻石
    public int GetRoseRMB() {

        ObscuredInt rmb = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        return rmb;
    }

    //获取当前体力
    public int GetRoseTili()
    {
        int tili = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Tili", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        Debug.Log("TiLi = " + tili);
        return tili;
    }

    //获取当前钻石
    public int GetRoseMoney()
    {

        ObscuredInt money = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        return money;
    }



    //获取角色当前等级
    //获取当前钻石
    public int GetRoseLv()
    {

        int roseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        return roseLv;
    }


    public void SetRoseLv(int setLv)
    {
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Lv", setLv.ToString(),"ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
    }



    //扣除指定的钻石
    public bool CostRMB(int rmb) { 
        //获取自身的货币
        int myRmb = GetRoseRMB();
        if (myRmb >= rmb)
        {
            myRmb = myRmb - rmb;
            //写入数据表
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", myRmb.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            return true;
        }
        else {
            return false;
        }
    }

    //增加指定的钻石
    public bool AddRMB(int rmb)
    {
        //获取自身的货币
        ObscuredInt myRmb = GetRoseRMB();
            myRmb = myRmb + rmb;
            //写入数据表
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", myRmb.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            return true;
    }

    //增加指定的充值额度
    public bool AddPayValue(float payValue)
    {
        float nowPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        nowPayValue = nowPayValue + payValue;

        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMBPayValue", nowPayValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //写入每日付费
        float nowEveryDayPayValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_8", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig"));
        nowEveryDayPayValue = nowEveryDayPayValue + payValue;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_8", nowEveryDayPayValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        return true;
    }

    //增加指定的体力值
    public bool AddTili(int tili) { 
        //获取自身体力
        int nowTili = GetRoseTili();
        nowTili = nowTili + tili;
        //体力不能超过最大值
        if (nowTili > 100) {
            nowTili = 100;
        }
        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Tili", nowTili.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        //更新显示
        if (Application.loadedLevelName != "StartGame") {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().UpdataRoseStatus = true;
        }
        return true;
    }

    //增加指定的经验值,expShowType参数传入非0的值将不显示飘字
    public bool AddExp(int exp,string expShowType = "0")
    {
        if (exp == 0) {
            return true;
        }
        //获取自身经验
        //int nowExp = GetRoseTili();
        int nowExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData")) + exp;
        //写入数据表
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseExpNow", nowExp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

		//判定是否升级
		int Rose_Lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        //等级超过65级不获得任何经验
        if (Rose_Lv >= 70) {
            return true;
        }

		int Rose_Exp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", Rose_Lv.ToString(), "RoseExp_Template"));
		int Rose_ExpNow = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

		if (Rose_ExpNow >= Rose_Exp) {
			Rose_Lv = Rose_Lv + 1;
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Lv", Rose_Lv.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
			//多余的经验自动转移到下一级
			int expValue = Rose_ExpNow - Rose_Exp;
			Rose_ExpNow = expValue;
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseExpNow", expValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            //写入当前SP值
            int spValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SP", (spValue+1).ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
			Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
			Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseUpLv = true;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().updataOnly = false;
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow = true; //升级更新任务

            //升级生成备份存档(大于5级每次升级自动存档)
            if (Rose_Lv >= 5) { 
                Game_PublicClassVar.Get_wwwSet.IfSaveRoseData = true;      //开启储备数据
            }
            //存储角色通用数据
            Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "Lv", Rose_Lv.ToString());
		}

		Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_GetExp = true;

		//更新主界面显示
		Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set> ().Obj_UI_RoseExp.GetComponent<UI_MainUIRoseExp>().UpdataRoseExp = true;
        //如果当前角色在建筑界面更新经验显示
        if (Application.loadedLevelName == "EnterGame") {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().UpdataRoseStatus = true;
        }

        //弹出获取提示
        UI_RoseGetItemHint hint = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseGetItemHint.GetComponent<UI_RoseGetItemHint>();
        hint.UpdataHintText = "获得" + exp + "点经验";
        hint.UpdataHint = true;
        //通用快捷提示框
        if (expShowType == "0") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("获得" + exp + "点经验");
        }

        //记录玩家等级数据
        //GA.SetUserLevel(Rose_Lv);
        return true;
    }

    //花费钻石换取体力
    public bool RMBtoTili(int rmb, int tili) {

        if (CostRMB(rmb)) {
            AddTili(tili);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    //根据类型发送对应奖励
    public bool SendReward(string type, string value) {

		//更新背包立即显示
		Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

        switch (type) { 
            
            //发送金币
            case "1":
                //写入对应的值
                ObscuredInt goldValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                goldValue = goldValue + int.Parse(value);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GoldNum", goldValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                return true;
            break;

            //发送钻石
            case "2":
                //写入对应的值
                ObscuredInt rmbValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                rmbValue = rmbValue + int.Parse(value);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", rmbValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                return true;
            break;
        }
        return false;
    }

	//根据类型扣除对应货币
	public bool CostReward(string type, string value) {

		//更新背包立即显示
		Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

		switch (type) { 
			
			//扣除金币
		case "1":
			//写入对应的值
            ObscuredInt goldValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
			goldValue = goldValue - int.Parse(value);
			if(goldValue>=0){
				Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("GoldNum", goldValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
				return true;
			}else{
				return false;
			}
			break;
			
			//扣除钻石
		case "2":
			//写入对应的值
            ObscuredInt rmbValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
			rmbValue = rmbValue - int.Parse(value);
			if(rmbValue>=0){
				Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RMB", rmbValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
				return true;
			}else{
				return false;
			}

			break;

        //扣除荣誉
        case "3":
            //写入对应的值
            ObscuredInt rongyuValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward"));
            rongyuValue = rongyuValue - int.Parse(value);
            if (rongyuValue >= 0)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("CountryHonor", rongyuValue.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                return true;
            }
            else
            {
                return false;
            }

            break;
		}
		return false;
	}

    //发送道具到背包,支持金币(发送道具ID,发送数量,是否广播,装备隐藏属性掉落概率,装备隐藏属性ID)
    public bool SendRewardToBag(string dropID, int dropNum, string broadcastType = "0", float hideProValue = 0,string equipHideID = "0")
    {
        //Debug.Log("hideProValue = " + hideProValue);
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
		//更新背包立即显示
		Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

        bool ifGold = false;
        //掉落为空
        if (dropID == "0") {
            return true;
        }

        //判定掉落是否为金币
        if (dropID == "1")
        {
            int goldNum = dropNum;
            Game_PublicClassVar.Get_function_Rose.SendReward("1", goldNum.ToString());
            ifGold = true;
            //弹窗提示
            //string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
            switch (broadcastType)
            {
                //广播
                case "0":
                    Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "金币");
                    break;
                //不广播
                case "1":
                    break;
            }
			//更新道具任务显示
			//Game_PublicClassVar.Get_function_Task.updataTaskItemID();
            return true;
        }
        else
        {
            ifGold = false;
        }

        if (dropID == "2") {
            AddExp(dropNum);
            //弹窗提示
            return true;
            //ifGold = true;
        }

        if (dropID == "3")
        {
            AddRMB(dropNum);
            return true;
            //ifGold = true;
        }

        if (!ifGold)
        {
            Game_PublicClassVar.Get_game_PositionVar.NpcTaskMainUIShow=true; //拾取道具更新任务
            //Debug.Log("dropID = " + dropID);
            string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
            //将掉落的道具ID添加到背包内
            for (int i = 1; i <= 64; i++)
            {
                //获得当前背包内对应格子的道具ID
                string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
                //Rdate = "0";
                //寻找背包内有没有相同的道具ID
                if (dropID == Rdate)
                {

                    //读取当前道具数量
                    string itemValue = function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag");
                    
                    //读取当前道具的堆叠数量的最大值
                    string itemPileSum = function_DataSet.DataSet_ReadData("ItemPileSum", "ID", dropID, "Item_Template");
                    int itemNum = int.Parse(itemValue) + dropNum; //将数量累加（此处没有顾忌到自己背包格子满的处理方式，以后添加）
                    //当满足堆叠数量,执行道具捡取
                    if (int.Parse(itemPileSum) >= itemNum)
                    {
                        //添加获得的道具数量
                        function_DataSet.DataSet_WriteData("ItemNum", itemNum.ToString(), "ID", i.ToString(), "RoseBag");
                        function_DataSet.DataSet_SetXml("RoseBag");
                        //弹窗提示
                        string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                        switch (broadcastType) { 
                            //广播
                            case "0":
                            Game_PublicClassVar.Get_function_UI.GameHint("你获得"+dropNum.ToString() +"个" + itemName);
                            break;
                            //不广播
                            case "1":
                            break;
                        }
                        
                        //ai_dorpitem.IfRoseTake = true;      //注销拾取的道具
						//更新道具任务显示
						//Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                        if (itemType != "3")
                        {
                            Game_PublicClassVar.Get_game_PositionVar.UpdatTaskStatus = true;
                        }
                        //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                        
                        if (itemType == "1")
                        {
                            Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                        }
                        return true;
                        break;
                    }
                }
                //发现背包格子为空，将数据直接塞进空的格子中（从前面排序）
                if (Rdate == "0")
                {
                    function_DataSet.DataSet_WriteData("ItemID", dropID, "ID", i.ToString(), "RoseBag");
                    function_DataSet.DataSet_WriteData("ItemNum", dropNum.ToString(), "ID", i.ToString(), "RoseBag");
                    function_DataSet.DataSet_WriteData("HideID", equipHideID, "ID", i.ToString(), "RoseBag");
                    
                    //弹窗提示
                    string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                    switch (broadcastType)
                    {
                        //广播
                        case "0":
                            Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                            break;
                        //不广播
                        case "1":
                            break;
                    }
					//更新道具任务显示
                    if (itemType != "3") {
                        Game_PublicClassVar.Get_game_PositionVar.UpdatTaskStatus = true;
                    }
                    
					//Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                    //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                    //string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
                    if (itemType == "1") {
                        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                    }
                    //获取当前道具是否增加极品属性  
                    if (itemType == "3")
                    {
                        //极品概率
                        if (Random.value < hideProValue)
                        {
                            string hideID = "0";
                            try
                            {
                                hideID = ReturnHidePropertyID(dropID);
                            }
                            catch {
                                //重新介入当前隐藏技能ID
                                hideID = "0";
                                int hideNumID = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig"));
                                hideNumID = hideNumID + 1000;
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", hideNumID.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                            }
                            
                            //Debug.Log("hideID = " + hideID + "dropID = "+dropID);
                            if (hideID != "0")
                            {
                                //写入极品属性ID
                                function_DataSet.DataSet_WriteData("HideID", hideID, "ID", i.ToString(), "RoseBag");
                                 Game_PublicClassVar.Get_game_PositionVar.ChouKaHindIdStr = hideID;
                                //Debug.Log("极品属性写入成功");
                            }
                            else
                            {
                                //清空极品属性字段
                                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                            }
                        }

                        //添加收藏
                        AddShouJiItem(dropID);
                    }
                    else {
                        //除装备外,其他道具添加时HideID均为0
                        //function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
                    }
                    function_DataSet.DataSet_SetXml("RoseBag");
                    return true;
                    break;  //跳出循环
                }
                //在结束循环的最后判定道具如果没有被拾取,判定为背包满了
                if (i == 64)
                {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint("背包已满,请及时清理背包！");
                    return false;
                }
            }
        }

        return false;

    }

	//获取一个道具在背包内的数量
	public int ReturnBagItemNum(string itemID){

		int num = 0;
        string bagItemID = "0";
        string bagItemNum = "0";
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //return 0;
		//获取道具ID
		for (int i = 1; i<=64; i++) {

			//获取当前背包格子的道具ID和数量；
            bagItemID = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
			if(bagItemID!="0"){
                bagItemNum = function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag");
				if(itemID == bagItemID){
					num = num+ int.Parse(bagItemNum);
				}
			}
		}
		return num;
	}

	//消耗一个道具在背包内的数量
	public bool CostBagItem(string itemID,int itemNum){
		//获取当前道具ID拥有数量
		int value = ReturnBagItemNum(itemID);
		if (value >= itemNum) {
			//获取道具ID
			for (int i = 1; i<=64; i++) {
				//获取当前背包格子的道具ID和数量；
				string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemID", "ID", i.ToString (), "RoseBag");
				if (bagItemID != "0") {
					string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemNum", "ID", i.ToString (), "RoseBag");
					if (itemID == bagItemID) {
						int num = 0;
						if (int.Parse (bagItemNum) >= itemNum) {
							num = int.Parse (bagItemNum) - itemNum;
							Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemNum", num.ToString (), "ID", i.ToString (), "RoseBag");
                            //如果道具为0,则清空道具
                            if (num == 0) {
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                            }
							Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml ("RoseBag");
							itemNum = 0;
                            i = 65; //跳出循环后面不执行

						} else {
							itemNum = itemNum - int.Parse (bagItemNum);
							num = 0;
							Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemID", "0", "ID", i.ToString (), "RoseBag");
							Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemNum", num.ToString (), "ID", i.ToString (), "RoseBag");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
							Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml ("RoseBag");
						}
					}
				}
			}
			//更新道具任务显示
			Game_PublicClassVar.Get_function_Task.updataTaskItemID();
			//更新背包立即显示
			Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
            if (itemType == "1")
            {
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
			return true;
		} else {
			return false;
		}

		//return num;
	}

    //消耗一个道具在背包内指定位置的数量  （道具ID 道具数量 格子位置, 是否扣除全部）
    public bool CostBagSpaceNumItem(string itemID, int itemNum,string spaceNum,bool costAllSpace)
    {
        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceNum, "RoseBag");
        string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceNum, "RoseBag");
        //是否扣除全部
        if (costAllSpace) {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseBag");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }

        if (itemID == bagItemID)
        {
            int otherValue = int.Parse(bagItemNum) - itemNum;
            if (otherValue > 0)
            {
                //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", otherValue.ToString(), "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
            }
            else {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseBag");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
            }
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }
        else {
            return false;
        }
    }

    //获取当前背包内空置位置的数量
    public int BagNullNum()
    {
        int nullNum = 0;
        for (int i = 1; i <= 64; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (bagItemID == "0")
            {
                nullNum = nullNum + 1;
            }
        }
        return nullNum;
    }


    //获取当前仓库内空置位置的数量
    public int StoreHouseNullNum()
    {
        int nullNum = 0;
        for (int i = 1; i <= 64; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
            if (bagItemID == "0")
            {
                nullNum = nullNum + 1;
            }
        }
        return nullNum;
    }

    //检查背包内是否有足够数量的位置
    public bool IfBagNullNum(int needNullNum)
    {
        int nullNum = 0;
        for (int i = 1; i <= 64; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (bagItemID == "0")
            {
                nullNum = nullNum + 1;
            }
        }

        if (nullNum >= needNullNum)
        {
            return true;
        }
        else {
            return false;
        }
        
    }


    //获取当前背包内第一个空置的位置
    public string BagFirstNullNum() {

        for (int i = 1; i <= 64; i++)
        {
            //获取当前背包格子的道具ID和数量；
            string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (bagItemID == "0")
            {
                return i.ToString();
            }
        }
        return "-1";
    }

    /*
	//消耗一个道具在背包内的数量，并增加该道具售卖价格的金币
	public bool CostBagItemMoney(string itemID,int itemNum){
        if (CostBagItem(itemID, itemNum))
        {
            //增加制定的金币
            //string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData ("ItemID", "ID",itemID, "Item_Template");
            string itemMoney = "100";
            SendRewardToBag("1", int.Parse(itemMoney));
            return true;
        }
        else {
            return false;
        }
	}
    */
    //出售制定背包格子的道具
    public bool SellBagSpaceItemToMoney(string spaceID) {
        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
        if(itemID!="0"){
            string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceID, "RoseBag");
            string itemHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceID, "RoseBag");
            string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", itemID, "Item_Template");
            int sellValue = int.Parse(itemMoney) * int.Parse(itemNum);
            //删除背包内的道具
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemID", "0", "ID", spaceID, "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData ("ItemNum","0", "ID", spaceID, "RoseBag");
			Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml ("RoseBag");
            //发送货币
            SendRewardToBag("1", sellValue);
            //发送经验
            if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template") == "3")
            {
                int itemQuality = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template"));
                float expProValue = 1;
                if (itemQuality < 3)
                {
                    expProValue = 0.35f;
                }
                else {
                    expProValue = 0.75f;
                }
                int BuyMoney = (int)(int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyMoney", "ID", itemID, "Item_Template")) * expProValue);
                SendRewardToBag("2", BuyMoney);
            }
            //记录出售数据
            /*
            string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (StoreSellItemListText == "")
            {
                StoreSellItemListText = itemID + "," + itemNum + "," + itemHide;
            }
            else {
                StoreSellItemListText = itemID + "," + itemNum + "," + itemHide + ";" + StoreSellItemListText;
            }
            
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", StoreSellItemListText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            */

            //更新回购界面UI
            Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = true;
            //更新背包立即显示
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
            string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
            if (itemType == "1")
            {
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
            return true;
        }else{
            return false;
        }
    }


    //一键出售背包对应类型的道具
    public void SellBagYiJianItemToMoney(string sellType)
    {

        for (int i = 1; i <= 64; i++) {
            string spaceID = i.ToString();
            //获取道具ID
            string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceID, "RoseBag");
            if (itemID != "0") {
                //获取道具类型
                string itemType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template");
                //获取道具品质
                string itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template");

                bool ifSellStatus = false;
                //判断出售类型
                if (itemType == sellType)
                {
                    //判断出售品质
                    if (int.Parse(itemQuality) <= 2)
                    {
                        ifSellStatus = true;
                    }

                    if (itemType == "1") {
                        string ItemSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", itemID, "Item_Template");
                        if (ItemSubType != "5") {
                            ifSellStatus = false;
                        }
                    }
                }

                if (ifSellStatus) {
                    string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceID, "RoseBag");
                    string itemHide = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", spaceID, "RoseBag");
                    string itemMoney = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellMoney", "ID", itemID, "Item_Template");
                    int sellValue = int.Parse(itemMoney) * int.Parse(itemNum);
                    //删除背包内的道具
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceID, "RoseBag");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceID, "RoseBag");
                    
                    //发送货币
                    SendRewardToBag("1", sellValue);
                    //发送经验
                    //int itemQuality1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template"));

                    if (Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", itemID, "Item_Template") == "3")
                    {

                        float expProValue = 1;
                        if (int.Parse(itemQuality) < 3)
                        {
                            expProValue = 0.35f;
                        }
                        else
                        {
                            expProValue = 0.75f;
                        }
                        int BuyMoney = (int)(int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyMoney", "ID", itemID, "Item_Template")) * expProValue);
                        SendRewardToBag("2", BuyMoney);
                        
                    }

                    //记录出售数据
                    /*
                    string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    if (StoreSellItemListText == "")
                    {
                        StoreSellItemListText = itemID + "," + itemNum + "," + itemHide;
                    }
                    else
                    {
                        StoreSellItemListText = itemID + "," + itemNum + "," + itemHide + ";" + StoreSellItemListText;
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", StoreSellItemListText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    */

                    //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                    if (itemType == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                    }
                }
            }
        }

        //更新回购界面UI
        Game_PublicClassVar.Get_game_PositionVar.UpdataSellUIStatus = true;
        //更新背包立即显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
        //存储角色
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBag");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

    }

    //消除指定回购道具里的值,参数填入文本,例如1000001.20
    public bool RemoveSellItemID(string removeItemText) {
        string StoreSellItemListText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SellItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] StoreSellItemList = StoreSellItemListText.Split(';');
        string writeText = "";
        for (int i = 0; i <= StoreSellItemList.Length - 1; i++) {
            if (StoreSellItemList[i] == removeItemText)
            {

            }
            else {
                writeText = writeText + StoreSellItemList[i]+";";
            }
        }

        if (writeText != "") {
            writeText = writeText.Substring(0, writeText.Length - 1);
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SellItemID", writeText, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

        return true;

    }


	//更新当前角色属性(是否提示属性变化)
	public bool  UpdateRoseProperty(bool ifHint = false) {
		
		int rose_Hp = 0;  //初始化血量
		int rose_ActMin = 0;  //初始化最低攻击
		int rose_ActMax = 0;  //初始化最大攻击
		int rose_DefMin = 0;  //初始化最小物防
		int rose_DefMax = 0;  //初始化最大物防
		int rose_AdfMin = 0;  //初始化最小魔防
		int rose_AdfMax = 0;  //初始化最大魔防
        float rose_MoveSpeed = 0.0f;    //初始化角色速度
		float rose_Cri = 0.0f;          //初始暴击值
		float rose_Hit = 0.0f;          //初始命中值
		float rose_Dodge = 0.0f;        //初始闪避值
		float rose_DefAdd = 0.0f;       //初始物理免伤
		float rose_AdfAdd = 0.0f;       //初始魔法免伤
		float rose_DamgeSub = 0.0f;     //初始总免伤
        float rose_DamgeAdd = 0.0f;     //初始总免伤
        float rose_Lucky = 0;           //幸运

		Rose_Proprety rose_Proprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
		Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;
		WWWSet wwwSet = Game_PublicClassVar.Get_wwwSet;

		//读取角色名称
		string rose_Name = functionDataSet.DataSet_ReadData("Name","ID", wwwSet.RoseID,"RoseData");
		rose_Proprety.Rose_Name = rose_Name;
		
		//获取角色上次保存等级
		int rose_Lv = int.Parse(functionDataSet.DataSet_ReadData("Lv","ID", wwwSet.RoseID,"RoseData"));
		rose_Proprety.Rose_Lv = rose_Lv;
		
		//------------------------------------更新血量
		//获取职业属性
		string 	rose_Occupation = functionDataSet.DataSet_ReadData("RoseOccupation", "ID", wwwSet.RoseID,"RoseData");
		rose_Proprety.Rose_Occupation = rose_Occupation;

		//获取装备属性
		int hp_Equip = 0;
		int act_EquipMin = 0;
		int act_EquipMax = 0;
		int def_EquipMin = 0; 
		int def_EquipMax = 0;
		int adf_EquipMin = 0;
		int adf_EquipMax = 0;
        float cir_Equip = 0;
        float hit_Equip = 0;
        float dodge_Equip = 0;
        float damgeAdd_Equip = 0;
        float damgeSub_Equip = 0;
        float speed_Equip = 0;
        int lucky_Equip = 0;


        //套装字符串
        string equipSuitIDStr = "";

		//循环自身的装备
		for (int i = 1; i <= 13; i++) {

			string equipID_1 = functionDataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(),"RoseEquip");
			if(equipID_1!="0"){
				string equipID = functionDataSet.DataSet_ReadData("ItemEquipID", "ID", equipID_1,"Item_Template");
                int hp_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Hp", "ID", equipID, "Equip_Template"));
				int act_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAct", "ID", equipID, "Equip_Template"));
				int act_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAct", "ID", equipID, "Equip_Template"));
				int def_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinDef", "ID", equipID, "Equip_Template"));
				int def_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxDef", "ID", equipID, "Equip_Template"));
				int adf_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAdf", "ID", equipID, "Equip_Template"));
				int adf_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equipID, "Equip_Template"));
                float cir_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Cri", "ID", equipID, "Equip_Template"));
                float hit_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Hit", "ID", equipID, "Equip_Template"));
                float dodge_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Dodge", "ID", equipID, "Equip_Template"));
                float damgeAdd_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equipID, "Equip_Template"));
                float damgeSub_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equipID, "Equip_Template"));
                float speed_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Speed", "ID", equipID, "Equip_Template"));
                int lucky_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Lucky", "ID", equipID, "Equip_Template"));

                //获取极品属性
                string hideID = functionDataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseEquip");
                if (hideID != "0") {
                    string hideProperListStr = functionDataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
                    string[] hideProperty = hideProperListStr.Split(';');
                    //隐藏属性
                    /*
                    1:血量上限
                    2:物理攻击最大值
                    3:物理防御最大值
                    4:魔法防御最大值
                    */
                    //循环加入各个隐藏属性
                    if (hideProperListStr != "")
                    {
                        for (int y = 0; y <= hideProperty.Length - 1; y++)
                        {
                            string hidePropertyType = hideProperty[y].Split(',')[0];
                            string hidePropertyValue = hideProperty[y].Split(',')[1];

                            switch (hidePropertyType)
                            {
                                //血量上限
                                case "1":
                                    hp_Equip_value = hp_Equip_value + int.Parse(hidePropertyValue);
                                    break;
                                //物理攻击最大值
                                case "2":
                                    act_EquipMax_value = act_EquipMax_value + int.Parse(hidePropertyValue);
                                    break;
                                case "3":
                                    //物理防御最大值
                                    def_EquipMax_value = def_EquipMax_value + int.Parse(hidePropertyValue);
                                    break;
                                //魔法防御最大值
                                case "4":
                                    adf_EquipMax_value = adf_EquipMax_value + int.Parse(hidePropertyValue);
                                    break;

                                //幸运值
                                case "101":
                                    lucky_Equip_value = lucky_Equip_value + int.Parse(hidePropertyValue);
                                    break;
                            }
                        }
                    }
                }



				//累加属性
				hp_Equip = hp_Equip + hp_Equip_value;
				act_EquipMin = act_EquipMin + act_EquipMin_value;
				act_EquipMax = act_EquipMax + act_EquipMax_value;
				def_EquipMin = def_EquipMin + def_EquipMin_value;
				def_EquipMax = def_EquipMax + def_EquipMax_value;
				adf_EquipMin = adf_EquipMin + adf_EquipMin_value;
				adf_EquipMax = adf_EquipMax + adf_EquipMax_value;
                cir_Equip = cir_Equip + cir_Equip_value;
                hit_Equip = hit_Equip + hit_Equip_value;
                dodge_Equip = dodge_Equip + dodge_Equip_value;
                damgeAdd_Equip = damgeAdd_Equip + dodge_Equip_value;
                damgeSub_Equip = damgeSub_Equip + damgeSub_Equip_value;
                speed_Equip = speed_Equip + speed_Equip_value;
                lucky_Equip = lucky_Equip + lucky_Equip_value;

                //循环自身装备所附带的技能ID
                //Game_PublicClassVar.Get_function_Skill.EquipCostSkillID(equipID_1);

                //循环自身装备套装的ID
                string equipSuitID = functionDataSet.DataSet_ReadData("EquipSuitID", "ID", equipID, "Equip_Template");
                if (equipSuitID != "0") {
                    if (equipSuitIDStr == "")
                    {
                        equipSuitIDStr = equipSuitIDStr + equipSuitID;
                    }
                    else
                    {
                        //循环判定,防止重复的套装ID
                        string[] equipSuitStrID = equipSuitIDStr.Split(';');
                        bool addStatus = true;
                        for (int y = 0; y <= equipSuitStrID.Length - 1; y++)
                        {
                            if (equipSuitID == equipSuitStrID[y])
                            {
                                addStatus = false;
                            }
                        }

                        if (addStatus)
                        {
                            equipSuitIDStr = equipSuitIDStr + ";" + equipSuitID;
                        }
                    }
                }
			}
		}



        //套装子ID
        ArrayList equipSuitPropertyList = new ArrayList();
        //Debug.Log("equipSuitIDStr = " + equipSuitIDStr);
        if (equipSuitIDStr != "") {
            string[] equipSuitStrID = equipSuitIDStr.Split(';');
            for (int i = 0; i <= equipSuitStrID.Length - 1; i++) { 
                //获得子套装属性
                string[] needEquipIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipID", "ID", equipSuitStrID[i], "EquipSuit_Template").Split(';');
                string[] needEquipNumSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipNum", "ID", equipSuitStrID[i], "EquipSuit_Template").Split(';');
                string[] suitPropertyIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuitPropertyID", "ID", equipSuitStrID[i], "EquipSuit_Template").Split(';');
                //获取自身套装数量
                int equipSuitNum = Game_PublicClassVar.Get_function_Rose.returnEquipSuitNum(needEquipIDSet, needEquipNumSet);

                for (int y = 0; y <= suitPropertyIDSet.Length - 1; y++)
                {
                    string triggerSuitNum = suitPropertyIDSet[y].Split(',')[0];
                    string triggerSuitPropertyID = suitPropertyIDSet[y].Split(',')[1];
                    //满足条件套装
                    if (equipSuitNum >= int.Parse(triggerSuitNum))
                    {
                        equipSuitPropertyList.Add(triggerSuitPropertyID);
                    }
                }
            }
        }


        //获取装备套装属性
        int hp_EquipSuit = 0;
        int act_EquipSuitMin = 0;
        int act_EquipSuitMax = 0;
        int def_EquipSuitMin = 0;
        int def_EquipSuitMax = 0;
        int adf_EquipSuitMin = 0;
        int adf_EquipSuitMax = 0;
        float cir_EquipSuit = 0;
        float hit_EquipSuit = 0;
        float dodge_EquipSuit = 0;
        float damgeAdd_EquipSuit = 0;
        float damgeSub_EquipSuit = 0;
        float speed_EquipSuit = 0;
        int lucky_EquipSuit = 0;

        string equipSuitSkillIDStr = "";
        foreach (string equipSuitPropertyID in equipSuitPropertyList) {
            
            //循环添加套装属性
            int hp_EquipSuit_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Hp", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int act_EquipSuitMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAct", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int act_EquipSuitMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAct", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int def_EquipSuitMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinDef", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int def_EquipSuitMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxDef", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int adf_EquipSuitMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAdf", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int adf_EquipSuitMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float cir_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Cri", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float hit_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Hit", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float dodge_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Dodge", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float damgeAdd_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float damgeSub_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            float speed_EquipSuit_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Speed", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));
            int lucky_EquipSuit_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Lucky", "ID", equipSuitPropertyID, "EquipSuitProperty_Template"));


            //累加属性
            hp_EquipSuit = hp_EquipSuit + hp_EquipSuit_value;
            act_EquipSuitMin = act_EquipSuitMin + act_EquipSuitMin_value;
            act_EquipSuitMax = act_EquipSuitMax + act_EquipSuitMax_value;
            def_EquipSuitMin = def_EquipSuitMin + def_EquipSuitMin_value;
            def_EquipSuitMax = def_EquipSuitMax + def_EquipSuitMax_value;
            adf_EquipSuitMin = adf_EquipSuitMin + adf_EquipSuitMin_value;
            adf_EquipSuitMax = adf_EquipSuitMax + adf_EquipSuitMax_value;
            cir_EquipSuit = cir_EquipSuit + cir_EquipSuit_value;
            hit_EquipSuit = hit_EquipSuit + hit_EquipSuit_value;
            dodge_EquipSuit = dodge_EquipSuit + dodge_EquipSuit_value;
            damgeAdd_EquipSuit = damgeAdd_EquipSuit + damgeAdd_EquipSuit_value;
            damgeSub_EquipSuit = damgeSub_EquipSuit + damgeSub_EquipSuit_value;
            speed_EquipSuit = speed_EquipSuit + speed_EquipSuit_value;
            lucky_EquipSuit = lucky_EquipSuit + lucky_EquipSuit_value;

            //Debug.Log("子套装ID");
            string equipSuitSkillID = functionDataSet.DataSet_ReadData("EquipSuitSkillID", "ID", equipSuitPropertyID, "EquipSuitProperty_Template");
            if (equipSuitSkillID != "0") {
                //写入套装ID技能
                if (equipSuitSkillIDStr == "")
                {
                    equipSuitSkillIDStr = equipSuitSkillID;
                }
                else
                {
                    equipSuitSkillIDStr = equipSuitSkillIDStr + "," + equipSuitSkillID;
                }
            }
        }

        //写入套装技能
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSuitSkillID", equipSuitSkillIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //更新装备技能（放在这里需要上面先写入套装技能ID后再进行更新）
        Game_PublicClassVar.Get_function_Skill.UpdataEquipSkillID();

        //更新收集装备
        int hp_ShouJiItem = 0;
        float hpPro_ShouJiItem = 0;
        int act_ShouJiItemMin = 0;
        int act_ShouJiItemMax = 0;
        float actPro_ShouJiItemMin = 0;
        float actPro_ShouJiItemMax = 0;
        int def_ShouJiItemMin = 0;
        int def_ShouJiItemMax = 0;
        float defPro_ShouJiItemMin = 0;
        float defPro_ShouJiItemMax = 0;
        int adf_ShouJiItemMin = 0;
        int adf_ShouJiItemMax = 0;
        float adfPro_ShouJiItemMin = 0;
        float adfPro_ShouJiItemMax = 0;

        float cir_ShouJiItem = 0;
        float hit_ShouJiItem = 0;
        float dodge_ShouJiItem = 0;
        float defAdd_ShouJiItem = 0.0f;       //初始物理免伤
        float adfAdd_ShouJiItem = 0.0f;       //初始魔法免伤
        float damgeAdd_ShouJiItem = 0;
        float damgeSub_ShouJiItem = 0;
        float speed_ShouJiItem = 0;
        int lucky_ShouJiItem = 0;

        string startListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (startListStr != "" && startListStr != "0") {
            string[] shouJiStartList = startListStr.Split(';');
            string[] ShouJiItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ShouJiItemList", "GameMainValue").Split(';');

            //循环章节星数
            for (int i = 0; i < shouJiStartList.Length; i++) {

                //循环章节里面3个难度
                for (int y = 1; y <= 3; y++) {

                    string souSuoStr_1 = "ProList" + y + "_StartNum";
                    string souSuoStr_2 = "ProList"+ y +"_Value";
                    int nowStarNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(souSuoStr_1, "ID", ShouJiItemList[i], "ShouJiItemPro_Template"));
                    //对当前拥有的星数大于要求的星数就激活属性
                    if (int.Parse(shouJiStartList[i]) >= nowStarNum) {

                        string ShouJipropretyStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(souSuoStr_2, "ID", ShouJiItemList[i], "ShouJiItemPro_Template");
                        string[] ShouJiproprety = ShouJipropretyStr.Split(',');

                        string propretyShouJiType = ShouJiproprety[0];
                        string propretyShouJiAddType = ShouJiproprety[1];
                        string propretyShouJiValue = ShouJiproprety[2];

                        switch (propretyShouJiType)
                        {
                            //血量
                            case "10":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        hp_ShouJiItem = hp_ShouJiItem + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        hpPro_ShouJiItem = hpPro_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //物理最大攻击
                            case "11":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        act_ShouJiItemMax = act_ShouJiItemMax + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        actPro_ShouJiItemMax = actPro_ShouJiItemMax + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //物理防御
                            case "17":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        def_ShouJiItemMax = def_ShouJiItemMax + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        defPro_ShouJiItemMax = defPro_ShouJiItemMax + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //魔法防御
                            case "20":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        adf_ShouJiItemMax = adf_ShouJiItemMax + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        adfPro_ShouJiItemMax = adfPro_ShouJiItemMax + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;


                            //暴击
                            case "30":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        cir_ShouJiItem = cir_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        cir_ShouJiItem = cir_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //命中
                            case "31":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        hit_ShouJiItem = hit_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        hit_ShouJiItem = hit_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //闪避
                            case "32":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        dodge_ShouJiItem = dodge_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        dodge_ShouJiItem = dodge_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //物理免伤
                            case "33":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        defAdd_ShouJiItem = defAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        defAdd_ShouJiItem = defAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //魔法免伤
                            case "34":
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        adfAdd_ShouJiItem = adfAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        adfAdd_ShouJiItem = adfAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //速度
                            case "35":
                                //Debug.Log("开始出发加速效果");
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        speed_ShouJiItem = speed_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        speed_ShouJiItem = speed_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //伤害免伤
                            case "36":
                                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        damgeSub_ShouJiItem = damgeSub_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        damgeSub_ShouJiItem = damgeSub_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;

                            //伤害加成
                            case "37":
                                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        damgeAdd_ShouJiItem = damgeAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        damgeAdd_ShouJiItem = damgeAdd_ShouJiItem + float.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;


                            //幸运
                            case "101":
                                //Debug.Log("Rose_DamgeSubtractMul_1 == " + roseProprety.Rose_DamgeSubtractMul_1 + "buffValue_add = " + buffValue_add);
                                switch (propretyShouJiAddType)
                                {
                                    //固定值
                                    case "1":
                                        lucky_ShouJiItem = lucky_ShouJiItem + int.Parse(propretyShouJiValue);
                                        break;
                                    //百分比
                                    case "2":
                                        lucky_ShouJiItem = lucky_ShouJiItem + int.Parse(propretyShouJiValue);
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }







        



        //获取角色属性
        Rose_Proprety roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();

		//血量上限
		int hp_Base = int.Parse(functionDataSet.DataSet_ReadData("BaseHp", "ID", rose_Occupation, "Occupation_Template"));
		int hp_LvUp = int.Parse(functionDataSet.DataSet_ReadData("LvUpHp", "ID", rose_Occupation, "Occupation_Template"));
		//hp_Equip = 0; //装备属性，以后添加
		//血量总和计算
        rose_Hp = (int)((hp_Base + hp_LvUp * rose_Lv + hp_Equip + hp_EquipSuit + roseProprety.Rose_HpAdd_1 + hp_ShouJiItem) * (1.0f + roseProprety.Rose_HpMul_1 + hpPro_ShouJiItem) + roseProprety.Rose_HpAdd_2);
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_Hp = rose_Hp;
		
		
		//------------------------------------更新最小攻击
        float act_BaseMin = float.Parse(functionDataSet.DataSet_ReadData("BaseMinAct", "ID", rose_Occupation, "Occupation_Template"));
        float act_LvUpMin = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinAct", "ID", rose_Occupation, "Occupation_Template"));
		//act_EquipMin = 0;  //装备属性预留
		//最小攻击总和计算
        rose_ActMin = (int)((act_BaseMin + act_LvUpMin * rose_Lv + act_EquipMin + act_EquipSuitMin + roseProprety.Rose_ActMinAdd_1) * (1.0f + roseProprety.Rose_ActMinMul_1) + roseProprety.Rose_ActMinAdd_2);
		//弹出属性提示
        if (rose_ActMin > rose_Proprety.Rose_ActMin) {
            if (ifHint) {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("攻击下限+" + (rose_ActMin - rose_Proprety.Rose_ActMin), "7FFF00FF");
            }
        }
        //结算出结果后进行属性赋值
		rose_Proprety.Rose_ActMin = rose_ActMin;
		
		
		//------------------------------------更新最大攻击
        float act_BaseMax = float.Parse(functionDataSet.DataSet_ReadData("BaseMaxAct", "ID", rose_Occupation, "Occupation_Template"));
        float act_LvUpMax = float.Parse(functionDataSet.DataSet_ReadData("LvUpMaxAct", "ID", rose_Occupation, "Occupation_Template"));
		//act_EquipMax = 0;  //装备属性预留

		//最大攻击总和计算
        rose_ActMax = (int)((act_BaseMax + act_LvUpMax * rose_Lv + act_EquipMax + act_EquipSuitMax + roseProprety.Rose_ActMaxAdd_1 + act_ShouJiItemMax) * (1.0f + roseProprety.Rose_ActMaxMul_1 + actPro_ShouJiItemMax) + roseProprety.Rose_ActMaxAdd_2);
        //弹出属性提示
        if (rose_ActMax > rose_Proprety.Rose_ActMax)
        {
            if (ifHint){
                Game_PublicClassVar.Get_function_UI.GameGirdHint("攻击上限+" + (rose_ActMax - rose_Proprety.Rose_ActMax), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_ActMax = rose_ActMax;
		
		//------------------------------------更新最小物防
        float def_BaseMin = float.Parse(functionDataSet.DataSet_ReadData("BaseMinDef", "ID", rose_Occupation, "Occupation_Template"));
        float def_LvUpMin = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinDef", "ID", rose_Occupation, "Occupation_Template"));
		//def_EquipMin = 0;  //装备属性预留
		//最大攻击总和计算
        rose_DefMin = (int)((def_BaseMin + def_LvUpMin * rose_Lv + def_EquipMin + def_EquipSuitMin + roseProprety.Rose_DefMinAdd_1) * (1.0f + roseProprety.Rose_DefMinMul_1) + roseProprety.Rose_DefMinAdd_2);
        //弹出属性提示
        if (rose_DefMin > rose_Proprety.Rose_DefMin)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("防御下限+" + (rose_DefMin - rose_Proprety.Rose_DefMin), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_DefMin = rose_DefMin;
		
		//------------------------------------更新最大物防
        float def_BaseMax = float.Parse(functionDataSet.DataSet_ReadData("BaseMaxDef", "ID", rose_Occupation, "Occupation_Template"));
        float def_LvUpMax = float.Parse(functionDataSet.DataSet_ReadData("LvUpMaxDef", "ID", rose_Occupation, "Occupation_Template"));
		//def_EquipMax = 0;  //装备属性预留
		//最大攻击总和计算
        rose_DefMax = (int)((def_BaseMax + def_LvUpMax * rose_Lv + def_EquipMax + def_EquipSuitMax + roseProprety.Rose_DefMaxAdd_1 + def_ShouJiItemMax) * (1.0f + roseProprety.Rose_DefMaxMul_1 + defPro_ShouJiItemMax) + roseProprety.Rose_DefMaxAdd_2);
        //弹出属性提示
        if (rose_DefMax > rose_Proprety.Rose_DefMax)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("防御上限+" + (rose_DefMax - rose_Proprety.Rose_DefMax), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_DefMax = rose_DefMax;
		
		//------------------------------------更新最小魔防
        float adf_BaseMin = float.Parse(functionDataSet.DataSet_ReadData("BaseMinAdf", "ID", rose_Occupation, "Occupation_Template"));
        float adf_LvUpMin = float.Parse(functionDataSet.DataSet_ReadData("LvUpMinAdf", "ID", rose_Occupation, "Occupation_Template"));
		//adf_EquipMin = 0;  //装备属性预留
		//最大攻击总和计算
        rose_AdfMin = (int)((adf_BaseMin + adf_LvUpMin * rose_Lv + adf_EquipMin + adf_EquipSuitMin + roseProprety.Rose_AdfMinAdd_1) * (1.0f + roseProprety.Rose_AdfMinMul_1) + roseProprety.Rose_AdfMinAdd_2);
        //弹出属性提示
        if (rose_AdfMin > rose_Proprety.Rose_AdfMin)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("魔防下限+" + (rose_AdfMin - rose_Proprety.Rose_AdfMin), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_AdfMin = rose_AdfMin;
		
		//------------------------------------更新最大魔防
        float adf_BaseMax = float.Parse(functionDataSet.DataSet_ReadData("BaseMaxAdf", "ID", rose_Occupation, "Occupation_Template"));
        float adf_LvUpMax = float.Parse(functionDataSet.DataSet_ReadData("LvUpMaxAdf", "ID", rose_Occupation, "Occupation_Template"));
		//adf_EquipMax = 0;  //装备属性预留

		//最大攻击总和计算
        rose_AdfMax = (int)((adf_BaseMax + adf_LvUpMax * rose_Lv + adf_EquipMax + adf_EquipSuitMax + roseProprety.Rose_AdfMaxAdd_1 + adf_ShouJiItemMax) * (1.0f + roseProprety.Rose_AdfMaxMul_1 + adfPro_ShouJiItemMax) + roseProprety.Rose_AdfMaxAdd_2);
        //弹出属性提示
        if (rose_AdfMax > rose_Proprety.Rose_AdfMax)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("魔防上限+" + (rose_AdfMax - rose_Proprety.Rose_AdfMax), "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_AdfMax = rose_AdfMax;

        //------------------------------------更新移动速度
        rose_MoveSpeed = float.Parse(functionDataSet.DataSet_ReadData("BaseMoveSpeed", "ID", rose_Occupation, "Occupation_Template"));
        rose_MoveSpeed = (rose_MoveSpeed + speed_Equip + speed_EquipSuit + roseProprety.Rose_MoveSpeedAdd_1) * (1.0f + roseProprety.Rose_MoveSpeedMul_1 + speed_ShouJiItem) + roseProprety.Rose_MoveSpeedAdd_2;
        //提示
        if (rose_MoveSpeed > rose_Proprety.Rose_MoveSpeed)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("移动速度提升" + (rose_MoveSpeed - rose_Proprety.Rose_MoveSpeed)*100+"%", "7FFF00FF");
            }
        }
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_MoveSpeed = rose_MoveSpeed;
        rose_Proprety.rose_LastMoveSpeed = rose_MoveSpeed;
		
		//------------------------------------更新暴击值
		rose_Cri = float.Parse(functionDataSet.DataSet_ReadData("BaseCri", "ID", rose_Occupation, "Occupation_Template"));
        rose_Cri = rose_Cri + cir_Equip + cir_EquipSuit + roseProprety.Rose_CriMul_1 + cir_ShouJiItem;
        //提示
        if (rose_Cri > rose_Proprety.Rose_Cri)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("暴击率提升" + (rose_Cri - rose_Proprety.Rose_Cri) * 100 + "%", "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_Cri = rose_Cri;
		
		//------------------------------------更新命中值
		rose_Hit = float.Parse(functionDataSet.DataSet_ReadData("BaseHit", "ID", rose_Occupation, "Occupation_Template"));
        rose_Hit = rose_Hit + hit_Equip + hit_EquipSuit + rose_Proprety.Rose_HitMul_1 + hit_ShouJiItem;

        //提示
        if (rose_Hit > rose_Proprety.Rose_Hit)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("命中率提升" + (rose_Hit - rose_Proprety.Rose_Hit) * 100 + "%", "7FFF00FF");
            }
        }

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_Hit = rose_Hit;
		
		//------------------------------------更新闪避值
		rose_Dodge = float.Parse(functionDataSet.DataSet_ReadData("BaseDodge", "ID", rose_Occupation, "Occupation_Template"));
        rose_Dodge = rose_Dodge + dodge_Equip + dodge_EquipSuit + rose_Proprety.Rose_DodgeMul_1 + dodge_ShouJiItem;

        //提示
        if (rose_Dodge > rose_Proprety.Rose_Dodge)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("闪避率提升" + (rose_Dodge - rose_Proprety.Rose_Dodge) * 100 + "%", "7FFF00FF");
            }
        }
		
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_Dodge = rose_Dodge;
		
		//------------------------------------更新物理免伤
		rose_DefAdd = float.Parse(functionDataSet.DataSet_ReadData("BaseDefAdd", "ID", rose_Occupation, "Occupation_Template"));
        rose_DefAdd = rose_DefAdd + rose_Proprety.Rose_DefMul_1 + defAdd_ShouJiItem;

        //提示
        if (rose_DefAdd > rose_Proprety.Rose_DefAdd)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("物理免伤提升" + (rose_DefAdd - rose_Proprety.Rose_DefAdd) * 100 + "%", "7FFF00FF");
            }
        }

		//结算出结果后进行属性赋值
		rose_Proprety.Rose_DefAdd = rose_DefAdd;
		
		//------------------------------------更新魔法免伤
		rose_AdfAdd = float.Parse(functionDataSet.DataSet_ReadData("BaseAdfAdd", "ID", rose_Occupation, "Occupation_Template"));
        rose_AdfAdd = rose_AdfAdd + rose_Proprety.Rose_AdfMul_1 + adfAdd_ShouJiItem;
        //提示
        if (rose_AdfAdd > rose_Proprety.Rose_AdfAdd)
        {
            if (ifHint)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("魔法免伤提升" + (rose_AdfAdd - rose_Proprety.Rose_AdfAdd) * 100 + "%", "7FFF00FF");
            }
        }
		//结算出结果后进行属性赋值
		rose_Proprety.Rose_AdfAdd = rose_AdfAdd;
		
		//------------------------------------更新总免伤
		rose_DamgeSub = float.Parse(functionDataSet.DataSet_ReadData("DamgeAdd", "ID", rose_Occupation, "Occupation_Template"));
        rose_DamgeSub = rose_DamgeSub + damgeSub_Equip + damgeSub_EquipSuit + rose_Proprety.Rose_DamgeSubtractMul_1;
		//结算出结果后进行属性赋值
        rose_Proprety.Rose_DamgeSub = rose_DamgeSub + damgeSub_Equip + damgeSub_EquipSuit + damgeSub_ShouJiItem;

        //------------------------------------更新伤害加成
        rose_DamgeAdd = 0;
        rose_DamgeAdd = rose_DamgeAdd + damgeAdd_Equip + damgeAdd_EquipSuit + damgeAdd_ShouJiItem;
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_DamgeAdd = rose_DamgeAdd;

        //------------------------------------更新幸运值
        rose_Lucky = 0;
        rose_Lucky = rose_Lucky + lucky_Equip + lucky_EquipSuit + lucky_ShouJiItem;
        //结算出结果后进行属性赋值
        rose_Proprety.Rose_Lucky = rose_Lucky;

		return true;
		
	}

    //增加角色固定生命值
    public bool addRoseHp(int addHp) {

        Game_PositionVar game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        rose_Proprety.Rose_HpNow = rose_Proprety.Rose_HpNow + addHp;
        //防止过量加血
        if (rose_Proprety.Rose_HpNow >= rose_Proprety.Rose_Hp) {
            rose_Proprety.Rose_HpNow = rose_Proprety.Rose_Hp;
        }
        return true;
    }

    //减少角色固定生命值
    public bool costRoseHp(int costHp)
    {

        Game_PositionVar game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        int costValue = rose_Proprety.Rose_HpNow - costHp;
        //防止扣血过量
        if (costValue <= 0)
        {
            rose_Proprety.Rose_HpNow = 0;
        }
        else {
            rose_Proprety.Rose_HpNow = costValue;
        }
        return true;
    }

    //增加角色当前故事模式状态值
    public void UpdataRoseStoryStatus() {

        string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //roseStoryStatus = roseStoryStatus + 1;
        //获取下一级
        string nextStoryID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextStoryID", "ID", roseStoryStatus, "GameStory_Template");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("StoryStatus", nextStoryID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //Game_PublicClassVar.Get_function_DataSet
    
    }

    //传入值更新当前关卡
    public void UpdataPVEChapter(string pveChapter)
    {
        //Debug.Log("pveChapter = " + pveChapter);
        //获取自己的关卡和章节,验证要开启的关卡是否大于当前开启的开关
        string[] pve1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        string[] pve2 = pveChapter.Split(';');
        //Debug.Log("pve1[0] = " + pve1[0] + "pve2[1]" + pve1[1]);
        if (int.Parse(pve2[0]) >= int.Parse(pve1[0]))
        {
            //判断如果大章节大则直接覆盖不必判断子章节
            if (int.Parse(pve2[0]) > int.Parse(pve1[0])) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PVEChapter", pveChapter, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                string[] chapterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneIDSet", "ID", pve2[0], "Chapter_Template").Split(';');
                string chapterSonName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterSonName", "ID", chapterName[int.Parse(pve2[1]) - 1], "ChapterSon_Template");
                Game_PublicClassVar.Get_function_UI.GameHint("关卡快捷传送激活成功!!!  " + chapterSonName);
                return;

            }

            //判断子关卡
            if (int.Parse(pve2[1]) >= int.Parse(pve1[1]))
            {
                //开启新关卡
                //Debug.Log("pveChapter22222 = " + pveChapter);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PVEChapter", pveChapter, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                //Debug.Log("数据存储完毕");
                //提示
                //Debug.Log("pve2[0] = " + pve2[0]);
                string[] chapterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneIDSet", "ID", pve2[0], "Chapter_Template").Split(';');
                //Debug.Log("chapterName[int.Parse(pve2[1])] = " + chapterName[int.Parse(pve2[1])]);
                string chapterSonName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterSonName", "ID", chapterName[int.Parse(pve2[1])-1], "ChapterSon_Template");
                Game_PublicClassVar.Get_function_UI.GameHint("关卡快捷传送激活成功!!!  " + chapterSonName);
            }
            else {
                Debug.Log("开启关卡失败！");
            }
        }
        else {
            Debug.Log("开启关卡失败！");
        }
    }

    //传入套装ID返回当前套装满足数量 装备数组
    public int returnEquipSuitNum(string[] equipIDStr, string[] equipNumStr) {

        int returnEquipSuitNum = 0;
        //循环判定当前身上携带的套装装备数量
        for (int i = 0; i <= equipIDStr.Length - 1; i++) {
            //获取传入装备的数量
            int wearEquipNum = IfWearEquipID(equipIDStr[i]);
            //如果穿戴的装备大于套装要求的数量就会返回套装要求的数量
            if (wearEquipNum >= int.Parse(equipNumStr[i]))
            {
                returnEquipSuitNum = returnEquipSuitNum + int.Parse(equipNumStr[i]);
            }
            else {
                returnEquipSuitNum = returnEquipSuitNum + wearEquipNum;
            }

        }
        return returnEquipSuitNum;
    }

    //传入道具ID判定当前是否装备传入ID 返回当前装备的数量
    public int IfWearEquipID(string equipIDPar) {

        Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;
        int returnEquipNum = 0;
        //获取当前身上装备ID
        for (int i = 1; i <= 13; i++)
        {
            string equipID_1 = functionDataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
            if (equipID_1 == equipIDPar)
            {
                returnEquipNum = returnEquipNum + 1;
            }
        }

        return returnEquipNum;
    }

    //传入子ID查看套装条件是否满足
    /*
    public void ifEquipSuitProperty(string suitPropertyID) {
        //获取自身套装数量
        int equipSuitNum = Game_PublicClassVar.Get_function_Rose.returnEquipSuitNum(needEquipIDSet, needEquipNumSet);


            string triggerSuitNum = suitPropertyIDSet[i].Split(',')[0];
            string triggerSuitPropertyID = suitPropertyIDSet[i].Split(',')[1];
            //显示套装属性
            GameObject propertyObj = (GameObject)Instantiate(Obj_EquipSuitPropertyText);
            propertyObj.transform.SetParent(Obj_UIEquipSuit.transform);
            propertyObj.transform.localScale = new Vector3(1, 1, 1);
            string equipSuitDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitDes", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template");
            propertyObj.GetComponent<Text>().text = triggerSuitNum + "件套：" + equipSuitDes;
            float suitShowTextNum = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowTextNum", "ID", triggerSuitPropertyID, "EquipSuitProperty_Template"));
            suitShowTextNumSum = suitShowTextNumSum + suitShowTextNum;
            propertyObj.transform.localPosition = new Vector3(10, -30 - 25 * suitShowTextNumSum, 0);

    
    }
    */


    //消耗一个道具在仓库内指定位置的数量  （道具ID 道具数量 格子位置, 是否扣除全部）
    public bool CostStoreHouseSpaceNumItem(string itemID, int itemNum, string spaceNum, bool costAllSpace)
    {
        string bagItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", spaceNum, "RoseStoreHouse");
        string bagItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", spaceNum, "RoseStoreHouse");
        //是否扣除全部
        if (costAllSpace)
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseStoreHouse");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseStoreHouse");
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseStoreHouse");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }

        if (itemID == bagItemID)
        {
            int otherValue = int.Parse(bagItemNum) - itemNum;
            if (otherValue > 0)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", otherValue.ToString(), "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
            }
            else
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemID", "0", "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("HideID", "0", "ID", spaceNum, "RoseStoreHouse");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseStoreHouse");
            }
            Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    //发送道具到仓库
    public bool SendRewardToStoreHouse(string dropID, int dropNum, string broadcastType ="0",string hideID = "0")
    {

        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        //更新背包立即显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;

        bool ifGold = false;
        //掉落为空
        if (dropID == "0")
        {
            return true;
        }

        //判定掉落是否为金币
        if (dropID == "1")
        {
            int goldNum = dropNum;
            Game_PublicClassVar.Get_function_Rose.SendReward("1", goldNum.ToString());
            ifGold = true;
            //弹窗提示
            //string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
            
            switch (broadcastType)
            {
                //广播
                case "0":
                    Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "金币");
                    break;
                //不广播
                case "1":
                    break;
            }
            //更新道具任务显示
            Game_PublicClassVar.Get_function_Task.updataTaskItemID();
            return true;
        }
        else
        {
            ifGold = false;
        }

        if (!ifGold)
        {

            //将掉落的道具ID添加到背包内
            for (int i = 1; i <= 64; i++)
            {
                //获得当前背包内对应格子的道具ID
                string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
                //Rdate = "0";
                //寻找背包内有没有相同的道具ID
                if (dropID == Rdate)
                {

                    //读取当前道具数量
                    string itemValue = function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseStoreHouse");
                    //读取当前道具的堆叠数量的最大值
                    string itemPileSum = function_DataSet.DataSet_ReadData("ItemPileSum", "ID", dropID, "Item_Template");
                    int itemNum = int.Parse(itemValue) + dropNum; //将数量累加（此处没有顾忌到自己背包格子满的处理方式，以后添加）
                    //当满足堆叠数量,执行道具捡取
                    if (int.Parse(itemPileSum) >= itemNum)
                    {
                        //添加获得的道具数量
                        function_DataSet.DataSet_WriteData("ItemNum", itemNum.ToString(), "ID", i.ToString(), "RoseStoreHouse");
                        function_DataSet.DataSet_SetXml("RoseStoreHouse");
                        //弹窗提示
                        string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                        
                        switch (broadcastType)
                        {
                            //广播
                            case "0":
                                Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                                break;
                            //不广播
                            case "1":
                                break;
                        }
                        //ai_dorpitem.IfRoseTake = true;      //注销拾取的道具
                        //更新道具任务显示
                        Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                        //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                        string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
                        if (itemType == "1")
                        {
                            Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                        }
                        return true;
                        break;
                    }
                }

                //发现背包格子为空，将数据直接塞进空的格子中（从前面排序）
                if (Rdate == "0")
                {
                    function_DataSet.DataSet_WriteData("ItemID", dropID, "ID", i.ToString(), "RoseStoreHouse");
                    function_DataSet.DataSet_WriteData("ItemNum", dropNum.ToString(), "ID", i.ToString(), "RoseStoreHouse");
                    function_DataSet.DataSet_WriteData("HideID", hideID, "ID", i.ToString(), "RoseStoreHouse");
                    function_DataSet.DataSet_SetXml("RoseStoreHouse");
                    //弹窗提示
                    string itemName = function_DataSet.DataSet_ReadData("ItemName", "ID", dropID, "Item_Template");
                    
                    switch (broadcastType)
                    {
                        //广播
                        case "0":
                            Game_PublicClassVar.Get_function_UI.GameHint("你获得" + dropNum.ToString() + "个" + itemName);
                            break;
                        //不广播
                        case "1":
                            break;
                    }
                    //更新道具任务显示
                    Game_PublicClassVar.Get_function_Task.updataTaskItemID();
                    //获取增加的道具是否为消耗品,如果是消耗品开启主界面快捷图标
                    string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", dropID, "Item_Template");
                    if (itemType == "1")
                    {
                        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
                    }
                    return true;
                    break;  //跳出循环
                }

                //在结束循环的最后判定道具如果没有被拾取,判定为背包满了
                if (i == 64)
                {
                    return false;

                }
            }
        }

        return false;

    }

    //整理背包
    public void RoseArrangeBag() {

        //检索移动不成功的条件
        if (Game_PublicClassVar.Get_game_PositionVar.EquipXiLianStatus)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("洗练装备时禁止整理背包！");
            return;
        }

        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;
        string bagItemIDStr = "";
        string bagItemNumStr = "";
        string bagItemHideIDStr = "";
        //将掉落的道具ID添加到背包内
        for (int i = 1; i <= 64; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (Rdate != "" && Rdate != "0")
            {
                //读取当前道具数量
                int itemValue = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag"));
                string hideID = function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseBag");
                //防止整理背包出错,如果道具ID不为0,默认道具数量为1
                if (itemValue == 0) {
                    itemValue = 1;
                }

                //获得背包道具数量
                //获取道具背包最大堆叠数量
                //Debug.Log("Rdate = " + Rdate);
                int itemPileSum = int.Parse(function_DataSet.DataSet_ReadData("ItemPileSum", "ID", Rdate, "Item_Template"));
                if (itemPileSum > itemValue) { 
                    //向背包后面道具查询数量
                    for (int y = i + 1; y <= 64; y++) { 
                        //对比道具ID
                        string itemID_DuiBi = function_DataSet.DataSet_ReadData("ItemID", "ID", y.ToString(), "RoseBag");
                        if (Rdate == itemID_DuiBi) {
                            //道具ID相同获取数量
                            int itemNum_DuiBi = int.Parse(function_DataSet.DataSet_ReadData("ItemNum", "ID", y.ToString(), "RoseBag"));

                            //数量相加判定是否满足前面堆叠数量
                            int itemSunNum_DuiBi = itemValue + itemNum_DuiBi;
                            if (itemSunNum_DuiBi >= itemPileSum)
                            {
                                int itemCostNum_DuiBi = itemSunNum_DuiBi - itemPileSum;
                                itemValue = itemPileSum;
                                //当满足上一次的堆叠数量自己还有剩余数量时进行记录
                                if (itemCostNum_DuiBi > 0) { 
                                    function_DataSet.DataSet_WriteData("ItemNum", itemCostNum_DuiBi.ToString(),"ID", y.ToString(), "RoseBag");
                                }else{
                                    function_DataSet.DataSet_WriteData("ItemNum", "0","ID", y.ToString(), "RoseBag");
                                    function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RoseBag");
                                }

                                if (itemCostNum_DuiBi >= 0) {
                                    y = 64; //跳出循环
                                }
                            }
                            else {
                                itemValue = itemValue + itemNum_DuiBi;
                                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", y.ToString(), "RoseBag");
                                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", y.ToString(), "RoseBag");
                            }
                        }
                    }
                }

                //背包整理字符串累计
                bagItemIDStr = bagItemIDStr + Rdate + ",";
                bagItemNumStr = bagItemNumStr + itemValue + ",";
                bagItemHideIDStr = bagItemHideIDStr + hideID + ",";
                //清空当前格子数据
                function_DataSet.DataSet_WriteData("ItemID", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("ItemNum", "0", "ID", i.ToString(), "RoseBag");
                function_DataSet.DataSet_WriteData("HideID", "0", "ID", i.ToString(), "RoseBag");
            }

            //Debug.Log("Rdate_" + i.ToString() + ":" + Rdate);
        }
        if (bagItemIDStr != "") {
            //Debug.Log("bagItemIDStr1 = " + bagItemIDStr);
            bagItemIDStr = bagItemIDStr.Substring(0, bagItemIDStr.Length - 1);
            bagItemNumStr = bagItemNumStr.Substring(0, bagItemNumStr.Length - 1);
            //Debug.Log("bagItemIDStr2 = " + bagItemIDStr);
        }
        //存储当前背包道具ID和数量
        string[] bagItemID_Now;
        string[] bagItemNum_Now;
        string[] bagItemHide_Now;

        if (bagItemIDStr != "")
        {
            bagItemID_Now = bagItemIDStr.Split(',');
            bagItemNum_Now = bagItemNumStr.Split(',');
            bagItemHide_Now = bagItemHideIDStr.Split(',');
        }
        else
        {
            //Debug.Log("当前背包没有道具,不需要整理");
            return;
        }
        //整理当前背包道具数量
        //--消耗性道具
        string bagItemIDType1_Str_Q1 = "";
        string bagItemIDType1_Str_Q2 = "";
        string bagItemIDType1_Str_Q3 = "";
        string bagItemIDType1_Str_Q4 = "";
        string bagItemIDType1_Str_Q5 = "";
        string bagItemNumType1_Str_Q1 = "";
        string bagItemNumType1_Str_Q2 = "";
        string bagItemNumType1_Str_Q3 = "";
        string bagItemNumType1_Str_Q4 = "";
        string bagItemNumType1_Str_Q5 = "";
        string bagHideIDType1_Str_Q1 = "";
        string bagHideIDType1_Str_Q2 = "";
        string bagHideIDType1_Str_Q3 = "";
        string bagHideIDType1_Str_Q4 = "";
        string bagHideIDType1_Str_Q5 = "";

        //--材料道具
        string bagItemIDType2_Str_Q1 = "";
        string bagItemIDType2_Str_Q2 = "";
        string bagItemIDType2_Str_Q3 = "";
        string bagItemIDType2_Str_Q4 = "";
        string bagItemIDType2_Str_Q5 = "";
        string bagItemNumType2_Str_Q1 = "";
        string bagItemNumType2_Str_Q2 = "";
        string bagItemNumType2_Str_Q3 = "";
        string bagItemNumType2_Str_Q4 = "";
        string bagItemNumType2_Str_Q5 = "";
        string bagHideIDType2_Str_Q1 = "";
        string bagHideIDType2_Str_Q2 = "";
        string bagHideIDType2_Str_Q3 = "";
        string bagHideIDType2_Str_Q4 = "";
        string bagHideIDType2_Str_Q5 = "";

        //--装备道具
        string bagItemIDType3_Str_Q1 = "";
        string bagItemIDType3_Str_Q2 = "";
        string bagItemIDType3_Str_Q3 = "";
        string bagItemIDType3_Str_Q4 = "";
        string bagItemIDType3_Str_Q5 = "";
        string bagItemNumType3_Str_Q1 = "";
        string bagItemNumType3_Str_Q2 = "";
        string bagItemNumType3_Str_Q3 = "";
        string bagItemNumType3_Str_Q4 = "";
        string bagItemNumType3_Str_Q5 = "";
        string bagHideIDType3_Str_Q1 = "";
        string bagHideIDType3_Str_Q2 = "";
        string bagHideIDType3_Str_Q3 = "";
        string bagHideIDType3_Str_Q4 = "";
        string bagHideIDType3_Str_Q5 = "";


        for (int i = 0; i <= bagItemID_Now.Length - 1; i++) {
            //获取道具类型和品质
            string itemType = function_DataSet.DataSet_ReadData("ItemType", "ID", bagItemID_Now[i], "Item_Template");
            string itemQuality = function_DataSet.DataSet_ReadData("ItemQuality", "ID", bagItemID_Now[i], "Item_Template");
            switch (itemType) { 
                //消耗性道具
                case "1":
                    switch (itemQuality) { 
                        //品质-1
                        case "1":
                            bagItemIDType1_Str_Q1 = bagItemIDType1_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q1 = bagItemNumType1_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q1 = bagHideIDType1_Str_Q1 + bagItemHide_Now[i] + ",";
                        break;
                        //品质-2
                        case "2":
                            bagItemIDType1_Str_Q2 = bagItemIDType1_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q2 = bagItemNumType1_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q2 = bagHideIDType1_Str_Q2 + bagItemHide_Now[i] + ",";
                        break;
                        //品质-3
                        case "3":
                            bagItemIDType1_Str_Q3 = bagItemIDType1_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q3 = bagItemNumType1_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q3 = bagHideIDType1_Str_Q3 + bagItemHide_Now[i] + ",";
                        break;
                        //品质-4
                        case "4":
                            bagItemIDType1_Str_Q4 = bagItemIDType1_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q4 = bagItemNumType1_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q4 = bagHideIDType1_Str_Q4 + bagItemHide_Now[i] + ",";
                        break;
                        //品质-5
                        case "5":
                            bagItemIDType1_Str_Q5 = bagItemIDType1_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType1_Str_Q5 = bagItemNumType1_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType1_Str_Q5 = bagHideIDType1_Str_Q5 + bagItemHide_Now[i] + ",";
                        break;
                    }
                break;
                //材料
                case "2":
                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType2_Str_Q1 = bagItemIDType2_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q1 = bagItemNumType2_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q1 = bagHideIDType2_Str_Q1 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType2_Str_Q2 = bagItemIDType2_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q2 = bagItemNumType2_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q2 = bagHideIDType2_Str_Q2 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType2_Str_Q3 = bagItemIDType2_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q3 = bagItemNumType2_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q3 = bagHideIDType2_Str_Q3 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType2_Str_Q4 = bagItemIDType2_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q4 = bagItemNumType2_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q4 = bagHideIDType2_Str_Q4 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType2_Str_Q5 = bagItemIDType2_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType2_Str_Q5 = bagItemNumType2_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType2_Str_Q5 = bagHideIDType2_Str_Q5 + bagItemHide_Now[i] + ",";
                            break;
                    }
                break;
                //装备
                case "3":

                    switch (itemQuality)
                    {
                        //品质-1
                        case "1":
                            bagItemIDType3_Str_Q1 = bagItemIDType3_Str_Q1 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q1 = bagItemNumType3_Str_Q1 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q1 = bagHideIDType3_Str_Q1 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-2
                        case "2":
                            bagItemIDType3_Str_Q2 = bagItemIDType3_Str_Q2 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q2 = bagItemNumType3_Str_Q2 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q2 = bagHideIDType3_Str_Q2 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-3
                        case "3":
                            bagItemIDType3_Str_Q3 = bagItemIDType3_Str_Q3 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q3 = bagItemNumType3_Str_Q3 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q3 = bagHideIDType3_Str_Q3 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-4
                        case "4":
                            bagItemIDType3_Str_Q4 = bagItemIDType3_Str_Q4 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q4 = bagItemNumType3_Str_Q4 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q4 = bagHideIDType3_Str_Q4 + bagItemHide_Now[i] + ",";
                            break;
                        //品质-5
                        case "5":
                            bagItemIDType3_Str_Q5 = bagItemIDType3_Str_Q5 + bagItemID_Now[i] + ",";
                            bagItemNumType3_Str_Q5 = bagItemNumType3_Str_Q5 + bagItemNum_Now[i] + ",";
                            bagHideIDType3_Str_Q5 = bagHideIDType3_Str_Q5 + bagItemHide_Now[i] + ",";
                            break;
                    }
                break;
            }
        }

        //拼接整理后的字符串
        string bagItemID_ArrangeStr;
        string bagItemNum_ArrangeStr;
        string bagItemHideID_ArrangeStr;
        string[] bagItemID_Arrange;
        string[] bagItemNum_Arrange;
        string[] bagItemHideID_Arrange;

        bagItemID_ArrangeStr = bagItemIDType1_Str_Q5 + bagItemIDType1_Str_Q4 + bagItemIDType1_Str_Q3 + bagItemIDType1_Str_Q2 + bagItemIDType1_Str_Q1 + bagItemIDType2_Str_Q5 + bagItemIDType2_Str_Q4 + bagItemIDType2_Str_Q3 + bagItemIDType2_Str_Q2 + bagItemIDType2_Str_Q1 + bagItemIDType3_Str_Q5 + bagItemIDType3_Str_Q4 + bagItemIDType3_Str_Q3 + bagItemIDType3_Str_Q2 + bagItemIDType3_Str_Q1;
        bagItemNum_ArrangeStr = bagItemNumType1_Str_Q5 + bagItemNumType1_Str_Q4 + bagItemNumType1_Str_Q3 + bagItemNumType1_Str_Q2 + bagItemNumType1_Str_Q1 + bagItemNumType2_Str_Q5 + bagItemNumType2_Str_Q4 + bagItemNumType2_Str_Q3 + bagItemNumType2_Str_Q2 + bagItemNumType2_Str_Q1 + bagItemNumType3_Str_Q5 + bagItemNumType3_Str_Q4 + bagItemNumType3_Str_Q3 + bagItemNumType3_Str_Q2 + bagItemNumType3_Str_Q1;
        bagItemHideID_ArrangeStr = bagHideIDType1_Str_Q5 + bagHideIDType1_Str_Q4 + bagHideIDType1_Str_Q3 + bagHideIDType1_Str_Q2 + bagHideIDType1_Str_Q1 + bagHideIDType2_Str_Q5 + bagHideIDType2_Str_Q4 + bagHideIDType2_Str_Q3 + bagHideIDType2_Str_Q2 + bagHideIDType2_Str_Q1 + bagHideIDType3_Str_Q5 + bagHideIDType3_Str_Q4 + bagHideIDType3_Str_Q3 + bagHideIDType3_Str_Q2 + bagHideIDType3_Str_Q1;
        
        if (bagItemID_ArrangeStr != "")
        {
            //去掉背包隐藏属性ID
            //Debug.Log("bagItemID_ArrangeStr1 = " + bagItemID_ArrangeStr);
            bagItemID_ArrangeStr = bagItemID_ArrangeStr.Substring(0, bagItemID_ArrangeStr.Length - 1);
            bagItemNum_ArrangeStr = bagItemNum_ArrangeStr.Substring(0, bagItemNum_ArrangeStr.Length - 1);
            bagItemHideID_ArrangeStr = bagItemHideID_ArrangeStr.Substring(0, bagItemHideID_ArrangeStr.Length - 1);
            //转换成数组
            //Debug.Log("bagItemID_ArrangeStr2 = " + bagItemID_ArrangeStr);
            bagItemID_Arrange = bagItemID_ArrangeStr.Split(',');
            bagItemNum_Arrange = bagItemNum_ArrangeStr.Split(',');
            bagItemHideID_Arrange = bagItemHideID_ArrangeStr.Split(',');
            //Debug.Log("Lenght = " + bagItemID_Arrange.Length);
        }
        else {
            //Debug.Log("背包没有东西需要整理——2");
            return;
        }

        //循环写入背包数据
        for (int i = 0; i <= bagItemID_Arrange.Length - 1; i++) {
            //Debug.Log("bagItemID_Arrange[i] = " + i+ bagItemID_Arrange[i]);
            function_DataSet.DataSet_WriteData("ItemID", bagItemID_Arrange[i], "ID", (i+1).ToString(), "RoseBag");
            function_DataSet.DataSet_WriteData("ItemNum", bagItemNum_Arrange[i], "ID", (i + 1).ToString(), "RoseBag");
            function_DataSet.DataSet_WriteData("HideID", bagItemHideID_Arrange[i], "ID", (i + 1).ToString(), "RoseBag");
        }
        function_DataSet.DataSet_SetXml("RoseBag");
        //Debug.Log("整理背包成功");
        Game_PublicClassVar.Get_function_UI.PlaySource("10004", "1");
        Game_PublicClassVar.Get_game_PositionVar.UpdataBagAll = true;
    }

    //随机获取装备极品属性ID
    public string ReturnHidePropertyID(string itemID) {

        //获取装备等级和装备类型
        string equipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", itemID, "Item_Template");
        string hidePropertyID = "0";
        if (equipID != "0")
        {
            string HideType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideType", "ID", equipID, "Equip_Template");
            //string HideType = "1";
            string hideProperListStr = "";          //特殊属性字符串
            float hideShowPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideShowPro", "ID", equipID, "Equip_Template"));
            //float hideShowPro = 0.25f;              //每个特殊属性出现的概率
            string roseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            string roseEquipHideNumID_Now = (int.Parse(roseEquipHideNumID) + 1).ToString();
            /*
            1:血量上限
            2:物理攻击最大值
            3:物理防御最大值
            4:魔法防御最大值
             */
            switch (HideType)
            { 
                //可出现随机属性
                case "1":
                    for (int i = 2; i <= 4; i++) { 
                        //检测概率是否触发随机概率
                        if (Random.value <= hideShowPro) { 
                            //获取随机范围,并随机获取一个值
                            string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
                            int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr));
                            hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
                        }
                    }
                    if (hideProperListStr != "") {
                        hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
                    }
                break;

                //可出现随机属性
                case "2":
                for (int i = 1; i <= 4; i++)
                {
                    //检测概率是否触发随机概率
                    if (Random.value <= hideShowPro)
                    {
                        //获取随机范围,并随机获取一个值
                        string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
                        //血量属性翻5倍
                        if (i == 1) {
                            hideMaxStr = (int.Parse(hideMaxStr) * 5).ToString();
                        }
                        int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr));
                        hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
                    }
                }
                if (hideProperListStr != "")
                {
                    hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
                }
                break;

                //可出现随机属性
                case "3":
                for (int i = 1; i <= 1; i++)
                {
                    //检测概率是否触发随机概率
                    if (Random.value <= hideShowPro)
                    {
                        //获取随机范围,并随机获取一个值
                        string hideMaxStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideMax", "ID", equipID, "Equip_Template");
                        //血量属性翻5倍
                        if (i == 1)
                        {
                            hideMaxStr = (int.Parse(hideMaxStr) * 5).ToString();
                        }
                        int addValue = ReturnEquipRamdomValue(1, int.Parse(hideMaxStr));
                        hideProperListStr = hideProperListStr + i.ToString() + "," + addValue.ToString() + ";";
                    }
                }
                if (hideProperListStr != "")
                {
                    hideProperListStr = hideProperListStr.Substring(0, hideProperListStr.Length - 1);
                }
                break;
            }


            //附加幸运值(101属性类型表示幸运值)
            if (Random.value >= 0.99f)
            {
                int addValue = 1;

                if (hideProperListStr != "")
                {
                    hideProperListStr = hideProperListStr + ";101" + "," + addValue.ToString();
                }
                else {
                    hideProperListStr = hideProperListStr + "101" + "," + addValue.ToString();
                }
            }


            //写入极品装备数据
            if (hideProperListStr != "") {
                Game_PublicClassVar.Get_function_DataSet.AddRoseEquipHidePropertyXml(roseEquipHideNumID_Now, hideProperListStr);
                hidePropertyID = roseEquipHideNumID_Now;
                //存储当前极品ID
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseEquipHideNumID", roseEquipHideNumID_Now, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            }

        }
        return hidePropertyID;
    }

    //传入随机范围,生成一个随机数,越到后面的随机数获取概率越低
    public int ReturnEquipRamdomValue(int randomMinValue, int randomMaxValue) {

        int randomChaValue = randomMaxValue - randomMinValue;
        //随机4次,获得取值范围
        /*
        0-25%       0.5
        26%-50%     0.3
        51%-75%     0.15
        76%-100%    0.05
        */
        float randomRangeValue = Random.value;
        float randomRangeValue_Now =0;
        if (randomRangeValue <= 0.5f)
        {
            //0-0.25f
            randomRangeValue_Now = Random.value / 4;

        }
        if (randomRangeValue > 0.5f && randomRangeValue <= 0.8f)
        {
            //0.25-0.5
            randomRangeValue_Now = Random.value / 4+0.25f;
        }
        if (randomRangeValue > 0.8f && randomRangeValue <= 0.95f)
        {
            //0.5-0.75
            randomRangeValue_Now = Random.value / 4+0.5f;
        }
        if (randomRangeValue > 0.95f && randomRangeValue <= 1f)
        {
            //0.75-1
            randomRangeValue_Now = Random.value / 4+0.75f;
        }
        //计算最终值
        int retunrnValue = (int)(randomMinValue + randomChaValue * randomRangeValue_Now);
        return retunrnValue;
    }
    
    //获取角色当前某个对应的属性值
    public int ReturnRosePropertyValue(string propertyType) {

        switch (propertyType) { 
            //获取最大攻击力
            case "1":
                return Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMax;
            break;
        }

        return 0;
    
    }


    //获取某个收集道具是否被收集
    public bool ifShouJiItem(string itemID)
    {
        string shoujiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_5", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] shoujiList = shoujiStr.Split(',');
        for (int i = 0; i <= shoujiList.Length - 1; i++) {

            if (itemID == shoujiList[i]) {
                return true;
            }
        }
            return false;
    }

    //收集值
    public bool AddShouJiItem(string itemID)
    {
        //BeiYong
        string shoujiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_5","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] shoujiList = shoujiStr.Split(',');

        //先判定
        bool ifShouJiItem = false;
        int startNum = 0;
        int chapter = 0;
        //获取当前
        string[] ShouJiItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ShouJiItemList", "GameMainValue").Split(';');

        for (int i = 0; i < ShouJiItemList.Length; i++) {
            //Debug.Log(" ShouJiItemList[i] = " + ShouJiItemList[i]);
            //循环判定ID
            string itemListID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemListID", "ID", ShouJiItemList[i], "ShouJiItemPro_Template");
            string nextID = itemListID;
            //Debug.Log("nextID = " + nextID);
            //获取下级ID
            do
            {
                //新建兑换道具
                string duibiItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", nextID, "ShouJiItem_Template");
                if (duibiItemID == itemID)
                {
                    ifShouJiItem = true;
                    startNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StartNum", "ID", nextID, "ShouJiItem_Template"));
                    chapter = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterNum", "ID", ShouJiItemList[i], "ShouJiItemPro_Template"));
                    //nextID = "0";   //立即跳出循环
                    Debug.Log("ID一致");
                    break;
                }
                else {
                    nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");
                }

            }
            while (nextID != "0");

            if (ifShouJiItem) {
                break;
            }

        }

            //添加收集ID
        if (ifShouJiItem)
        {
            bool ifGetItem = true;
            for (int i = 0; i <= shoujiList.Length - 1; i++)
            {
                if (itemID == shoujiList[i])
                {
                    //已存在收藏ID不用重复激活
                    ifGetItem = false;
                }
            }

            if (ifGetItem)
            {
                if (shoujiStr == "")
                {
                    shoujiStr = itemID;
                }
                else
                {
                    shoujiStr = shoujiStr + "," + itemID;
                }
                //Debug.Log("shoujiStr = " + shoujiStr);
                //获取当前星数进行记录
                string startListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (startListStr == "0" || startListStr == "")
                {
                    startListStr = "0;0;0;0;0;0";
                    //Debug.Log("startListStr = " + startListStr);
                }
                string[] startList = startListStr.Split(';');
                if (chapter != 0) {
                    int nowChapStarNum = int.Parse(startList[chapter - 1]);
                    startList[chapter - 1] = (nowChapStarNum + startNum).ToString();
                    startListStr = "";
                    for (int i = 0; i < startList.Length; i++)
                    {
                        //Debug.Log("i = " + startList[i]);
                        if (startListStr == "")
                        {
                            startListStr = startList[i];
                            //Debug.Log(" startListStrPPPPP = " + startListStr);
                        }
                        else {
                            startListStr = startListStr + ";" + startList[i];
                            //Debug.Log(" startListStrPPPPP = " + startListStr);
                        }
                    }
                }

                string shouJiItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", itemID, "Item_Template");
                Game_PublicClassVar.Get_function_UI.GameGirdHint(shouJiItemName + "激活收藏道具成功！");
                //Debug.Log("startListStr = " + startListStr);
                //添加记录的道具ID
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_5", shoujiStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_6", startListStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            }
            //Debug.Log("添加道具");
            return true;
        }
        else {
            //Debug.Log("添加道具未收藏");
        }



        return true;

    }


    //核对收集
    public bool ShouJiJianYan() {

        //已拥有道具列表
        string shoujiItemListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_5", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] shoujiItemStrList = shoujiItemListStr.Split(',');

        //收藏的星数
        string shoujiStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (shoujiStr == "0") {
            shoujiStr = "0;0;0;0;0;0";
        }
        string[] shoujiList = shoujiStr.Split(';');

        //收藏的道具
        string shoujiItemStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ShouJiItemList", "GameMainValue");
        string[] shoujiItemList = shoujiItemStr.Split(';');
        //Debug.Log("shoujiItemStr = " + shoujiItemStr);
        string chapterStr = "";
        for (int i = 0; i < shoujiList.Length; i++)
        {

            //循环判定ID
            string itemListID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemListID", "ID", shoujiItemList[i], "ShouJiItemPro_Template");
            string nextID = itemListID;
            int startNum_Chapter = 0;

            //Debug.Log("nextID = " + nextID);
            int iii = 0;
            //获取下级ID
            do
            {
                //新建兑换道具
                string duibiItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", nextID, "ShouJiItem_Template");
                //Debug.Log("nextID = " + nextID);

                for (int y = 0; y < shoujiItemStrList.Length; y++)
                {

                    if (shoujiItemStrList[y] == duibiItemID)
                    {
                        //获取星数
                        int startNum = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StartNum", "ID", nextID, "ShouJiItem_Template"));
                        startNum_Chapter = startNum_Chapter + startNum;
                        //Debug.Log("获得星数startNum_Chapter = " + startNum_Chapter + " nextID = " + nextID + "duibiItemID = " + duibiItemID);
                    }
                    else
                    {
                        //nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");
                        
                    }
                }
                nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");
                iii = iii + 1;
                if (iii >= 50) {
                    nextID = "0";
                    Debug.Log("太多");
                }
            }
            while (nextID != "0");


            if (chapterStr != "")
            {
                chapterStr = chapterStr + ";" + startNum_Chapter.ToString();
                //Debug.Log("123 = " + chapterStr);
            }
            else {
                chapterStr = startNum_Chapter.ToString();
                //Debug.Log("456 = " + chapterStr);
            }
            //Debug.Log("chapterStr = " + chapterStr);
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_6", chapterStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        }
        return true;
    }



    //验证序列号是否正确
    public bool IfTrueXuLieHao(string[] xuliehaoList)
    {
        //验证账号信息
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (zhanghaoID != xuliehaoList[3])
        {
            return false;
            Debug.Log("账号信息验证错误！");
        }
        //验证时间
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
        {

#if UNITY_IPHONE
                string worldTime = Game_PublicClassVar.Get_wwwSet.GetWorldTime();
#else
                string worldTime = Game_PublicClassVar.Get_wwwSet.XuliehaoTime();
#endif

            


            if (worldTime != "0000000000" && worldTime != "1 = 0000000000")
            {
                Debug.Log("worldTime = " + worldTime);
                int chazhi = int.Parse(worldTime) - int.Parse(xuliehaoList[0]);
                if (chazhi <= 86400)
                {
                    return true;
                }
                else
                {
                    Game_PublicClassVar.Get_function_UI.GameHint("此序列号已过期");
                    Debug.Log("此序列号已过期");
                }
            }
            else
            {
                Game_PublicClassVar.Get_function_UI.GameHint("请链接网络在输入序列号!");
            }
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameHint("请链接网络在输入序列号");
        }
        return false;
    }

    //获取某个序列号是否领取
    public bool ifGetXuLieHao(string xuLieHaoStr)
    {
        string[] xuLieHao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XuLieHaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig").Split(';');
        for (int i = 0; i <= xuLieHao.Length - 1; i++)
        {
            if (xuLieHaoStr == xuLieHao[i])
            {
                return true;
            }
        }
        return false;
    }

    //写入序列号是否领取
    public void WriteXuLieHao(string xuLieHaoStr)
    {
        string xuLieHao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XuLieHaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (xuLieHao == "")
        {
            xuLieHao = xuLieHaoStr;
        }
        else
        {
            xuLieHao = xuLieHao + ";" + xuLieHaoStr;
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("XuLieHaoSet", xuLieHao, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

    }


    //写入交易号
    public void WritePayID(string payID,string payValue)
    {
        string payStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

        if (payStr != "")
        {
            payStr = payStr + ";" +payID + "," + payValue;
        }
        else {
            payStr = payID + "," + payValue;
        }

        //存入支付ID
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_1", payStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }

    //删除支付ID
    public void DeletePayID(string payID)
    {
        string payStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] payStrList = payStr.Split(';');
        string savePayStr = "";
        for (int i = 0; i < payStrList.Length; i++)
        {
            if (payStrList[i] != "0" && payStrList[i] != "")
            {
                string[] payList = payStrList[i].Split(',');
                if (payList[0] == payID)
                {

                }
                else
                {
                    if (savePayStr != "")
                    {
                        savePayStr = savePayStr + ";" + payStrList[i];
                    }
                    else
                    {
                        savePayStr = payStrList[i];
                    }
                }
            }
        }

        //存入支付ID
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_1", savePayStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }

    //根据订单获取充值额度
    public string DingDanReturnPayValue(string payID)
    {
        string payStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] payStrList = payStr.Split(';');
        for (int i = 0; i < payStrList.Length; i++)
        {
            if (payStrList[i] != "0" && payStrList[i] != "")
            {
                string[] payList = payStrList[i].Split(',');
                if (payList[0] == payID)
                {
                    return payList[1];
                }
            }
        }
        return "0";
    }

    //根据充值额度发送对应钻石
    public void DingDanSendPayValue(string payValue) { 

        int returnZuanShiValue = 0;
        switch (payValue) {

            case "9.8":
                returnZuanShiValue = 1000;
                Game_PublicClassVar.Get_function_UI.GameGirdHint("匹配到了9.8");
                break;
            case "49.8":
                returnZuanShiValue = 6000;
                break;
            case "99.8":
                returnZuanShiValue = 13000;
                break;
            case "498":
                returnZuanShiValue = 75000;
                break;
            case "888":
                returnZuanShiValue = 145000;
                break;
        }

        //累计当前充值额度,发送指定钻石奖励
        Game_PublicClassVar.Get_function_Rose.AddRMB(returnZuanShiValue);
        Game_PublicClassVar.Get_function_Rose.AddPayValue(float.Parse(payValue));
    }


    //召唤宠物（参数1：是否广播,宠物类型）
    public void RosePetCreate(bool ifSpeak = true,int PetType = 0) {

        //PetType = 1;
        GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RosePetSet;
        
        //循环删除宠物
        for (int i = 0; i < monsterSetObj.transform.childCount; i++)
        {
            GameObject go = monsterSetObj.transform.GetChild(i).gameObject;
            //清空AI血条显示
            MonoBehaviour.Destroy(go.GetComponent<AIPet>().UI_Hp);
            MonoBehaviour.Destroy(go);
        }

        string ifPetChuZhan = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //ifPetChuZhan = "0";
        if (ifPetChuZhan == "0")
        {
            //获取怪物
            GameObject monsterObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("PetSet/" + "PetObj_1", typeof(GameObject)));
            monsterObj.transform.SetParent(monsterSetObj.transform);
            Vector3 CreateVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
            monsterObj.transform.position = CreateVec3;
            monsterObj.SetActive(false);
            monsterObj.SetActive(true);

            //设置当前宠物
            Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
            for (int i = 0; i <= roseStatus.RosePetObj.Length-1; i++)
            {
                if (roseStatus.RosePetObj[i] == null) {
                    roseStatus.RosePetObj[i] = monsterObj;
                    break;
                }

                if (i == roseStatus.RosePetObj.Length) {
                    Game_PublicClassVar.Get_function_UI.GameHint("召唤失败！宠物位置已满！");
                    return; //跳出召唤方法
                }
            }

            //设置宠物出战
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_3", "1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            if (ifSpeak) {
                Game_PublicClassVar.Get_function_UI.GameHint("召唤宠物成功！");

                //实例化一个特效
                GameObject zhaoHuanEffect = (GameObject)MonoBehaviour.Instantiate((GameObject)Resources.Load("Effect/Skill/Eff_Skill_ZhaoHuan_1", typeof(GameObject)));        //实例化特效
                zhaoHuanEffect.transform.position = CreateVec3;
                zhaoHuanEffect.transform.localScale = new Vector3(1, 1, 1);
            }
            /*
            //判定召唤类型
            if (PetType == 1) {
                monsterObj.GetComponent<AIPet>().PetType = PetType;
                //循环删除宠物
                int petNum = monsterSetObj.transform.childCount;
                petNum = petNum + 1;
                GameObject petObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().PetPositionSet.transform.Find("Posi_" + petNum.ToString()).gameObject;
                if (petObj != null) { 
                    //设定宠物移动位置
                    monsterObj.GetComponent<AIPet>().PositionObj = petObj;
                }
            }
            */
        }
        else
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_3", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");

            if (ifSpeak)
            {
                Game_PublicClassVar.Get_function_UI.GameHint("收回宠物,再次使用可以再次召唤!");
            }
        }
    }

    //存储当前角色信息GameConfig.Xml
    public void SaveGameConfig_Rose(string roseID,string key,string value)
    {

        string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + roseID + "/";
        Game_PublicClassVar.Get_xmlScript.Xml_SetDate(key, value, "ID", roseID, set_XmlPath + "GameConfig.xml");
        //Debug.Log("设置Name");
    }


   //序列号检测
    public void JianCeShouJi(){

        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;

        //将掉落的道具ID添加到背包内
        for (int i = 1; i <= 64; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
            if (Rdate != "0" && Rdate != "") {
                AddShouJiItem(Rdate);
            }
        }

        //检测背包内的
        for (int i = 1; i <= 64; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseBag");
            if (Rdate != "0" && Rdate != "")
            {
                AddShouJiItem(Rdate);
            }
        }

        //检测当前装备的EquipItemID
        for (int i = 1; i <= 13; i++)
        {
            //获得当前背包内对应格子的道具ID
            string Rdate = function_DataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
            if (Rdate != "0" && Rdate != "")
            {
                AddShouJiItem(Rdate);
            }
        }
    }


    public void GamePay(ObscuredString rmbValue, ObscuredString rmbDingDan)
    {

        //安卓调用
        /*
		#if UNITY_ANDROID
		if (rmbDingDan == "" || rmbDingDan == "0" || rmbDingDan == null)
		{
			Game_PublicClassVar.Get_function_UI.GameHint("支付错误,错误提示111！");
		}

		//获取订单号是否为空
		if (rmbDingDan != Game_PublicClassVar.Get_game_PositionVar.PayDingDanIDNow) {
			Game_PublicClassVar.Get_function_UI.GameHint("支付错误,错误提示222！");
			return;
		}

		//获取订单支付状态是否为2
		if (Game_PublicClassVar.Get_game_PositionVar.PayDingDanStatus==2)
		{
			Game_PublicClassVar.Get_function_UI.GameHint("支付错误,错误提示3333！");
			return;
		}
		#endif
        */
        Game_PublicClassVar.Get_game_PositionVar.PayStr = "1;成功";

        string payStr = "支付状态开启:" + Game_PublicClassVar.Get_game_PositionVar.PayStr;
        string payStatusType = Game_PublicClassVar.Get_game_PositionVar.PayStr.ToString().Split(';')[0];
        string payStatusStr = Game_PublicClassVar.Get_game_PositionVar.PayStr.ToString().Split(';')[1];
        int rmbToZuanShiValue = GameReturnPayValue(rmbValue);
        float rmbPayValue = float.Parse(rmbValue);

        //通知UI
        UI_RmbStore ui_rmbStore = null;
        if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>() != null)
        {
            ui_rmbStore = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>();
        }

        //版号包测试
        payStatusType = "1";


        switch (payStatusType)
        {
            //支付状态
            case "0":
                payStr = "支付中……" + payStatusStr;
                break;

            //支付成功
            case "1":
                //删除充值记录
                Game_PublicClassVar.Get_function_Rose.DeletePayID(Game_PublicClassVar.Get_game_PositionVar.PayDingDanIDNow);
                payStr = rmbPayValue + "支付成功！" + payStatusStr;
                //累计当前充值额度,发送指定钻石奖励
                Game_PublicClassVar.Get_function_Rose.AddRMB(rmbToZuanShiValue);
                Game_PublicClassVar.Get_function_Rose.AddPayValue(rmbPayValue);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint("支付成功:" + rmbPayValue + "元");


                //清理支付值
                float youmengRmbValue = rmbPayValue;
                rmbPayValue = 0;


                //发送友盟支付信息
                try
                {
                    //GA.Pay(youmengRmbValue, GA.PaySource.Source10, rmbToZuanShiValue);
                }
                catch
                {
                    Debug.Log("充值报错！");
                }

                rmbToZuanShiValue = 0;

                //更新通用界面显示
                //Game_PublicClassVar.Get_function_UI.UpdateUI_CommonHuoBiSet();

                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付成功!钻石已成功到账,请查收");

                break;

            //支付失败
            case "2":
                payStr = "支付失败！" + payStatusStr;
                rmbToZuanShiValue = 0;
                rmbPayValue = 0;

#if UNITY_ANDROID

                //防止支付宝第一次调用错误
                int ZhiFuBaoOpenNum = PlayerPrefs.GetInt("ZhiFuBaoOpenNum");
                if (ZhiFuBaoOpenNum == 0)
                {
                    PlayerPrefs.SetInt("ZhiFuBaoOpenNum", 1);
                    if (Game_PublicClassVar.Get_game_PositionVar.PayValueNow != "")
                    {
                        //通知UI
                        if (ui_rmbStore != null)
                        {
                            ui_rmbStore.Btn_BuyZuanShi(Game_PublicClassVar.Get_game_PositionVar.PayValueNow);
                        }
                    }
                }
#endif
                break;

            //其他原因
            case "3":
                payStr = "支付未知原因！" + payStatusStr;
                //通知UI
                if (ui_rmbStore != null)
                {
                    //Game_PublicClassVar.Get_game_PositionVar.PayStatus = false;
                    //Game_PublicClassVar.Get_game_PositionVar.PayStr = "";
                    //ui_rmbStore.ClearnPayValue();       //清理支付值
                }
                break;

            default:
                payStr = "支付default" + payStatusStr;
                //通知UI
                if (ui_rmbStore != null)
                {
                    //Game_PublicClassVar.Get_game_PositionVar.PayStatus = false;
                    //Game_PublicClassVar.Get_game_PositionVar.PayStr = "";
                    //ui_rmbStore.ClearnPayValue();       //清理支付值
                }
                break;

                Debug.Log("payStr: " + payStr);
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);
        }

        //更新钻石显示
        Game_PublicClassVar.Get_game_PositionVar.PayQueryStatus = true;
    }


    //根据支付额度返回应获取的额度
    private int GameReturnPayValue(string payValue)
    {
        int returnZuanShiValue = 0;
        switch (payValue)
        {
            case "9.8":
                returnZuanShiValue = 1000;
                break;
            case "49.8":
                returnZuanShiValue = 6000;
                break;
            case "99.8":
                returnZuanShiValue = 13000;
                break;
            case "498":
                returnZuanShiValue = 75000;
                break;
            case "888":
                returnZuanShiValue = 145000;
                break;
        }
        return returnZuanShiValue;
    }

}
