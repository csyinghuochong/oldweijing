using UnityEngine;
using System.Collections;

public class Function_MonsterSkill{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //加血,恢复当前血量上限
    public void MonsterAddHp(GameObject monsterObj, string skillID) {
        
        AI_Property ai_property = monsterObj.GetComponent<AI_Property>();
        //Debug.Log("AI的前血量 = " + ai_property.AI_Hp);
        //获取本次治疗血量
        string actDamge = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ActDamge", "ID", skillID, "Skill_Template");
        string damgeValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue", "ID", skillID, "Skill_Template");
        //Debug.Log("ai_property = " + ai_property.name);
        //Debug.Log("ai_property.AI_Hp = " + ai_property.AI_Hp + " actDamge = " + actDamge + "damgeValue = " + damgeValue + "ai_property.AI_HpMax = " + ai_property.AI_HpMax);
        ai_property.AI_Hp = ai_property.AI_Hp + int.Parse(damgeValue) + (int)(float.Parse(actDamge) * (float)(ai_property.AI_HpMax));


        //Debug.Log("AI的后血量 = " + ai_property.AI_Hp);
    
    }

    //怪物附加技能
    public void AddSkillID(string skillID,GameObject MonsterObj) {
        //触发BUFF
        //string skillObjName = "Monster_FireWall_1";
        string skillObjName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectName", "ID", skillID, "Skill_Template");
        //skillObjName = "Monster_FireWall_1";
        GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill_Obj/" + skillObjName, typeof(GameObject));
        GameObject SkillObject_p = (GameObject)MonoBehaviour.Instantiate(SkillObj);
        SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = MonsterObj.GetComponent<AI_1>().AI_Target;
        string skillParent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParent", "ID", skillID, "Skill_Template");
        //Debug.Log("间接触发技能+" + skillID);
        /*
        if (skillID[i] == "62000103") {
            Debug.Log("触发召唤技能");
        }
        */
        //skillParent = "2";      //测试无绑定点用的,后面需删掉
        switch (skillParent)
        {
            //绑定在身上
            case "0":
                //目前只支持对自己附加
                //Debug.Log("技能挂在自己身上+" + skillID[i]);
                string skillParentPosition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID, "Skill_Template");
                SkillObject_p.transform.SetParent(MonsterObj.GetComponent<AI_1>().BoneSet.transform.Find(skillParentPosition));
                SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
                SkillObject_p.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID;
                SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = MonsterObj;
                //triggerSkillID = skillID;
                //SkillStatus = true;         //开启技能状态
                break;
            //无绑定点
            case "1":
                //目前只支持对攻击目标区域释放
                //获取攻击目标位置
                Vector3 skillPosition = MonsterObj.GetComponent<AI_1>().AI_Target.transform.position;
                SkillObject_p.transform.position = skillPosition;
                //skillID[i] = "60090002";
                SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID;
                SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = MonsterObj;

                //SkillStatus = true;         //开启技能状态
                //triggerSkillID = skillID[i];
                break;

            //无绑定点,释放起始位置位于AI中心
            case "2":
                //目前只支持对攻击目标区域释放
                //获取攻击目标位置
                skillPosition = MonsterObj.GetComponent<AI_1>().AI_Target.transform.position;
                string playStartPoisition = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillParentPosition", "ID", skillID, "Skill_Template");
                SkillObject_p.transform.position = MonsterObj.GetComponent<AI_1>().BoneSet.transform.Find(playStartPoisition).transform.position;
                //skillID[i] = "60090002";
                SkillObject_p.GetComponent<SkillObjBase>().SkillID = skillID;
                SkillObject_p.GetComponent<SkillObjBase>().SkillOpen = true;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetPoint = skillPosition;
                SkillObject_p.GetComponent<SkillObjBase>().MonsterSkillObj = MonsterObj;
                SkillObject_p.GetComponent<SkillObjBase>().SkillTargetObj = MonsterObj.GetComponent<AI_1>().AI_Target;

                //SkillStatus = true;         //开启技能状态
                //triggerSkillID = skillID[i];
                break;
        }
    
    }


}
