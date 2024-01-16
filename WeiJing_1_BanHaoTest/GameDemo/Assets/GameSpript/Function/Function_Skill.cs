using UnityEngine;
using System.Collections;

public class Function_Skill{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //根据BuffID触发对应的技能  (参数1：BuffID组成的字符串，参数2：附加Buff的Obj)
    public bool SkillBuff(string buffIDStr, GameObject obj)
    {
        //Debug.Log("进入触发BUFF = " + buffIDStr);
        //循环获取BuffID
        string[] buffID = buffIDStr.Split(',');
        //Debug.Log("进入触发BUFF[0] = " + buffID[0]);
        for (int i = 0; i <= buffID.Length - 1; i++)
        {
            //Debug.Log("进入触发BUFF0000000 = " + buffID[i]);
            if (buffID[i] != "") { 
                //获取Buff释放目标
                //Debug.Log("可能出错的BUFFID = " + buffID[i]);
                string targetType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TargetType", "ID", buffID[i], "SkillBuff_Template");
                //Debug.Log("进入触发targetType = " + targetType);
                switch (targetType) { 
                    //玩家自身
                    case "1":
                        //判定传入obj是否相符
                        //Debug.Log("进入触发BUFF1111111111 = " + buffIDStr);
                        if (obj.gameObject.name == "Rose") {
                            //Debug.Log("进入触发BUFF22222222222 = " + buffIDStr);
                            BuffTypeReturnScriptName(obj,buffID[i]);
                        }
                        break;
                    //怪物
                    case "2":
                        //判定传入obj是否相符
                        //Debug.Log("怪物触发BUFF,obj.gameObject.name = " + obj.gameObject.name);
                        if (obj.gameObject.name != "Rose")
                        {
                            //Debug.Log("怪物触发BUFF:" + buffID[i]);
                            BuffTypeReturnScriptName(obj, buffID[i]);
                        }
                        break;
                }
            }
        }

        return true;
        
    }

    //根据buff类型返回Buff脚本名称(参数1：需要绑定对象,参数2：BuffID)             注意：此处不做是否绑定的判定,请在其他地方判定
    public void BuffTypeReturnScriptName(GameObject obj,string buffID)
    {
        
        //获取Buff类型
        string buffType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffType", "ID", buffID, "SkillBuff_Template");
        //Debug.Log("进入附加BUFF,buffType = " + buffType);
        switch (buffType) { 
            case "1":

                //检测目标是否用此Buff如果有执行替换

                //绑定脚本
                obj.AddComponent<Buff_1>().BuffID = buffID;
                
                break;

            case "2":
                //绑定脚本
                obj.AddComponent<Buff_2>().BuffID = buffID;
                //Debug.Log("绑定BUFF提示：" + obj.name + "绑定BUFF_2");
                //给BUFF传入释放技能时的攻击值  (目标怪物不支持按照怪物攻击的百分比减血,因为不知道是那个怪物放的)
                if (obj.layer == 14) {
                    //value = obj.GetComponent<AI_Property>().AI_Act;
                    //Debug.Log("攻击角色");
                }
                //攻击怪物
                if (obj.layer == 12){
                    int actValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act;
                    obj.GetComponent<Buff_2>().ActValue = actValue;
                    //Debug.Log("攻击怪物" + actValue);
                }
                
                break;
                

                //眩晕
            case "3":
                obj.AddComponent<Buff_3>().BuffID = buffID;
                //Debug.Log("绑定眩晕BUFF");
                //obj.GetComponent<Buff_3>();
                break;

            //属性改变
            case "4":
                Buff_4[] buff_4 = obj.GetComponents<Buff_4>();
                for (int i = 0; i <= buff_4.Length - 1; i++)
                {
                    string[] continuedTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffParameter", "ID", buffID, "SkillBuff_Template").Split(',');
                    string propretyType = continuedTime[0];
                    string propretyAddType = continuedTime[1];
                    //表示两个Buff的Buff效果一致,后者将替代前者的Buff
                    if (buff_4[i].propretyType == propretyType) {
                        if (buff_4[i].propretyAddType == propretyAddType) {
                            //注销
                            MonoBehaviour.Destroy(buff_4[i]);
                            //Debug.Log("有相同的Buff,已经替换");
                        }
                    }
                }
                    obj.AddComponent<Buff_4>().BuffID = buffID;
                break;
        }
        
    }

    //根据目标类型判定Buff附带的目标是否死亡
    public bool ifDeath(string targetType, GameObject targetObj) {

        switch (targetType)
        {
            //玩家
            case "1":
                if (targetObj.GetComponent<Rose_Proprety>().Rose_HpNow <= 0)
                {
                    //Debug.Log("玩家死了Buff注销");
                    return true;
                }
                break;
            //怪物
            case "2":
                if (targetObj.GetComponent<AI_Property>().AI_Hp <= 0)
                {
                    return true;
                }
                break;
        }
        return false;
    }


    //传入技能ID,播放攻击特效(参数1：技能ID,参数2：释放技能的本体)
    public void PlayActSkillEffect(string skillID, GameObject SelfObj) {

        string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", skillID, "Skill_Template");
        if (effectName != "" && effectName != "0") {

            //实例化技能特效
            GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + effectName, typeof(GameObject));
            GameObject effect = (GameObject)MonoBehaviour.Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = SelfObj.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.transform.localRotation = Quaternion.Euler( new Vector3(0.0f, 0.0f, 0.0f));
            effect.SetActive(true);

        }
    }



    //传入技能ID,播放受击特效




    //传入眩晕时间和眩晕目标,设置目标眩晕状态 (参数一：眩晕时间 参数二：眩晕目标Obj 参数三：目标类型)
    public bool XuanYunBuff(float xuanYunBuffTime, GameObject xuanYunObj, string xuanYunType) {

        //获取眩晕类型
        switch (xuanYunType) { 
            
            //己方
            case "1":
                //Debug.Log("进入眩晕");
                //获取目标眩晕状态
                bool xuanYunStatus = xuanYunObj.GetComponent<Rose_Status>().XuanYunStatus;
                float xuanYunTimeSum = xuanYunObj.GetComponent<Rose_Status>().xuanYunTimeSum;
                float xuanYunTime = xuanYunObj.GetComponent<Rose_Status>().XuanYunTime;
                if (xuanYunStatus)
                {
                    //获取目标眩晕时间,保持2个眩晕之间,存在一个眩晕长的
                    if (xuanYunTimeSum < xuanYunTime)
                    {
                        xuanYunObj.GetComponent<Rose_Status>().XuanYunTime = xuanYunBuffTime;
                        xuanYunObj.GetComponent<Rose_Status>().XuanYunStatus = true;
                        //Debug.Log("更新眩晕！111");
                    }
                }
                else {

                    xuanYunObj.GetComponent<Rose_Status>().XuanYunTime = xuanYunBuffTime;
                    xuanYunObj.GetComponent<Rose_Status>().XuanYunStatus = true;
                    //Debug.Log("更新眩晕！222");
                }
                break;

            //敌方
            case "2":
                //Debug.Log("XuanYunStatus");
                //获取目标眩晕状态
                xuanYunStatus = xuanYunObj.GetComponent<AI_1>().XuanYunStatus;
                xuanYunTimeSum = xuanYunObj.GetComponent<AI_1>().xuanYunTimeSum;
                xuanYunTime = xuanYunObj.GetComponent<AI_1>().XuanYunTime;
                if (xuanYunStatus)
                {
                    //获取目标眩晕时间,保持2个眩晕之间,存在一个眩晕长的
                    if (xuanYunTimeSum < xuanYunTime)
                    {
                        //xuanYunStatus = true;
                        //xuanYunTime = xuanYunBuffTime;
                        xuanYunObj.GetComponent<AI_1>().XuanYunStatus = true;
                        xuanYunObj.GetComponent<AI_1>().XuanYunTime = xuanYunBuffTime;
                    }
                }
                else
                {
                    //xuanYunStatus = true;
                    //xuanYunTime = xuanYunBuffTime;
                    xuanYunObj.GetComponent<AI_1>().XuanYunStatus = true;
                    xuanYunObj.GetComponent<AI_1>().XuanYunTime = xuanYunBuffTime;

                }
                break;
        }

        return true;
    }

    //根据传入的属性类型,降低目标的值

    //设置技能范围（参数1：技能ID 参数2:技能自身的Obj）
    public void AddSkillRange(string skillID, GameObject skillObj) { 
        
        //获取
        string damgeRangeType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRangeType", "ID", skillID, "Skill_Template");
        string damgeRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRange", "ID", skillID, "Skill_Template");
        skillObj.transform.localRotation = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.localRotation;
        string[] rang = damgeRange.Split(';');

        switch(damgeRangeType) {
            //球形
            case "1":
                skillObj.AddComponent<SphereCollider>();
                skillObj.GetComponent<SphereCollider>().radius = float.Parse(damgeRange);
                skillObj.GetComponent<SphereCollider>().isTrigger = true;
                //设定技能技能范围的位置,而不是居中位置
                if (rang.Length > 1)
                {
                    string[] p_value = rang[1].Split(',');
                    float p_x = float.Parse(p_value[0]);
                    float p_y = float.Parse(p_value[1]);
                    float p_z = float.Parse(p_value[2]);
                    skillObj.GetComponent<SphereCollider>().center = new Vector3(p_x, p_y, p_z);
                }
                break;
            //方形
            case "2":
                string[] value = rang[0].Split(',');
                float x = float.Parse(value[0]);
                float z = float.Parse(value[1]);
                skillObj.AddComponent<BoxCollider>();
                skillObj.GetComponent<BoxCollider>().size = new Vector3(x, 1, z);
                skillObj.GetComponent<BoxCollider>().isTrigger = true;

                //设定技能技能范围的位置,而不是居中位置
                if (rang.Length > 1)
                {
                    string[] p_value = rang[1].Split(',');
                    float p_x = float.Parse(p_value[0]);
                    float p_y = float.Parse(p_value[1]);
                    float p_z = float.Parse(p_value[2]);
                    skillObj.GetComponent<BoxCollider>().center = new Vector3(p_x, p_y, p_z);
                }

                break;

            //对目标立即造成伤害
            case "3":
                skillObj.AddComponent<SphereCollider>();
                skillObj.GetComponent<SphereCollider>().radius = 1.0f;
                skillObj.GetComponent<SphereCollider>().isTrigger = true;
            break;
        }


    }

    //技能升级(参数1：技能ID , 参数2：是否删除原本技能ID(0 不删除  1 删除))
    public bool SkillUp(string skillID,string ifDelSkillID){
        
        //获取技能下一级id
        string nextSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextSkillID", "ID", skillID, "Skill_Template");
        if (nextSkillID != "0")
        {
            //获取学习技能需要的SP
            int costSPValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostSPValue", "ID", skillID, "Skill_Template"));
            //获取自己拥有的SP
            int RoseSP = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            //扣除SP,学习对应的技能
            if (RoseSP >= costSPValue)
            {
                RoseSP = RoseSP - costSPValue;

                string roseSkillStr = "";
                switch (ifDelSkillID)
                {

                    //不删除
                    case "0":
                        roseSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        if (roseSkillStr != "")
                        {
                            roseSkillStr = roseSkillStr + "," + nextSkillID;
                        }
                        else
                        {
                            roseSkillStr = nextSkillID;
                        }
                        break;
                    //删除
                    case "1":
                        //获取自身的技能数组
                        string[] roseSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
                        for (int i = 0; i <= roseSkill.Length - 1; i++)
                        {
                            if (roseSkill[i] == skillID)
                            {
                                roseSkillStr = roseSkillStr + nextSkillID + ",";
                            }
                            else
                            {
                                roseSkillStr = roseSkillStr + roseSkill[i] + ",";
                            }
                        }
                        //删除最后逗号
                        roseSkillStr = roseSkillStr.Substring(0, roseSkillStr.Length - 1);
                        break;
                }

                //写入对应数据
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SP", RoseSP.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillID", roseSkillStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                //PassiveSkill(skillID);
                //获取技能是否为被动技能
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;
                Game_PublicClassVar.Get_function_UI.PlaySource("10008", "1");
                return true;
            }
            else {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("SP值不足,每升一级获得1点SP值!");
            }
        }
        return false;
    }

    /*
    //传入技能ID,获取自身的被动技能并记录
    public bool PassiveSkill(string skillID) {

        //获取技能是否为被动技能
        string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", skillID, "Skill_Template");
        if (skillType == "2")
        {
            //获取自己的被动技能
            string aa = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong;
            string[] beidongSkillIDStr = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong.Split(';');
            for (int i = 0; i <= beidongSkillIDStr.Length - 1; i++)
            {
                if (beidongSkillIDStr[i] != "")
                {
                    string[] beidongSkillID = beidongSkillIDStr[i].Split(',');
                    if (beidongSkillID[0] == skillID)
                    {
                        //如果附加的被动技能和自己的拥有的被动技能ID一致,则跳出循环,什么也不发生改变
                        return true;
                    }
                }
            }

            //获取被动技能触发概率
            string passiveSkillPro = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PassiveSkillPro", "ID", skillID, "Skill_Template");
            //设置被动技能的相关值
            string passiveSkillStr = "";
            if (beidongSkillIDStr[0] != "")
            {
                passiveSkillStr = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong + ";" + skillID + "," + passiveSkillPro;
            }
            else
            {
                passiveSkillStr = skillID + "," + passiveSkillPro;
            }
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().skillIDBeiDong = passiveSkillStr;
        }
        return true;
    }

    //初始化被动技能
    public void InitializePassiveSkill() { 
        //获取自身拥有的技能
        string[] roseSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        for (int i = 0; i <= roseSkill.Length - 1; i++) {
            if (roseSkill[i] != "" && roseSkill[i] != "0") {
                //PassiveSkill(roseSkill[i]);
                //获取技能是否为被动技能
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;
            }
        }
    }
    */
    //更新装备技能ID
    public void UpdataEquipSkillID() {
        //Debug.Log("更新装备技能");
        Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;
        string equipSkillIDStr = "";
        string equipSkillType3_Str = "";            //装备技能类型为3的字符串

        //循环读取装备技能
        for (int i = 1; i <= 13; i++)
        {
            string equipID = functionDataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
            //Debug.Log("i = " + i + ";   equipID = " + equipID);
            if (equipID == "0")
            {
                continue;   //立即执行下次循环
            }
            //根据装备ID获取对应的技能ID
            string[] equipSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", equipID, "Item_Template").Split(',');
            for (int y = 0; y <= equipSkillID.Length - 1; y++) {

                if (equipSkillID[y] != "0")
                {
                    //更新装备ID字符串
                    if (equipSkillIDStr == "")
                    {
                        equipSkillIDStr = equipSkillID[y];
                    }
                    else
                    {
                        equipSkillIDStr = equipSkillIDStr + "," + equipSkillID[y];
                    }

                    //更新装备技能类型为3的字符串
                    //获取SkillID的技能类型为3则跳过循环
                    string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", equipSkillID[y], "Skill_Template");
                    if (skillType == "3")
                    {
                        if (equipSkillType3_Str == "")
                        {
                            equipSkillType3_Str = equipSkillID[y];
                        }
                        else
                        {
                            equipSkillType3_Str = equipSkillType3_Str + "," + equipSkillID[y];
                        }
                    }
                }
            }
            //Debug.Log("equipSkillID[0] = " + equipSkillID[0]);
            //Debug.Log("equipSkillIDStr = " + equipSkillIDStr);
        }
        //Debug.Log("equipSkillType3_Str = " + equipSkillType3_Str);
        //写入对应数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSkillID", equipSkillIDStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        
        //更新技能ID
        string learnSkillStrSet = "";
        //获取自身的技能数组
        string roseSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string[] roseSkill = roseSkillStr.Split(',');

        //获取类型为3的技能
        string[] equipSkillType3 = equipSkillType3_Str.Split(',');
        //Debug.Log("equipSkillType3 = " + equipSkillType3_Str);
        //循环判定自身有没有对应的技能ID
     
        for (int i = 0; i <= roseSkill.Length - 1; i++)
        {
            for (int y = 0; y <= equipSkillType3.Length - 1; y++)
            {
                //获取升级附加数据
                if (equipSkillType3[0] == "") {
                    break;
                }
                string[] equipSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkill", "ID", equipSkillType3[y], "Skill_Template").Split(';');
                for (int x = 0; x <= equipSkill.Length - 1; x++)
                {
                    string equipSkillID_1 = equipSkill[x].Split(',')[0];
                    string equipSkillID_2 = equipSkill[x].Split(',')[1];

                    if (roseSkill[i] == equipSkillID_1)
                    {
                        //Debug.Log("equipSkillID_2 = " + equipSkillID_2 + "equipSkillID_1 = " + equipSkillID_1);
                        roseSkill[i] = equipSkillID_2;      //更换升级后的技能
                        Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(equipSkillID_1, roseSkill[i]);        //替换主界面图标
                        //Debug.Log("技能替换：更换了技能");
                        //立即跳出循环
                        break;
                    }
                }
            }

            if (learnSkillStrSet != "" && learnSkillStrSet != "0")
            {
                learnSkillStrSet = learnSkillStrSet + "," + roseSkill[i];
            }
            else
            {
                learnSkillStrSet = roseSkill[i];
            }
        }

        //Debug.Log("learnSkillStrSet = " + learnSkillStrSet);
        //写入对应数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillIDSet", learnSkillStrSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //装备最终数据
        string equipSkillIDStrSet = "";
        string equipSkillID_Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        
        //获取自身套装携带的技能
        string equipSuitSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (equipSuitSkillStr != "")
        {
            if (equipSkillID_Str != "")
            {
                equipSkillID_Str = equipSkillID_Str + "," + equipSuitSkillStr;
            }
            else {
                equipSkillID_Str = equipSuitSkillStr;
            }
        }

        //Debug.Log("equipSkillID_Str = " + equipSkillID_Str);
         
        //获取自身的装备技能数组
        string[] equipSkillStrID = equipSkillID_Str.Split(',');
        //循环判定自身有没有对应的技能ID
        for (int i = 0; i <= equipSkillStrID.Length - 1; i++)
        {
            if (equipSkillStrID[0] == "") {
                break;
            }
            string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", equipSkillStrID[i], "Skill_Template");
            if (skillType == "3") {
                continue;     //立即执行下次循环
            }

            for (int y = 0; y <= equipSkillType3.Length - 1; y++)
            {
                //如果附加等级类技能为空则直接跳出循环
                if (equipSkillType3[0] == "")
                {
                    break;
                }

                //获取升级附加数据
                string[] equipSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkill", "ID", equipSkillType3[y], "Skill_Template").Split(';');
                for (int x = 0; x <= equipSkill.Length - 1; x++)
                {
                    string equipSkillID_1 = equipSkill[x].Split(',')[0];
                    string equipSkillID_2 = equipSkill[x].Split(',')[1];

                    if (equipSkillStrID[i] == equipSkillID_1)
                    {
                        equipSkillStrID[i] = equipSkillID_2;      //更换升级后的技能
                        //Debug.Log("更换了技能");
                        //立即跳出循环
                        break;
                    }
                }
            }

            //获取前面是否有相同的技能ID
            bool xiangTongID = false;
            for (int z = 0; z < i; z++)
            {
                if (equipSkillStrID[i] == equipSkillStrID[z]) {
                    xiangTongID = true;
                }
            }
            if (!xiangTongID) {
                if (equipSkillIDStrSet != "" && equipSkillIDStrSet != "0")
                {
                    equipSkillIDStrSet = equipSkillIDStrSet + "," + equipSkillStrID[i];
                }
                else
                {
                    equipSkillIDStrSet = equipSkillStrID[i];
                }
            }
        }

        //写入对应数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipSkillIDSet", equipSkillIDStrSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");


        //获取当前快捷技能
        //获取当前拥有的全部技能列表
        string piPeiSkillIDStr = "";
        string LearnSkillIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (LearnSkillIDSet != "")
        {
            piPeiSkillIDStr = LearnSkillIDSet + "," + equipSkillIDStrSet;
        }
        else
        {
            piPeiSkillIDStr = equipSkillIDStrSet;
        }

        //对比技能ID
        string[] piPeiSkillID = piPeiSkillIDStr.Split(',');
        //Debug.Log("piPeiSkillIDStr = " + piPeiSkillIDStr);
        for (int i = 1; i <= 8; i++)
        {
            string skillIDValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (skillIDValue != "") {
                bool ifShowSkillID = false;

                if (piPeiSkillID[0] != "")
                {
                    for (int x = 0; x <= piPeiSkillID.Length - 1; x++)
                    {
                        if (skillIDValue == piPeiSkillID[x])
                        {
                            ifShowSkillID = true;
                        }
                        else { 
                            //判定装备上的技能附加属性是否需要替换
                            /*
                            if (skillIDValue != "")
                            {

                            }
                            */
                        }
                    }
                }

                //获取当前ID是否为道具
                if (skillIDValue.Substring(0, 1) == "1") {
                    ifShowSkillID = true;
                }

                //写入快捷技能
                if (!ifShowSkillID)
                {
                    //Debug.Log("更新技能：" + i);
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + i, "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;  //更新技能图标
                }
            }
        }
        //更新被动技能
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;
        //Debug.Log("更新了装备的技能");
    }

    //传入技能ID判定并获取加成后的技能ID
    public string ReturnAddSkillID(string skillID){

        string returnSkillID = skillID;

        return skillID;

    }

    //替换主界面快捷施法技能ID
    //参数1：替换源ID   参数2：替换后ID
    public void UpdataMainUISkillID(string skillID_1,string skillID_2) {

        for (int i = 1; i <= 8; i++) {
            string mainUISkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            if (mainUISkillID == skillID_1) {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("MainSkillUI_" + i, skillID_2, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;
            }
        }
    }

    //传入要卸下的装备技能ID,主界面快捷施法更换技能ID
    public void UpdataMainUIEquipSkillID(string equipSkillID) {
        //ID为空结束
        if (equipSkillID == "" && equipSkillID == "0") {
            return;
        }
        string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", equipSkillID, "Skill_Template");
        //不为被动技能结束
        if (skillType != "3")
        {
            return;     //立即执行下次循环
        }
        //检测并替换装备附加技能ID
        string[] equipSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkill", "ID", equipSkillID, "Skill_Template").Split(';');
        for (int x = 0; x <= equipSkill.Length - 1; x++)
        {
            string equipSkillID_1 = equipSkill[x].Split(',')[0];
            string equipSkillID_2 = equipSkill[x].Split(',')[1];
            for (int i = 1; i <= 8; i++)
            {
                string mainUISkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + i, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                if (mainUISkillID == equipSkillID_2)
                {
                    Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(equipSkillID_2, equipSkillID_1);        //替换主界面图标
                }
            }

        }


    }

    //技能重置
    public void RoseReSkillSP()
    {

        Game_PublicClassVar.Get_function_UI.GameGirdHint("技能重置成功!");

        //替换主界面图标
        string[] changeStringID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(',');
        for (int i = 0; i <= changeStringID.Length - 1; i++)
        {
            Debug.Log("changeStringID_" + i + " = " + changeStringID[i]);
            Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(changeStringID[i], "");        //替换主界面图标
        }

        //更改技能数据
        //获取当前职业
        string occType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string learnSkillID = "60010101,60010200,60010300,60010400,60010500,60010600";  //默认战士
        switch (occType)
        {
            //战士初始技能
            case "1":
                learnSkillID = "60010101,60010200,60010300,60010400,60010500,60010600";
                break;
            //法师初始技能
            case "2":
                learnSkillID = "60030011,60030020,60030030,60030040,60030050,60030060";
                break;
        }
        
        Debug.Log("learnSkillID = " + learnSkillID);
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillID", learnSkillID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LearnSkillIDSet", learnSkillID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SP", roseLv, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //关闭技能界面OpenRoseSkill
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenRoseSkill();

        //更新主界面图标显示
        Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI = true;


    }

}
