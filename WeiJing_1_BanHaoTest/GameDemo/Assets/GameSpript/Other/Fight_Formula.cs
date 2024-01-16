using UnityEngine;
using System.Collections;

//public class Fight_Formult : MonoBehaviour {

//战斗公式

public class Fight_Formult{

	public int FightHurt(int act,int def){

		int FightHurt = (int)((act-def)* (100+Random.value*20-10)/100);

        //最低伤害
        if (FightHurt <= 1) {
            FightHurt = 1;
        }

		return FightHurt;

    }

    //宠物攻击,传入技能ID和怪物ID计算角色攻击怪物的伤害值
    public bool PetActMonster(GameObject actObj, string skillID, GameObject monsterObj, bool ifCri)
    {



        //读取技能属性
        float actDamge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID, "Skill_Template"));
        int damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template"));
        int damgeType = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeType", "ID", skillID, "Skill_Template"));


        //读取角色攻击值
        int roseLv = actObj.GetComponent<AI_Property>().AI_Lv;
        int roseAct = actObj.GetComponent<AI_Property>().AI_Act;
        //roseAct = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act*0.1f);
        float roseCri = actObj.GetComponent<AI_Property>().AI_Cri;
        float roseHit = actObj.GetComponent<AI_Property>().AI_Hit;
        float roseDodge = actObj.GetComponent<AI_Property>().AI_Dodge;
        float roseDefAdd = actObj.GetComponent<AI_Property>().AI_DefAdd;
        float roseAdfAdd = actObj.GetComponent<AI_Property>().AI_AdfAdd;
        //float roseDamgeAdd = actObj.GetComponent<AI_Property>().AI_DamgeSub;

        //宠物强制命中等于
        //roseHit = 0.75f;
        roseAct = (int)(roseAct * (0.8f + Random.value * 0.4f));

        //设置必中
        //string ifMustAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfMustAct", "ID", skillID, "Skill_Template");
        string ifMustAct = "1";
        if (ifMustAct == "1")
        {
            roseHit = 99;
        }
        //Debug.Log("roseHit = " + roseHit);
        //读取怪物属性
        int monsterLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        int monsterDef = monsterObj.GetComponent<AI_Property>().AI_Def;
        int monsterAdf = monsterObj.GetComponent<AI_Property>().AI_Adf;
        float monsteCri = monsterObj.GetComponent<AI_Property>().AI_Cri;
        float monsteHit = monsterObj.GetComponent<AI_Property>().AI_Hit;
        float monsteDodge = monsterObj.GetComponent<AI_Property>().AI_Dodge;
        float monsteDefAdd = monsterObj.GetComponent<AI_Property>().AI_DefAdd;
        float monsteAdfAdd = monsterObj.GetComponent<AI_Property>().AI_AdfAdd;
        float monsteDamgeAdd = monsterObj.GetComponent<AI_Property>().AI_DamgeAdd;

        roseLv = 1;
        monsterLv = 1;

        //判定是否攻击到目标
        /*
        int lvNum = roseLv - monsterLv;
        float now_RoseHit = 0.8f + 0.05f * lvNum + roseHit;
        float hitChance = now_RoseHit - monsteDodge;
        //Debug.Log(" hitChance = " + hitChance);
        //设置保底命中值(设置最低命中为20%)
        if (hitChance < 0.2f) {
            hitChance = 0.2f;
        }
        //Debug.Log(" hitChance11 = " + hitChance);
        //根据随机数判定是否命中目标
        if (Random.value > hitChance) { 
            
            //如果随机数大于命中值表示未命中目标
            return true;

        }
        */
        int damge = 0;
        switch (damgeType)
        {

            //物理攻击
            case 1:

                damge = (int)((roseAct - monsterDef) * actDamge * (1.0f - monsteDefAdd) * (1.0f - monsteDamgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else
                {
                    damge = damge + damgeValue;
                }

                //保底1点伤害
                if (damge < 1)
                {
                    damge = 1;
                }

                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;

                break;

            //魔法攻击
            case 2:

                damge = (int)((roseAct - monsterAdf) * actDamge * (1.0f - monsteAdfAdd) * (1.0f - monsteDamgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else
                {
                    damge = damge + damgeValue;
                }

                //保底1点伤害
                if (damge < 1)
                {
                    damge = 1;
                }

                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;

                break;

        }

        damge = fightdamge(roseLv, monsterLv, roseHit, roseCri, monsteDodge, roseAct, damgeValue, monsterDef, monsterAdf, actDamge, damgeType, monsteDefAdd, monsteAdfAdd, monsteDamgeAdd);


        //判定是否暴击
        if (ifCri)
        {
            damge = damge * 2;
            monsterObj.GetComponent<AI_1>().HitCriStatus = true;
        }

        //扣血
        monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;
        //开启怪物飘字
        monsterObj.GetComponent<AI_1>().HitStatus = true;

        //Debug.Log("怪物收到伤害：" + damge);

        return true;
    }


    //传入技能ID和怪物ID计算角色攻击怪物的伤害值
    public bool RoseActMonster(string skillID,GameObject monsterObj,bool ifCri) {

        if (monsterObj.layer == 18) {
            return false;
        }

        //读取技能属性
        float actDamge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID, "Skill_Template"));
        int damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template"));
        int damgeType = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeType", "ID", skillID, "Skill_Template"));
        

        //读取角色攻击值
        int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
        int roseAct = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act;
        float roseCri = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Cri;
        float roseHit = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hit;
        float roseDodge = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Dodge;
        float roseDefAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DefAdd;
        float roseAdfAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_AdfAdd;
        float roseDamgeAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DamgeSub;

        //设置必中
        string ifMustAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfMustAct", "ID", skillID, "Skill_Template");
        //string ifMustAct = "1";
        if (ifMustAct == "1") {
            roseHit = 99;
        }
        //Debug.Log("roseHit = " + roseHit);
        //读取怪物属性
        int monsterLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        int monsterDef = monsterObj.GetComponent<AI_Property>().AI_Def;
        int monsterAdf = monsterObj.GetComponent<AI_Property>().AI_Adf;
        float monsteCri = monsterObj.GetComponent<AI_Property>().AI_Cri;
        float monsteHit = monsterObj.GetComponent<AI_Property>().AI_Hit;
        float monsteDodge = monsterObj.GetComponent<AI_Property>().AI_Dodge;
        float monsteDefAdd = monsterObj.GetComponent<AI_Property>().AI_DefAdd;
        float monsteAdfAdd = monsterObj.GetComponent<AI_Property>().AI_AdfAdd;
        float monsteDamgeAdd = monsterObj.GetComponent<AI_Property>().AI_DamgeAdd;
        
        //判定是否攻击到目标
        /*
        int lvNum = roseLv - monsterLv;
        float now_RoseHit = 0.8f + 0.05f * lvNum + roseHit;
        float hitChance = now_RoseHit - monsteDodge;
        //Debug.Log(" hitChance = " + hitChance);
        //设置保底命中值(设置最低命中为20%)
        if (hitChance < 0.2f) {
            hitChance = 0.2f;
        }
        //Debug.Log(" hitChance11 = " + hitChance);
        //根据随机数判定是否命中目标
        if (Random.value > hitChance) { 
            
            //如果随机数大于命中值表示未命中目标
            return true;

        }
        */
        int damge = 0;
        switch (damgeType) { 
            
            //物理攻击
            case 1 :

                damge = (int)((roseAct - monsterDef) * actDamge * (1.0f - monsteDefAdd) * (1.0f - monsteDamgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else {
                    damge = damge + damgeValue;
                }

                //保底1点伤害
                if (damge < 1) {
                    damge = 1;
                }

                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;

            break;

            //魔法攻击
            case 2:

            damge = (int)((roseAct - monsterAdf) * actDamge * (1.0f - monsteAdfAdd) * (1.0f - monsteDamgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else {
                    damge = damge + damgeValue;
                }

                //保底1点伤害
                if (damge < 1) {
                    damge = 1;
                }

                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;

            break;

        }

        damge = fightdamge(roseLv, monsterLv, roseHit, roseCri, monsteDodge, roseAct, damgeValue, monsterDef, monsterAdf, actDamge, damgeType, monsteDefAdd, monsteAdfAdd, monsteDamgeAdd);
        
        
        //判定是否暴击
        if (ifCri)
        {
            damge = damge * 2;
            monsterObj.GetComponent<AI_1>().HitCriStatus = true;
        }
        
        //扣血
        monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;
        //开启怪物飘字
        monsterObj.GetComponent<AI_1>().HitStatus = true;

        //Debug.Log("怪物收到伤害：" + damge);

        return true;
    }




    //传入技能ID和怪物ID计算怪物攻击角色的伤害值
    public bool MonsterActRose(string skillID, GameObject monsterObj)
    {

        //读取技能属性
        float actDamge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID,"Skill_Template"));
        int damgeValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template"));
        int damgeType = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeType", "ID", skillID, "Skill_Template"));

        //读取角色攻击值
        int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
        int roseAct = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act;
        int roseDef = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Def;
        int roseAdf = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Adf;
        float roseCri = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Cri;
        float roseHit = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hit;
        float roseDodge = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Dodge;
        float roseDefAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DefAdd;
        float roseAdfAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_AdfAdd;
        float roseDamgeAdd = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_DamgeSub;

        //读取怪物属性
        int monsterLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        int monsterAct = monsterObj.GetComponent<AI_Property>().AI_Act;
        int monsterDef = monsterObj.GetComponent<AI_Property>().AI_Def;
        int monsterAdf = monsterObj.GetComponent<AI_Property>().AI_Adf;
        float monsteCri = monsterObj.GetComponent<AI_Property>().AI_Cri;
        float monsteHit = monsterObj.GetComponent<AI_Property>().AI_Hit;
        float monsteDodge = monsterObj.GetComponent<AI_Property>().AI_Dodge;
        float monsteDefAdd = monsterObj.GetComponent<AI_Property>().AI_DefAdd;
        float monsteAdfAdd = monsterObj.GetComponent<AI_Property>().AI_AdfAdd;
        float monsteDamgeAdd = monsterObj.GetComponent<AI_Property>().AI_DamgeAdd;

        //设置必中
        string ifMustAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfMustAct", "ID", skillID, "Skill_Template");
        //string ifMustAct = "1";
        if (ifMustAct == "1")
        {
            monsteHit = 99;
        }

        int damge = fightdamge(monsterLv, roseLv, monsteHit, monsteCri, roseDodge, monsterAct, damgeValue, roseDef, roseAdf, actDamge, damgeType, roseDefAdd, roseAdfAdd, roseDamgeAdd);
        //获取当前角色血量
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow > 0) {

            //检测自身是否有护盾
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunStatus)
            {
                int hudunDamge = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunValue - damge;
                if (hudunDamge > 0)
                {
                    Debug.Log("护盾抵消伤害:" + damge);
                    damge = 0;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunValue = hudunDamge;
                    
                }
                else {
                    //护盾消失,清空护盾状态
                    
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunValue = 0;
                    /*
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunStatus = false;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunTime = 0;
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HudunTimeSum = 0;
                    Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().HuDunEffect);
                    */
                    
                    damge = System.Math.Abs(hudunDamge);
                     
                }
            }

            int value = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow - damge;



            if (value <= 0)
            {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = 0;
                //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus = true;
            }
            else {
                Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = value;
                //此处不做存储本地本地文件处理,通过其他方式保存RoseData时一起保存,减少内存消耗
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", value.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"); 
            }
        }
        
        return true;
    }

    //传入对应的属性计算造成的伤害
    //参数说明
    /*
    lv                  攻击者等级
    target_lv           目标者等级
    hit                 攻击者命中值
    cri                 攻击者暴击值
    target_dodge        目标者闪避值
    act                 攻击者攻击值
    damgeValue          技能的附加的固定伤害
    target_def          目标的物理防御
    target_adf          目标的魔法防御
    actDamge            技能伤害来源的攻击值系数,为1时表示造成100%攻击力的伤害
    damgeType           伤害类型，分物理和魔法
    target_DefAdd       目标物理免伤值
    target_AdfAdd       目标魔法免伤值
    target_damgeAdd     目标总免伤值
    */
    private int fightdamge(int lv, int target_lv,float hit, float cri, float target_dodge, int act,int damgeValue, int target_def,int target_adf, float actDamge,int damgeType,float target_DefAdd,float target_AdfAdd,float target_damgeAdd){

        //判定是否攻击到目标
        int lvNum = lv - target_lv;
        float now_RoseHit = 0.8f + 0.05f * lvNum + hit;
        float hitChance = now_RoseHit - target_dodge;

        int criCof = 1;  //暴击系数
        if (Random.value <= cri)
        {
            criCof = 2;
        }
        else {
            criCof = 1;
        }

        //设置保底命中值(设置最低命中为20%)
        if (hitChance < 0.2f)
        {
            hitChance = 0.2f;
        }

        //根据随机数判定是否命中目标
        if (Random.value > hitChance)
        {

            //如果随机数大于命中值表示未命中目标
            return 0;

        }

        switch (damgeType)
        {

            //物理攻击
            case 1:

                int damge = (int)((act - target_def) * actDamge * (1.0f - target_DefAdd) * (1.0f - target_damgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else
                {
                    damge = damge * criCof + damgeValue;
                }

                //保底1点伤害
                if (damge < 1)
                {
                    damge = 1;
                }

                //Debug.Log("怪物造成了伤害：" + damge);
                return damge;

                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;

                break;

            //魔法攻击
            case 2:

                damge = (int)((act - target_adf) * actDamge * (1.0f - target_AdfAdd) * (1.0f - target_damgeAdd));
                if (damge <= 0)
                {
                    //固定伤害
                    damge = damgeValue;
                }
                else
                {
                    damge = damge * criCof + damgeValue;
                }

                //保底1点伤害
                if (damge < 1)
                {
                    damge = 1;
                }

                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_Hp - damge;
                //Debug.Log("怪物造成了伤害：" + damge);
                return damge;

                break;

        }

        return 0;

    }

}