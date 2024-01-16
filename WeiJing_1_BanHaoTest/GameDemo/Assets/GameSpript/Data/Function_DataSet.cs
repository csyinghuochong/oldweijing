using UnityEngine;
using System.Collections;
using System.Data;
using System.Xml;
using System.IO;


//本表为缓存Xml数据的表,在游戏初始化的时候将全部表缓存
public class Function_DataSet{

    //private DataSet dataSet; 
    private WWWSet wwwSet_p = Game_PublicClassVar.Get_wwwSet;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    

    //缓存XML数据表
    public DataTable DataSet_ReadXml(string xmlPath,string datatableName) {

        if (File.Exists(xmlPath))
        {
            //开始解密文件
            if (Game_PublicClassVar.Get_wwwSet.IfAddKey) {
                xmlPath = Game_PublicClassVar.Get_xmlScript.CostKey(xmlPath, datatableName);
            }

            //读取XML
            DataSet dateSet = new DataSet();
            dateSet.ReadXml(xmlPath);

            DataTable dataTable = new DataTable(datatableName);
            dataTable = dateSet.Tables[0].Copy();
            dataTable.TableName = datatableName;
            //删除指定加密文件
            if (Game_PublicClassVar.Get_wwwSet.IfAddKey)
            {
                File.Delete(xmlPath);
            }
            return dataTable;

        }
        else {

            Debug.Log(datatableName+"文件不存在！");
            return null;
        }
    }

    //写入全部XML数据表（游戏存档调用的比较多）
    public bool DataSet_AllWriteXml() {

        //Game_PublicClassVar.Get_game_PositionVar.DataSetXml.Tables["Item_Template"].WriteXml(Game_PublicClassVar.Get_game_PositionVar.Set_XmlPath + "Item_Template.xml");
        return true;
    
    }


    //单独将xml缓存的写入对应的XML文档
    public bool DataSet_SetXml(string xmlName)
    {
        try
        {
            if (Game_PublicClassVar.Get_wwwSet.IfAddKey)
            {
                Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables[xmlName].WriteXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + "_AddJie.xml");
                //加密
                //Debug.Log("开始加密存储文件");
                Game_PublicClassVar.Get_xmlScript.setKey(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + "_AddJie.xml", Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + ".xml");
            }
            else
            {
                Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables[xmlName].WriteXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + xmlName + ".xml");
            }
        }
        catch {
            Debug.Log("储存Xml报错" + xmlName);
        }
        return true;
    }

    //缓存全部XML数据表
    public bool DataSet_AllReadXml() {

        //获取绑点
        //GameObject gameStartVar = GameObject.FindWithTag("Tag_GameStartVar");
        //Game_PositionVar game_PositionVar = gameStartVar.GetComponent<Game_PositionVar>();

        //初始化数据
        Function_DataSet function_DataSet = new Function_DataSet();
        DataSet dateSet = new DataSet();
        DataTable dataTable = new DataTable();

        //Debug.Log("执行一次");

        //更新DataTable
        //缓存道具表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Item_Template.xml", "Item_Template");
        dataTable.PrimaryKey = new DataColumn[]{dataTable.Columns["ID"]};       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存掉落表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Drop_Template.xml", "Drop_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存掉落表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Monster_Template.xml", "Monster_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

		//缓存技能表
		dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Skill_Template.xml", "Skill_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
		dateSet.Tables.Add(dataTable);

        //缓存任务表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Task_Template.xml", "Task_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存NPC表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Npc_Template.xml", "Npc_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存角色经验表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "RoseExp_Template.xml", "RoseExp_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["RoseLv"] };       //设置主键
        dateSet.Tables.Add(dataTable);
		
		//缓存装备表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipSuit_Template.xml", "EquipSuit_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
		dateSet.Tables.Add(dataTable);

        //缓存装备套装表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipSuitProperty_Template.xml", "EquipSuitProperty_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存装备套装属性表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Equip_Template.xml", "Equip_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

		//缓存职业表
		dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Occupation_Template.xml", "Occupation_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
		dateSet.Tables.Add(dataTable);

        //缓存场景道具表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SceneItem_Template.xml", "SceneItem_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存Buff表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SkillBuff_Template.xml", "SkillBuff_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存故事对话表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "GameStory_Template.xml", "GameStory_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存场景表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Scene_Template.xml", "Scene_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存场景传送表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SceneTransfer_Template.xml", "SceneTransfer_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存建筑表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Building_Template.xml", "Building_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存章节表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Chapter_Template.xml", "Chapter_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存章节子表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ChapterSon_Template.xml", "ChapterSon_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存任务追踪坐标表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "TaskMovePosition_Template.xml", "TaskMovePosition_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //装备合成表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "EquipMake_Template.xml", "EquipMake_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //主参数配置表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "GameMainValue.xml", "GameMainValue");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //特殊事件
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "SpecialEvent_Template.xml", "SpecialEvent_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //每日任务
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "TaskCountry_Template.xml", "TaskCountry_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //抽卡
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "TakeCard_Template.xml", "TakeCard_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //荣誉大厅
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "HonorStore_Template.xml", "HonorStore_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //王国表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "Country_Template.xml", "Country_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //收集表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ShouJiItem_Template.xml", "ShouJiItem_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //收集表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Get_XmlPath + "ShouJiItemPro_Template.xml", "ShouJiItemPro_Template");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

		//缓存背包表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseData.xml", "RoseData");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存背包表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseBag.xml", "RoseBag");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存装备表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseEquip.xml", "RoseEquip");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

		//缓存装备表
		dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseConfig.xml", "RoseConfig");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
		dateSet.Tables.Add(dataTable);

        //缓存建筑表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseBuilding.xml", "RoseBuilding");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存仓库表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseStoreHouse.xml", "RoseStoreHouse");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存装备极品表
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseEquipHideProperty.xml", "RoseEquipHideProperty");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //缓存角色每日奖励
        dataTable = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadXml(Game_PublicClassVar.Get_wwwSet.Set_XmlPath + "RoseDayReward.xml", "RoseDayReward");
        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };       //设置主键
        dateSet.Tables.Add(dataTable);

        //更新DateSet
        Game_PublicClassVar.Get_wwwSet.DataSetXml = dateSet.Copy();
        Game_PublicClassVar.Get_wwwSet.DataUpdataStatus = true;
        Debug.Log("缓存所有表成功！");

        //判定DataSet内的数据是否有错误
        if (Game_PublicClassVar.Get_wwwSet.DataSetXml.HasErrors){
            return false;
        }
        else {
            return true;
        }

    }

    //读取对应的DataSet数据值
    public string DataSet_ReadData(string seclectKey, string primaryKey, string primaryValue, string xmlName)
    {
        try {
            //DataRow[] rows = wwwSet_p.DataSetXml.Tables[xmlName].Select(primaryKey + " = " + "'" + primaryValue + "'");
            //string seclectValue = rows[0][seclectKey].ToString();
            DataRow rows = wwwSet_p.DataSetXml.Tables[xmlName].Rows.Find(primaryValue);
            string seclectValue = rows[seclectKey].ToString();
        
            return seclectValue;
        }catch{

            string logStr = "报错读取数据：" + seclectKey + "," + primaryKey + "," + primaryValue + "," + xmlName;

            Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr = logStr + "\n" + Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr;

            return "0";
        }

    }

    public bool DataSet_WriteData(string writeKey, string writeValue, string primaryKey, string primaryValue, string xmlName)
    {
        try
        { 
            DataRow[] rows = Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables[xmlName].Select(primaryKey + " = " + "'" + primaryValue + "'");
            //Debug.Log("rows = "+rows);
            DataRow row = rows[0];
            //Debug.Log("row = " + row);
            row[writeKey] = writeValue;
            //Debug.Log("row = " + row);
            return true;
        }catch{

           string logStr = "报错写入数据：" + writeKey + "," + writeValue + "," + primaryKey + "," + primaryValue + "," + xmlName;
           Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr = logStr + "\n" + Game_PublicClassVar.Get_game_PositionVar.ErrorLogStr;
           return false;
        }
    }
    //新增装备极品属性
    public void AddRoseEquipHidePropertyXml(string addID, string addProperty)
    {

        //新建行数据
        DataTable dataTable = Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables["RoseEquipHideProperty"];
        DataRow row = dataTable.NewRow();
        //设置数据
        row["ID"] = addID;
        row["PrepeotyList"] = addProperty;
        //存储数据
        Game_PublicClassVar.Get_wwwSet.DataSetXml.Tables["RoseEquipHideProperty"].Rows.Add(row);
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseEquipHideProperty");
    }

}
