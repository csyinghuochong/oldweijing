using UnityEngine;
using System.Collections;


public class Function_AI : MonoBehaviour {

    private int randomSet;  //记录货币数量的随机值

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //掉落函数
    public bool AI_MonsterDrop(string monsterID,Vector3 vec3) {
        
        //根据怪物ID获得掉落ID
        string dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", monsterID, "Monster_Template");
        if (dropID != "0")
        {
            DropIDToDropItem(dropID, vec3, monsterID);
            return true;
        }
        else {
            return false;
        }
    }

	//传入掉落ID，生成掉落数据
    public bool DropIDToDropItem(string dropID, Vector3 vec3, string monsterID="0")
    {
        //Debug.Log("11111111");
		//是否有子掉落
		bool DropSonStatus = false;
        //int dropLimit = 0;      //设置掉落最大数量
        int dropLimit = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropLimit", "ID", dropID, "Drop_Template"));
        int dropNumNow = 0;     //当前掉落道具的数量
        int dropLoopNum = 0;    //掉落循环次数
        string dropIDInitial = dropID;      //设置初始掉落
		//循环每行掉落数据
		do
		{
			DropSonStatus = true;
            //传入当前掉落数量和最大掉落数量给行掉落数据
            //Debug.Log("dropID = " + dropID);
			//生成每行掉落数据
            dropNumNow = RowDrop(dropID, vec3, monsterID, dropNumNow, dropLimit);
			
			//获取子掉落
			dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropSonID", "ID", dropID,"Drop_Template");
			//没有子掉落循环取消（因为掉落ID里面可以套掉落ID）
            if (dropID == "0")
            {
                DropSonStatus = false;
                //如果掉落数量没达到指定数量再次随机
                if (dropLimit != 0)
                {
                    //子级为空时,判定当前数量是否已达到掉落上线,不足上线则再次执行一边
                    if (dropNumNow < dropLimit)
                    {
                        dropLoopNum = dropLoopNum + 1;
                        dropID = dropIDInitial;
                        DropSonStatus = true;
                        //Debug.Log("22222222");
                    }
                }
                //循环10次强制结束循环
                if (dropLoopNum > 10)
                {
                    DropSonStatus = false;
                    //Debug.Log("3333333333");
                    //return true;
                }

                //Debug.Log("Loop = " + dropLoopNum);
            }
            else {
                //当掉落数量已达上线时,自动取消掉落
                if (dropLimit != 0) {
                    if (dropNumNow >= dropLimit)
                    {
                        DropSonStatus = false;
                    }
                }
            }
		}
		while (DropSonStatus);
		
		return true;
	}

    //行掉落ID数据
    public int RowDrop(string dropID, Vector3 vec3, string monsterID,int dropNumNow=0,int dropNumMax=0)
    {
        //清空隐藏属性ID
        Game_PublicClassVar.Get_game_PositionVar.ChouKaHindIdStr = "0";
        //根据掉落ID获取掉落
        int dropNum = 0;
        //int dropNum

        for (int i = 1; i <= 10; i++)
        {
            //Debug.Log("Str1111 = " + i);
            //获取掉落道具的ID
            //string dropItemID = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("DropItemID" + i.ToString(), "ID", dropID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Drop_Template.xml");
            //Debug.Log("dropID = " + dropID);
            
            string dropItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemID" + i.ToString(), "ID", dropID, "Drop_Template");
            string dropType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropType", "ID", dropID, "Drop_Template");
            //如果道具ID不为空则触发掉落概率
            //Debug.Log("Str2222 = " + i);
            if (dropItemID != "0")
            {
                dropNum = dropNum + 1;      //掉落数量累计

                //每个掉落
                //获取每个掉落的概率
                //string dropChance = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("DropChance" + i.ToString(), "ID", dropID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Drop_Template.xml");
                string dropChance = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropChance" + i.ToString(), "ID", dropID, "Drop_Template");
                float randomdrop = Random.Range(0, 1000000);
                float dropChanceData = int.Parse(dropChance);
                //Debug.Log("Str3333 = " + i);
                //当随机值小于掉落概率值判定为掉落成功
                if (randomdrop <= dropChanceData)
                {
                    //掉落成功
                    //获取掉落数量
                    //string dropMinNum = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("DropItemMinNum" + i.ToString(), "ID", dropID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Drop_Template.xml");
                    //string dropMaxNum = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("DropItemMaxNum" + i.ToString(), "ID", dropID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Drop_Template.xml");
                    string dropMinNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemMinNum" + i.ToString(), "ID", dropID,"Drop_Template");
                    string dropMaxNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemMaxNum" + i.ToString(), "ID", dropID,"Drop_Template");
                    //随机掉落数量
                    int itemDropNum = Random.Range(int.Parse(dropMinNum), int.Parse(dropMaxNum));
                    randomSet = itemDropNum;
                    //读取掉落道具ID
                    //string itemID = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("DropItemID" + i.ToString(), "ID", dropID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Drop_Template.xml");
                    string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemID" + i.ToString(), "ID", dropID,"Drop_Template");
                    //如果是金币掉落显示掉落数量为1

                    if (itemID == "1")
                    {
                        itemDropNum = 1;
                    }
                    //Debug.Log("Str4444 = " + i);
                    switch (dropType) {
                        //发送道具到地上实体
                        case "1":
                            //当掉落数量不为0时,循环实例化每个掉落
                            if (itemDropNum != 0)
                            {
                                for (int n = 1; n <= itemDropNum; n++)
                                {
                                    //执行掉落
                                    AI_DropItem(itemID, vec3, monsterID);
                                }
                            }
                        break;
                        //发送道具到背包
                        case "2":
                            //Debug.Log("Str5555 = " + i);
                            //获取道具的极品值
                            float HideDropPro = 0;
                            if (monsterID != "0") {
                                HideDropPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideDropPro", "ID", monsterID, "Monster_Template"));
                            }
                            //Debug.Log("Str6666 = " + i);
                            //Debug.Log("HideDropPro = " + HideDropPro);
                            //Debug.Log("itemID = " + itemID + "itemDropNum = " + itemDropNum + "HideDropPro = " + HideDropPro);
                            bool ifDrop = Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, itemDropNum, "0", HideDropPro);
                            //Debug.Log("Str7777 = " + i);
                            //背包满了的话直接掉落到地上
                            if (!ifDrop)
                            {
                                //背包满了执行掉落地上
                                AI_DropItem(itemID, vec3, monsterID, itemDropNum);
                                //Debug.Log("Str8888 = " + i);
                            }
                            else {
                                //Debug.Log("Str9999 = " + i);
                                //道具进入背包成功,记录抽卡数据
                                if (Game_PublicClassVar.Get_game_PositionVar.ChouKaUIOpenStatus)
                                {
                                    //Debug.Log("Str0000 = " + i);
                                    string Str = Game_PublicClassVar.Get_game_PositionVar.ChouKaStr;
                                    string hindIDStr = Game_PublicClassVar.Get_game_PositionVar.ChouKaHindIdStr;
                                    Str = Str + itemID + "," + itemDropNum + "," + hindIDStr + ";";
                                    Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = Str;
                                    Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus = true;
                                    //Debug.Log("Str = " + Str);
                                }
                            }
                            
                        break;
                    }
                    //累计掉落数量
                    //dropNum = dropNum + 1;
                    if (dropNumMax != 0)
                    {
                        //Debug.Log("掉落1");
                        dropNumNow = dropNumNow + 1;
                        if (dropNumNow >= dropNumMax) {
                            //Debug.Log("掉落2");
                            return dropNumNow;
                        }
                    }
                }
                else
                {
                    //掉落失败
                }


            }
            else {
                i = 10; //因为一条掉落最大支持10个道具数据
            }
        }

        return dropNumNow;
    }

    //生成掉落数量
    public bool AI_DropItem(string itemID, Vector3 vec3, string monsterID,int dropNum = 1)
    {

        //Debug.Log("掉落执行了1次");

        //获得道具名称
        //string itemName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("ItemIcon", "ID", itemID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Item_Template.xml");
        //Debug.Log("掉落ID" + itemID);
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", itemID, "Item_Template");
        //获得道具品质
        //string itemQuality = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("ItemQuality", "ID", itemID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Item_Template.xml");

        string itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template");

        //实例化一个掉落Obj
        GameObject obj_DropItem = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_Model_Drop);
        obj_DropItem.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Tr_Drop;
        obj_DropItem.transform.position = new Vector3(vec3.x, vec3.y, vec3.z);
        obj_DropItem.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        obj_DropItem.GetComponent<UI_DropName>().DropItemID = itemID;           //设置掉落物品的ID
        if (itemID != "1")
        {
            obj_DropItem.GetComponent<UI_DropName>().DropItemNum = dropNum;               //设置掉落物品的数量
        }
        else {
            obj_DropItem.GetComponent<UI_DropName>().DropItemNum = randomSet;    
        }
        //添加极品属性字段
        if (monsterID != "0")
        {
            float HideDropPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideDropPro", "ID", monsterID, "Monster_Template"));
            obj_DropItem.GetComponent<UI_DropName>().HideDropPro = HideDropPro;
        }
        else {
            obj_DropItem.GetComponent<UI_DropName>().HideDropPro = 0;
        }

        return true;
        
    }

    //AI加血（参数1：怪物Obj 参数2：加血的值）
    public bool AI_addHp(GameObject monster,int addHpValue) {
        //获取ai属性脚本
        AI_Property ai_Property = monster.GetComponent<AI_Property>();
        //判定加血后当前血量是否超越上限
        if (ai_Property.AI_Hp < ai_Property.AI_HpMax)
        {
            int addHp = ai_Property.AI_Hp + addHpValue;
            if (addHp >= ai_Property.AI_HpMax)
            {
                ai_Property.AI_Hp = ai_Property.AI_HpMax;
            }
            else {
                ai_Property.AI_Hp = ai_Property.AI_Hp + addHpValue;
            }
        }
        
        return true;
    }

    //AI扣血（参数1：怪物Obj 参数2：扣血的值）
    public bool AI_costHp(GameObject monster, int costHpValue)
    {
        //获取ai属性脚本
        AI_Property ai_Property = monster.GetComponent<AI_Property>();
        //判定加血后当前血量是否超越上限
        int addHp = ai_Property.AI_Hp - costHpValue;
        if (addHp <= 0 )
        {
            ai_Property.AI_Hp = 0;
        }
        else
        {
            ai_Property.AI_Hp = addHp;
        }
        return true;
    }

    
    //创建怪物
    //创建怪物的ID,必须在Resources/CreateMonster目录下创建一同ID的怪物Obj   ,GameObject newMonsterObj
    public void AI_CreatMonster(string monsterID, Vector3 CreateVec3, GameObject createrMonsterObj = null, Object newMonsterObj = null)
    { 
        
        //获取怪物
        //GameObject monsterObj = Instantiate((GameObject)Resources.Load("CreateMonster/" + monsterID, typeof(GameObject)));
        GameObject monsterObj = (GameObject)Instantiate(newMonsterObj, CreateVec3,new Quaternion(0,0,0,0));
        //GameObject monsterSetObj = GameObject.Find("Monster");
        
        GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.MonsterSet;
        if (monsterSetObj == null) {
            //monsterSetObj = GameObject.Find("Monster");
            monsterSetObj = GameObject.FindWithTag("Monster");
        }
        
        monsterObj.transform.SetParent(monsterSetObj.transform);
        //monsterObj.transform.position = CreateVec3;
        monsterObj.SetActive(false);
        monsterObj.SetActive(true);

        if (createrMonsterObj != null) {
            monsterObj.GetComponent<AI_1>().MonsterCreateObj = createrMonsterObj;       //设置父级 用于父级怪物返回时删除召唤怪物
        }
        
        //return monsterObj;

    }

    //存储怪物复活时间
    //怪物唯一ID   在线复活时间 离线复活时间
    public void SaveMonsterDeathTime(string ai_ID_Only, string deathTime,string offLineTime) {

        bool saveStatus = true;
        //检测存储的复活数据里有没有相同的怪物ID
        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                if (deathMonsterID[0] == ai_ID_Only)
                {
                    saveStatus = false;
                }
            }
        }

        //存储状态怪物复活
        if (saveStatus) {
            if (deathMonsterIDListStr == "")
            {
                deathMonsterIDListStr = ai_ID_Only + "," + deathTime + "," + offLineTime;
            }
            else {
                deathMonsterIDListStr = deathMonsterIDListStr + ";" + ai_ID_Only + "," + deathTime + "," + offLineTime;
            }
            //Debug.Log("更新ai_ID_Only = " + ai_ID_Only + "     deathTime = " + deathTime + "    deathMonsterIDListStr = " + deathMonsterIDListStr);
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDListStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }

    //更新怪物复活时间
    public void UpdataMonsterDeathTime(float updataTime)
    {
        //Debug.Log("更新怪物复活时间");
        bool saveStatus = true;
        string deathMonsterIDListWriteStr = "";
        //Debug.Log("更新怪物复活时间11111111111");
        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //Debug.Log("更新怪物复活时间22222222222");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "") {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                float deathTimeValue = float.Parse(deathMonsterID[1]) - updataTime;
                if (deathTimeValue < 0)
                {
                    deathTimeValue = 0;
                }
                else {
                    deathMonsterIDListWriteStr = deathMonsterIDListWriteStr + deathMonsterID[0] + "," + deathTimeValue.ToString() + ";";
                }
            }
        }

        //Debug.Log("更新怪物复活时间3333333333333");
        //写入复活时间值
        if (deathMonsterIDListWriteStr != "") {
            deathMonsterIDListWriteStr = deathMonsterIDListWriteStr.Substring(0, deathMonsterIDListWriteStr.Length - 1);
        }
        //Debug.Log("更新怪物复活时间4444444444444");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDListWriteStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Debug.Log("更新怪物复活时间5555555555555");
    }


    //更新怪物离线复活时间
    public void UpdataMonsterDeathOffLineTime(float updataTime)
    {
        //Debug.Log("更新怪物复活时间");
        bool saveStatus = true;
        string deathMonsterIDListWriteStr = "";
        //Debug.Log("更新怪物复活时间11111111111");
        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //Debug.Log("更新怪物复活时间22222222222");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                float deathTimeValue = float.Parse(deathMonsterID[1]);
                float deathOffLineTimeValue = float.Parse(deathMonsterID[2]) - updataTime;
                if (deathOffLineTimeValue < 0)
                {
                    //deathOffLineTimeValue = 0;
                    //置空删除本条复活数据
                }
                else
                {
                    deathMonsterIDListWriteStr = deathMonsterIDListWriteStr + deathMonsterID[0] + "," + deathTimeValue.ToString() + ","+ deathOffLineTimeValue+";";
                }
            }
        }

        //Debug.Log("更新怪物复活时间3333333333333");
        //写入复活时间值
        if (deathMonsterIDListWriteStr != "")
        {
            deathMonsterIDListWriteStr = deathMonsterIDListWriteStr.Substring(0, deathMonsterIDListWriteStr.Length - 1);
        }
        //Debug.Log("更新怪物复活时间4444444444444");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDListWriteStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Debug.Log("更新怪物复活时间5555555555555");
    }

    //获取怪物复活时间
    public float GetMonsterDeathTime(string ai_ID_Only)
    {

        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                if (deathMonsterID[0] == ai_ID_Only)
                {
                    //如果离线复活的时间小于在线复活的时间,则返回离线复活的时间
                    if (float.Parse(deathMonsterID[2]) < float.Parse(deathMonsterID[1])) {
                        return float.Parse(deathMonsterID[2]);
                    }
                    return float.Parse(deathMonsterID[1]);
                }
            }
        }
        return 0;
    }

    //获取怪物离线复活时间
    public float GetMonsterDeathOffLineTime(string ai_ID_Only)
    {

        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                if (deathMonsterID[0] == ai_ID_Only)
                {
                    //如果离线复活的时间小于在线复活的时间,则返回离线复活的时间
                    /*
                    if (float.Parse(deathMonsterID[2]) < float.Parse(deathMonsterID[1]))
                    {
                        return float.Parse(deathMonsterID[2]);
                    }
                     */
                    return float.Parse(deathMonsterID[2]);
                }
            }
        }
        return 0;
    }
}
